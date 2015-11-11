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
using Common;

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
			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-5"));

			this.AddChildViewController (menu);
			this.View.AddSubview (menu.View);
		}
	}

	/// <summary>
	/// Menu dialog view controller.
	/// </summary>
	public partial class MenuDialogViewController : DialogViewController
	{
		Font font = new Font ();

		/// <summary>
		/// Initializes a new instance of the <see cref="OHouse.MenuDialogViewController"/> class.
		/// </summary>
		public MenuDialogViewController () : base (UITableViewStyle.Plain, new RootElement (""))
		{
			FBContainerView fbContainer = new FBContainerView (new RectangleF (0, 0, (float)View.Frame.Width, 120));
			UIImage iconFind = UIImage.FromBundle ("images/icons/icon-find");
			UIImage iconCdt = UIImage.FromBundle ("images/icons/icon-credit");
			UIImage iconMan = UIImage.FromBundle ("images/icons/icon-manual");

			Root = new RootElement ("Menu") {
				new Section (fbContainer) {
					new CustomElement ("Find", () => NavigationController.PushViewController (new MapViewController (), true)) {
						Image = iconFind,
						TextColor = font.White,
						SubTitle = "Find nearest toilets within 500 m from your location!"
					},
					new CustomElement ("Guide", () => NavigationController.PushViewController (new NearestDialogViewController (), true)) {
						Image = iconMan,
						TextColor = font.White,
						SubTitle = "Lost? Go back to presentation slide to re-read the user manual!"
					},
					new CustomElement ("Credit", () => NavigationController.PushViewController (new NearestDialogViewController (), true)) {
						Image = iconCdt,
						TextColor = font.White,
						SubTitle = "Development teams and their members. Thank you!"
					}
				}
			};
		}

		public override void LoadView ()
		{
			base.LoadView ();
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.BackgroundColor = UIColor.Clear;
		}
	}
}

