﻿
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
using MapUtils;
using LocationUtils;
using Commons;
using OHouse.Connectivity;

namespace OHouse
{
	public partial class MapViewController : UIViewController
	{
		public static LocationUtil Manager { get; set; }

		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		bool networkStatus;

		UIButton addLocationButton;
		UIButton myLocationButton;
		float w = 50;
		float h = 50;

		Common common = new Common ();
		FormViewController form;

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

			CGRect screen = View.Bounds;

			// Create MKMapView and set bounds to fit with the UIScreen
			var mapV = new MKMapView (View.Bounds);
			mapV.Delegate = new MapDelegate (this);

			mapV.ShowsUserLocation = true;
			mapV.ZoomEnabled = true;
			mapV.ScrollEnabled = true;

			// Create button for MyLocation
			myLocationButton = new UIButton (UIButtonType.System);
			myLocationButton.TintColor = UIColor.White;
			myLocationButton.SetImage (UIImage.FromFile ("images/icons/icon-pin"), UIControlState.Normal);
			myLocationButton.BackgroundColor = common.ColorStyle_1;
			myLocationButton.Frame = new RectangleF ((float)screen.Width - w - 10, (float)screen.Height - h - 10, w, h);
			myLocationButton.Layer.CornerRadius = myLocationButton.Frame.Size.Width / 2;
			myLocationButton.Layer.ShadowOffset = new CGSize (0,2);
			myLocationButton.Layer.ShadowOpacity = 1f;
			myLocationButton.Layer.ShadowRadius = 2;
			myLocationButton.Layer.ShadowColor = new CGColor (0, 0, 0);

			// Create button for Add location
			addLocationButton = new UIButton (UIButtonType.System);
			addLocationButton.TintColor = UIColor.White;
			addLocationButton.SetImage (UIImage.FromFile ("images/icons/icon-add"), UIControlState.Normal);
			addLocationButton.BackgroundColor = common.ColorStyle_1;
			addLocationButton.Frame = new RectangleF (0 + 10, (float)screen.Height - h - 10, w, h);
			addLocationButton.Layer.CornerRadius = myLocationButton.Frame.Size.Width / 2;
			addLocationButton.Layer.ShadowOffset = new CGSize (0,2);
			addLocationButton.Layer.ShadowOpacity = 1f;
			addLocationButton.Layer.ShadowRadius = 2;
			addLocationButton.Layer.ShadowColor = new CGColor (0, 0, 0);

			// Action for MyLocation button
			myLocationButton.TouchUpInside += (sender, e) => {
				if (mapV.UserLocation != null) {
					CLLocationCoordinate2D coords = mapV.UserLocation.Coordinate;
					mapV.SetRegion (MKCoordinateRegion.FromDistance (coords, MapDelegate.latMeter, MapDelegate.lonMeter), true);
				}
			};
				
			addLocationButton.TouchUpInside += (sender, e) => {
				if (mapV.UserLocation != null) {

					CLLocationCoordinate2D userloc = mapV.UserLocation.Coordinate;
					form = new FormViewController (userloc);

					UINavigationController nav = new UINavigationController (form);
					this.PresentViewController (nav, true, () => {
					});
				}
			};



				
			mapV.AddSubview (addLocationButton);
			mapV.AddSubview (myLocationButton);

			View.AddSubview (mapV);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
		}

		/// <summary>
		/// Views the did unload.
		/// </summary>
		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

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
	}
}