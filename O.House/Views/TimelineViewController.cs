
using System;
using System.Collections.Generic;

using Foundation;
using UIKit;
using MonoTouch.Dialog;
using Commons;
using CustomElements;
using Utils;
using OHouse.DRM;

namespace OHouse
{
	public partial class TimelineViewController : UIViewController
	{
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

			TimelineDialogViewController tdvc = new TimelineDialogViewController ();

			//View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-7-nightlife"));

			this.AddChildViewController (tdvc);
			this.View.AddSubview (tdvc.View);
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}

	/// <summary>
	/// Menu dialog view controller.
	/// </summary>
	public partial class TimelineDialogViewController : DialogViewController
	{
		DataRequestManager drm;
		List<ToiletsBase> tb;
		Section section;

		bool dataLoaded;

		/// <summary>
		/// Initializes a new instance of the <see cref="OHouse.TimelineDialogViewController"/> class.
		/// </summary>
		public TimelineDialogViewController () : base (UITableViewStyle.Plain, new RootElement (""))
		{
//			drm = new DataRequestManager ();
//			tb = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php");
//
//			startIndex = 2;
//			endIndex = 4;
//
//			//Console.WriteLine (tb.Count);
//
//			section = new Section ("");
//
//			for (var i = startIndex - 1; i < tb.Count; i++) {
//				Console.WriteLine (tb [i].spot_id);
//			}
//
//			foreach (var d in tb) {
//				section.Add (
//					new TimelineElement (() => {
//						NavigationController.PushViewController (new SubmitViewController (), true);
//					}) {
//						Count = d.vote_cnt,
//						Header = d.title,
//						Description = d.sub_title
//					}
//				);
//			}
//
//			Root = new RootElement ("Timeline") {
//				section
//			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			dataLoaded = false;
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			var limit = 0;

			if (!dataLoaded) {

				drm = new DataRequestManager ();
				tb = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php");

				section = new Section ("");

				foreach (var d in tb) {

//					if (limit >= 4) {
//						break;
//					}

					limit++;

					section.Add (
						new TimelineElement (() => {
							NavigationController.PushViewController(new DetailViewController(d.spot_id), true);
						}) {
							Count = d.vote_cnt,
							Header = d.title,
							Description = d.sub_title
						}
					);
				}

				Root = new RootElement ("Timeline") {
					section
				};
				dataLoaded = true;
			}

		}

		public override void LoadView ()
		{
			base.LoadView ();
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.BackgroundColor = UIColor.FromRGB (238, 238, 238);
		}
	}
}

