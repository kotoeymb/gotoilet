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
	[Register ("MenuSBViewController")]
	partial class MenuSBViewController
	{
		[Outlet]
		UIKit.UIButton fbLoginButton { get; set; }

		[Outlet]
		UIKit.UILabel fbProfileName { get; set; }

		[Outlet]
		UIKit.UIView fbProfilePicture { get; set; }

		[Outlet]
		UIKit.UITableView menuView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (fbProfilePicture != null) {
				fbProfilePicture.Dispose ();
				fbProfilePicture = null;
			}

			if (fbProfileName != null) {
				fbProfileName.Dispose ();
				fbProfileName = null;
			}

			if (fbLoginButton != null) {
				fbLoginButton.Dispose ();
				fbLoginButton = null;
			}

			if (menuView != null) {
				menuView.Dispose ();
				menuView = null;
			}
		}
	}
}
