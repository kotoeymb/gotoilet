using System;
using System.Drawing;
using Foundation;

using UIKit;
using OHouse;
using Utils;
using CoreGraphics;
using Facebook.CoreKit;
using Facebook.LoginKit;
using System.Collections.Generic;
using OHouse.DRM;

namespace O.House
{
	public partial class MenuSBViewController : UIViewController
	{
		List<string> readPermissions = new List<string> { "email" };
		ProfilePictureView profileView;
		DataRequestManager drm;
		LoginManager lm;

		public MenuSBViewController () : base ("MenuSBViewController", null)
		{


		}

		private NSObject _profileDidChangeEventArgs;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			drm = new DataRequestManager ();

			Profile.Notifications.ObserveDidChange ((sender, e) => {
				if (e.NewProfile == null)
					return;
				

				fbProfileName.Text = e.NewProfile.Name;
				// register user
				drm.RegisterUser (e.NewProfile.UserID);
				new UIAlertView ("Login", "Welcome back! " + fbProfileName.Text.ToString (), null, "OK", null).Show ();
			});

//			if (AccessToken.CurrentAccessToken.) {
//				Console.WriteLine ("current access token is available");
//			}

			// setup table

			menuView.Source = new TableSource (NavigationController);
			menuView.RowHeight = 80f;
			menuView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

			// setup facebook
			profileView = new ProfilePictureView (new CGRect (0, 0, 70, 70));
			fbProfilePicture.AddSubview (profileView);

//			AccessToken.Notifications.ObserveDidChange ((sender, e) => {
//				if (e.NewToken == null)
//					return;
//
//				if (Profile.CurrentProfile != null) {
//					Profile p = Profile.CurrentProfile;
//					fbProfileName.Text = p.Name;
//
//					drm.RegisterUser (p.UserID);
//
//					fbLoginButton.SetBackgroundImage (UtilImage.GetColoredImage ("images/icons/icon-logout", UIColor.White), UIControlState.Normal);
////					fbLoginButton.TouchUpInside -= LoginClick;
////					fbLoginButton.TouchUpInside += LogoutClick;
//
//					new UIAlertView ("Login", "Welcome back! " + fbProfileName.Text.ToString (), null, "OK", null).Show ();
//
//				}

//			});

			if (AccessToken.CurrentAccessToken != null) {
				fbLoginButton.SetTitle ("Logout", UIControlState.Normal);
				fbProfileName.Text = Profile.CurrentProfile.Name;
			} else {
				fbLoginButton.SetTitle ("Login", UIControlState.Normal);
				fbProfileName.Text = "Welcome";
			}

//			Profile.Notifications.ObserveDidChange ((sender, e) => {
//				if (e.NewProfile == null)
//					return;
//
//				fbProfileName.Text = e.NewProfile.Name;
//
//
//				// register user
//				drm.RegisterUser (e.NewProfile.UserID);
//
//
//				new UIAlertView ("Login", "Welcome back! " + fbProfileName.Text.ToString (), null, "OK", null).Show ();
//			});



			fbProfilePicture.Layer.CornerRadius = fbProfilePicture.Frame.Width / 2;
			fbProfilePicture.Layer.BorderWidth = 1f;
			fbProfilePicture.Layer.BorderColor = UIColor.White.CGColor;
			fbProfilePicture.ClipsToBounds = true;

			fbLoginButton.TouchUpInside += LoginClick;

		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public void LoginClick (object s, EventArgs e)
		{
			lm = new LoginManager ();

			if (AccessToken.CurrentAccessToken != null) {
				
				lm.LogOut ();
				fbProfileName.Text = "Welcome";
			} else {

				lm.LoginBehavior = LoginBehavior.Native;
				lm.LogInWithReadPermissions (readPermissions.ToArray (), (LoginManagerLoginResult result, NSError error) => {
					if (error != null) {
						Console.WriteLine ("Error logging in " + error.ToString ());
					} else if (result.IsCancelled) {
						Console.WriteLine ("Use cancelled ");
				
					} else {
						Console.WriteLine ("Good job!");
					}
				});
			}
		}

		private void profileDidChangeEventArgs (NSNotification notification)
		{
			Profile.Notifications.ObserveDidChange ((sender, e) => {
				if (e.NewProfile == null)
					return;
			
				fbProfileName.Text = e.NewProfile.Name;

				// register user
				drm.RegisterUser (e.NewProfile.UserID);
				new UIAlertView ("Login", "Welcome back! " + fbProfileName.Text.ToString (), null, "OK", null).Show ();
			});
		}
	}

	public class TableSource : UITableViewSource
	{
		UINavigationController navi;

		public TableSource (UINavigationController parent)
		{
			this.navi = parent;
		}

		public override nint RowsInSection (UITableView tableview, nint section)
		{
			return 3;
		}

		public override nint NumberOfSections (UITableView tableView)
		{
			return 1;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = null;

			cell = tableView.DequeueReusableCell (MenuCellDesign.Key.ToString ());
			if (cell == null) {
				cell = MenuCellDesign.Create ();
			}

			CreateMenu (cell, indexPath);
			cell.SelectionStyle = UITableViewCellSelectionStyle.None;

			return cell;
		}

		private void CreateMenu (UITableViewCell cell, NSIndexPath indexPath)
		{
			int index = indexPath.Row;
			MenuCellDesign c = ((MenuCellDesign)cell);

			UIImage iconFind = UtilImage.GetColoredImage ("images/icons/icon-find", UIColor.FromRGB (255, 255, 255));
			UIImage iconTL = UtilImage.GetColoredImage ("images/icons/icon-timeline", UIColor.FromRGB (255, 255, 255));
			UIImage iconOL = UtilImage.GetColoredImage ("images/icons/icon-offline", UIColor.FromRGB (255, 255, 255));

			switch (index) {
			case 0:
				c.MenuLabel.Text = "Timeline";
				c.MenuIcon.Image = iconTL;
				break;
			case 1:
				c.MenuLabel.Text = "Find";
				c.MenuIcon.Image = iconFind;
				break;
			case 2:
				c.MenuLabel.Text = "Offline";
				c.MenuIcon.Image = iconOL;
				break;
			default:
				break;
			}
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			int index = indexPath.Row;

			switch (indexPath.Row) {
			case 0:
				navi.PushViewController (new TimelineViewController (), true);
				break;
			case 1:
				navi.PushViewController (new MapViewController (), true);
				break;
			case 2:
				navi.PresentViewController (new OfflineViewController (), true, null);
				break;
			default:
				Console.WriteLine ("DUEE");
				break;
			}
		}
	}
}


