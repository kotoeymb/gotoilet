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
			if (loader != null) {
				loader.Dispose ();
				loader = null;
			}
			if (Map != null) {
				Map.Dispose ();
				Map = null;
			}
			if (MyLocation != null) {
				MyLocation.Dispose ();
				MyLocation = null;
			}
		}
	}
}
