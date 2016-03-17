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
	[Register ("FormViewController")]
	partial class FormViewController
	{
		[Outlet]
		UIKit.UIImageView bgToilet { get; set; }

		[Outlet]
		UIKit.UIButton CancleButton { get; set; }

		[Outlet]
		UIKit.UITextView DesTextField { get; set; }

		[Outlet]
		UIKit.UIImageView iconLocation { get; set; }

		[Outlet]
		UIKit.UILabel lblLocation { get; set; }

		[Outlet]
		UIKit.UITextField NameTextField { get; set; }

		[Outlet]
		UIKit.UIButton SaveButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (bgToilet != null) {
				bgToilet.Dispose ();
				bgToilet = null;
			}

			if (CancleButton != null) {
				CancleButton.Dispose ();
				CancleButton = null;
			}

			if (DesTextField != null) {
				DesTextField.Dispose ();
				DesTextField = null;
			}

			if (NameTextField != null) {
				NameTextField.Dispose ();
				NameTextField = null;
			}

			if (SaveButton != null) {
				SaveButton.Dispose ();
				SaveButton = null;
			}

			if (iconLocation != null) {
				iconLocation.Dispose ();
				iconLocation = null;
			}

			if (lblLocation != null) {
				lblLocation.Dispose ();
				lblLocation = null;
			}
		}
	}
}
