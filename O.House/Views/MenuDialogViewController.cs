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
			View.BackgroundColor = UIColor.Clear;
			FBContainerView fbContainer = new FBContainerView (new RectangleF (0, 0, (float)View.Frame.Width, 100));

			UIImage iconFind = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-find"), 16, 16);
			UIImage iconCdt = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-credit"), 16, 16);
			UIImage iconMan = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-manual"), 16, 16);

			Root = new RootElement ("Menu") {
				new Section () {
					new UIViewElement ("", fbContainer, true),
					new StyledStringElement ("Find", () => NavigationController.PushViewController (new MapViewController (), true)) {
						Image = iconFind,
						BackgroundColor = UIColor.Clear,
						TextColor = UIColor.White,
					},
					new StyledStringElement ("Guide") {
						Image = iconMan,
						BackgroundColor = UIColor.Clear,
						TextColor = UIColor.White
					},
					new StyledStringElement ("Credit") {
						Image = iconCdt,
						BackgroundColor = UIColor.Clear,
						TextColor = UIColor.White
					}
				}
			};
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-2"));
		}
	}
}
