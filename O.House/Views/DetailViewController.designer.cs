// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

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

			if (mapLocation != null) {
				mapLocation.Dispose ();
				mapLocation = null;
			}

			if (loader != null) {
				loader.Dispose ();
				loader = null;
			}

			if (bgToilet != null) {
				bgToilet.Dispose ();
				bgToilet = null;
			}
		}
	}
}
