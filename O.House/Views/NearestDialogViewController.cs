
using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Dialog;

using Foundation;
using UIKit;

using Utils;
using MapUtils;
using LocationUtils;

using Common;

using CoreLocation;
using CoreGraphics;

namespace OHouse
{
	public partial class NearestDialogViewController : DialogViewController
	{
		List<ToiletsBase> tBaseList = new List<ToiletsBase> ();
		List<ToiletsBase> tBaseListOrdered = new List<ToiletsBase> ();
		Section section = new Section ();
		Font font = new Font ();
		CustomHdrFtr customhf = new CustomHdrFtr();

		public NearestDialogViewController () : base (UITableViewStyle.Grouped, null, true)
		{
			var frameW = View.Frame.Width;
			var frameH = View.Frame.Height;

			UIView headerView = customhf.CreateHdrFtr ("NOTICE", (float)frameW, 30);
			UIView footerView = customhf.CreateHdrFtr ("Please refer to GoogleMap for more accurate travel time and distance information.", (float)frameW, 40);

			UIView headerViewWithin = customhf.CreateHdrFtr ("WINTHIN 500 m", (float)frameW, 30);

			tBaseList = MapDelegate.nearToiletList;
			tBaseListOrdered = tBaseList.OrderBy (o => o.Distance).ToList ();
			section.HeaderView = headerViewWithin;

			/// Add elements to section
			foreach (var v in tBaseListOrdered) {
				section.Add (
					new StyledStringElement (
						v.Name.ToString () + ", " + Math.Ceiling (v.Distance).ToString () + " m",
						() => {
							var url = new NSUrl ("comgooglemaps://?q=" + v.Latitude + "," + v.Longitude + "&zoom=14");
							UIApplication.SharedApplication.OpenUrl (url);
						}
					) {
						Font = font.Font16F,
						TextColor = font.White
					}
				);
			}


			Root = new RootElement ("Nearest") {
				section,
				new Section (
					headerView,
					footerView
				)
			};
		}

		public override void LoadView ()
		{
			base.LoadView ();

			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-5"));
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;

//			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-5"));
//			TableView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 100);
//			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
//			TableView.Frame = new CGRect (10, 10, (float)View.Frame.Width - 20, (float)View.Frame.Height - 20);
		}
	}
}
