
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

			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-7-nightlife"));

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
		RootElement root;
		DataRequestManager drm;
		List<ToiletsBase> tb;
		Section section;
		/// <summary>
		/// Initializes a new instance of the <see cref="OHouse.TimelineDialogViewController"/> class.
		/// </summary>
		public TimelineDialogViewController () : base (UITableViewStyle.Plain, new RootElement (""))
		{
			drm = new DataRequestManager();
			tb = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php");

			section = new Section ("");
			foreach (var d in tb) {
				Console.WriteLine (d.picture.ToString ());

				section.Add (
					new TimelineElement (() => {
						NavigationController.PushViewController (new SubmitViewController (), true);
					}) {
						Count = d.vote_cnt,
						Header = d.title,
						//Map = UtilImage.FromURL(d.picture),
						Description = d.sub_title
					}
				);
			}

			root = new RootElement ("Timeline") {
				section
			};

			root.UnevenRows = true;
			Root = root;
		}

		public override void LoadView ()
		{
			base.LoadView ();
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 130);
		}
	}
}

