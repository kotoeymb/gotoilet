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
	[Register ("AboutViewController")]
	partial class AboutViewController
	{
		[Outlet]
		UIKit.UIButton CloseBtn { get; set; }

		[Outlet]
		MapKit.MKMapView jpMapView { get; set; }

		[Outlet]
		MapKit.MKMapView mmMapView { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (CloseBtn != null) {
				CloseBtn.Dispose ();
				CloseBtn = null;
			}
			if (jpMapView != null) {
				jpMapView.Dispose ();
				jpMapView = null;
			}
			if (mmMapView != null) {
				mmMapView.Dispose ();
				mmMapView = null;
			}
		}
	}
}
