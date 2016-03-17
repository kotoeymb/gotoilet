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
			if (fbLoginButton != null) {
				fbLoginButton.Dispose ();
				fbLoginButton = null;
			}
			if (fbProfilePicture != null) {
				fbProfilePicture.Dispose ();
				fbProfilePicture = null;
			}
			if (menuView != null) {
				menuView.Dispose ();
				menuView = null;
			}
		}
	}
}
