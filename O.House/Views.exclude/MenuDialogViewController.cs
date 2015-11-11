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

/// <summary>
/// Menu dialog view controller.
/// </summary>
namespace OHouse
{
	/// <summary>
	/// Menu dialog view controller.
	/// </summary>
	public partial class MenuDialogViewController : DialogViewController
	{
		Font font = new Font();
		/// <summary>
		/// Initializes a new instance of the <see cref="OHouse.MenuDialogViewController"/> class.
		/// </summary>
		public MenuDialogViewController () : base (UITableViewStyle.Plain, new RootElement (""))
		{
			FBContainerView fbContainer = new FBContainerView (new RectangleF (0, 0, (float)View.Frame.Width, 120));
			//Section section
			UIImage iconFind = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-find"), 16, 16);
			UIImage iconCdt = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-credit"), 16, 16);
			UIImage iconMan = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-manual"), 16, 16);

			Root = new RootElement ("Menu") {
				new Section(fbContainer) {
					new CustomElement("Find", () => NavigationController.PushViewController(new MapViewController(), true)) {
						Image = iconFind,
						TextColor = font.White,
						SubTitle = "Find nearest toilets within 500 m from your location!"
					},
					new CustomElement("Guide", () => NavigationController.PushViewController(new NearestDialogViewController(), true)) {
						Image = iconMan,
						TextColor = font.White,
						SubTitle = "Lost? Go back to presentation slide to re-read the user manual!"
					},
					new CustomElement("Credit", () => NavigationController.PushViewController(new NearestDialogViewController(), true)) {
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
			//View.AddSubview(fixedBackground);
			//View.BackgroundColor = font.Clear;
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.BackgroundColor = UIColor.Clear;
			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-5"));
		}
	}
}
