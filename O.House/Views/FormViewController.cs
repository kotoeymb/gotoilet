
using System;
using System.Drawing;
using System.IO;

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
using OHouse.DRM;
using AssetsLibrary;

namespace OHouse
{
	public partial class FormViewController : UIViewController
	{
		Common common = new Common ();
		TextField tf;
		TextField stf;
		UIImagePickerController imagePicker;
		DataRequestManager drm;
		UIImageView thumbnail;
		UILabel imageURL;

		public FormViewController (CLLocationCoordinate2D coords) : base ("FormViewController", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			Title = "Location";

			drm = new DataRequestManager ();

			this.NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem (
					UIImage.FromBundle ("images/icons/icon-mark"), 
					UIBarButtonItemStyle.Plain, 
					(ss, ee) => {

						if (GetDataFromTextField (tf) != "" && GetDataFromTextField (stf) != "") {
							drm.RegisterSpot (new ToiletsBase (0, 0, GetDataFromTextField (tf), GetDataFromTextField (stf), "", coords.Longitude, coords.Latitude, 0, true), this);
						} else {
							UIAlertView av = new UIAlertView (
								                 "Data Require",
								                 "Please insert at least title for the location.",
								                 null,
								                 "Alright!",
								                 null
							                 );

							av.Show ();
						}
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
			UIScrollView sv = new UIScrollView (this.NavigationController.View.Bounds);

			CGRect screen = this.NavigationController.View.Bounds;
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
				common.Black, 
				"Title", 
				UtilImage.GetColoredImage ("images/icons/icon-notes", common.ColorStyle_1)
			);

			// Subtitle
			CGRect stfRec = new CGRect (items.X, tf.Frame.Y + tf.Frame.Height + 24, items.Width, items.Height);
			stf = new TextField (
				stfRec,
				common.Black, 
				"Description", 
				UtilImage.GetColoredImage ("images/icons/icon-notes", common.ColorStyle_1)
			);

			imageURL = new UILabel ();

			// Camera upload
			CGRect cameraRec = new CGRect (stf.Frame.X, stf.Frame.Y + stf.Frame.Height + 24, stf.Frame.Width, 40);
			UIButton camera = new UIButton (cameraRec);

			CGRect thumbnailRec = new CGRect (camera.Frame.X, camera.Frame.Y + camera.Frame.Height + 15, 75, 75);
			thumbnail = new UIImageView (thumbnailRec);

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
							imagePicker = new UIImagePickerController ();
							imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;

							// set what media types
							imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes (UIImagePickerControllerSourceType.PhotoLibrary);

							imagePicker.FinishedPickingMedia += Handle_FinishPickingMedia;
							imagePicker.Canceled += Handle_Canceled;

							// show the picker
							NavigationController.PresentModalViewController (imagePicker, true);
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



			sv.AddSubviews (tf, stf, camera, thumbnail);

			View.AddSubview (sv);
		}

		/// <summary>
		/// Handles the finish picking media.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected void Handle_FinishPickingMedia (object sender, UIImagePickerMediaPickedEventArgs e)
		{
			bool isImage = false;

			switch (e.Info [UIImagePickerController.MediaType].ToString ()) {

			case "public.image":
				isImage = true;
				break;

			case "public.video":
				isImage = false;
				break;
			}

			//NSUrl ip = e.Info [new NSString ("UIImagePickerControllerReferenceUrl")] as NSUrl;
			//NSUrl ip = e.Info[UIImagePickerController] as NSUrl;
			Console.WriteLine(Path.GetFileName(e.Info[UIImagePickerController.ReferenceUrl].ToString()));

			//Console.WriteLine (ip.ToString ());
			string ip = e.Info[UIImagePickerController.ReferenceUrl].ToString();

			if (isImage) {
				UIImage originalImage = e.Info [UIImagePickerController.OriginalImage] as UIImage;

				imageURL.Text = ip.ToString ();
				Console.WriteLine (imageURL.Text);

				if (originalImage != null) {
					// Do something with image source
					thumbnail.Image = originalImage;

				}
			} else {
				NSUrl mediaURL = e.Info [UIImagePickerController.MediaURL] as NSUrl;
				if (mediaURL != null) {
					//
				}
			}

			imagePicker.DismissModalViewController (true);
		}

		void Handle_Canceled (object sender, EventArgs e)
		{
//			Console.WriteLine ("picker cancelled");
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