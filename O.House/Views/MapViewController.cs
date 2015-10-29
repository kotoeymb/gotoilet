﻿
using System;
using System.Drawing;
using System.Collections.Generic;

using Foundation;
using UIKit;
using MapKit;
using CoreLocation;
using CoreGraphics;
using Utils;
using MapUtils;
using LocationUtils;

namespace OHouse
{
	public partial class MapViewController : UIViewController
	{
		public static LocationUtil Manager { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="GoToilet.MapViewController"/> class.
		/// </summary>
		public MapViewController () : base ("MapViewController", null)
		{
			Title = "Map";
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
			float w = 50;
			float h = 50;

			// Create MKMapView and set bounds to fit with the UIScreen
			var mapV = new MKMapView (View.Bounds);
			mapV.Delegate = new MapDelegate (this);

			mapV.ShowsUserLocation = true;
			mapV.ZoomEnabled = true;
			mapV.ScrollEnabled = true;
				
			// Create button for MyLocation
			var myLocation = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-mylocation"), w, h);
			UIButton myLocationButton = UtilImage.RoundButton (myLocation, new RectangleF ((float)View.Frame.Width - w - 10f, (float)View.Frame.Height - h - 10f, w, h), false);

			// Action for MyLocation button
			myLocationButton.TouchUpInside += (sender, e) => {
				if (mapV.UserLocation != null) {
					CLLocationCoordinate2D coords = mapV.UserLocation.Coordinate;
					mapV.SetRegion (MKCoordinateRegion.FromDistance (coords, MapDelegate.latMeter, MapDelegate.lonMeter), true);
				}
			};
				
			View.AddSubview (mapV);
			View.AddSubview (myLocationButton);

			base.ViewDidLoad ();
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

