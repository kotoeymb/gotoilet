using System;
using System.IO;
using System.Collections.Generic;

using Foundation;
using CoreGraphics;

using UIKit;
using OHouse;
using OHouse.Connectivity;
using OHouse.DRM;

namespace O.House
{
	public partial class OfflineViewController : UIViewController
	{
		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		bool connectivity;
		DataRequestManager drm;

		public OfflineViewController () : base ("OfflineViewController", null)
		{

		}

		public void closeBtnEvent (object sender, EventArgs e)
		{
			this.DismissModalViewController (true);
		}

		public void downloadBtnEvent (object sender, EventArgs e)
		{

			drm = new DataRequestManager ();

			Console.WriteLine ("Updating local list from server ...");
			List<ToiletsBase> dataFromServer;

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
			List<ToiletsBase> sync = drm.GetToiletList(fileName);

			bool willSynData = true;

			if (sync.Count > 0) {
				willSynData = false;
			}

			if (willSynData) {
				//////
				/// retrieve data from server
				dataFromServer = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php", 0, 0);
				foreach (var obj in dataFromServer) {
			
					Console.WriteLine ("Retriving data from server ...");
			
					//////
					/// add object to dataToWrite (NSMutableDictionary)
					NSMutableDictionary temp = new NSMutableDictionary ();
					temp.Add (new NSString ("spot_id"), new NSString (obj.spot_id.ToString ()));
					temp.Add (new NSString ("vote_cnt"), new NSString (obj.vote_cnt.ToString ()));
					temp.Add (new NSString ("title"), new NSString (obj.title.ToString ()));
					temp.Add (new NSString ("sub_title"), new NSString (obj.sub_title.ToString ()));
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
				Console.WriteLine ("Datas are updated!");
			}
		}

		private NSObject _didEnterForeground;

		public void didEnterForeground (NSNotification notification)
		{
			UpdateStatus ();

			if (internetStatus == NetworkStatus.NotReachable) {
				connectivity = false;
			} else {
				connectivity = true;
			}
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			connectivity = true;

			UpdateStatus ();

			if (internetStatus == NetworkStatus.NotReachable) {
				connectivity = false;
			}

			closeBtn.TouchUpInside += (object sender, EventArgs e) => {

				this.DismissModalViewController (true);
			};

			downloadBtn.BackgroundColor = UIColor.White;
			downloadBtn.Layer.BorderWidth = 1;
			downloadBtn.Layer.BorderColor = UIColor.Green.CGColor;
			downloadBtn.SetTitleColor (UIColor.Green, UIControlState.Normal);

			downloadBtn.TouchUpInside += downloadBtnEvent;

			if (!connectivity) {
				downloadBtn.Layer.BorderColor = UIColor.Red.CGColor;
				downloadBtn.SetTitleColor (UIColor.Red, UIControlState.Normal);
				downloadBtn.SetTitle ("Need connection", UIControlState.Normal);
				downloadBtn.TouchUpInside -= downloadBtnEvent;
			}

			_didEnterForeground = NSNotificationCenter.DefaultCenter.AddObserver (UIApplication.WillEnterForegroundNotification, didEnterForeground);
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		void UpdateStatus (object sender = null, EventArgs e = null)
		{
			remoteHostStatus = ConnectionManager.RemoteHostStatus ();
			internetStatus = ConnectionManager.InternetConnectionStatus ();
			localWifiStatus = ConnectionManager.LocalWifiConnectionStatus ();
		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();
			NSNotificationCenter.DefaultCenter.RemoveObserver (_didEnterForeground);
			ResignFirstResponder ();
		}
	}
}


