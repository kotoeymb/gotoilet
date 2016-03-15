
using System;
using System.Collections.Generic;
using CoreGraphics;
using System.Drawing;
using Foundation;
using UIKit;
using Commons;
using MonoTouch.Dialog;
using Utils;
using OHouse.DRM;
using OHouse.Connectivity;

namespace OHouse
{


	/// <summary>
	/// Detail view controller.
	/// </summary>
	/// 
	/// 
	public partial class DetailViewController : UIViewController
	{
		DataRequestManager drm;
		List<ToiletsBase> tb;
	
	//	Common Common;
		ConnectionManager connMgr;

	

		public DetailViewController (int datas) : base ("DetailViewController", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			drm = new DataRequestManager ();
			connMgr = new ConnectionManager ();
			connMgr.UpdateStatus ();
//
			if (connMgr.internetStatus == NetworkStatus.NotReachable) {
				tb = drm.GetSpotInfoFromLocal ("database/Toilets", datas);
			} else {
				tb = drm.GetSpotInfo (datas);
			}


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

			TitleLabel.Text = tb [0].title;
			ToiletName.Text = tb [0].title;
			PositionLabel.Text =  tb [0].latitude + ", " + tb [0].longitude;
			OKButton.BackgroundColor = Common.ColorStyle_1;
			OKButton.TouchUpInside += CloseButtonClicked;

		}

		void CloseButtonClicked (object sender, EventArgs e)
		{

			this.DismissModalViewController (true);
		}


	}

	/// <summary>
	/// Info dialog view controller.
	/// </summary>

}

