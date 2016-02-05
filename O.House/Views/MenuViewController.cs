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
		/// <summary>
		/// Initializes a new instance of the <see cref="OHouse.MenuDialogViewController"/> class.
		/// </summary>
		public MenuDialogViewController () : base (UITableViewStyle.Plain, new RootElement (""))
		{
			FBContainerView fbContainer = new FBContainerView (new RectangleF (0, 0, (float)View.Frame.Width, 200));
			UIImage iconFind = UtilImage.GetColoredImage ("images/icons/icon-find", UIColor.FromRGB (0, 235, 255));
			UIImage iconTL = UIImage.FromBundle ("images/icons/icon-timeline");
			//UIImage iconMan = UIImage.FromBundle ("images/icons/icon-manual");

			Root = new RootElement ("Menu") {
				new Section (fbContainer) {
					new CustomElement ("Timeline", () => NavigationController.PushViewController (new TimelineViewController (), true)) {
						Image = iconTL,
						SubTitle = "View timeline to check what's is up! Like to approve the location !"
					},
					new CustomElement ("Find", () => NavigationController.PushViewController (new MapViewController (), true)) {
						Image = iconFind,
						SubTitle = "Find nearest toilets within 500 m from your location!"
					},
//					new CustomElement ("Guide", () => NavigationController.PushViewController (new TestListVC (), true)) {
//						Image = iconMan,
//						SubTitle = "Lost? Go back to presentation slide to re-read the user manual!"
//					}
//					new CustomElement ("Credit", () => NavigationController.PushViewController (new ScrollViewController (), true)) {
//						Image = iconCdt,
//						SubTitle = "Development teams and their members. Thank you!"
//					}
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

