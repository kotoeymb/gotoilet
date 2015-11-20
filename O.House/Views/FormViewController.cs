
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

namespace OHouse
{
	public partial class FormViewController : UIViewController
	{
//		UIButton cancelBtn;
//		UIButton doneBtn;
//		UILabel title;
//		Common common = new Common ();
//		UISwipeGestureRecognizer gesture;

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
						this.DismissModalViewController(true);
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

			SetupUI ();
		}

		void SetupUI ()
		{

			View.UserInteractionEnabled = true;

			CGRect screen = View.Bounds;
			float sw = (float)screen.Width;
			float sh = (float)screen.Height;
			CGRect ssize = new CGRect (0, sh, sw, sh - 100);

//			var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Light);
//			var blurView = new UIVisualEffectView (blur) {
//				Frame = screen
//			};

//			View.BackgroundColor = UIColor.FromRGBA (18, 140, 255, 100);
//			View.AddSubview (blurView);
//			View.BackgroundColor = UIColor.FromRGB (18, 140, 255);
			View.BackgroundColor = UIColor.White;
			View.Frame = ssize;

//			cancelBtn = new UIButton (new CGRect (0, 24, 40, 40));
//			cancelBtn.SetImage (UIImage.FromBundle ("images/icons/icon-cross"), UIControlState.Normal);
//			cancelBtn.Font = common.Font13F;
//			//cancelBtn.BackgroundColor = common.BlackishWithAlpha;
//			cancelBtn.BackgroundColor = UIColor.FromRGB(255, 162, 69);
//			cancelBtn.SetTitleColor (common.White, UIControlState.Normal);

			// For custom popup
//			cancelBtn.TouchUpInside += (sender, e) => {
//				UIView.Animate (0.2, 0.0, UIViewAnimationOptions.CurveEaseIn, () => {
//					View.Frame = new CGRect (new PointF (0, sh), new CGSize (sw, sh - common.PopUpDistance));
//				}, () => {
//					this.WillMoveToParentViewController (null);
//					this.View.RemoveFromSuperview ();
//					this.RemoveFromParentViewController ();
//				});
//			};

			// For present view controller
//			cancelBtn.TouchUpInside += (sender, e) => {
//				this.DismissModalViewController (true);
//			};
//
//			doneBtn = new UIButton (new CGRect (sw - 40, 24, 40, 40));
//			doneBtn.SetImage (UIImage.FromBundle ("images/icons/icon-mark"), UIControlState.Normal);
//			doneBtn.Font = common.Font13F;
//			//doneBtn.BackgroundColor = common.BlackishWithAlpha;
//			doneBtn.BackgroundColor = UIColor.FromRGB(255, 162, 69);
//
//			doneBtn.TouchUpInside += (sender, e) => {
//				// Do form submit stuffs
//				NavigationController.PushViewController (new SubmitViewController (), true);
//			};
//
//			title = new UILabel (new CGRect (0, 24, sw, 40));
//			title.Text = "Add location";
//			title.TextAlignment = UITextAlignment.Center;
//			title.Font = common.Font16F;
//			title.TextColor = common.White;
//			title.BackgroundColor = UIColor.FromRGB(255, 162, 69);
//
//			UIView statusBar = new UIView (new CGRect (0, 0, (int)sw, 64));
//			statusBar.BackgroundColor = UIColor.FromRGB (255, 162, 69);

			// Also work from swipe down
//			gesture = new UISwipeGestureRecognizer (() => {
//				UIView.Animate (0.2, 0.0, UIViewAnimationOptions.CurveEaseIn, () => {
//					View.Frame = new CGRect (new PointF (0, sh), new CGSize (sw, sh - common.PopUpDistance));
//				}, () => {
//					// On completion
//					this.WillMoveToParentViewController (null);
//					this.View.RemoveFromSuperview ();
//					this.RemoveFromParentViewController ();
//				});
//			});

			//
			//			// Check user logged in or not
			//			if (AccessToken.CurrentAccessToken != null) {
			//				// Show form
			//			} else {
			//				// Request to login
			//			}

//			gesture.Direction = UISwipeGestureRecognizerDirection.Down;

			//View.AddGestureRecognizer (gesture);
			//View.AddSubview (statusBar);
			//View.AddSubview (title);
			//View.AddSubview (cancelBtn);
			//View.AddSubview (doneBtn);
		}
	}
}

