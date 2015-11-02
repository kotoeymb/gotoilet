
using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Dialog;

using Foundation;
using UIKit;

namespace OHouse
{
	public partial class InfoDialogViewController : DialogViewController
	{
		private string[] _datas;

		public InfoDialogViewController (string[] datas) : base (UITableViewStyle.Grouped, null, true)
		{
			_datas = datas;

			this.NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem ("Direction", UIBarButtonItemStyle.Plain, (s, e) => {
					var url = new NSUrl ("comgooglemaps://?q=" + datas [1] + "," + datas [2] + "&zoom=14");
					UIApplication.SharedApplication.OpenUrl (url);
				}
				), true
			);

			Root = new RootElement (datas [0]) {
				new Section ("Name") {
					new StringElement (datas [0])
				},
				new Section ("Lat & Lon") {
					new StringElement (datas [1]),
					new StringElement (datas [2])
				},
			};
		}
	}
}