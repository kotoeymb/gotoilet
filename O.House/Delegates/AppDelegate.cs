using Foundation;
using UIKit;
using MonoTouch.SlideoutNavigation;
using MonoTouch.Dialog;
using OHouse;
using Facebook;
using Facebook.CoreKit;
using System;
using System.Diagnostics;
using CoreLocation;
using Utils;
using Commons;
using OHouse.Connectivity;
using OHouse.DRM;

namespace OHouse
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		string appId = "923120891088664";
		string appName = "Toil­e­t­G­uide - Test1";

		UIWindow window;
		UIBarButtonItem menuButton;
		Common common;
		DataRequestManager drm;

		public static bool connectivity;
		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;

		public SlideoutNavigationController Menu { get; private set; }

		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			//common = new Common ();

			Profile.EnableUpdatesOnAccessTokenChange (true);
			Settings.AppID = appId;
			Settings.DisplayName = appName;

			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

			// Setting up navigation
			UINavigationBar.Appearance.BarTintColor = Common.ColorStyle_1;
			UINavigationBar.Appearance.TintColor = UIColor.White;
			UINavigationBar.Appearance.SetTitleTextAttributes (
				new UITextAttributes { TextColor = UIColor.White, Font = Common.Font16F }
			);

			// create a new window instance based on the screen size
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			menuButton = new UIBarButtonItem (UIImage.FromBundle ("images/button/three_lines"), UIBarButtonItemStyle.Plain, (s, e) => {
			});

			Menu = new SlideoutNavigationController ();

			Menu.MainViewController = new MainNavigationController (new MapViewController (), Menu, menuButton);
			Menu.MenuViewController = new MenuNavigationController (new MenuViewController (), Menu) { NavigationBarHidden = true };

			// If you have defined a root view controller, set it here:
			window.RootViewController = Menu;

			// make the window visible
			window.MakeKeyAndVisible ();

			///// start
			///// check internet connection status and availability
			//UpdateStatus ();
			UpdateStatus();

			if (internetStatus == NetworkStatus.NotReachable) {
				connectivity = false;
			} else {

				//////
				/// Update local plist
				drm = new DataRequestManager();
				drm.UpdateList();

				connectivity = true;
			}
			///// end

//			dict.Add (new NSString("CONNECTION_AVAILABILITY"), new NSString("NO"));
//
//			defaults = NSUserDefaults.StandardUserDefaults;
//			defaults.RegisterDefaults (dict);

			return ApplicationDelegate.SharedInstance.FinishedLaunching (application, launchOptions);
			//return true;
		}

		public override void OnResignActivation (UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground (UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
			Console.WriteLine("App DidEnterBackground");
		}

		public override void WillEnterForeground (UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
			Console.WriteLine("App WillEnterBackground");

			// Update connection status first
			UpdateStatus();

			if (internetStatus == NetworkStatus.NotReachable) {
				UIAlertView noConnectionAlert = new UIAlertView (
					"Disconnected", 
					"This application need constant internet connection to show up to date correct location",
					null,
					"Cancel",
					new string[]{"Settings"}
				);
				noConnectionAlert.Show ();

				// Given 2 options "Cancel" and "Setting", this event is for "Setting" redirection
				noConnectionAlert.Clicked += (object sender, UIButtonEventArgs e) => {
					if(e.ButtonIndex != noConnectionAlert.CancelButtonIndex) {
						UIApplication.SharedApplication.OpenUrl(new NSUrl(UIApplication.OpenSettingsUrlString));
					}
				};

				// Set connectivity to false
				connectivity = false;
			} else {
				connectivity = true;
			}
		}

		public override void OnActivated (UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate (UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}

		public override bool OpenUrl (UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			return ApplicationDelegate.SharedInstance.OpenUrl (application, url, sourceApplication, annotation);
		}
			
		void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
		}
			
	}
}