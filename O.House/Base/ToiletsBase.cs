using System;

namespace OHouse
{
	public class ToiletsBase
	{
		public string Name { get; set; }

		public double Latitude { get; set; }

		public double Longitude { get; set; }

		public double Distance { get; set; }

		public ToiletsBase (string name, double latitude, double longitude, double distance)
		{
			this.Name = name;
			this.Latitude = latitude;
			this.Longitude = longitude;
			this.Distance = distance;
		}
	}
}

