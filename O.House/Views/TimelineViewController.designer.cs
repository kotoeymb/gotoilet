// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace OHouse
{
	[Register ("TimelineViewController")]
	partial class TimelineViewController
	{
		[Outlet]
		UIKit.UIButton btnLoadMore { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView loader { get; set; }

		[Outlet]
		UIKit.UITableView timelineTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (timelineTable != null) {
				timelineTable.Dispose ();
				timelineTable = null;
			}

			if (loader != null) {
				loader.Dispose ();
				loader = null;
			}

			if (btnLoadMore != null) {
				btnLoadMore.Dispose ();
				btnLoadMore = null;
			}
		}
	}
}
