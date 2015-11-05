
using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Dialog;

using Foundation;
using UIKit;

using Utils;
using MapUtils;
using LocationUtils;

using FontBase;

using CoreLocation;
using CoreGraphics;

namespace OHouse
{
	public partial class NearestDialogViewController : DialogViewController
	{
		List<ToiletsBase> tBaseList = new List<ToiletsBase> ();
		List<ToiletsBase> tBaseListOrdered = new List<ToiletsBase> ();
		Section section = new Section ("Within 500 m", "Locations are listed from the nearest to the furthest within 500 m.");
		Font font = new Font ();

		public NearestDialogViewController () : base (UITableViewStyle.Grouped, null, true)
		{
			tBaseList = MapDelegate.nearToiletList;
			tBaseListOrdered = tBaseList.OrderBy (o => o.Distance).ToList ();

//			Section section = new Section ("Within 500 m", "Locations are listed from the nearest to the furthest within 500 m.") {
//				HeaderView = UtilImage.ResizeImageViewKeepAspect (UIImage.FromBundle ("images/background/bg-3"), (float)View.Frame.Width, 0),
//			};

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
						Font = Font.Font16F,
						TextColor = Font.White
					}
				);
			}

			Root = new RootElement ("Nearest") {
				section,
				new Section (
					"Notice", 
					"Please refer to GoogleMap for more accurate travel time and distance information."
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
