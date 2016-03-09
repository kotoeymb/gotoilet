// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace O.House
{
	[Register ("MenuCellDesign")]
	partial class MenuCellDesign
	{
		[Outlet]
		UIKit.UIImageView menuIcon { get; set; }

		[Outlet]
		UIKit.UILabel menuLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (menuIcon != null) {
				menuIcon.Dispose ();
				menuIcon = null;
			}

			if (menuLabel != null) {
				menuLabel.Dispose ();
				menuLabel = null;
			}
		}
	}
}
