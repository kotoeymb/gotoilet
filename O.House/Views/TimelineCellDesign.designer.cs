// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Facebook.ShareKit;
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace O.House
{
	[Register ("TimelineCellDesign")]
	partial class TimelineCellDesign
	{
		[Outlet]
		UIKit.UIButton cellLikeBtn { get; set; }

		[Outlet]
		UIKit.UIButton cellShareBtn { get; set; }

		[Outlet]
		UIKit.UILabel cellSubtitle { get; set; }

		[Outlet]
		UIKit.UILabel cellTitle { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (cellLikeBtn != null) {
				cellLikeBtn.Dispose ();
				cellLikeBtn = null;
			}
			if (cellShareBtn != null) {
				cellShareBtn.Dispose ();
				cellShareBtn = null;
			}
			if (cellSubtitle != null) {
				cellSubtitle.Dispose ();
				cellSubtitle = null;
			}
			if (cellTitle != null) {
				cellTitle.Dispose ();
				cellTitle = null;
			}
		}
	}
}
