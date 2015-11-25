
using System;
using System.Drawing;

using Foundation;
using UIKit;
using CoreGraphics;
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

		public FormViewController () : base ("FormViewController", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			Title = "Location";

			this.NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem (
					UIImage.FromBundle ("images/icons/icon-mark"), UIBarButtonItemStyle.Plain, (ss, ee) => {
					NavigationController.PushViewController (new SubmitViewController (), true);	
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
			TextField tf = new TextField (
				new CGRect (items.X, items.Y, items.Width, items.Height), 
				common.ColorStyle_1, 
				"Title", 
				UtilImage.GetColoredImage ("images/icons/icon-notes", common.ColorStyle_1)
			);

			// Subtitle
			TextField stf = new TextField (
				new CGRect (items.X, tf.Frame.Y + tf.Frame.Height + 24, items.Width, items.Height), 
				common.ColorStyle_1, 
				"Description", 
				UtilImage.GetColoredImage ("images/icons/icon-notes", common.ColorStyle_1)
			);

			// Camera upload
			UIButton camera = new UIButton (
				new CGRect (stf.Frame.X, stf.Frame.Y + stf.Frame.Height + 24, stf.Frame.Width, 40)
			);

			UIImageView cameraIcon = new UIImageView (
				new CGRect (camera.Frame.Width - 16 - 10, camera.Frame.Height / 2 - 8, 16, 16)
			) {
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
				actionSheetAlert.AddAction (UIAlertAction.Create ("Choose from gallery", UIAlertActionStyle.Default, (action) => Console.WriteLine ("Item One pressed.")));
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
	}
}