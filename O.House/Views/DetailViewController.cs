
using System;

using Foundation;
using UIKit;
using Commons;
using MonoTouch.Dialog;
using Utils;

namespace OHouse
{
	/// <summary>
	/// Detail view controller.
	/// </summary>
	public partial class DetailViewController : UIViewController
	{
		public string[] Datas { get; set; }

		public DetailViewController (string[] datas) : base ("DetailViewController", null)
		{

			EdgesForExtendedLayout = UIRectEdge.None;
			Datas = datas;
			Title = datas [0];

			this.NavigationItem.SetRightBarButtonItem (
				new UIBarButtonItem (UIImage.FromBundle ("images/icons/icon-direction"), UIBarButtonItemStyle.Plain, (s, e) => {
					var url = new NSUrl ("comgooglemaps://?q=" + datas [1] + "," + datas [2] + "&zoom=14");
					UIApplication.SharedApplication.OpenUrl (url);
				}),
				true
			);

			this.NavigationItem.SetLeftBarButtonItem (
				new UIBarButtonItem (
					UIImage.FromBundle ("images/icons/icon-cross"), UIBarButtonItemStyle.Plain, (ss, ee) => {
						this.DismissModalViewController(true);
					}
				),
				true
			);
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

			View.BackgroundColor = UIColor.FromPatternImage (UIImage.FromBundle ("images/background/bg-7-nightlife"));
			
			// Perform any additional setup after loading the view, typically from a nib.
			DialogView detail = new DialogView (Datas);
			View.AddSubview (detail.View);
		}
	}

	/// <summary>
	/// Info dialog view controller.
	/// </summary>
	public partial class DialogView : DialogViewController
	{
		Common common = new Common ();

		public DialogView (string[] datas) : base (UITableViewStyle.Grouped, null, true)
		{
			Section section = new Section () {
				HeaderView = UtilImage.ResizeImageViewKeepAspect (UIImage.FromBundle ("images/background/bg-3"), (float)View.Frame.Width, 0)
			};

			Root = new RootElement (datas [0]) {
				section,
				new Section ("Name") {
					new StyledStringElement (datas [0]) {
						Font = common.Font16F,
						TextColor = common.White
					}
				},
				new Section ("Lat & Lon") {
					new StyledStringElement (datas [1] + ", " + datas [2]) {
						Font = common.Font16F,
						TextColor = common.White
					}
				},
			};
		}

		public override void LoadView ()
		{
			base.LoadView ();

			TableView.BackgroundColor = UIColor.FromRGBA(13,13,13, 200);
			TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
		}
	}
}

