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
	[Register ("OfflineViewController")]
	partial class OfflineViewController
	{
		[Outlet]
		UIKit.UIButton closeBtn { get; set; }

		[Outlet]
		UIKit.UIButton downloadBtn { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView loader { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (closeBtn != null) {
				closeBtn.Dispose ();
				closeBtn = null;
			}
			if (downloadBtn != null) {
				downloadBtn.Dispose ();
				downloadBtn = null;
			}
			if (loader != null) {
				loader.Dispose ();
				loader = null;
			}
		}
	}
}
