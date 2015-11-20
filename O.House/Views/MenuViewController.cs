using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

using MonoTouch.Dialog;
using MonoTouch.SlideoutNavigation;

using Foundation;
using UIKit;
using Utils;
using CoreGraphics;

using CustomElements;
using Commons;

namespace OHouse
{
	public partial class MenuViewController : UIViewController
	{
		public MenuViewController () : base ("MenuViewController", null)
		{
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

			MenuDialogViewController menu = new MenuDialogViewController ();
			
			// Perform any additional setup after loading the view, typically from a nib.
			//View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-7-nightlife"));
			View.BackgroundColor = UIColor.FromRGB (41, 41, 41);

			this.AddChildViewController (menu);
			this.View.AddSubview (menu.View);
		}
	}

	/// <summary>
	/// Menu dialog view controller.
	/// </summary>
	public partial class MenuDialogViewController : DialogViewController
	{
		Common common = new Common ();

		/// <summary>
		/// Initializes a new instance of the <see cref="OHouse.MenuDialogViewController"/> class.
		/// </summary>
		public MenuDialogViewController () : base (UITableViewStyle.Plain, new RootElement (""))
		{
			FBContainerView fbContainer = new FBContainerView (new RectangleF (0, 0, (float)View.Frame.Width, 200));
			UIImage iconFind = UIImage.FromBundle ("images/icons/icon-find");
			UIImage iconTL = UIImage.FromBundle ("images/icons/icon-timeline");
			UIImage iconCdt = UIImage.FromBundle ("images/icons/icon-credit");
			UIImage iconMan = UIImage.FromBundle ("images/icons/icon-manual");

			Root = new RootElement ("Menu") {
				new Section (fbContainer) {
					new CustomElement ("Timeline", () => NavigationController.PushViewController (new TimelineViewController (), true)) {
						Image = iconTL,
						TextColor = common.White,
						SubTitle = "View timeline to check what's is up! Like to approve the location !"
					},
					new CustomElement ("Find", () => NavigationController.PushViewController (new MapViewController (), true)) {
						Image = iconFind,
						TextColor = common.White,
						SubTitle = "Find nearest toilets within 500 m from your location!"
					},
					new CustomElement ("Guide", () => NavigationController.PushViewController (new NearestDialogViewController (), true)) {
						Image = iconMan,
						TextColor = common.White,
						SubTitle = "Lost? Go back to presentation slide to re-read the user manual!"
					},
					new CustomElement ("Credit", () => NavigationController.PushViewController (new NearestDialogViewController (), true)) {
						Image = iconCdt,
						TextColor = common.White,
						SubTitle = "Development teams and their members. Thank you!"
					}
				}
			};
		}

		public override void LoadView ()
		{
			base.LoadView ();
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			//TableView.BackgroundColor = UIColor.FromRGBA(13,13,13, 200);
			TableView.BackgroundColor = UIColor.Clear;
		}
	}
}

