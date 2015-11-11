
using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Dialog;
using Common;
using CoreGraphics;

using Foundation;
using UIKit;
using Utils;

namespace OHouse
{
	public partial class InfoDialogViewController : DialogViewController
	{
		private string[] _datas;
		Font font = new Font ();

		public string[] Datas {
			get {
				return _datas;
			}
			set { ; }
		}

		public InfoDialogViewController (string[] datas) : base (UITableViewStyle.Grouped, null, true)
		{
			Section section = new Section () {
				HeaderView = UtilImage.ResizeImageViewKeepAspect (UIImage.FromBundle ("images/background/bg-3"), (float)View.Frame.Width, 0),
			};

			Datas = datas;
			//this._datas = datas;

			this.NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem (UIImage.FromBundle ("images/icons/icon-direction"), UIBarButtonItemStyle.Plain, (s, e) => {
					var url = new NSUrl ("comgooglemaps://?q=" + datas [1] + "," + datas [2] + "&zoom=14");
					UIApplication.SharedApplication.OpenUrl (url);
				}),
				true
			);

			Root = new RootElement (datas [0]) {
				section,
				new Section ("Name") {
					new StyledStringElement (datas [0]) {
						Font = font.Font16F,
						TextColor = font.White
					}
				},
				new Section ("Lat & Lon") {
					new StyledStringElement (datas [1] + ", " + datas [2]) {
						Font = font.Font16F,
						TextColor = font.White
					}
				},
			};
		}

		public override void LoadView ()
		{
			base.LoadView ();

			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-5"));
			TableView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 100);
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			TableView.Frame = new CGRect (10, 10, (float)View.Frame.Width - 20, (float)View.Frame.Height - 20);
		}
	}
}