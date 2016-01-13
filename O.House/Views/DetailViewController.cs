
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



			DView = new UIView();
			DView.Frame = new CGRect (10, 100,300, 400);

			buttonRect = UIButton.FromType (UIButtonType.RoundedRect);
			buttonRect.SetTitle ("", UIControlState.Normal);
			buttonRect.Frame = new RectangleF (0,0,(float)View.Frame.Width,(float) View.Frame.Height);
			buttonRect.BackgroundColor = UIColor.Clear;
			buttonRect.TouchUpInside += CloseButtonClicked;


			ImageView = new UIImageView();
			if (tb [0].picture != null && tb [0].picture != "") {
				ImageView = UtilImage.ResizeImageViewKeepAspect (UtilImage.FromURL (tb [0].picture),(float)View.Frame.Width, 100);
				ImageView.Frame = new CGRect (10, 10, 280, 250);
				label1 = new UILabel () {
					TextColor = UIColor.Black,
					Font = common.Font16F,
				};
				label1.Text =tb [0].title;
				label1.Frame = new CGRect (10, 250, View.Frame.Width, 100);

				label2 = new UILabel () {
					TextColor = UIColor.Black,
					Font = common.Font16F,
				};
				label2.Text =  tb [0].latitude + ", " + tb [0].longitude;
				label2.Frame = new CGRect (10, 300,View.Frame.Width, 100);


				DView.BackgroundColor = UIColor.White;

				View.Add (buttonRect);

				DView.Add (ImageView);
				DView.Add(label1);
				DView.Add(label2);
				View.AddSubview(DView);
			};
		}

		void CloseButtonClicked (object sender, EventArgs e)
		{

			this.View.RemoveFromSuperview();
			this.RemoveFromParentViewController();
		}


	}

	/// <summary>
	/// Info dialog view controller.
	/// </summary>
	public partial class DialogView : DialogViewController
	{
		Common common;
		UIView pictureView;

		public DialogView (List<ToiletsBase> datas) : base (UITableViewStyle.Grouped, null, true)
		{
			common = new Common ();

			if (datas [0].picture != null && datas [0].picture != "") {
				pictureView = UtilImage.ResizeImageViewKeepAspect (UtilImage.FromURL (datas [0].picture), (float)View.Frame.Width, 0);
			}

			Section section = new Section () {
				HeaderView = pictureView
			};

			Root = new RootElement (datas [0].title) {
				section,
				new Section ("Name") {
					new StyledStringElement (datas [0].title) {
						Font = common.Font16F,
						TextColor = common.Blackish
					}
				},
				new Section ("Lat & Lon") {
					new StyledStringElement (datas [0].latitude + ", " + datas [0].longitude) {
						Font = common.Font16F,
						TextColor = common.Blackish
					}
				}
			};

			//System.Text.Encoding.UTF8.GetString ();
		}

		public override void LoadView ()
		{
			base.LoadView ();

			//TableView.BackgroundColor = UIColor.FromRGBA (13, 13, 13, 200);
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
		}
	}
}

