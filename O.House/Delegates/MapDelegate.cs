using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using CoreLocation;
using MapKit;
using Utils;
using MapUtils;
using OHouse.DRM;
using OHouse.Connectivity;

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
		MapViewController parent;
		public static double latMeter = 1000;
		public static double lonMeter = 1000;

		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		bool networkStatus;

		// *** modifying
		// MKPointAnnotation[] pins;
		static MKCircle circleOverlay;
		public static double maxDistance = 500;
		// *** modifying

		List<ToiletsBase> toiletsList = new List<ToiletsBase> ();
		//List<ToiletsBase> nearToiletList = new List<ToiletsBase> ();
		public static List<ToiletsBase> nearToiletList = new List<ToiletsBase> ();

		/// <summary>
		/// Initializes a new instance of the <see cref="MapUtils.MapDelegate"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public MapDelegate (MapViewController parent)
		{
			this.parent = parent;
			DataRequestManager drm = new DataRequestManager ();

			/// Check network status
			UpdateStatus ();
			if (internetStatus == NetworkStatus.NotReachable) {
				networkStatus = false;
				toiletsList = drm.GetToiletList ("database/Toilets");
			} else {
				networkStatus = true;
				toiletsList = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php");
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
				CLLocation userLC = new CLLocation (coords.Latitude, coords.Longitude);

				circleOverlay = MKCircle.Circle (new CLLocationCoordinate2D (coords.Latitude, coords.Longitude), maxDistance);
				foreach (var a in toiletsList) {


					CLLocation loc = new CLLocation (a.latitude, a.longitude);
					double dist = loc.DistanceFrom (userLC);

					// Calculate minimal distance
					if (dist > maxDistance) {
						continue;
					}
						
					// For near toilet list page
					nearToiletList.Add (new ToiletsBase (a.spot_id, a.vote_cnt, a.title, a.sub_title, a.picture, a.longitude, a.latitude, dist, true));

					CustomAnnotation point = new CustomAnnotation () {
						LocationID = a.spot_id,
						Title = a.title,
						Subtitle = "",
						Coordinate = new CLLocationCoordinate2D (a.latitude, a.longitude),
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

			// Clear temp list
			nearToiletList.Clear ();
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
			} else { 
				// if we did dequeue one for reuse, assign the annotation to it
				annotationView.Annotation = annotation;
			}

			// configure our annotation view properties
			annotationView.CanShowCallout = true;
			(annotationView as MKPinAnnotationView).AnimatesDrop = false;
			annotationView.Selected = true;

			annotationView.RightCalloutAccessoryView = UIButton.FromType (UIButtonType.DetailDisclosure);
			return annotationView;
		}

		/// <summary>
		/// Callouts the accessory control tapped.
		/// </summary>
		/// <param name="mapView">Map view.</param>
		/// <param name="view">View.</param>
		/// <param name="control">Control.</param>
//		public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
//		{
//			CustomAnnotation id = view.Annotation as CustomAnnotation;
//
//			int datas = id.GetLocationID ();
//			DetailViewController infoView = new DetailViewController (datas);
//			UINavigationController nav = new UINavigationController (infoView);
//			parent.PresentViewController (nav, true, () => {
//			});
//		}


		public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
		{
			CustomAnnotation id = view.Annotation as CustomAnnotation;

			int datas = id.GetLocationID ();
			DetailViewController infoView = new DetailViewController (datas);
			UINavigationController nav = new UINavigationController (infoView);
			parent.PresentViewController (nav, true, () => {


				UIView.Animate (
					0.25,
					() => {
						infoView.View.Alpha = 0.9f;

						infoView.View.BackgroundColor = UIColor.Black;
		
					}
				);
			});
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

		/// <summary>
		/// Updates connection status.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
		}
	}

	/// <summary>
	/// Custom annotation.
	/// </summary>
	public class CustomAnnotation : MKPointAnnotation
	{

		private int _locationID;

		/// <summary>
		/// Gets or sets the location I.
		/// </summary>
		/// <value>The location I.</value>
		public int LocationID {
			get {
				return _locationID;
			}
			set {
				_locationID = value;
			}
		}

		/// <summary>
		/// Gets the location I.
		/// </summary>
		/// <returns>The location I.</returns>
		public int GetLocationID ()
		{
			return LocationID;
		}
	}
}