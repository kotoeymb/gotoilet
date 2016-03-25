using MapKit;
using System;
using Utils;
using Foundation;
using UIKit;
using CoreLocation;
using Commons;

namespace O.House
{
	public partial class AboutViewController : UIViewController
	{
		public AboutViewController () : base ("AboutViewController", null)
		{
		}

		public void CloseBtnEvent (object sender, EventArgs e)
		{
			this.DismissModalViewController (true);
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			CloseBtn.TouchUpInside += (object sender, EventArgs e) => {

				this.DismissModalViewController (true);
			};

			double distance = 100;

			////// JP Office
			/// Pin information
			var JPOfficeLocation = new CLLocationCoordinate2D (35.663908, 139.702534);
			var JPOfficePin = new MKPointAnnotation () {
				Title = "ピーシーフェーズ株式会社",
				Subtitle = "Software Development Company",
				Coordinate = JPOfficeLocation
			};

			jpMapView.AddAnnotation (JPOfficePin);
			jpMapView.CenterCoordinate = JPOfficeLocation;
			jpMapView.SetRegion (MKCoordinateRegion.FromDistance (JPOfficePin.Coordinate, distance, distance), false);

			////// MM Office
			/// Pin information
			var MMOfficeLocation = new CLLocationCoordinate2D (16.777489, 96.162055);
			var MMOfficePin = new MKPointAnnotation () {
				Title = "ピーチーフェーズミャンマー",
				Subtitle = "Software Development Company",
				Coordinate = MMOfficeLocation
			};

			mmMapView.AddAnnotation (MMOfficePin);
			mmMapView.CenterCoordinate = MMOfficeLocation;
			mmMapView.SetRegion (MKCoordinateRegion.FromDistance (MMOfficePin.Coordinate, distance, distance), false);

			jpMapView.UserInteractionEnabled = false;
			mmMapView.UserInteractionEnabled = false;
		}
	}
}

