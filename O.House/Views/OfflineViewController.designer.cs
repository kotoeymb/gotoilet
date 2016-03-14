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
	[Register ("OfflineViewController")]
	partial class OfflineViewController
	{
		[Outlet]
		UIKit.UIButton closeBtn { get; set; }

		[Outlet]
		UIKit.UIButton downloadBtn { get; set; }
		
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
		}
	}
}
