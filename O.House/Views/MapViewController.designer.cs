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
	[Register ("MapViewController")]
	partial class MapViewController
	{
		[Outlet]
		UIKit.UIButton AddBtn { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView loader { get; set; }

		[Outlet]
		MapKit.MKMapView Map { get; set; }

		[Outlet]
		UIKit.UIButton MyLocation { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AddBtn != null) {
				AddBtn.Dispose ();
				AddBtn = null;
			}

			if (Map != null) {
				Map.Dispose ();
				Map = null;
			}

			if (MyLocation != null) {
				MyLocation.Dispose ();
				MyLocation = null;
			}

			if (loader != null) {
				loader.Dispose ();
				loader = null;
			}
		}
	}
}
