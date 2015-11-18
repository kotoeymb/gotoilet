
using System;

using Foundation;
using UIKit;
using MonoTouch.Dialog;
using Commons;
using CustomElements;
using Utils;

namespace OHouse
{
	public partial class TimelineViewController : UIViewController
	{
		public TimelineViewController () : base ("TimelineViewController", null)
		{
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

			TimelineDialogViewController tdvc = new TimelineDialogViewController ();

			this.AddChildViewController (tdvc);
			this.View.AddSubview (tdvc.View);
			
			// Perform any additional setup after loading the view, typically from a nib.
		}
	}

	/// <summary>
	/// Menu dialog view controller.
	/// </summary>
	public partial class TimelineDialogViewController : DialogViewController
	{
		Common common = new Common ();

		/// <summary>
		/// Initializes a new instance of the <see cref="OHouse.MenuDialogViewController"/> class.
		/// </summary>
		public TimelineDialogViewController () : base (UITableViewStyle.Plain, new RootElement (""))
		{
			Root = new RootElement ("Timeline") {
				new Section () {
					new StyledStringElement ("Gabriel Westin") {
						Image = UIImage.FromBundle ("images/icons/icon-share")
					},
					new TimelineElement (() => {
						NavigationController.PushViewController (new SubmitViewController (), true);
					}, () => {
						NavigationController.PushViewController (new FormViewController (), true);
					}) {
						ProfileName = "Name",
						ProfilePic = UIImage.FromBundle ("images/background/bg-3"),
						LastPostTime = "13 mins",
						Count = "16",
						Header = "Ocean",
						Map = UIImage.FromBundle ("images/background/bg-3"),
						Description = "Photoshop online is an on-line version. It is very popular among many users as the editor of images."
					},
					new TimelineElement (() => {
					}, () => {}) {
						ProfileName = "Name",
						ProfilePic = UIImage.FromBundle ("images/background/bg-3"),
						LastPostTime = "13 mins",
						Count = "16",
						Header = "Ocean",
						Map = UIImage.FromBundle ("images/background/bg-3"),
						Description = "Photoshop online is an on-line version. It is very popular among many users as the editor of images."
					}
				}
			};
		}

		public override void LoadView ()
		{
			base.LoadView ();
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			//TableView.BackgroundColor = UIColor.Clear;
			TableView.BackgroundColor = UIColor.FromRGB (235, 235, 235);
		}
	}
}

