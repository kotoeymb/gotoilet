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
		
		/// <summary>
		/// Initializes a new instance of the <see cref="GoToilet.MenuDialogViewController"/> class.
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
						TextColor = UIColor.White
					},
					new CustomElement("Guide", () => NavigationController.PushViewController(new NearestDialogViewController(), true)) {
						Image = iconMan,
						TextColor = UIColor.White
					},
					new CustomElement("Credit", () => NavigationController.PushViewController(new NearestDialogViewController(), true)) {
						Image = iconCdt,
						TextColor = UIColor.White
					}
				}
			};
		}

		public override void LoadView ()
		{
			base.LoadView ();
			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-5"));
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			//TableView.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-5"));
		}
	}
}
