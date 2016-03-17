using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Foundation;
using UIKit;
using CoreLocation;
using MapKit;
using Utils;

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
		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		bool connectivity;
		protected string annId = "Basic Annotation";
		MapViewController parent;
		public static double latMeter = 1000;
		public static double lonMeter = 1000;
		UINavigationController Navigationcontroller;
		// *** modifying
		// MKPointAnnotation[] pins;
		static MKCircle circleOverlay;
		public static double maxDistance = 500;
		// *** modifying

		List<ToiletsBase> toiletsList = new List<ToiletsBase> ();
		//List<ToiletsBase> nearToiletList = new List<ToiletsBase> ();
		public static List<ToiletsBase> nearToiletList = new List<ToiletsBase> ();

		DataRequestManager drm;

		/// <summary>
		/// Initializes a new instance of the <see cref="MapUtils.MapDelegate"/> class.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public MapDelegate (MapViewController parent)
		{
			this.parent = parent;
			connectivity = true;
			drm = new DataRequestManager ();
			this.Navigationcontroller = parent.NavigationController;

			//UpdateStatus (null, null);
		}

		async void initDataReset() {
			if (!connectivity) {
				toiletsList = drm.GetToiletList ("Update.plist", 0, 10, false);

			} else {
				parent.Loader.StartAnimating ();
				toiletsList = await downloadStringAsync ("http://gstore.pcp.jp/api/get_spots.php");
				parent.Loader.StopAnimating ();
			}
		}

		async Task<List<ToiletsBase>> downloadStringAsync (string url)
		{
			List<ToiletsBase> t;

			try {
				HttpClient client = new HttpClient ();
				Task<string> json = client.GetStringAsync (url);

				string data = await json;

				t = drm.GetDataListJSON (data);

				return t;

			} catch (Exception e) {
				Console.WriteLine (e.Message);
				return null;
			}
		}

		/// <summary>
		/// Dids the update user location.
		/// </summary>
		/// <param name="mapView">Map view.</param>
		/// <param name="userLocation">User location.</param>
		public override void DidUpdateUserLocation (MKMapView mapView, MKUserLocation userLocation)
		{
			UpdateStatus (null, null);

			// Clear map first
			clearMap (mapView);

			Console.WriteLine ("DidUpdateUserLocation");

			if (mapView.UserLocation != null) {
				CLLocationCoordinate2D coords = mapView.UserLocation.Coordinate;
				CLLocation userLC = new CLLocation (coords.Latitude, coords.Longitude);

				circleOverlay = MKCircle.Circle (new CLLocationCoordinate2D (coords.Latitude, coords.Longitude), maxDistance);
				foreach (var a in toiletsList) {


					CLLocation loc = new CLLocation (a.latitude, a.longitude);
					double dist = loc.DistanceFrom (userLC);

					// Calculate minimal distance
//					if (dist > maxDistance) {
//						continue;
//					}
						
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


		public override void CalloutAccessoryControlTapped (MKMapView mapView, MKAnnotationView view, UIControl control)
		{
			CustomAnnotation id = view.Annotation as CustomAnnotation;

			int datas = id.GetLocationID ();
			DetailViewController infoView = new DetailViewController (datas);
				
			Navigationcontroller.PresentViewController (infoView, true, null);

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

		void UpdateConnectivity ()
		{
			connectivity = true;

			switch (remoteHostStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;
				break;
			}

			switch (internetStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;
				break;
			}

			switch (localWifiStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;
				break;
			}

			initDataReset ();
		}

		void UpdateStatus (object sender, EventArgs e)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();

			//////
			/// Update internet connectivity status
			UpdateConnectivity ();
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