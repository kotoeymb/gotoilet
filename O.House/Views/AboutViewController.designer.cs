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
