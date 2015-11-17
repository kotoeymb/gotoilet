
using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Dialog;

using Foundation;
using UIKit;

using Utils;
using MapUtils;
using LocationUtils;

using Commons;

using CoreLocation;
using CoreGraphics;

namespace OHouse
{
	public partial class NearestDialogViewController : DialogViewController
	{
		List<ToiletsBase> tBaseList = new List<ToiletsBase> ();
		List<ToiletsBase> tBaseListOrdered = new List<ToiletsBase> ();
		Section section = new Section ();
		Common common= new Common ();
		CustomHdrFtr customhf = new CustomHdrFtr();

		public NearestDialogViewController () : base (UITableViewStyle.Grouped, null, true)
		{
			var frameW = View.Frame.Width;
			var frameH = View.Frame.Height;

			UIView headerView = customhf.CreateHdrFtr ("NOTICE", (float)frameW, 30, null);
			UIView footerView = customhf.CreateHdrFtr ("Please refer to GoogleMap for more accurate travel time and distance information.", (float)frameW, 40, null);

			UIView headerViewWithin = customhf.CreateHdrFtr ("WINTHIN 500 m", (float)frameW, 30, null);

			tBaseList = MapDelegate.nearToiletList;
			//tBaseListOrdered = tBaseList.OrderBy (o => o.Distance).ToList ();
			tBaseListOrdered = tBaseList.OrderBy (o => o.distance).ToList ();
			section.HeaderView = headerViewWithin;

			/// Add elements to section
			foreach (var v in tBaseListOrdered) {
				section.Add (
					new StyledStringElement (
						//v.Name.ToString () + ", " + Math.Ceiling (v.Distance).ToString () + " m",
						v.title.ToString () + ", " + Math.Ceiling (v.distance).ToString () + " m",
						() => {
							//var url = new NSUrl ("comgooglemaps://?q=" + v.Latitude + "," + v.Longitude + "&zoom=14");
							var url = new NSUrl ("comgooglemaps://?q=" + v.latitude + "," + v.longitude + "&zoom=14");
							UIApplication.SharedApplication.OpenUrl (url);
						}
					) {
						Font = common.Font13F,
						TextColor = common.White
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
		}
	}
}
