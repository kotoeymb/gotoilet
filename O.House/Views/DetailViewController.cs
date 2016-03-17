using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using CoreGraphics;
using CoreLocation;
using System.Drawing;
using Foundation;
using UIKit;
using Commons;
using MonoTouch.Dialog;
using Utils;
using OHouse.DRM;
using OHouse.Connectivity;
using MapKit;

namespace OHouse
{
	/// <summary>
	/// Detail view controller.
	/// </summary>
	public partial class DetailViewController : UIViewController
	{
		DataRequestManager drm;
		bool connectivity;
		NetworkStatus remoteHostStatus, internetStatus, localWifiStatus;
		int spot_id;
		public List<ToiletsBase> d;

		public DetailViewController (int datas) : base ("DetailViewController", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			this.spot_id = datas;
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		public async override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			connectivity = true;

			//////
			/// First time check for connectivity when view did loaded
			UpdateStatus (null, null);

			//////
			/// Observe connectivity change to show warning
			/// Not updating the UI or Data
			ConnectionManager.ReachabilityChanged += UpdateStatus;

			drm = new DataRequestManager ();
			mapLocation.UserInteractionEnabled = false;
			iconLike.Image = UIImage.FromBundle ("images/icons/icon-heart");
			btnClose.TouchUpInside += CloseButtonClicked;
			bgToilet.Image = UIImage.FromBundle ("images/background/bg-toilet-big");

			if (!connectivity) {
				////// Load from local
				d = drm.GetSpotInfoFromLocal ("Update.plist", spot_id);
				initView ();
			} else {
				////// Load from web server
				loader.StartAnimating ();
				d = await loadDetailInformation ("http://gstore.pcp.jp/api/get_spots_info.php?spot_id=" + spot_id);
				loader.StopAnimating ();
				initView ();
			}
		}

		/// <summary>
		/// Inits the view.
		/// </summary>
		void initView ()
		{
			double lat = d [0].latitude;
			double lon = d [0].longitude;

			lblTitle.Text = d [0].title;
			lblDescription.Text = d [0].sub_title;
			lblApproveCount.Text = d [0].vote_cnt + " people approved this location is valid!";
			lblLocation.Text = d [0].latitude + ", " + d [0].longitude;

			var annotation = new MKPointAnnotation () {
				Title = d [0].title,
				Coordinate = new CLLocationCoordinate2D (lat, lon)
			};

			mapLocation.AddAnnotation (annotation);
			mapLocation.CenterCoordinate = new CLLocationCoordinate2D (lat, lon);
			mapLocation.SetRegion (MKCoordinateRegion.FromDistance (annotation.Coordinate, 500, 500), false);
		}

		void noConnection ()
		{
			var alert = new UIAlertView ("Download", "Please connect to Internet or download the data for offline use.", null, "OK", null);
			alert.Show ();
		}

		public async Task<List<ToiletsBase>> loadDetailInformation (string url)
		{
			List<ToiletsBase> infoUrl;

			try {

				HttpClient client = new HttpClient ();
				Task<string> data = client.GetStringAsync (url);

				string jsonData = await data;

				infoUrl = drm.GetSpotInfoJSON (jsonData);

				return infoUrl;
			} catch (Exception e) {
				Console.WriteLine (e.Message);
				return null;
			}
		}

		void CloseButtonClicked (object sender, EventArgs e)
		{
			this.DismissModalViewController (true);
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
			/// Show alert
			if (!connectivity) {
				noConnection ();
			}
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

		public override bool ShouldAutorotate ()
		{
			return false;
		}
	}
}

