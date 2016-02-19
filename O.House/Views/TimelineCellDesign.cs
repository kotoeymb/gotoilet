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

		public static readonly UINib Nib = UINib.FromName("TimelineCellDesign", NSBundle.MainBundle);
		public static readonly NSString Key = new NSString("MyCustomCell");

		public TimelineCellDesign (IntPtr handle) : base(handle) {

		}

		public static TimelineCellDesign Create() {
			return (TimelineCellDesign)Nib.Instantiate(null, null) [0];
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			this.cellTitle.Text = Model.title;
			this.cellSubtitle.Text = Model.sub_title;

			cellShareBtn.SetTitle("S", UIControlState.Normal);
			//cellShareBtn.SetBackgroundImage(UIImage.FromBundle("images/icons/icon-share"), UIControlState.Normal);
			cellShareBtn.Layer.CornerRadius = cellShareBtn.Frame.Size.Width/2;
			cellShareBtn.Layer.BorderWidth = 1;
			cellShareBtn.Layer.BorderColor = UIColor.LightGray.CGColor;
			cellShareBtn.ClipsToBounds = true;

			cellLikeBtn.SetTitle ("L", UIControlState.Normal);
			//cellLikeBtn.SetBackgroundImage (UIImage.FromBundle ("images/icons/icon-heart"), UIControlState.Normal);
			cellLikeBtn.Layer.CornerRadius = cellLikeBtn.Frame.Size.Width/2;
			cellLikeBtn.Layer.BorderWidth = 1;
			cellLikeBtn.Layer.BorderColor = UIColor.LightGray.CGColor;
			cellLikeBtn.ClipsToBounds = true;
		}
	}
}
