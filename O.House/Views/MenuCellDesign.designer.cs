// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

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
