using System;

using Foundation;
using UIKit;

namespace O.House
{
	public partial class MenuCellDesign : UITableViewCell
	{
		public static readonly NSString Key = new NSString ("MenuCellDesign");
		public static readonly UINib Nib;

		public UILabel MenuLabel {
			get {
				return menuLabel;
			}
			set{ ; }
		}

		public UIImageView MenuIcon {
			get {
				return menuIcon;
			}
			set { ; }
		}

		static MenuCellDesign ()
		{
			Nib = UINib.FromName ("MenuCellDesign", NSBundle.MainBundle);
		}

		public MenuCellDesign (IntPtr handle) : base (handle)
		{
		}

		public static MenuCellDesign Create() {
			return (MenuCellDesign)Nib.Instantiate (null, null) [0];
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			BackgroundColor = UIColor.Clear;

			menuLabel.TextColor = UIColor.White;
		}
	}
}
