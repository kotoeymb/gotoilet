using System;
using CoreLocation;
using UIKit;

namespace LocationUtils
{
	public class LocationUtil
	{
		private CLLocationManager locMgr;

		public LocationUtil ()
		{
			this.locMgr = new CLLocationManager ();
			if (UIDevice.CurrentDevice.CheckSystemVersion (8, 0)) {
				locMgr.RequestWhenInUseAuthorization ();
			}
		}

		public CLLocationManager LocMgr {
			get { return this.locMgr; }
		}
	}
}

