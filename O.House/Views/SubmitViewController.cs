
using System;
using System.Net;
using System.Collections.Generic;

using Foundation;
using UIKit;

using OHouse.DRM;

namespace OHouse
{
	public partial class SubmitViewController : UIViewController
	{
		public SubmitViewController () : base ("SubmitViewController", null)
		{
			Title = "Submitted";
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
			DataRequestManager drm = new DataRequestManager();

			string fbid = "sadsadee11666";

			Console.WriteLine(drm.RegisterUser (fbid));
		}
	}
}

