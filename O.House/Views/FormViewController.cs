
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

						if
							
							(GetDataFromTextField (tf) !=""&& GetDataFromTextField (stf) != "") {
							drm.RegisterSpot (new ToiletsBase (0, 0, GetDataFromTextField(tf),GetDataFromTextField(stf),"", coords.Longitude, coords.Latitude, 0, true), this);

						} 
						else {
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
			);//end of this.NavigationItem.SetRightBarButtonItem

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

			TextField tex = new TextField (new CoreGraphics.CGRect (10, 180, 300, 60),Common.Blue, 
			"Title", UtilImage.GetColoredImage ("images/icons/icon-notes", Common.ColorStyle_1));
			tex.BackgroundColor = UIColor.Gray;
			View.Add (tex);
		}

	
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
				Common.Black, 
				"Title", 
				UtilImage.GetColoredImage ("images/icons/icon-notes", Common.ColorStyle_1)
			);

			// Subtitle
			CGRect stfRec = new CGRect (items.X, tf.Frame.Y + tf.Frame.Height + 24, items.Width, items.Height);
			stf = new TextField (
				stfRec,
				Common.Black, 
				"Description", 
				UtilImage.GetColoredImage ("images/icons/icon-notes", Common.ColorStyle_1)
			);

			sv.AddSubviews (tf, stf);

			View.AddSubview (sv);
		}
			
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