
using System;
using System.Drawing;

using Foundation;
using UIKit;
using CoreGraphics;
using CoreLocation;
using Commons;
using Utils;

using Facebook.CoreKit;
using Facebook.LoginKit;

using MonoTouch.Dialog;
using CustomElements;

namespace OHouse
{
	public partial class FormViewController : UIViewController
	{
		Common common = new Common ();
		TextField tf;
		TextField stf;
		string message;
		UIImagePickerController imagePicker;

		private CLLocationCoordinate2D _userLocation;

		public CLLocationCoordinate2D userLocation {
			get {
				return _userLocation;
			}
			set {
				_userLocation = value;
			}
		}

		public FormViewController () : base ("FormViewController", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			Title = "Location";

			this.NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem (
					UIImage.FromBundle ("images/icons/icon-mark"), UIBarButtonItemStyle.Plain, (ss, ee) => {

					if (GetDataFromTextField (tf) != "" && GetDataFromTextField (stf) != "") {
						message = GetDataFromTextField (tf) + ", " + GetDataFromTextField (stf);
					} else {
						message = "Please insert data!!! MTFK!!";
					}

					UIAlertView av = new UIAlertView (
						                  "Data",
						                  message,
						                  null,
						                  "OK",
						                  null
					                  );

					av.Show ();
					//NavigationController.PushViewController (new SubmitViewController (), true);	
				}
				),
				true
			);

			this.NavigationItem.SetLeftBarButtonItem (
				new UIBarButtonItem (
					UIImage.FromBundle ("images/icons/icon-cross"), UIBarButtonItemStyle.Plain, (ss, ee) => {
					this.DismissModalViewController (true);
				}
				),
				true
			);
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.FromPatternImage (UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/background/bg-map"), (float)View.Frame.Width, 0));

			SetupUI ();
		}

		/// <summary>
		/// Setups the UI.
		/// </summary>
		void SetupUI ()
		{
			UIScrollView sv = new UIScrollView (View.Bounds);

			CGRect screen = View.Bounds;
			sv.BackgroundColor = UIColor.FromRGBA (255, 255, 255, 240);
			float sw = (float)screen.Width;
			float sh = (float)screen.Height;

			CGRect ssize = new CGRect (0, sh, sw, sh - 100);
			CGRect items = new CGRect (15, 15, sw - 30, 40);

			View.Frame = ssize;
			View.UserInteractionEnabled = true;

			// Title
			CGRect tfRec = new CGRect (items.X, items.Y, items.Width, items.Height);
			tf = new TextField (
				tfRec,
				common.ColorStyle_1, 
				"Title", 
				UtilImage.GetColoredImage ("images/icons/icon-notes", common.ColorStyle_1)
			);

			// Subtitle
			CGRect stfRec = new CGRect (items.X, tf.Frame.Y + tf.Frame.Height + 24, items.Width, items.Height);
			stf = new TextField (
				stfRec,
				common.ColorStyle_1, 
				"Description", 
				UtilImage.GetColoredImage ("images/icons/icon-notes", common.ColorStyle_1)
			);

			// Camera upload
			CGRect cameraRec = new CGRect (stf.Frame.X, stf.Frame.Y + stf.Frame.Height + 24, stf.Frame.Width, 40);
			UIButton camera = new UIButton (cameraRec);

			CGRect cameraIconRec = new CGRect (camera.Frame.Width - 16 - 10, camera.Frame.Height / 2 - 8, 16, 16);
			UIImageView cameraIcon = new UIImageView (cameraIconRec) {
				// Change image color overlay
				Image = UtilImage.GetColoredImage ("images/icons/icon-camera", common.White)
			};

			camera.SetTitle ("Upload photo", UIControlState.Normal);
			camera.Font = common.Font16F;
			camera.BackgroundColor = common.ColorStyle_1;
			camera.AddSubview (cameraIcon);

			// Action when pressed
			camera.TouchUpInside += (sender, e) => {
				// Create a new Alert Controller
				UIAlertController actionSheetAlert = UIAlertController.Create ("Upload photo", "Please choose methods from below", UIAlertControllerStyle.ActionSheet);

				// Add Actions
				actionSheetAlert.AddAction (
					UIAlertAction.Create (
						"Choose from gallery",
						UIAlertActionStyle.Default,
						(action) => {
							Console.WriteLine("WTF");
						})
				);
				actionSheetAlert.AddAction (UIAlertAction.Create ("Take photo", UIAlertActionStyle.Default, (action) => Console.WriteLine ("Item Two pressed.")));
				actionSheetAlert.AddAction (UIAlertAction.Create ("Cancel", UIAlertActionStyle.Cancel, (action) => Console.WriteLine ("Cancel button pressed.")));

				// Required for iPad - You must specify a source for the Action Sheet since it is
				// displayed as a popover
				UIPopoverPresentationController presentationPopover = actionSheetAlert.PopoverPresentationController;
				if (presentationPopover != null) {
					presentationPopover.SourceView = this.View;
					presentationPopover.PermittedArrowDirections = UIPopoverArrowDirection.Up;
				}

				// Display the alert
				this.PresentViewController (actionSheetAlert, true, null);
			};

			sv.AddSubviews (tf, stf, camera);

			View.AddSubview (sv);
		}

		/// <summary>
		/// Handles the finish picking media.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected void Handle_FinishPickingMedia(object sender, UIImagePickerMediaPickedEventArgs e) {
			bool isImage = false;

			switch (e.Info [UIImagePickerController.MediaType].ToString ()) {

			case "public.image":
				isImage = true;
				break;

			case "public.video":
				isImage = false;
				break;
			}

			if (isImage) {
				UIImage originalImage = e.Info [UIImagePickerController.OriginalImage] as UIImage;
				if (originalImage != null) {
					// Do something with image source
				}
			} else {
				NSUrl mediaURL = e.Info [UIImagePickerController.MediaURL] as NSUrl;
				if (mediaURL != null) {
					//
				}
			}

			imagePicker.DismissModalViewController (true);
		}

		/// <summary>
		/// Only for TextField class
		/// </summary>
		/// <returns>The data.</returns>
		private string GetDataFromTextField (TextField textfield)
		{
			foreach (var subview in textfield.Subviews) {
				if (subview is UIView) {
					foreach (var t in subview.Subviews) {
						if (t is UITextField) {
							var tf = t as UITextField;
							return tf.Text.ToString ();
						}
					}
				}
			}

			return "";
		}
	}
}