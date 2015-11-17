using System;
using CoreGraphics;
using Foundation;
using UIKit;
using System.Drawing;
using Utils;
using Facebook.LoginKit;
using Facebook.CoreKit;
using System.Collections.Generic;
using MonoTouch.Dialog.Utilities;
using MonoTouch.SlideoutNavigation;
using Commons;

namespace OHouse
{
	/// <summary>
	/// FB container view.
	/// </summary>
	public partial class FBContainerView : UIView
	{
		List<string> readPermissions = new List<string> { "public_profile" };
		LoginButton loginButton;
		ProfilePictureView profileView;
		UILabel nameLabel;
		Common common = new Common ();
		float menuWidth = 290;

		/// <summary>
		/// Initializes a new instance of the <see cref="GoToilet.FBContainerView"/> class.
		/// </summary>
		/// <param name="frame">Frame.</param>
		public FBContainerView (RectangleF frame) : base (frame)
		{
			BackgroundColor = UIColor.FromRGB (41, 41, 41);

			Profile.Notifications.ObserveDidChange ((sender, e) => {
				if (e.NewProfile == null) {
					nameLabel.Text = "Welcome!";
				} else {
					nameLabel.Text = e.NewProfile.Name;
				}
			});

			// The user image profile is set automatically once is logged in
			//profileView = new ProfilePictureView (new CGRect (15, ((float)this.Frame.Height / 2) - 40, 80, 80));
			profileView = new ProfilePictureView (new CGRect (menuWidth / 2 - 40, ((float)this.Frame.Height / 2 - 40) - 20, 80, 80));

			// Set the Read and Publish permissions you want to get
			//loginButton = new LoginButton (new CGRect (110, 60, 100, 30)) {
			loginButton = new LoginButton (new CGRect (menuWidth / 2 - 50, (float)this.Frame.Height / 2 + 60, 100, 30)) {	
				LoginBehavior = LoginBehavior.Native,
				//LoginBehavior = LoginBehavior.SystemAccount,
				ReadPermissions = readPermissions.ToArray (),

			};

			//nameLabel = new UILabel (new RectangleF (110, -10, (float)this.Frame.Width, 100)) {
			nameLabel = new UILabel (new RectangleF (menuWidth / 2 - menuWidth / 2, (float)this.Frame.Height / 2 + 30, menuWidth, 30)) {
				TextAlignment = UITextAlignment.Center,
				TextColor = common.White,
				BackgroundColor = common.Clear,
				Font = common.Font16F
			};

			//loginButton.BackgroundColor = common.ColorStyle_1;
			loginButton.SetBackgroundImage (UIImage.FromBundle ("images/background/bg-0"), UIControlState.Normal);
			loginButton.SetBackgroundImage (UIImage.FromBundle ("images/background/bg-0"), UIControlState.Selected);

			// Handle actions once the user is logged in
			loginButton.Completed += (sender, e) => {
				if (e.Error != null) {
					// Handle if there was an error
					new UIAlertView ("Login", e.Error.Description, null, "Ok", null).Show ();
				}

				if (e.Result.IsCancelled) {
					// Handle if the user cancelled the login request
					//new UIAlertView ("Login", "The user cancelled the login", null, "Ok", null).Show ();
				}

				// Handle your successful login
				//new UIAlertView ("Login", "Success!!", null, "Ok", null).Show ();
			};

			// Handle actions once the user is logged out
			loginButton.LoggedOut += (sender, e) => {
				// Handle your logout
				nameLabel.Text = "Welcome!";
			};

			profileView.Layer.CornerRadius = profileView.Frame.Size.Width / 2;
			profileView.Layer.BorderWidth = 1f;
			profileView.Layer.BorderColor = new CGColor (52, 52, 52);
			profileView.ClipsToBounds = true;

			nameLabel.Text = "Welcome!";

			// If you have been logged into the app before, ask for the your profile name
			if (AccessToken.CurrentAccessToken != null) {
				var request = new GraphRequest ("/me?fields=name", null, AccessToken.CurrentAccessToken.TokenString, null, "GET");
				request.Start ((connection, result, error) => {
					// Handle if something went wrong with the request
					if (error != null) {
						//new UIAlertView ("Error...", error.Description, null, "Ok", null).Show ();
						new UIAlertView ("Need internet connection", "Please connect to internet so that you can use this app's full features.", null, "OK", null).Show ();
						return;
					}

					// Get your profile name
					var userInfo = result as NSDictionary;
					nameLabel.Text = userInfo ["name"].ToString ();
				});
			}
		
//			AddSubview (blurView);
			AddSubview (loginButton);
			AddSubview (nameLabel);
			AddSubview (profileView);
		}
	}
}

