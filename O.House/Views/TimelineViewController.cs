using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;

using Foundation;
using UIKit;
using MonoTouch.Dialog;
using Commons;
using CustomElements;
using Utils;
using OHouse.DRM;
using CoreGraphics;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Facebook;
using Facebook.ShareKit;
using OHouse.Connectivity;
using O.House;

namespace OHouse
{
	public partial class TimelineViewController : UIViewController
	{
		Feed feed;
		DataRequestManager drm;
		
		List<ToiletsBase> posts;

		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;

		public TimelineViewController () : base ("TimelineViewController", null)
		{
			Title = "Timeline";
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			feed = new Feed ();
			drm = new DataRequestManager ();

			//////
			/// update network status
			UpdateStatus ();

//			if (internetStatus == NetworkStatus.NotReachable) {
//				Console.WriteLine ("Network not available loading from local plist");
//				posts = drm.GetToiletList ("Update.plist", 0, 10, false);
//
//				//////
//				/// If local file is not updated yet and not available to update, fill dummy data
//				if (posts.Count < 1) {
//					//////
//					/// setup spot_id 0 as error row
//					posts.Add (new ToiletsBase (0, 1, "Connection error", "Please connect to Internet ...", "", 0, 0, 0, true));
//				}
//			} else {
				Console.WriteLine ("Network available");
				posts = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php", 0, 10, false);
			//}

			// Perform any additional setup after loading the view, typically from a nib.
			UITableView tbl = new UITableView (this.NavigationController.View.Bounds);

			tbl.Bounces = false; ////// disable bounce to prevent multiple actions when reached to tableview bottom, see Scrolled()
			tbl.Source = new TableSource (posts, this.NavigationController.View);
			//tbl.RowHeight = 120f;
			tbl.RowHeight = UITableView.AutomaticDimension;
			tbl.EstimatedRowHeight = 160.0f;

			View.AddSubview (tbl);

		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

		}

		void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
		}

	}

	public class TableSource : UITableViewSource
	{
		List<ToiletsBase> datas;
		DataRequestManager drm;
		UITableView tableViews;
		UIView parentView;
		UIButton loadMoreButton;
		float pHeight;
		float pWidth;
		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		bool hasConnection;

		public TableSource (List<ToiletsBase> data, UIView parentView)
		{
			hasConnection = true;
			//////
			/// check connectivity
			UpdateStatus ();

			if (internetStatus == NetworkStatus.NotReachable) {
				hasConnection = false;
			}

			this.datas = data;
			this.parentView = parentView;

			pHeight = (float)this.parentView.Frame.Height;
			pWidth = (float)this.parentView.Frame.Width;

			UIImage coloredImage = UtilImage.GetColoredImage ("images/icons/icon-reload", UIColor.White);
			loadMoreButton = UtilImage.RoundButton (coloredImage, new RectangleF (pWidth - 48, pHeight, 32, 32), UIColor.Black, false);
			parentView.AddSubview (loadMoreButton);

			loadMoreButton.TouchUpInside += (object sender, EventArgs e) => loadMore (sender, e, hasConnection);
		}

		/// <summary>
		/// Loads more data from online.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		public void loadMore (object sender, EventArgs e, bool hasConnection)
		{
			int noOfRowsInSection = (int)tableViews.NumberOfRowsInSection (0);
			List<NSIndexPath> indexPathSet = new List<NSIndexPath> ();
			int loadRows = 5;

			List<ToiletsBase> loadData = new List<ToiletsBase> ();

			//////
			/// load from plist or from server
			if (hasConnection) {
				
				loadData = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php", noOfRowsInSection, 5, false);
			} else {
				Console.WriteLine ("Connection not available, loading from local list...");
				loadData = drm.GetToiletList ("Update.plist", noOfRowsInSection, 5, false);
			}

			Console.WriteLine (loadData.Count);

			datas.AddRange (loadData);

			if (loadData.Count < 5) {
				loadRows = loadData.Count;
			}

			for (var i = noOfRowsInSection - 1; i < (noOfRowsInSection - 1) + loadRows; i++) {
				indexPathSet.Add (NSIndexPath.FromRowSection (i, 0));
			}

			tableViews.BeginUpdates ();
			tableViews.InsertRows (indexPathSet.ToArray (), UITableViewRowAnimation.None);
			tableViews.EndUpdates ();
		}

		/// <summary>
		/// Rowses the in section.
		/// </summary>
		/// <returns>The in section.</returns>
		/// <param name="tableview">Tableview.</param>
		/// <param name="section">Section.</param>
		public override nint RowsInSection (UITableView tableview, nint section)
		{
			this.tableViews = tableview;
			return datas.Count;
		}

		/// <summary>
		/// Numbers the of sections.
		/// </summary>
		/// <returns>The of sections.</returns>
		/// <param name="tableView">Table view.</param>
		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		/// <summary>
		/// Gets the cell.
		/// </summary>
		/// <returns>The cell.</returns>
		/// <param name="tableView">Table view.</param>
		/// <param name="indexPath">Index path.</param>
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			string errorCellId = "errorCell";

			drm = new DataRequestManager ();

			UITableViewCell cell = null;

			int row = indexPath.Row;
			int count = datas.Count;
			//string postCellId = "postCell" + indexPath.Row.ToString ();



			/// error row
			if (datas [0].spot_id == 0) {
				
				cell = tableView.DequeueReusableCell (errorCellId);
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, errorCellId);
				cell.TextLabel.Text = "Connection error";
				cell.DetailTextLabel.Text = "Please connect to the internet!";
				tableView.RowHeight = 40f;

				loadMoreButton.RemoveFromSuperview ();

			} else {
				cell = (TimelineCellDesign)tableView.DequeueReusableCell (TimelineCellDesign.Key);


				if (cell == null) {
					//Console.WriteLine("true");
					cell = TimelineCellDesign.Create ();
					((TimelineCellDesign)cell).Model = datas [indexPath.Row];

					((TimelineCellDesign)cell).likeButton.TouchUpInside += (object sender, EventArgs e) => {

						int vote_cnt = datas [indexPath.Row].vote_cnt;
						vote_cnt++;
						datas [indexPath.Row].vote_cnt = vote_cnt;
						drm.RegisterVote (datas [indexPath.Row].spot_id);

						((TimelineCellDesign)cell).Model = datas [indexPath.Row];
					};
					((TimelineCellDesign)cell).shareButton.TouchUpInside += (object sender, EventArgs e) => ShareClick (sender, e, datas [indexPath.Row]);

				}

			}

			return cell;

		}

		public void ShareClick (object sender, EventArgs e, ToiletsBase info)
		{
			Console.WriteLine (info.distance);
			ShareLinkContent slc = new ShareLinkContent ();
			slc.SetContentUrl (new NSUrl ("https://www.google.com/maps/@" +info.latitude+ "," + info.longitude + ",15z"));
			slc.ContentTitle = info.title;
			ShareDialog.Show (new TimelineViewController (), slc, null);


		}




		/// <summary>
		/// Scrolled the specified scrollView.
		/// </summary>
		/// <param name="scrollView">Scroll view.</param>
		public override void Scrolled (UIScrollView scrollView)
		{
			if (this.tableViews.ContentOffset.Y >= (tableViews.ContentSize.Height - tableViews.Frame.Size.Height)) {
				UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseOut, () => {
					loadMoreButton.Frame = new CGRect (pWidth - 48, pHeight - 48, 32, 32);
				}, null);
			} else {
				UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn, () => {
					loadMoreButton.Frame = new CGRect (pWidth - 48, pHeight, 32, 32);
				}, null);
			}
		}

		void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
		}
	}
}