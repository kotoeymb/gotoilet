
using System;
using System.Collections.Generic;

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
	public partial class DetailViewController : UIViewController
	{
		DataRequestManager drm;
		List<ToiletsBase> tb;
		Common common;
		ConnectionManager connMgr;

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

			this.NavigationItem.SetLeftBarButtonItem (
				new UIBarButtonItem (
					UIImage.FromBundle ("images/icons/icon-cross"), UIBarButtonItemStyle.Plain, (ss, ee) => {
					this.DismissModalViewController (true);
				}
				),
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

			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-7-nightlife"));
			
			// Perform any additional setup after loading the view, typically from a nib.
			DialogView detail = new DialogView (tb);
			View.AddSubview (detail.View);
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
		}

		public override void LoadView ()
		{
			base.LoadView ();

			//TableView.BackgroundColor = UIColor.FromRGBA (13, 13, 13, 200);
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
		}
	}
}

