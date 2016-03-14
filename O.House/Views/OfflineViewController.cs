using System;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;

using Foundation;
using CoreGraphics;

using UIKit;
using OHouse;
using OHouse.Connectivity;
using OHouse.DRM;

using Commons;

namespace O.House
{
	public partial class OfflineViewController : UIViewController
	{
		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		bool connectivity;
		DataRequestManager drm;
		UIActivityIndicatorView loadingView;
		UIProgressView progressView;
		string jsonData;
		int dataSize;

		public OfflineViewController () : base ("OfflineViewController", null)
		{
			
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			connectivity = true;

			UpdateStatus (null, null);

			//////
			/// Monitoring connectivity status
			ConnectionManager.ReachabilityChanged += UpdateStatus;
			initView ();
		}

		public void closeBtnEvent (object sender, EventArgs e)
		{
			this.DismissModalViewController (true);
		}

		/// <summary>
		/// Inits the view.
		/// </summary>
		private void initView ()
		{
			closeBtn.TouchUpInside += (object sender, EventArgs e) => {
				this.DismissModalViewController (true);
			};

			//////
			/// Clear actions on startup to avoid multiple action assigned
			downloadBtn.TouchUpInside -= startDownload;

			if (!connectivity) {
				downloadBtn.Layer.BorderColor = UIColor.Red.CGColor;
				downloadBtn.SetTitleColor (UIColor.Red, UIControlState.Normal);
				downloadBtn.SetTitle ("Need connection", UIControlState.Normal);
				downloadBtn.TouchUpInside -= startDownload;
			} else {
				downloadBtn.BackgroundColor = UIColor.White;
				downloadBtn.Layer.BorderWidth = 1;
				downloadBtn.Layer.BorderColor = Common.ColorStyle_1.CGColor;
				downloadBtn.SetTitleColor (Common.ColorStyle_1, UIControlState.Normal);
				downloadBtn.SetTitle ("Check", UIControlState.Normal);
				downloadBtn.TouchUpInside += startDownload;
			}
		}

		/// <summary>
		/// Updates the local file.
		/// </summary>
		public void updateLocalFile ()
		{
			Console.WriteLine ("Updating local list from server ...");
			List<ToiletsBase> dataFromServer;

			drm = new DataRequestManager ();

			NSMutableDictionary dataToWrite = new NSMutableDictionary ();

			NSFileManager fileMgn = NSFileManager.DefaultManager;
			NSUrl[] paths = fileMgn.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
			string documentsDirectory = paths [0].Path;
			string fileName = Path.Combine (documentsDirectory, "Update.plist");
			NSError error = null;

			//////
			/// Create file if not exist
			if (!fileMgn.FileExists (fileName)) {
				var bundle = NSBundle.MainBundle.PathForResource ("database/Update", "plist");
				fileMgn.Copy (bundle, fileName, out error);
			}

			//////
			/// Check latest datas are available
			//List<ToiletsBase> sync = drm.GetToiletList (fileName);

			bool willSynData = true;

			if (jsonData.Length <= 0) {
				willSynData = false;
			}

			if (willSynData) {
				//////
				/// retrieve data from server
				dataFromServer = drm.GetDataListJSON (jsonData, 0, 0);
				foreach (var obj in dataFromServer) {

					Console.WriteLine ("Retriving data from server ...");

					//////
					/// add object to dataToWrite (NSMutableDictionary)
					NSMutableDictionary temp = new NSMutableDictionary ();
					temp.Add (new NSString ("spot_id"), new NSString (obj.spot_id.ToString ()));
					temp.Add (new NSString ("vote_cnt"), new NSString (obj.vote_cnt.ToString ()));
					temp.Add (new NSString ("title"), new NSString (obj.title.ToString ()));
					temp.Add (new NSString ("sub_title"), obj.sub_title == null ? new NSString ("Sub title not available.") : new NSString (obj.sub_title.ToString ()));
					temp.Add (new NSString ("picture"), obj.picture == null ? new NSString ("null") : new NSString (obj.picture.ToString ()));
					temp.Add (new NSString ("longitude"), new NSString (obj.longitude.ToString ()));
					temp.Add (new NSString ("latitude"), new NSString (obj.latitude.ToString ()));

					dataToWrite.Add (new NSString (obj.spot_id.ToString ()), temp);
				}

				NSData plistData = NSPropertyListSerialization.DataWithPropertyList (dataToWrite, NSPropertyListFormat.Xml, NSPropertyListWriteOptions.MutableContainers, out error);

				if (plistData != null) {

					Console.WriteLine ("Checking file existence ...");
					if (fileMgn.FileExists (fileName)) {
						Console.WriteLine ("File exists and saving to local plist ...");
						if (plistData.Save (fileName, true)) {
							Console.WriteLine ("Updated!");
						} else {
							Console.WriteLine ("Error updating file!");
						}
					} else {
						Console.WriteLine ("File to Update does not exist!");
					}
				} else {
					Console.WriteLine ("Error!! NSData");
				}
			} else {
				Console.WriteLine ("Data already up-to-date");
			}
		}

		/// <summary>
		/// Starts the download.
		/// </summary>
		/// <param name="s">S.</param>
		/// <param name="e">E.</param>
		async void startDownload (object s, EventArgs e)
		{
			Task<int> sizeTask = downloadStringAsync ("http://gstore.pcp.jp/api/get_spots.php");
			int intSize = await sizeTask;
		}

		UIView blinder;
		UIActivityIndicatorView loading;

		public async Task<int> downloadStringAsync (string urlToDownload)
		{
			blinder = new UIView (View.Bounds);
			blinder.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 0.8f);

			loading = new UIActivityIndicatorView (View.Bounds);
			loading.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.White;

			blinder.AddSubview (loading);

			try {
				var httpClient = new HttpClient ();

				Task<string> contentsTask = httpClient.GetStringAsync (urlToDownload);

				////// Show loading view
				/// start animating it
				/// contents are loading
				loading.StartAnimating ();
				this.View.AddSubview (blinder);

				string contents = await contentsTask;

				jsonData = contents;
				dataSize = contents.Length;

				////// Remove loading view
				/// datas are loaded
				var alert = new UIAlertView ("Download", "Complete Updating/Downloading data \nDownloaded size : " + (dataSize / 1024).ToString () + "KB", null, "OK", null);
				alert.Show ();

				loading.StopAnimating();
				blinder.RemoveFromSuperview ();
				blinder.Dispose ();

				updateLocalFile ();

				return dataSize;

			} catch (Exception e) {
				Console.WriteLine ("Error : " + e.Message);
				return -1;
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
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

			////// 
			/// Reload view
			initView ();
		}

		void UpdateStatus (object sender, EventArgs e)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();

			//////
			/// Update internet connectivity status
			UpdateConnectivity ();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			ResignFirstResponder ();
		}
	}
}

