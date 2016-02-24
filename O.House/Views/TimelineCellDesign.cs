using System;
using System.Drawing;

using CoreGraphics;

using Foundation;
using UIKit;
using OHouse;
using Utils;
using Commons;

namespace O.House
{
	public partial class TimelineCellDesign : UITableViewCell
	{
		public ToiletsBase Model { get; set; }

		//		Common common;

		public static readonly UINib Nib = UINib.FromName ("TimelineCellDesign", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString ("MyCustomCell");

		public TimelineCellDesign (IntPtr handle) : base (handle)
		{

		}

		public static TimelineCellDesign Create ()
		{
			return (TimelineCellDesign)Nib.Instantiate (null, null) [0];
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			this.cellTitle.Text = Model.title;
			this.cellSubtitle.Text = Model.sub_title;
			this.cellSubtitle.Lines = 0;
			this.cellSubtitle.LineBreakMode = UILineBreakMode.WordWrap;
	
			cellShareBtn.SetTitle ("Share", UIControlState.Normal);
			cellShareBtn.SetImage(UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-share"), 16, 16), UIControlState.Normal);
			cellShareBtn.ImageEdgeInsets = new UIEdgeInsets (0, 0, 0, 5);
			cellShareBtn.TintColor = UIColor.LightGray;


			cellLikeBtn.SetTitle ("Approve", UIControlState.Normal);
			cellLikeBtn.SetImage (UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-heart"), 16, 16), UIControlState.Normal);
			cellLikeBtn.TitleEdgeInsets = new UIEdgeInsets (0, 5, 0, 0);
			cellLikeBtn.TintColor = UIColor.LightGray;

		}
	}
}