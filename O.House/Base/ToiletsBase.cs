using System;

namespace OHouse
{
	public class ToiletsBase
	{
		//		public string Name { get; set; }
		//
		//		public double Latitude { get; set; }
		//
		//		public double Longitude { get; set; }
		//
		//		public double Distance { get; set; }
		//
		//		public ToiletsBase (string name, double latitude, double longitude, double distance)
		//		{
		//			this.Name = name;
		//			this.Latitude = latitude;
		//			this.Longitude = longitude;
		//			this.Distance = distance;
		//		}

		public int spot_id { get; set; }

		public int vote_cnt { get; set; }

		public string title { get; set; }

		public string sub_title { get; set; }

		public string picture { get; set; }

		public double latitude { get; set; }

		public double longitude { get; set; }

		public double distance { get; set; }

		public ToiletsBase (int spot_id, int vote_cnt, string title, string sub_title, string picture, double latitude, double longitude, double distance)
		{
			this.spot_id = spot_id;
			this.vote_cnt = vote_cnt;
			this.title = title;
			this.sub_title = sub_title;
			this.picture = picture;
			this.latitude = latitude;
			this.longitude = longitude;
			this.distance = distance;
		}
	}
}

