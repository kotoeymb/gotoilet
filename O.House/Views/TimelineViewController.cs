using System;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

using Foundation;
using UIKit;
using MonoTouch.Dialog;
using Commons;
using CustomElements;
using Utils;
using OHouse.DRM;
using CoreGraphics;
using CoreAnimation;

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
		DataRequestManager drm;
		List<ToiletsBase> posts;
		bool connectivity;

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
			
			base.ViewDidLoad ();

			connectivity = true;

			UpdateStatus (null, null);

			//////
			/// update network status
			ConnectionManager.ReachabilityChanged += UpdateStatus;
		}

		async void initView ()
		{
			try {
				drm = new DataRequestManager ();

				btnLoadMore.Layer.CornerRadius = btnLoadMore.Frame.Width/2;
				btnLoadMore.SetTitle("", UIControlState.Normal);
				btnLoadMore.SetBackgroundImage (UtilImage.GetColoredImage("images/icons/icon-reload", UIColor.White), UIControlState.Normal);
				btnLoadMore.BackgroundColor = UIColor.Black;
				btnLoadMore.TouchUpInside += (object sender, EventArgs e) => loadMore (sender, e, connectivity);
				btnLoadMore.ClipsToBounds = true;

				if (!connectivity) {
					Console.WriteLine ("Network not available loading from local plist");
					posts = drm.GetToiletList ("Update.plist", 0, 10, false);

					//////
					/// If local file is not updated yet and not available to update, fill dummy data
					if (posts.Count < 1) {
						//////
						/// setup spot_id 0 as error row
						posts.Add (new ToiletsBase (0, 1, "Connection error", "Please connect to Internet or download the data for offline use.", "", 0, 0, 0, true));
					}

				} else {
					
					Console.WriteLine ("Network available");

					loader.StartAnimating ();
					posts = await downloadStringAsync("http://gstore.pcp.jp/api/get_spots.php");
					loader.StopAnimating ();
				
				}

				// Perform any additional setup after loading the view, typically from a nib.
				timelineTable.Bounces = false;
				timelineTable.DataSource = new TableSource(posts);
				timelineTable.RowHeight = UITableView.AutomaticDimension;
				timelineTable.EstimatedRowHeight = 160.0f;

				var pWidth = this.NavigationController.View.Frame.Width;
				var pHeight = this.NavigationController.View.Frame.Height;

				timelineTable.Scrolled += (object sender, EventArgs e) => {
					if (this.timelineTable.ContentOffset.Y >= (timelineTable.ContentSize.Height - timelineTable.Frame.Size.Height)) {
						UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseOut, () => {
							btnLoadMore.Frame = new CGRect (pWidth - 48, pHeight - 48, 32, 32);
						}, null);
					} else {
						UIView.Animate (0.3, 0, UIViewAnimationOptions.CurveEaseIn, () => {
							btnLoadMore.Frame = new CGRect (pWidth - 48, pHeight, 32, 32);
						}, null);
					}
				};

			} catch (Exception e) {
				Console.WriteLine (e.Message);
			}
		}

		async void loadMore (object sender, EventArgs e, bool hasConnection)
		{
			int noOfRowsInSection = (int)timelineTable.NumberOfRowsInSection (0);
			List<NSIndexPath> indexPathSet = new List<NSIndexPath> ();
			int loadRows = 5;

			btnLoadMore.UserInteractionEnabled = false;
		
			List<ToiletsBase> loadData = new List<ToiletsBase> ();
		
			//////
			/// load from plist or from server
			if (hasConnection) {
				loadData = await loadMoreData (noOfRowsInSection);
				btnLoadMore.UserInteractionEnabled = true;
			} else {
				Console.WriteLine ("Connection not available, loading from local list...");
				loadData = drm.GetToiletList ("Update.plist", noOfRowsInSection, 5, false);
			}
		
			Console.WriteLine (loadData.Count);
		
			posts.AddRange (loadData);
		
			if (loadData.Count < 5) {
				loadRows = loadData.Count;
			}
		
			for (var i = noOfRowsInSection - 1; i < (noOfRowsInSection - 1) + loadRows; i++) {
				indexPathSet.Add (NSIndexPath.FromRowSection (i, 0));
			}
		
			timelineTable.BeginUpdates ();
			timelineTable.InsertRows (indexPathSet.ToArray (), UITableViewRowAnimation.None);
			timelineTable.EndUpdates ();
		}

		async Task<List<ToiletsBase>> loadMoreData (int numOfRows)
		{
			List<ToiletsBase> d;
		
			try {
				var httpClient = new HttpClient ();
				Task<string> contentsTask = httpClient.GetStringAsync ("http://gstore.pcp.jp/api/get_spots.php");
		
				//////
				/// Waiting for data to load fully
				string contents = await contentsTask;
		
				//////
				/// After fully loaded
				d = drm.GetDataListJSON (contents, numOfRows, 5, false);
		
				return d;
		
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				return null;
			}
		
		}

		public async Task<List<ToiletsBase>> downloadStringAsync (string urlToDownload)
		{
			List<ToiletsBase> data;

			try {
				var httpClient = new HttpClient ();

				Task<string> contentsTask = httpClient.GetStringAsync (urlToDownload);

				////// Show loading view
				/// start animating it
				/// contents are loading

				string contents = await contentsTask;

				////// Remove loading view
				/// datas are loaded
				data = drm.GetDataListJSON (contents, 0, 10, false);

				return data;

			} catch (Exception e) {
				Console.WriteLine ("Error : " + e.Message);
				return null;
			}
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

		}

		void UpdateConnectivity ()
		{
			switch (remoteHostStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;
				break;
			}

			switch (internetStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;
				break;
			}

			switch (localWifiStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;
				break;
			}

			////// 
			/// Reload view
			initView ();
		}

		void UpdateStatus (object sender, EventArgs e)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
			//////
			/// Update internet connectivity status
			UpdateConnectivity ();
		}
	}

	/// <summary>
	/// Table source.
	/// </summary>
	public class TableSource : UITableViewDataSource
	{
		List<ToiletsBase> datas;
		DataRequestManager drm;

		public TableSource (List<ToiletsBase> data)
		{
			this.datas = data;
		}

		/// <summary>
		/// Rowses the in section.
		/// </summary>
		/// <returns>The in section.</returns>
		/// <param name="tableview">Tableview.</param>
		/// <param name="section">Section.</param>
		public override nint RowsInSection (UITableView tableview, nint section)
		{
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

		void animateThis(UIButton sender) {
			UIView.Animate (0.1, () => {
				sender.Transform = CGAffineTransform.MakeScale(1.3f, 1.3f);
				sender.TintColor = UIColor.Blue;
			}, ()=> {
				UIView.Animate(0.1, () => {
					sender.Transform = CGAffineTransform.MakeScale(1f,1f);
				});
			});
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

			/// error row
			if (datas [0].spot_id == 0) {
				
				cell = tableView.DequeueReusableCell (errorCellId);
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, errorCellId);
				cell.TextLabel.Text = "Connection error";
				cell.DetailTextLabel.Text = "Please connect to the internet!";
				tableView.RowHeight = 40f;
			

			} else {
				cell = (TimelineCellDesign)tableView.DequeueReusableCell (TimelineCellDesign.Key);


				if (cell == null) {
					//Console.WriteLine("true");
					cell = TimelineCellDesign.Create ();
					((TimelineCellDesign)cell).Model = datas [indexPath.Row];

					((TimelineCellDesign)cell).likeButton.TouchUpInside += (object sender, EventArgs e) => {

						UIButton btn = sender as UIButton;
						animateThis(btn);

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
			ShareLinkContent slc = new ShareLinkContent ();
			slc.SetContentUrl (new NSUrl ("https://www.google.com/maps/@" + info.latitude + "," + info.longitude + ",15z"));
			slc.ContentTitle = info.title;
			ShareDialog.Show (new TimelineViewController (), slc, null);
		}
	}
}