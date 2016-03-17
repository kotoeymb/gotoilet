
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
using System.Text.RegularExpressions;
using OHouse.Connectivity;

namespace OHouse
{
	public partial class FormViewController : UIViewController
	{
		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		bool connectivity;
		DataRequestManager drm;
		CLLocationCoordinate2D coords;
		UITapGestureRecognizer tapGesture;
		float keyboardOffset;

		public FormViewController (CLLocationCoordinate2D coords) : base ("FormViewController", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			Title = "Location";
			this.coords = coords;
			drm = new DataRequestManager ();
			keyboardOffset = 80f;
		}


		static bool IsValid (string value)
		{
			return Regex.IsMatch (value, @"^[a-zA-Z0-9]*$");
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

		}

		void dismissKeyboard ()
		{
			NameTextField.ResignFirstResponder ();
			DesTextField.ResignFirstResponder ();
		}

		void moveViewUp (bool moveUp)
		{
			UIView.BeginAnimations (null);
			UIView.SetAnimationDuration (0.5);
			var mainViewFrame = mainView.Frame;

			if (moveUp) {
				mainViewFrame.Y -= keyboardOffset;
				mainView.Frame = mainViewFrame;

			} else {
				mainViewFrame.Y += keyboardOffset;
				mainView.Frame = mainViewFrame;
			}

			UIView.CommitAnimations ();
		}

		NSObject _keyboardWillShow;
		NSObject _keyboardWillHide;

		void KeyboardWillShow (NSNotification noti)
		{
			Console.WriteLine ("Keyboard shown");
			if (mainView.Frame.Y >= 0) {
				moveViewUp (true);
			} else if (mainView.Frame.Y <= 0) {
				moveViewUp (false);
			}
		}

		void KeyboardWillHide (NSNotification noti)
		{
			Console.WriteLine ("Keyboard hide");
			if (mainView.Frame.Y >= 0) {
				moveViewUp (true);
			} else if (mainView.Frame.Y <= 0) {
				moveViewUp (false);
			}
		}



		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			moveViewUp (true);

			tapGesture = new UITapGestureRecognizer ();
			tapGesture.AddTarget (dismissKeyboard);
			View.AddGestureRecognizer (tapGesture);

			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, KeyboardWillShow);
			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, KeyboardWillHide);	

			if (coords.Latitude == 0 && coords.Longitude == 0) {
				var noConnectionAlert = new UIAlertView ("Your location", "Please wait until your location appear on the map, OK to dismiss this form", null, "Dismiss", null);
				noConnectionAlert.Show ();

				noConnectionAlert.Clicked += (object sender, UIButtonEventArgs e) => {
					if (e.ButtonIndex == noConnectionAlert.CancelButtonIndex) {
						this.DismissModalViewController (true);
					}
				};
			}

			CancleButton.TouchUpInside += (s, e) => {
				this.DismissModalViewController (true);
			};

			bgToilet.Image = UIImage.FromBundle ("images/background/bg-toilet-big");

			SaveButton.SetImage (UIImage.FromBundle ("images/icons/icon-save"), UIControlState.Normal);
			SaveButton.TitleEdgeInsets = new UIEdgeInsets (0, 12, 3, 0);
			SaveButton.TintColor = UIColor.FromRGB (7, 204, 0);
			SaveButton.Layer.BorderWidth = 1f;
			SaveButton.Layer.BorderColor = UIColor.FromRGB (7, 204, 0).CGColor;
			SaveButton.SetTitleColor (UIColor.FromRGB (7, 204, 0), UIControlState.Normal);
			SaveButton.Layer.CornerRadius = 5f;

			SaveButton.TouchUpInside += saveData;

			iconLocation.Image = UtilImage.GetColoredImage ("images/icons/icon-pin", UIColor.Black);

			lblLocation.Text = coords.Latitude + ", " + coords.Longitude;

			connectivity = true;
			ConnectionManager.ReachabilityChanged += UpdateStatus;
			initView ();

		}

		public override bool ShouldAutorotate ()
		{
			//////
			/// Disable auto rotation for this page
			return false;
		}

		void saveData (object s, EventArgs e)
		{
			
			string title = NameTextField.Text;
			string description = DesTextField.Text;
			int tlength = title.Length;
			int dlength = description.Length;

			Console.WriteLine (title);
			Console.WriteLine (description);

			UpdateStatus ();
			Console.WriteLine (coords.Latitude);

			if ((tlength >= 3 && tlength <= 15) && (dlength >= 3 && dlength <= 15)) {
				if ((title != "" && IsValid (title)) && (description != "" && IsValid (description))) {
					if (internetStatus == NetworkStatus.NotReachable) {
						connectivity = false;
						UIAlertView av = new UIAlertView (
							                 "Need Internet Connection",
							                 "Check Your Internet Connection",
							                 null,
							                 "Check",
							                 null
						                 );

						av.Show ();
					} else {
						connectivity = true;

						drm.RegisterSpot (new ToiletsBase (0, 0, title, description, "", coords.Longitude, coords.Latitude, 0, true), this);	

					}

				} else {

					UIAlertView av = new UIAlertView (
						                 "Data Require",
						                 "Please insert Only Character and number title & description for the location.",
						                 null,
						                 "Try Again!",
						                 null
					                 );

					av.Show ();
				}
			} else {

				UIAlertView av = new UIAlertView (
					                 "Data Limitation",
					                 "Please insert at least three char & max fifteen char for title and description.",
					                 null,
					                 "Try Again!",
					                 null
				                 );

				av.Show ();
			}
		}

		void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
			UpdateConnectivity ();
		}



		private void initView ()
		{
			if (!connectivity) {
				
					
				UIAlertView av = new UIAlertView (
					                 "Need Internet Connection first",
					                 "Check Your Internet Connection",
					                 null,
					                 "Ok",
					                 null
				                 );
				av.Show ();
				Console.WriteLine ("internet Connection timeout");
			} else {

				Console.WriteLine ("internet Connection successful");

			}
	
		}

		void UpdateConnectivity ()
		{
			connectivity = true;

			switch (remoteHostStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;
				break;
			}

			switch (internetStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;
				break;
			}

			switch (localWifiStatus) {
			case NetworkStatus.NotReachable:
				connectivity = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connectivity = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connectivity = true;
				break;
			default:
				connectivity = true;

				break;
			}


			initView ();
		}
	
	}

}