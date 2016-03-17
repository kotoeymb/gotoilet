using MapKit;
using System;
using Utils;
using Foundation;
using UIKit;
using CoreLocation;
using Commons;
namespace O.House
{
	public partial class AboutViewController : UIViewController
	{
		//UIScrollView scroll;
		CLLocationManager locationManager = new CLLocationManager();
		public AboutViewController () : base ("AboutViewController", null)
		{
		}
		public void CloseBtnEvent (object sender, EventArgs e)
		{
			this.DismissModalViewController (true);
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
			//scroll = new UIScrollView (new CoreGraphics.CGRect (0, 0, 450, 900));

			CloseBtn.TouchUpInside += (object sender, EventArgs e) => {

				this.DismissModalViewController (true);
			};
			var Annotation =new BasicMapAnnotation(new CLLocationCoordinate2D (16.737380,96.152120), "", "");
			var Annotation1= new BasicMapAnnotation(new CLLocationCoordinate2D (16.777426, 96.162050), "", "");
			mview.AddAnnotation (Annotation);
			mView.AddAnnotation (Annotation1);

			//CloseBtn.TintColor = UIColor.Black;

		}

			//BroundColor = UIColor.FromPatternImage (UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/background/bg-map"), (float)View.Frame.Width, 0));
			//View.BackgroundColor = UIColor.;
			//View.Add (Scroller);
			//UIColor.FromRGBA (50, 100, 200, 250);
			//UIImage cross=UtilImage.GetColoredImage("images/icons/icon-across",UIColor.FromRGB(20,20,20));
		   // CloseBtn.SetImage(cross, UIControlState.Normal);

		    
			//btnClose.BackgroundColor = UtilImage.GetColoredImage(cross, UIColor.FromRGB (20, 20, 20));


			// Perform any additional setup after loading the view, typically from a nib.
		
		class BasicMapAnnotation:MKAnnotation
		{
			public CLLocationCoordinate2D coord;
			string title,subtitle;
			public override CLLocationCoordinate2D Coordinate{get{return coord ;}}

			public override string Title 
			{
				get {
					return title;}

			}

			public override string Subtitle
			{
				get{
					return subtitle;}
			}

			public BasicMapAnnotation(CLLocationCoordinate2D coordinate,string title,string subtitle):base ()
			{
				this.coord=coordinate;
				this.title=title;
				this.subtitle=subtitle;

			}

		}
		//View.Backg
	}
}

