
using System;
using System.Drawing;
using System.Collections.Generic;

using Foundation;
using UIKit;
using MapKit;
using CoreLocation;
using CoreGraphics;
using CoreAnimation;
using Utils;
using LocationUtils;
using Commons;
using OHouse.Connectivity;

namespace OHouse
{
	public partial class MapViewController : UIViewController
	{
		public static LocationUtil Manager { get; set; }

		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		FormViewController form;

		public UIActivityIndicatorView Loader {
			get { return loader; }
			set { ; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GoToilet.MapViewController"/> class.
		/// </summary>
		public MapViewController () : base ("MapViewController", null)
		{
			Title = "Map";
			EdgesForExtendedLayout = UIRectEdge.All;
			Manager = new LocationUtil ();
		}

		/// <summary>
		/// Dids the receive memory warning.
		/// </summary>
		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		/// <summary>
		/// Views the did load.
		/// </summary>
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			Map.Delegate = new MapDelegate (this);
			Map.ShowsUserLocation = true;

			CGRect screen = this.NavigationController.View.Bounds;

			AddBtn.BackgroundColor = Common.ColorStyle_1;
			AddBtn.Layer.CornerRadius = AddBtn.Frame.Size.Width / 2;
			AddBtn.Layer.ShadowOffset = new CGSize (0, 1);
			AddBtn.Layer.ShadowOpacity = 1f;
			AddBtn.Layer.ShadowRadius = 0.3f;
			AddBtn.Layer.ShadowColor = new CGColor (0, 0, 0);

			MyLocation.BackgroundColor = Common.ColorStyle_1;
			MyLocation.Layer.CornerRadius = MyLocation.Frame.Size.Width / 2;
			MyLocation.Layer.ShadowOffset = new CGSize (0, 1);
			MyLocation.Layer.ShadowOpacity = 1f;
			MyLocation.Layer.ShadowRadius = 0.3f;
			MyLocation.Layer.ShadowColor = new CGColor (0, 0, 0);

			MyLocation.TouchUpInside += (sender, e) => {
				if (Map.UserLocation != null) {
					CLLocationCoordinate2D coords = Map.UserLocation.Coordinate;
					Map.SetRegion (MKCoordinateRegion.FromDistance (coords, MapDelegate.latMeter, MapDelegate.lonMeter), true);
				}
			};
				
//			AddBtn.TouchUpInside += (sender, e) => {
//				if (Map.UserLocation != null) {
//
//					CLLocationCoordinate2D userloc = Map.UserLocation.Coordinate;
//					form = new FormViewController (userloc);
//			
//					UINavigationController nav = new UINavigationController (form);
//					this.PresentViewController (nav, true, () => {
//					});
//				}
//			};

			AddBtn.TouchUpInside += (object sender, EventArgs e) => {
				NavigationController.PresentViewController(new DetailViewController(20), true, null);
			};
		}

		/// <summary>
		/// Dids the entered background.
		/// </summary>
		/// <param name="notification">Notification.</param>
		private void didEnteredBackground (NSNotification notification)
		{
			Console.WriteLine ("App Entered Background from MapViewController");
		}

		/// <summary>
		/// Dids the entered foreground.
		/// </summary>
		/// <param name="notification">Notification.</param>
		private void didEnteredForeground (NSNotification notification)
		{
			Console.WriteLine ("App Entered Foreground from MapViewController");
		}

		/// <summary>
		/// Views the will appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			// Update network connection
			UpdateStatus ();

			// Check network connection
			if (internetStatus == NetworkStatus.NotReachable) {

			}
		}

		/// <summary>
		/// Views the did unload.
		/// </summary>
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			//NSNotificationCenter.DefaultCenter.RemoveObserver (_didEnteredBackground);
			//NSNotificationCenter.DefaultCenter.RemoveObserver (_didEnteredForeground);
			ReleaseDesignerOutlets ();
		}

		/// <summary>
		/// Shoulds the autorotate to interface orientation.
		/// </summary>
		/// <returns><c>true</c>, if autorotate to interface orientation was shoulded, <c>false</c> otherwise.</returns>
		/// <param name="io">Io.</param>
		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation io)
		{
			return (io != UIInterfaceOrientation.PortraitUpsideDown);
		}

		void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
		}
	}
}