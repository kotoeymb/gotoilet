
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
		Common common;
		ConnectionManager connMgr;
		UIView DView;
		UILabel label1;
		UILabel label2;
		UIButton buttonRect;
		UIImageView ImageView;

		public DetailViewController (int datas) : base ("DetailViewController", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
			drm = new DataRequestManager ();
			common = new Common ();
			connMgr = new ConnectionManager ();
			connMgr.UpdateStatus ();

			if (connMgr.internetStatus == NetworkStatus.NotReachable) {
				tb = drm.GetSpotInfoFromLocal ("database/Toilets", datas);
			} else {
				tb = drm.GetSpotInfo (datas);
			}

			UIBarButtonItem rightBarBtnItem = new UIBarButtonItem (
				                                  "Direction", 
				                                  UIBarButtonItemStyle.Plain, 
				                                  (s, e) => {
					var url = new NSUrl ("comgooglemaps://?q=" + tb [0].latitude + "," + tb [0].longitude + "&zoom=14");
					UIApplication.SharedApplication.OpenUrl (url);
				});

			rightBarBtnItem.SetTitleTextAttributes (common.commonStyle, UIControlState.Normal);

			this.NavigationItem.SetRightBarButtonItem (
				rightBarBtnItem,
				true
			);
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



			DView = new UIView ();
			DView.BackgroundColor = UIColor.White;
			DView.Frame = new CGRect (25, View.Frame.Width / 2.5, 270, 350);
			DView.Layer.BorderWidth = 1f;
			DView.Layer.BorderColor = new CGColor (112,128,144);
			DView.Layer.CornerRadius = 10f;

			DView.Layer.ShadowOffset = new CGSize (2,2);
			DView.Layer.ShadowOpacity = 1f;
			DView.Layer.ShadowRadius = 1;
			DView.Layer.ShadowColor = new CGColor(0,0,0);

			buttonRect = UIButton.FromType (UIButtonType.RoundedRect);
			buttonRect.SetTitle ("", UIControlState.Normal);
			buttonRect.Frame = new RectangleF (0, 0, (float)View.Frame.Width, (float)View.Frame.Height);
			buttonRect.BackgroundColor = UIColor.Clear;
			buttonRect.TouchUpInside += CloseButtonClicked;


			ImageView = new UIImageView ();
			if (tb [0].picture != null && tb [0].picture != "") {
				ImageView = UtilImage.ResizeImageViewKeepAspect (UtilImage.FromURL (tb [0].picture), (float)View.Frame.Width, 100);
				ImageView.Frame = new CGRect (0, 0, 270, 200);
				ImageView.Layer.CornerRadius = ImageView.Frame.Size.Width / 35;
				ImageView.Layer.BorderWidth = 1f;
				ImageView.Layer.BorderColor = new CGColor (52, 52, 52);
				ImageView.ClipsToBounds = true;

				label1 = new UILabel () {
					TextColor = UIColor.Black,
					Font = common.Font16F,
				};
				label1.Text = tb [0].title;
				label1.Frame = new CGRect (10, 210, View.Frame.Width, 30);
				label1.TextAlignment = UITextAlignment.Left;

				label2 = new UILabel () {
					TextColor = UIColor.Black,
					Font = common.Font16F,
				};
				label2.Text = tb [0].latitude + ", " + tb [0].longitude;
				label2.Frame = new CGRect (10, 250, View.Frame.Width, 30);
				label2.TextAlignment = UITextAlignment.Left;

				View.Add (buttonRect);
				DView.Add (ImageView);
				DView.Add (label1);
				DView.Add (label2);
				View.AddSubview (DView);
			}
			;
		}

		void CloseButtonClicked (object sender, EventArgs e)
		{

			this.View.RemoveFromSuperview ();
			this.RemoveFromParentViewController ();
		}


	}

	/// <summary>
	/// Info dialog view controller.
	/// </summary>

}

