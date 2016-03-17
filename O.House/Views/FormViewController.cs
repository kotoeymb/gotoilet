
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
		bool connection;
		TextField tf;
		TextField stf;
		DataRequestManager drm;


		public FormViewController (CLLocationCoordinate2D coords) : base ("FormViewController", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			Title = "Location";

			drm = new DataRequestManager ();




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
		public override void ViewDidUnload(){
			base.ViewDidUnload ();
			ResignFirstResponder ();
		}
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			CancleButton.TouchUpInside += (s, e) => {
				this.DismissModalViewController (true);
			};

			saveCheckData();
			connection = true;
			ConnectionManager.ReachabilityChanged += UpdateStatus;
			initView ();

		}
		private void saveCheckData()
		{
			SaveButton.TouchUpInside += (object sender, EventArgs e) => {
				string title = NameTextField.Text;
				string description = DesTextField.Text;
				int tlength = title.Length;
				int dlength = description.Length;

				Console.WriteLine (title);
				Console.WriteLine(description);

				UpdateStatus ();


		
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
						Console.WriteLine ("internet Connection Successful");
						//drm.RegisterSpot (new ToiletsBase (0, 0, title, description, "", coords.Longitude, coords.Latitude, 0, true), this);	

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
			};
		}
				
		void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
			UpdateConnectivity ();
		}



		private void initView()
		{
			if (!connection) {
				
					
					UIAlertView av = new UIAlertView (
						                "Need Internet Connection first",
						                "Check Your Internet Connection",
						                null,
						                "Ok",
						                null
					                );
					av.Show ();
				Console.WriteLine ("internet Connection timeout");
				}
	
			else{

				Console.WriteLine ("internet Connection successful");


			}
	
		}
	


	
		void UpdateConnectivity ()
		{
			connection = true;

			switch (remoteHostStatus) {
			case NetworkStatus.NotReachable:
				connection = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connection = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connection = true;
				break;
			default:
				connection = true;
				break;
			}

			switch (internetStatus) {
			case NetworkStatus.NotReachable:
				connection = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connection = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connection = true;
				break;
			default:
				connection = true;
				break;
			}

			switch (localWifiStatus) {
			case NetworkStatus.NotReachable:
				connection = false;
				break;
			case NetworkStatus.ReachableViaCarrierDataNetwork:
				connection = true;
				break;
			case NetworkStatus.ReachableViaWifiNetwork:
				connection = true;
				break;
			default:
				connection = true;

				break;
			}


			initView ();
		}
	
	}

}