using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreLocation;
using MapKit;
using Utils;

namespace OHouse
{
	/// <summary>
	/// Map delegate.
	/// </summary>
	public partial class MapDelegate : MKMapViewDelegate
	{
		/// <summary>
		/// Declaration
		/// </summary>
		protected string annId = "Basic Annotation";
		UIButton detailButton;
		MapViewController parent;
		public static double latMeter = 1000;
		public static double lonMeter = 1000;

		// *** modifying
		MKPointAnnotation[] pins;
		static MKCircle circleOverlay;
		static double maxDistance = 500;
		// *** modifying

		List<ToiletsBase> toiletsList = new List<ToiletsBase> ();

		/// <summary>
		/// Initializes a new instance of the <see cref="MapUtils.MapDelegate"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public MapDelegate (MapViewController parent)
		{
			this.parent = parent;

			var path = NSBundle.MainBundle.PathForResource ("database/Toilets", "plist");
			var toilets = NSDictionary.FromFile (path);

			foreach (var toilet in toilets) {
				var obj = toilet.Value;
				var nameKey = obj.ValueForKey ((NSString)"nameKey").ToString ();
				var latitudeKey = obj.ValueForKey ((NSString)"latitudeKey").ToString ();
				var longitudeKey = obj.ValueForKey ((NSString)"longitudeKey").ToString ();
				toiletsList.Add (
					new ToiletsBase (nameKey, double.Parse (latitudeKey), double.Parse (longitudeKey))	
				);
			}
		}

		/// <summary>
		/// Dids the update user location.
		/// </summary>
		/// <param name="mapView">Map view.</param>
		/// <param name="userLocation">User location.</param>
		public override void DidUpdateUserLocation (MKMapView mapView, MKUserLocation userLocation)
		{
			// Clear map first
			clearMap (mapView);

			if (mapView.UserLocation != null) {
				CLLocationCoordinate2D coords = mapView.UserLocation.Coordinate;

//				pins = new MKPointAnnotation[] {
//					new MKPointAnnotation () { Title = "Near 01", Coordinate = new CLLocationCoordinate2D (16.777024, 96.164964) },
//					new MKPointAnnotation () { Title = "Near 02", Coordinate = new CLLocationCoordinate2D (16.776768, 96.162622) },
//					new MKPointAnnotation () { Title = "Near 03", Coordinate = new CLLocationCoordinate2D (16.777364, 96.160315) },
//					new MKPointAnnotation () { Title = "Near 04", Coordinate = new CLLocationCoordinate2D (16.777114, 96.162486) },
//					new MKPointAnnotation () { Title = "Near 05", Coordinate = new CLLocationCoordinate2D (16.777024, 96.164964) },
//					new MKPointAnnotation () { Title = "Near 06", Coordinate = new CLLocationCoordinate2D (16.782406, 96.178641) },
//					new MKPointAnnotation () { Title = "Near 07", Coordinate = new CLLocationCoordinate2D (16.774438, 96.158796) },
//					new MKPointAnnotation () { Title = "Near 08", Coordinate = new CLLocationCoordinate2D (16.788157, 96.075439) },
//					new MKPointAnnotation () { Title = "Near 09", Coordinate = new CLLocationCoordinate2D (16.902507, 96.054153) },
//					new MKPointAnnotation () { Title = "Near 10", Coordinate = new CLLocationCoordinate2D (16.845341, 96.414642) },
//					new MKPointAnnotation () { Title = "Near 11", Coordinate = new CLLocationCoordinate2D (16.779903, 96.160169) },
//					new MKPointAnnotation () { Title = "Near 12", Coordinate = new CLLocationCoordinate2D (16.776863, 96.160040) },
//					new MKPointAnnotation () { Title = "Near 13", Coordinate = new CLLocationCoordinate2D (16.780458, 96.152852) },
//					new MKPointAnnotation () { Title = "Near 14", Coordinate = new CLLocationCoordinate2D (16.779472, 96.153217) },
//					new MKPointAnnotation () { Title = "Near 15", Coordinate = new CLLocationCoordinate2D (16.776801, 96.161736) },
//					new MKPointAnnotation () { Title = "Near 16", Coordinate = new CLLocationCoordinate2D (16.782533, 96.185918) }
//				};

				circleOverlay = MKCircle.Circle (new CLLocationCoordinate2D (coords.Latitude, coords.Longitude), maxDistance);

				CLLocation userLC = new CLLocation (coords.Latitude, coords.Longitude);

				foreach (var a in toiletsList) {
					//foreach (MKPointAnnotation a in pins) {
					//CLLocation loc = new CLLocation (a.Coordinate.Latitude, a.Coordinate.Longitude);
					CLLocation loc = new CLLocation (a.Latitude, a.Longitude);
					double dist = loc.DistanceFrom (userLC);
					if (dist > maxDistance) {
						continue;
					}
						
					MKPointAnnotation point = new MKPointAnnotation () {
						//Title = a.GetTitle (),
						//Coordinate = new CLLocationCoordinate2D (a.Coordinate.Latitude, a.Coordinate.Longitude)
						Title = a.Name,
						Coordinate = new CLLocationCoordinate2D (a.Latitude, a.Longitude)
					};

					// Then add
					mapView.AddOverlay (circleOverlay);
					mapView.AddAnnotations (point);
				}

				mapView.SetRegion (MKCoordinateRegion.FromDistance (coords, latMeter, lonMeter), true);
			}
		}


		/// <summary>
		/// Detects the current location.
		/// </summary>
		/// <returns><c>true</c>, if current location was detected, <c>false</c> otherwise.</returns>
		/// <param name="mapView">Map view.</param>
		/// <param name="annotation">Annotation.</param>
		private bool detectCurrentLocation (MKMapView mapView, IMKAnnotation annotation)
		{
			var userLocationAnnotation = ObjCRuntime.Runtime.GetNSObject (annotation.Handle) as MKUserLocation;
			if (userLocationAnnotation != null) {
				return userLocationAnnotation == mapView.UserLocation;
			}

			return false;
		}

		/// <summary>
		/// Clears the map.
		/// </summary>
		/// <param name="mapView">Map view.</param>
		private void clearMap (MKMapView mapView)
		{
			// Remove every annotations
			if (mapView.Annotations != null) {
				mapView.RemoveAnnotations (mapView.Annotations);
			}

			// Remove every overlays
			if (mapView.Overlays != null) {
				mapView.RemoveOverlays (mapView.Overlays);
			}
		}

		/// <summary>
		/// Gets the view for annotation.
		/// </summary>
		/// <returns>The view for annotation.</returns>
		/// <param name="mapView">Map view.</param>
		/// <param name="annotation">Annotation.</param>
		public override MKAnnotationView GetViewForAnnotation (MKMapView mapView, IMKAnnotation annotation)
		{
			// if userlocation, skip it
			if (detectCurrentLocation (mapView, annotation)) {
				return null;
			}

			// try and dequeue the annotation view
			MKAnnotationView annotationView = mapView.DequeueReusableAnnotation (annId);
			// if we couldn't dequeue one, create a new one
			if (annotationView == null) {
				annotationView = new MKPinAnnotationView (annotation, annId);
			} else { // if we did dequeue one for reuse, assign the annotation to it
				annotationView.Annotation = annotation;
			}

			// configure our annotation view properties
			annotationView.Image = UIImage.FromFile ("images/icons/icon-toilet.png");
			annotationView.CanShowCallout = true;
			(annotationView as MKPinAnnotationView).AnimatesDrop = false;
			annotationView.Selected = true;


			// you can add an accessory view, in this case, we'll add a button on the right, and an image on the left
			detailButton = UIButton.FromType (UIButtonType.DetailDisclosure);
			detailButton.TouchUpInside += (s, e) => {
				double lat = annotation.Coordinate.Latitude;
				double lon = annotation.Coordinate.Longitude;

				string[] datas = { annotation.GetTitle (), lat.ToString (), lon.ToString () };

				infoDialogViewController infoView = new infoDialogViewController (datas);
				parent.NavigationController.PushViewController (infoView, true);
			};

			annotationView.RightCalloutAccessoryView = detailButton;
			annotationView.Image = UIImage.FromBundle ("images/icons/icon-toilet");

			return annotationView;
		}


		/// <summary>
		/// Overlaies the renderer.
		/// </summary>
		/// <returns>The renderer.</returns>
		/// <param name="mapView">Map view.</param>
		/// <param name="overlay">Overlay.</param>
		public override MKOverlayRenderer OverlayRenderer (MKMapView mapView, IMKOverlay overlay)
		{
			return new MKCircleRenderer (overlay as MKCircle) {
				FillColor = UIColor.Blue,
				Alpha = 0.1f
			};
		}

		/// <summary>
		/// Regions the changed.
		/// </summary>
		/// <param name="mapView">Map view.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void RegionChanged (MKMapView mapView, bool animated)
		{
		}
	}
}