
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
		UIView view;
		UILabel label1;
		UILabel label2;
		UILabel label3;
		UILabel label4;
		UILabel label5;
		UIButton OKButton;
		UIImageView ImageView;
		UIImage image;
	//	UIImageView Image;

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
			CGRect screen = UIScreen.MainScreen.Bounds;

			//BGImage = UIImage.FromBundle ("images / background / Toilet1");

//			BGImage.Image = UtilImage.ResizeImageKeepAspect (UtilImage.FromURL (tb [0].picture),(float) screen.Width,(float)screen.Height);
			View.BackgroundColor = UIColor.FromPatternImage (UtilImage.ResizeImageKeepAspect (UtilImage.FromURL (tb [0].picture), (float)screen.Width, (float)screen.Height));
			//View.BackgroundColor = UIColor.FromPatternImage(UtilImage.ResizeImageKeepAspect(UIImage.FromBundle("images/background/Toilet1"),(float)screen.Width, (float)screen.Height));
			Title.Text = tb [0].title;
			//image = new UIImage("images/background/Toilet2");
			image = UtilImage.ResizeImageKeepAspect (UtilImage.FromURL (tb [0].picture),(float)ImageV.Frame.Width,(float)ImageV.Frame.Height);
			ImageV.Image = image;
			ImageV.Layer.CornerRadius = 10f;
			ImageV.Layer.BorderWidth = 1f;
			ImageV.Layer.BorderColor = new CGColor (52, 52, 52);
			ImageV.ClipsToBounds = true;
			//Title.TextAlignment = UITextAlignment.Center;
//
//			view = new UIView ();
//			view.Frame =  new CGRect (screen.Width/12, 6 * screen.Height/12 -10,10 * screen.Width/12, 4 * screen.Height/12);
//			view.BackgroundColor = UIColor.Clear;
//
//			var blur = UIBlurEffect.FromStyle (UIBlurEffectStyle.Light);
//
//			var blurView = new UIVisualEffectView (blur) {
//				Frame = screen,
//			};
//
//			OKButton = UIButton.FromType (UIButtonType.RoundedRect);
//			OKButton.SetTitle ("Click", UIControlState.Normal);
//			OKButton.BackgroundColor = UIColor.Clear;
//			OKButton.Frame =  new CGRect (screen.Width / 12, 10 * screen.Height / 12 -5 , 10 * screen.Width / 12, screen.Height / 12);
//			OKButton.TintColor = UIColor.White;
//			OKButton.Layer.CornerRadius = 5f;
//			OKButton.Layer.BorderColor= new CGColor (255, 255,255);
//			OKButton.Layer.BorderWidth = 1f;
//			OKButton.TouchUpInside += CloseButtonClicked;
//
//			label2 = new UILabel () {
//				TextColor = UIColor.White,
//				Font = UIFont.FromName("Helvetica-Bold",15f),
//			};
//			label2.Text = "Toilet:";
//			label2.Frame = new CGRect(15,0,view.Frame.Width,view.Frame.Height/4);
//			label2.TextAlignment = UITextAlignment.Left;
//
//			label4 = new UILabel () {
//				TextColor = UIColor.White,
//				Font = UIFont.FromName("Helvetica-Bold",15f),
//			};
//			label4.Text = "Position:";
//			label4.Frame = new CGRect (15, 2 *view.Frame.Height/4 ,view.Frame.Width, view.Frame.Height/4 );
//			label4.TextAlignment = UITextAlignment.Left;
//
//			view.Add (label2);
//			view.Add (label4);
//
//			if (tb [0].picture != null && tb [0].picture != "") {
//				ImageView = UtilImage.ResizeImageViewKeepAspect (UtilImage.FromURL (tb [0].picture),(float) screen.Width,(float)screen.Height);
//				//ImageView =  new UIImageView(UIImage.FromBundle ("images/background/Toilet2"));
//				ImageView.Frame = new CGRect  (screen.Width/12, 2 * screen.Height/12 -15,  10 *screen.Width/12,   4 *screen.Height/ 12 );
//				ImageView.Layer.CornerRadius = 10f;
//				ImageView.Layer.BorderWidth = 1f;
//				ImageView.Layer.BorderColor = new CGColor (52, 52, 52);
//				ImageView.ClipsToBounds = true;
//
//				//Image =  new UIImageView(UIImage.FromBundle ("images/background/Toilet2"));
//				Image =  UtilImage.ResizeImageViewKeepAspect (UtilImage.FromURL (tb [0].picture), (float)screen.Width,(float)screen.Height);
//				Image.Frame = new CGRect(0,0,screen.Width,screen.Height);
//
//
//				label1 = new UILabel () {
//					TextColor = UIColor.White,
//					Font = UIFont.FromName("Helvetica-Bold",18f),
//
//				};
//				label1.Text =tb [0].title;
//				label1.Frame = new CGRect ( screen.Width/12,screen.Height/12  - 20,10 * screen.Width/12, screen.Height/12);
//				label1.TextAlignment = UITextAlignment.Center;
//				label1.BackgroundColor = UIColor.Clear;
//
//				label3 = new UILabel () {
//					TextColor = UIColor.White,
//					Font = UIFont.FromName ("Helvetica", 15f),
//				};
//				label3.Text =  tb [0].title;
//				label3.Frame = new CGRect (15,view.Frame.Height/4 ,view.Frame.Width, view.Frame.Height/4 );
//				label3.TextAlignment = UITextAlignment.Left;
//
//				label5 = new UILabel () {
//					TextColor = UIColor.White,
//					Font = UIFont.FromName ("Helvetica", 15f),
//				};
//				label5.Text =  tb [0].latitude + ", " + tb [0].longitude;
//				label5.Frame = new CGRect (15, 3 *view.Frame.Height/4 ,view.Frame.Width, view.Frame.Height/4);
//				label5.TextAlignment = UITextAlignment.Left;
//
//				View.Add (Image);
//				View.Add (blurView);
//				View.Add (label1);
//				View.Add (ImageView);
//
//				view.Add (label3);
//				view.Add (label5);
//				View.Add (view);
//			};
//			View.Add (OKButton);

			//View.Add (BGImage);
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

