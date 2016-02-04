using System;
using System.Collections.Generic;
using System.Drawing;
using Foundation;
using OHouse.DRM;
using Utils;
using Facebook.ShareKit;
using Commons;
using CoreGraphics;

using UIKit;

namespace OHouse
{
	public partial class TestListVC : UIViewController
	{
		Feed feed;
		DataRequestManager drm;

		public TestListVC () : base ("TestListVC", null)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			feed = new Feed ();
			drm = new DataRequestManager ();
			//posts = new List<Post> ();

			//NSMutableArray newPost = feed.GetFeeds ();
			List<ToiletsBase> newPost = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php");
			List<ToiletsBase> chunk = new List<ToiletsBase> ();

			for (var i = 0; i < 5; i++) {
				chunk.Add (newPost [i]);
			}

			// Perform any additional setup after loading the view, typically from a nib.
			UITableView tbl = new UITableView (this.NavigationController.View.Bounds);

//			tbl.DataSource = new TableSource (chunk, newPost);
			tbl.Source = new TableSource (chunk, newPost);
//			tbl.RowHeight = 120f;
//			tbl.EstimatedRowHeight = 80f;
//			tbl.ReloadData ();

			View = tbl;
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		/// <summary>
		/// Return 5 posts
		/// </summary>
		/// <returns>The posts.</returns>
	}

	public class TableSource : UITableViewSource
	{
		List<ToiletsBase> posts;
		List<ToiletsBase> dataToLoad;
		DataRequestManager drm;

		public TableSource (List<ToiletsBase> items, List<ToiletsBase> data)
		{
			posts = items;
			dataToLoad = data;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return posts.Count + 1;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			string postCellId = "postCell";
			string moreCellId = "moreCell";

			UITableViewCell cell = null;

			int row = indexPath.Row;
			int count = posts.Count;

			int totalRows = (int)tableView.NumberOfRowsInSection (0);
//			Console.WriteLine (totalRows); ///logged
//			Console.WriteLine(row + ":" + totalRows); ///logged

			if (row == totalRows - 1) {
				//cell = tableView.DequeueReusableCell (moreCellId) as TimelineCell;
				cell = tableView.DequeueReusableCell (moreCellId);
				if (cell == null) {
					cell = new UITableViewCell (UITableViewCellStyle.Default, moreCellId);
					//cell = new TimelineCell(moreCellId);
					//cell.UpdateCell ();
				}

				if (row == dataToLoad.Count - 1) {
					cell.TextLabel.Text = "Nothing more to load ...";
				} else {
					cell.TextLabel.Text = "Load more items ...";

				}

				tableView.RowHeight = 40f;
			} else {
				//cell = tableView.DequeueReusableCell (postCellId) as TimelineCell;
				cell = (TimelineCell)tableView.DequeueReusableCell (postCellId);

				if (cell == null) {
//					cell = new UITableViewCell (UITableViewCellStyle.Default, postCellId);

					cell = new TimelineCell((NSString)postCellId);
					((TimelineCell)cell).UpdateCell (posts [indexPath.Row]);
				}

//				ToiletsBase currentPost = posts [indexPath.Row];
//				cell.TextLabel.Text = currentPost.title;
				tableView.RowHeight = 120f;
			}

			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			int row = indexPath.Row;
			int count = (int)posts.Count;
			int noOfRowsToLoad = (int)tableView.NumberOfRowsInSection (0);
			int rowsToLoad = 5;

			List<NSIndexPath> indexPathSet = new List<NSIndexPath> ();
			Console.WriteLine ("Current row : " + row);

			if (row == noOfRowsToLoad - 1) {

				posts.RemoveAt (indexPath.Row - 1);

				if (row + rowsToLoad > dataToLoad.Count) {
					rowsToLoad = (row + rowsToLoad) - dataToLoad.Count - 1;
				}

				Console.WriteLine ("Row and dataToLoad.count : " + row + ", " + dataToLoad.Count);

				if (row == dataToLoad.Count - 1) {
					rowsToLoad = 0;
				}

				for (var i = row; i < row + rowsToLoad; i++) {
					indexPathSet.Add (NSIndexPath.FromRowSection (i, 0));
					posts.Add (dataToLoad [i]);
				}

				tableView.BeginUpdates ();
				tableView.DeleteRows (new NSIndexPath[] { NSIndexPath.FromRowSection (indexPath.Row, 0) }, UITableViewRowAnimation.Fade);
				tableView.InsertRows (indexPathSet.ToArray (), UITableViewRowAnimation.Fade);
				tableView.EndUpdates ();
			}
		}

	
	}

	public class TimelineCell : UITableViewCell
	{
		private UIButton ShareBtn;
		private UIButton Title;
		private UILabel Info;

		private UILabel Count;

		private UIButton LikeBtn;

		private UIView Border;

		private UIView customColorView = new UIView ();

		private ShareButton shareButton;

		private ToiletsBase toiletBase;
		private ShareLinkContent slc;

		Common common = new Common ();

		public TimelineCell (NSString cellId) : base (UITableViewCellStyle.Default, cellId)
		{
			BackgroundColor = UIColor.White;

			customColorView.BackgroundColor = UIColor.White;
			SelectedBackgroundView = customColorView;

			shareButton = new ShareButton (new CGRect (0, 0, 34, 34)) {
				BackgroundColor = common.ColorStyle_1
			};
					
			Title = new UIButton () {
				Font = common.Font16F,
				HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
				TintColor = common.ColorStyle_1,
				BackgroundColor = UIColor.Clear
			};


			Info = new UILabel () {
				Font = common.Font13F,
				TextColor = common.Blackish,
				TextAlignment = UITextAlignment.Left,
				Lines = 0,
				AdjustsFontSizeToFitWidth = false,
				BackgroundColor = UIColor.Clear

			};

			Count = new UILabel () {
				Font = common.Font13F,
				TextColor = common.Blackish,
				TextAlignment = UITextAlignment.Center,
				BackgroundColor = UIColor.Clear
			};

			ShareBtn = UtilImage.RoundButton (
				UtilImage.ResizeImageKeepAspect (
					UtilImage.GetColoredImage (
						"images/icons/icon-share", 
						common.White
					), 
					24, 
					24
				), 
				new RectangleF (0, 0, 35, 35), 
				common.ColorStyle_1, 
				true
			);

			ShareBtn.BackgroundColor = common.ColorStyle_1;
//			ShareBtn.TouchUpInside += (s, e) => {
//				UIAlertView alert = new UIAlertView (title, count.ToString (), null, "ok");
//				alert.Show ();
//			};

			//LikeBtn.SetImage (UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-heart"), 24, 24), UIControlState.Normal);
			LikeBtn = UtilImage.RoundButton (
				UtilImage.ResizeImageKeepAspect (
					UtilImage.GetColoredImage (
						"images/icons/icon-heart", 
						common.White), 
					24, 
					24
				),
				new RectangleF (0, 0, 35, 35),
				common.ColorStyle_1,
				false
			);

			//UpdateCell (mapView, title, info, count);

			Border = new UIView () {
				BackgroundColor = UIColor.FromRGB (238, 238, 238)
			};

			UpdateCell (toiletBase);

			ContentView.AddSubviews (Title, Info, Count, LikeBtn, Border, shareButton);
		}

		//		public void UpdateCell (UIImage mapview, string title, string info, int count)
		public void UpdateCell (ToiletsBase toiletBaseInfo)
		{
			if (toiletBaseInfo != null) {

				NSAttributedString As = new NSAttributedString (toiletBaseInfo.title);
				Title.SetAttributedTitle (As, UIControlState.Normal);
				Info.Text = toiletBaseInfo.sub_title;
				Count.Text = toiletBaseInfo.vote_cnt.ToString ();

				slc = new ShareLinkContent ();
				slc.SetContentUrl (new NSUrl ("https://www.google.com/maps/@"+toiletBaseInfo.latitude+","+toiletBaseInfo.longitude+",15z"));
				slc.ContentTitle = toiletBaseInfo.title;
				shareButton.SetShareContent (slc);
			}
		}

		public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			Border.BackgroundColor = UIColor.FromRGB (238, 238, 238);
			ShareBtn.BackgroundColor = common.ColorStyle_1;
			LikeBtn.BackgroundColor = common.ColorStyle_1;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			//CGRect full = ContentView.Bounds;
			CGRect full = ContentView.Frame;

			// UIView - container
			Border.Frame = new CGRect (0, 0, full.Width, 2);

			// UILabel title
			Title.Frame = new CGRect (65, 12, full.Width - 65 + 15, 24);

			// UILabe info
			CGSize size = UIStringDrawing.StringSize (Info.Text, common.Font13F, new CGSize (full.Width, 60), UILineBreakMode.WordWrap);
			Info.Frame = new CGRect (Title.Frame.X, Title.Frame.Y + Title.Frame.Height, Title.Frame.Width, size.Height);

			// UIImage likeBtn
			LikeBtn.Frame = new CGRect (15, full.Height - 15 - 35, 35, 35);

			// UILabel count

			Count.Frame = new CGRect (15, LikeBtn.Frame.Y - 28, 35, 35);

			// UIButton shareBtn
			//ShareBtn.Frame = new CGRect (15, 15, 35, 35);
			shareButton.Frame = new CGRect (15, 15, 35, 35);
		}
	}
}