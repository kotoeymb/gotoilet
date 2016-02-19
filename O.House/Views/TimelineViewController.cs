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

using Facebook.ShareKit;
using OHouse.Connectivity;
using O.House;

namespace OHouse
{
	public partial class TimelineViewController : UIViewController
	{
		Feed feed;
		DataRequestManager drm;

		List<ToiletsBase> newPost;
		List<ToiletsBase> chunk;

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

			if (internetStatus == NetworkStatus.NotReachable) {
				Console.WriteLine ("Network not available");
				newPost = drm.GetToiletList ("database/Update");

				//////
				/// If local file is not updated yet and not available to update, fill dummy data
				if (newPost.Count < 1) {
					//////
					/// setup spot_id 0 as error row
					newPost.Add (new ToiletsBase (0, 1, "Connection error", "Please connect to Internet ...", "", 0, 0, 0, true));
				}
			} else {
				Console.WriteLine ("Connection available");
				newPost = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php");
			}

			chunk = new List<ToiletsBase> ();
//			if (newPost.Count < 10) {
//				for (var i = 0; i < newPost.Count; i++) {
//					chunk.Add (newPost [i]);
//				}
//			} else {
				for (var i = 0; i < 10; i++) {
					chunk.Add (newPost [i]);
				}
//			}

			// Perform any additional setup after loading the view, typically from a nib.
			UITableView tbl = new UITableView (this.NavigationController.View.Bounds);

			tbl.Bounces = false; ////// disable bounce to prevent multiple actions when reached to tableview bottom, see Scrolled()
			tbl.Source = new TableSource (chunk, newPost);
			//tbl.RowHeight = 120f;
			tbl.RowHeight = UITableView.AutomaticDimension;
			tbl.EstimatedRowHeight = 160.0f;

			View = tbl;
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
		List<ToiletsBase> posts;
		List<ToiletsBase> dataToLoad;
		DataRequestManager drm;
		UITableView tableViews;

		public TableSource (List<ToiletsBase> chunks, List<ToiletsBase> data)
		{
			posts = chunks;
			dataToLoad = data;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			this.tableViews = tableview;
			return posts.Count;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			string errorCellId = "errorCell";

			drm = new DataRequestManager ();

			UITableViewCell cell = null;

			int row = indexPath.Row;
			int count = posts.Count;

			//////
			/// error row
			if (posts [0].spot_id == 0) {

				cell = tableView.DequeueReusableCell (errorCellId);
				cell = new UITableViewCell (UITableViewCellStyle.Subtitle, errorCellId);
				cell.TextLabel.Text = "Connection error";
				cell.DetailTextLabel.Text = "Please connect to the internet!";
				tableView.RowHeight = 40f;

			} else {
//				cell = (TimelineCell)tableView.DequeueReusableCell (postCellId);
//				
//
//				if (cell == null) {
//					cell = new TimelineCell ((NSString)postCellId);
//					
//
//					//////
//					/// like
//					((TimelineCell)cell).LikeBtn.TouchUpInside += (object sender, EventArgs e) => {
//						
//						int vote_cnt = posts [indexPath.Row].vote_cnt;
//						vote_cnt++;
//						posts [indexPath.Row].vote_cnt = vote_cnt;
//						drm.RegisterVote (posts [indexPath.Row].spot_id);
//
//						((TimelineCell)cell).UpdateCell (posts [indexPath.Row]);
//					};
//				}
//
//				((TimelineCell)cell).UpdateCell (posts [indexPath.Row]);
				cell = (TimelineCellDesign)tableView.DequeueReusableCell(TimelineCellDesign.Key);

				if (cell == null) {

					cell = TimelineCellDesign.Create();
				}

				((TimelineCellDesign)cell).Model = posts [indexPath.Row];
			}

			return cell;
		}

		public override void Scrolled (UIScrollView scrollView)
		{
			var numOfRows = (int)this.tableViews.NumberOfRowsInSection (0);
			List<NSIndexPath> indexPathSet = new List<NSIndexPath> ();
			int rowsToLoad = 5;

			//////
			/// end of tableview, last row
			if (this.tableViews.ContentOffset.Y >= (tableViews.ContentSize.Height - tableViews.Frame.Size.Height)) {
				Console.WriteLine ("End of TableView");
				//////
				/// spot_id 0 mean error row
				if (posts [0].spot_id == 0) {
					Console.WriteLine ("No connection or No rows");
				} else {
					//////
					/// if rows are available
					for (var i = numOfRows - 1; i <= (numOfRows - 1) + rowsToLoad; i++) {

						if (i <= dataToLoad.Count) {

							posts.Add (dataToLoad [i - 1]);
							indexPathSet.Add (NSIndexPath.FromRowSection (i, 0));
						}
					}

					//////
					/// Add rows after 2 secs delay
//					NSTimer.CreateScheduledTimer (new TimeSpan (0, 0, 0, 0), delegate {
//						this.tableViews.BeginUpdates ();
//						this.tableViews.InsertRows (indexPathSet.ToArray (), UITableViewRowAnimation.Fade);
//						this.tableViews.EndUpdates ();
//					});

					this.tableViews.BeginUpdates ();
					this.tableViews.InsertRows (indexPathSet.ToArray (), UITableViewRowAnimation.Fade);
					this.tableViews.EndUpdates ();
				}


			}
		}
	}

	public class TimelineCell : UITableViewCell
	{

		private UIButton ShareBtn;
		private UIButton Title;
		private UILabel Info;

		private UILabel Count;

		public UIButton LikeBtn;

		private UIView Border;

		private UIView customColorView = new UIView ();

		private ShareButton shareButton;

		private ToiletsBase toiletBase;
		private ShareLinkContent slc;

		public TimelineCell (NSString cellId) : base (UITableViewCellStyle.Default, cellId)
		{
			BackgroundColor = UIColor.White;

			customColorView.BackgroundColor = UIColor.White;
			SelectedBackgroundView = customColorView;

			shareButton = new ShareButton (new CGRect (0, 0, 34, 34)) {
				BackgroundColor = Common.ColorStyle_1
			};

			Title = new UIButton () {
				Font = Common.Font16F,
				HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
				TintColor = Common.ColorStyle_1,
				BackgroundColor = UIColor.Clear
			};


			Info = new UILabel () {
				Font = Common.Font13F,
				TextColor = Common.Blackish,
				TextAlignment = UITextAlignment.Left,
				Lines = 0,
				AdjustsFontSizeToFitWidth = false,
				BackgroundColor = UIColor.Clear

			};

			Count = new UILabel () {
				Font = Common.Font13F,
				TextColor = Common.Blackish,
				TextAlignment = UITextAlignment.Center,
				BackgroundColor = UIColor.Clear
			};

			ShareBtn = UtilImage.RoundButton (
				UtilImage.ResizeImageKeepAspect (
					UtilImage.GetColoredImage (
						"images/icons/icon-share", 
						Common.White
					), 
					24, 
					24
				), 
				new RectangleF (0, 0, 35, 35), 
				Common.ColorStyle_1, 
				true
			);

			ShareBtn.BackgroundColor = Common.ColorStyle_1;
			LikeBtn = UtilImage.RoundButton (
				UtilImage.ResizeImageKeepAspect (
					UtilImage.GetColoredImage (
						"images/icons/icon-heart", 
						Common.White), 
					24, 
					24
				),
				new RectangleF (0, 0, 35, 35),
				Common.ColorStyle_1,
				false
			);

			Border = new UIView () {
				BackgroundColor = UIColor.FromRGB (238, 238, 238)
			};
					
			UpdateCell (toiletBase);

			ContentView.AddSubviews (Title, Info, Count, LikeBtn, Border, shareButton);
		}

		public void UpdateCell (ToiletsBase toiletBaseInfo)
		{
			if (toiletBaseInfo != null) {
				NSAttributedString As = new NSAttributedString (toiletBaseInfo.title);
				Title.SetAttributedTitle (As, UIControlState.Normal);
				Info.Text = toiletBaseInfo.sub_title;
				Count.Text = toiletBaseInfo.vote_cnt.ToString ();

				slc = new ShareLinkContent ();
				slc.SetContentUrl (new NSUrl ("https://www.google.com/maps/@" + toiletBaseInfo.latitude + "," + toiletBaseInfo.longitude + ",15z"));
				slc.ContentTitle = toiletBaseInfo.title;
				shareButton.SetShareContent (slc);
			}
		}

		public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			Border.BackgroundColor = UIColor.FromRGB (238, 238, 238);
			ShareBtn.BackgroundColor = Common.ColorStyle_1;
			LikeBtn.BackgroundColor = Common.ColorStyle_1;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			CGRect full = ContentView.Frame;

			// UIView - container
			Border.Frame = new CGRect (0, 0, full.Width, 2);

			// UILabel title
			Title.Frame = new CGRect (65, 12, full.Width - 65 + 15, 24);

			// UILabe info
			CGSize size = UIStringDrawing.StringSize (Info.Text, Common.Font13F, new CGSize (full.Width, 60), UILineBreakMode.WordWrap);
			Info.Frame = new CGRect (Title.Frame.X, Title.Frame.Y + Title.Frame.Height, Title.Frame.Width, size.Height);

			// UIImage likeBtn
			LikeBtn.Frame = new CGRect (15, full.Height - 15 - 35, 35, 35);

			// UILabel count

			Count.Frame = new CGRect (15, LikeBtn.Frame.Y - 28, 35, 35);

			// UIButton shareBtn
			shareButton.Frame = new CGRect (15, 15, 35, 35);
		}
	}
}