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
			if (btnLoadMore != null) {
				btnLoadMore.Dispose ();
				btnLoadMore = null;
			}
			if (loader != null) {
				loader.Dispose ();
				loader = null;
			}
			if (timelineTable != null) {
				timelineTable.Dispose ();
				timelineTable = null;
			}
		}
	}
}
