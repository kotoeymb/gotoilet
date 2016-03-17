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
	[Register ("DetailViewController")]
	partial class DetailViewController
	{
		[Outlet]
		UIKit.UIImageView bgToilet { get; set; }

		[Outlet]
		UIKit.UIButton btnClose { get; set; }

		[Outlet]
		UIKit.UIImageView iconLike { get; set; }

		[Outlet]
		UIKit.UILabel lblApproveCount { get; set; }

		[Outlet]
		UIKit.UILabel lblDescription { get; set; }

		[Outlet]
		UIKit.UILabel lblLocation { get; set; }

		[Outlet]
		UIKit.UILabel lblTitle { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView loader { get; set; }

		[Outlet]
		MapKit.MKMapView mapLocation { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (bgToilet != null) {
				bgToilet.Dispose ();
				bgToilet = null;
			}
			if (btnClose != null) {
				btnClose.Dispose ();
				btnClose = null;
			}
			if (iconLike != null) {
				iconLike.Dispose ();
				iconLike = null;
			}
			if (lblApproveCount != null) {
				lblApproveCount.Dispose ();
				lblApproveCount = null;
			}
			if (lblDescription != null) {
				lblDescription.Dispose ();
				lblDescription = null;
			}
			if (lblLocation != null) {
				lblLocation.Dispose ();
				lblLocation = null;
			}
			if (lblTitle != null) {
				lblTitle.Dispose ();
				lblTitle = null;
			}
			if (loader != null) {
				loader.Dispose ();
				loader = null;
			}
			if (mapLocation != null) {
				mapLocation.Dispose ();
				mapLocation = null;
			}
		}
	}
}
