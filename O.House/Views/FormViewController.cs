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
			return Regex.IsMatch (value, @"^[a-zA-Z0-9\s]*$");
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

			UpdateStatus (null, null);
			connectivity = true;

			ConnectionManager.ReachabilityChanged += UpdateStatus;

			tapGesture = new UITapGestureRecognizer ();
			tapGesture.AddTarget (dismissKeyboard);
			View.AddGestureRecognizer (tapGesture);

			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillShowNotification, KeyboardWillShow);
			NSNotificationCenter.DefaultCenter.AddObserver (UIKeyboard.WillHideNotification, KeyboardWillHide);	

			////// Check for user location
			/// Dismiss on error
			if (coords.Latitude == 0 && coords.Longitude == 0) {
				var noConnectionAlert = new UIAlertView ("Your location", "Please wait until your location appear on the map, OK to dismiss this form", null, "Dismiss", null);
				noConnectionAlert.Show ();

				noConnectionAlert.Clicked += (object sender, UIButtonEventArgs e) => {
					if (e.ButtonIndex == noConnectionAlert.CancelButtonIndex) {
						this.DismissModalViewController (true);
					}
				};
			}

		
			bgToilet.Image = UIImage.FromBundle ("images/background/bg-toilet-big");
			iconLocation.Image = UtilImage.GetColoredImage ("images/icons/icon-pin", UIColor.Black);
			lblLocation.Text = coords.Latitude + ", " + coords.Longitude;


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
			UpdateStatus ();
			string error = "";

			string title = NameTextField.Text;
			string description = DesTextField.Text;
			int tlength = title.Length;
			int dlength = description.Length;

			bool isNameBlank = false;
			bool isDescpBlank = false;

			bool isSaveSuccess = false;

			////// Connectivity
			/// Check for connection first
			if (!connectivity) {
				error += "Please connect to internet\n";
			}

			////// Blank check
			if (tlength <= 0) {
				error += "Name is blank\n";
				isNameBlank = true;
			}
			if (dlength <= 0) {
				error += "Description is blank\n";
				isDescpBlank = true;
			}
			////// Length check
			if (!isNameBlank && tlength < 3) {
				error += "Name is less than 3\n";
			}
			if (!isNameBlank && tlength > 15) {
				error += "Name is greater than 15\n";
			}
			if (!isDescpBlank && dlength > 50) {
				error += "Description is over 50\n";
			}
			////// Check illegal characters
			if (!isNameBlank && !IsValid (title)) {
				error += "Name contain some illegal chars\n";
			}
			if (!isDescpBlank && !IsValid (description)) {
				error += "Description contain some illegal chars\n";
			}

			////// Check for error
			/// If there're no errors
			if (error.Length <= 0 || error == null || error == "") {
				Console.WriteLine ("Saved!");
				isSaveSuccess = drm.RegisterSpot (new ToiletsBase (0, 0, title, description, "", coords.Longitude, coords.Latitude, 0, true));

			} else {
				new UIAlertView (
					"Invalid!",
					error,
					null,
					"OK",
					null
				).Show();
			}

			if (isSaveSuccess) {
				new UIAlertView (
					"Thank you!",
					"We have completed your submission! Please continue to support us. We humbly request you to rate our app 5 stars!!",
					null,
					"OK",
					null
				).Show();
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
			CancleButton.TouchUpInside += (s, e) => {
				this.DismissModalViewController (true);
			};

			SaveButton.Layer.BorderWidth = 1f;
			SaveButton.TouchUpInside -= saveData;
			SaveButton.Layer.CornerRadius = 5f;
			SaveButton.TitleEdgeInsets = new UIEdgeInsets (0, 12, 3, 0);
			SaveButton.SetImage (UIImage.FromBundle ("images/icons/icon-save"), UIControlState.Normal);

			if (!connectivity) {
				SaveButton.Layer.BorderColor = UIColor.Red.CGColor;
				SaveButton.SetTitle ("No connection", UIControlState.Normal);
				SaveButton.SetTitleColor (UIColor.Red, UIControlState.Normal);
				SaveButton.TouchUpInside -= saveData;
				SaveButton.TintColor = UIColor.Red;
			} else {
				SaveButton.Layer.BorderColor = UIColor.FromRGB (7, 204, 0).CGColor;
				SaveButton.SetTitle ("Save", UIControlState.Normal);
				SaveButton.SetTitleColor (UIColor.FromRGB (7, 204, 0), UIControlState.Normal);
				SaveButton.TouchUpInside += saveData;
				SaveButton.TintColor = UIColor.FromRGB (7, 204, 0);
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