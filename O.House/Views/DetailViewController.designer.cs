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
		UIKit.UIButton OKButton { get; set; }

		[Outlet]
		UIKit.UILabel PositionLabel { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }

		[Outlet]
		UIKit.UILabel ToiletName { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (OKButton != null) {
				OKButton.Dispose ();
				OKButton = null;
			}

			if (PositionLabel != null) {
				PositionLabel.Dispose ();
				PositionLabel = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (ToiletName != null) {
				ToiletName.Dispose ();
				ToiletName = null;
			}
		}
	}
}
