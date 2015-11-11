
using System;

using Foundation;
using UIKit;
using MonoTouch.Dialog;
using CoreGraphics;

namespace OHouse
{
	public partial class TestUIVC : UIViewController
	{
		RootElement Root;
		DialogViewController diag;

		public TestUIVC () : base ("TestUIVC", null)
		{
			EdgesForExtendedLayout = UIRectEdge.None;
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
			
			// Perform any additional setup after loading the view, typically from a nib.
			Root = new RootElement ("New root") {
				new Section ("Testing Session") {
					new StringElement ("Just testing string")
				}
			};

			diag = new DialogViewController (Root);
			diag.View.Frame = new CGRect (15, 15, View.Frame.Width - 30, View.Frame.Height - 30);
			diag.View.BackgroundColor = UIColor.FromRGBA (255, 255, 255, 30);
			diag.View.Layer.CornerRadius = 5;

			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-5"));

			this.AddChildViewController (diag);
			this.View.AddSubview (diag.View);
		}
	}
}

