
using System;
using System.Linq;
using System.Collections.Generic;

using MonoTouch.Dialog;

using Foundation;
using UIKit;

using MapUtils;
using LocationUtils;

using CoreLocation;

namespace OHouse
{
	public partial class NearestDialogViewController : DialogViewController
	{
		List<ToiletsBase> tBaseList = new List<ToiletsBase> ();
		List<ToiletsBase> tBaseListOrdered = new List<ToiletsBase> ();
		Section section = new Section ("Within 500 m");

		/// <summary>
		/// The average walk speed. 3.1 mph
		/// And estimated time travel
		/// </summary>
		double averageWalkSpeed = 3.1;
		double estimatedTimeTravel;

		public NearestDialogViewController () : base (UITableViewStyle.Grouped, null, true)
		{
			//tBaseList = MapUtil.GetToiletList ("database/Toilets");
			tBaseList = MapDelegate.nearToiletList;
			tBaseListOrdered = tBaseList.OrderBy (o => o.Distance).ToList ();

			/// Add elements to section
			foreach (var v in tBaseListOrdered) {

				estimatedTimeTravel = (v.Distance * 0.00062137119) % averageWalkSpeed;

				section.Add (
					new StyledStringElement (
						v.Name.ToString () + ", " + Math.Ceiling (v.Distance).ToString () + " m",
						//Math.Ceiling (v.Distance).ToString () + " m, " + Math.Ceiling(estimatedTimeTravel * 60) + " mins", 
						//UITableViewCellStyle.Subtitle
						() => {
							var url = new NSUrl ("comgooglemaps://?q=" + v.Latitude + "," + v.Longitude + "&zoom=14");
							UIApplication.SharedApplication.OpenUrl (url);
						}
					)
				);
			}

			Root = new RootElement ("Nearest") {
				section,
				new Section() {
					new StyledStringElement(
						"Notice",
						"Please refer to GoogleMap for more accurate travel time and distance information.",
						UITableViewCellStyle.Subtitle
					)
				}
			};

			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
		}
	}
}
