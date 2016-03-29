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
	[Register ("TimelineCellDesign")]
	partial class TimelineCellDesign
	{
		[Outlet]
		UIKit.UIButton cellDetailbtn { get; set; }

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

			if (cellDetailbtn != null) {
				cellDetailbtn.Dispose ();
				cellDetailbtn = null;
			}

			if (cellTitle != null) {
				cellTitle.Dispose ();
				cellTitle = null;
			}
		}
	}
}
