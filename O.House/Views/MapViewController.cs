
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

namespace OHouse
{
	public partial class MapViewController : UIViewController
	{
		public static LocationUtil Manager { get; set; }

		UIButton addLocationButton;
		UIButton myLocationButton;
		float w = 50;
		float h = 50;

		Common common = new Common ();
		FormViewController form = new FormViewController ();

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

			if (mapV.UserLocation != null) {
				CLLocationCoordinate2D coords = mapV.UserLocation.Coordinate;

				UIBarButtonItem rightBarBtnItem = new UIBarButtonItem (
					                                  "Near", 
					                                  UIBarButtonItemStyle.Plain, 
					                                  (s, e) => {
						NavigationController.PushViewController (
							new NearestDialogViewController (), 
							true);
					});

				rightBarBtnItem.SetTitleTextAttributes (common.commonStyle, UIControlState.Normal);

				// Get nearest location button
				this.NavigationItem.SetRightBarButtonItem (
					rightBarBtnItem,
					true
				);
			}

			// Create button for MyLocation
			var myLocation = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-pin"), w, h);
			myLocationButton = UtilImage.RoundButton (
				myLocation, 
				new RectangleF ((float)screen.Width - w - 10, (float)screen.Height - h - 10, w, h),
				common.ColorStyle_1,
				false
			);

			// Create button for Add location
			var addLocation = UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-add"), w, h);
			addLocationButton = UtilImage.RoundButton (
				addLocation, 
				new RectangleF (0 + 10, (float)screen.Height - h - 10, w, h), 
				common.ColorStyle_1,
				false
			);
				
			// Action for MyLocation button
			myLocationButton.TouchUpInside += (sender, e) => {
				if (mapV.UserLocation != null) {
					CLLocationCoordinate2D coords = mapV.UserLocation.Coordinate;
					mapV.SetRegion (MKCoordinateRegion.FromDistance (coords, MapDelegate.latMeter, MapDelegate.lonMeter), true);
				}
			};

			// Action for Create location button
			addLocationButton.TouchUpInside += (sender, e) => {
//				this.AddChildViewController(form);
//				this.DidMoveToParentViewController(this);
//				this.View.AddSubview(form.View);
//
//				// Animate
//				UIView.Animate(0.2, 0.0, UIViewAnimationOptions.CurveEaseOut, () => {
//					form.View.Frame = new CGRect(new PointF(0, common.PopUpDistance), new CGSize((float)screen.Width, (float)screen.Height - common.PopUpDistance));
//				}, () => {
//					//
//				});
//				form.ModalPresentationStyle = UIModalPresentationStyle.FormSheet;
//				NavigationController.PresentViewController(form, true, () => {});

				UINavigationController nav = new UINavigationController (form);
				this.PresentViewController (nav, true, () => {
				});
			};

			mapV.AddSubview (myLocationButton);
			mapV.AddSubview (addLocationButton);
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

