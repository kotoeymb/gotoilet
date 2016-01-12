using System;
using System.Drawing;
using UIKit;
using MonoTouch.Dialog;
using Foundation;
using CoreGraphics;
using CoreAnimation;
using Commons;
using Utils;

namespace CustomElements
{
	#region Timeline element
	/// <summary>
	/// Timeline cell.
	/// </summary>
	public class TimelineCell : UITableViewCell
	{
		private UIButton ShareBtn;

		//private UILabel Title;
		private UIButton Title;
		private UILabel Info;

		private UILabel Count;

		private UIButton LikeBtn;

		private UIView Border;
		//private UIImageView MapViewView;

		private UIView customColorView = new UIView ();

		Common common = new Common ();

		public TimelineCell (UITableViewCellStyle style, NSString id, UIImage mapView, string title, string info, int count) : base (style, id)
		{
			BackgroundColor = UIColor.White;

			customColorView.BackgroundColor = UIColor.White;
			SelectedBackgroundView = customColorView;

			Title = new UIButton () {
				Font = common.Font16F,
				HorizontalAlignment = UIControlContentHorizontalAlignment.Left,
				TintColor = common.ColorStyle_1,
				BackgroundColor = UIColor.Clear
			};


			Info = new UILabel () {
				Font = common.Font13F,
				TextColor = common.Blackish,
				TextAlignment = UITextAlignment.Left,
				Lines = 0,
				AdjustsFontSizeToFitWidth = false,
				BackgroundColor = UIColor.Clear
					
			};

			Count = new UILabel () {
				Font = common.Font13F,
				TextColor = common.Blackish,
				TextAlignment = UITextAlignment.Center,
				BackgroundColor = UIColor.Clear
			};

//			ShareBtn = new UIButton () {
//				BackgroundColor = UIColor.Clear
//			};



			//ShareBtn.SetImage (UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-share"), 24, 24), UIControlState.Normal);
			//ShareBtn.SetImage(UtilImage.RoundButton(UtilImage.ResizeImageKeepAspect (UtilImage.GetColoredImage ("images/icons/icon-share", common.White), 24, 24), new RectangleF(0, 0, 24, 24), common.ColorStyle_1, true), UIControlState.Normal);
			//ShareBtn.SetImage (UtilImage.ResizeImageKeepAspect (UtilImage.GetColoredImage ("images/icons/icon-share", common.White), 24, 24), UIControlState.Normal);
			ShareBtn = UtilImage.RoundButton(
				UtilImage.ResizeImageKeepAspect (
					UtilImage.GetColoredImage (
						"images/icons/icon-share", 
						common.White
					), 
					24, 
					24
				), 
				new RectangleF(0, 0, 35, 35), 
				common.ColorStyle_1, 
				true
			);

			ShareBtn.BackgroundColor = common.ColorStyle_1;
			ShareBtn.TouchUpInside += (s, e) => {
				UIAlertView alert = new UIAlertView ("hi", count.ToString (), null, "ok");
				alert.Show ();
			};

			//LikeBtn.SetImage (UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-heart"), 24, 24), UIControlState.Normal);
			LikeBtn = UtilImage.RoundButton(
				UtilImage.ResizeImageKeepAspect (
					UtilImage.GetColoredImage (
						"images/icons/icon-heart", 
						common.White), 
					24, 
					24
				),
				new RectangleF(0, 0, 35, 35), 
				common.ColorStyle_1,
				true
			);
			//LikeBtn.SetImage (UtilImage.ResizeImageKeepAspect (UtilImage.GetColoredImage ("images/icons/icon-heart", common.ColorStyle_1), 24, 24), UIControlState.Normal);

			UpdateCell (mapView, title, info, count);
			Border = new UIView () {
				BackgroundColor = UIColor.FromRGB (238, 238, 238)
			};
			ContentView.AddSubviews (Title, Info, Count, ShareBtn, LikeBtn, Border);
		}

		public void UpdateCell (UIImage mapview, string title, string info, int count)
		{
			//MapViewView.Image = mapview;
			//Title.Title = title;
			//Title.SetTitle(title, UIControlState.Normal);
			NSAttributedString As = new NSAttributedString (title);
			Title.SetAttributedTitle (As, UIControlState.Normal);
			Info.Text = info;
			Count.Text = count.ToString ();
		}

		public override void SetSelected (bool selected, bool animated)
		{
			base.SetSelected (selected, animated);
			Border.BackgroundColor = UIColor.FromRGB (238, 238, 238);
			ShareBtn.BackgroundColor = common.ColorStyle_1;
			LikeBtn.BackgroundColor = common.ColorStyle_1;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			//CGRect full = ContentView.Bounds;
			CGRect full = ContentView.Frame;

			// UIView - container
			Border.Frame = new CGRect (0, 0, full.Width, 2);

			// UILabel title
			//Title.Frame = new CGRect (10, 10, CFull.Width - 20, 24);
			Title.Frame = new CGRect (65, 12, full.Width - 65 + 15, 24);

			// UILabe info
			CGSize size = UIStringDrawing.StringSize (Info.Text, common.Font13F, new CGSize (full.Width, 60), UILineBreakMode.WordWrap);
			//Info.Frame = new CGRect (10, Title.Frame.Y + Title.Frame.Height, CFull.Width - 20, size.Height);
			Info.Frame = new CGRect (Title.Frame.X, Title.Frame.Y + Title.Frame.Height, Title.Frame.Width, size.Height);

			// UIImage likeBtn
			LikeBtn.Frame = new CGRect (15, full.Height - 15 - 35, 35, 35);

			// UILabel count
			//Count.Frame = new CGRect (10 + LikeBtn.Frame.Width + 5, LikeBtn.Frame.Y, 16, 16);
			Count.Frame = new CGRect (15, LikeBtn.Frame.Y - 28, 35, 35);

			// UIButton shareBtn
			//ShareBtn.Frame = new CGRect (CFull.Width - 10 - 16, LikeBtn.Frame.Y, 16, 16);
			ShareBtn.Frame = new CGRect (15, 15, 35, 35);
		}
	}

	/// <summary>
	/// Timeline element.
	/// </summary>
	public class TimelineElement : Element, IElementSizing
	{
		private static NSString Key = new NSString ("TimelineElement");

		public UIImage Map;
		//mapView

		public string Header;
		//title
		public string Description;
		//info

		public string LastPostTime;
		//lastposttime

		public int Count;

		private event Action Tapped = null;

		public TimelineElement (Action tapped) : base (null)
		{
			Tapped += tapped;
		}


		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (Key);

			if (cell == null) {
				cell = new TimelineCell (UITableViewCellStyle.Default, Key, Map, Header, Description, Count);
			} else {
				((TimelineCell)cell).UpdateCell (Map, Header, Description, Count);
			}

			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			base.Selected (dvc, tableView, path);
//			if (Tapped != null) {
//				Tapped ();
//			}
			//base.Selected (dvc, tableView, path);
		}

		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return 120f;
		}
	}

	#endregion

	#region Custom element
	/// <summary>
	/// Custom cell.
	/// </summary>
	public class CustomCell : UITableViewCell
	{
		UILabel label;
		UILabel sublabel;
		UIImageView imageView;
		UIView customColorView = new UIView ();
		Common common = new Common ();

		public CustomCell (UITableViewCellStyle style, NSString id, string caption, string subtitle, UIImage img) : base (style, id)
		{
			//SelectionStyle = UITableViewCellSelectionStyle.;
			BackgroundColor = UIColor.Clear;

			// Setting custom cell select color
			customColorView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 100);
			SelectedBackgroundView = customColorView;

			label = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = common.Clear,
				TextColor = common.ColorStyle_1,
				Font = common.Font16F,
				Lines = 0
			};

			sublabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = common.Clear,
				TextColor = common.White,
				Font = common.Font13F,
				AdjustsFontSizeToFitWidth = false,
				Lines = 2,
			};

			imageView = new UIImageView ();
			imageView.Image = img;
			UpdateCell (caption, subtitle, img);

			ContentView.Add (label);
			ContentView.Add (sublabel);
			ContentView.Add (imageView);
		}

		public void UpdateCell (string caption, string subtitle, UIImage img)
		{
			label.Text = caption;
			sublabel.Text = subtitle;
			imageView.Image = img;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;

			CGSize size = UIStringDrawing.StringSize (label.Text, common.Font16F, new CGSize (full.Width, 40), UILineBreakMode.WordWrap);
			CGSize size0 = UIStringDrawing.StringSize (sublabel.Text, common.Font16F, new CGSize (full.Width, 40), UILineBreakMode.WordWrap);

			nfloat x = 15 + 32 + 15;
			imageView.Frame = new CGRect (15, 15, 32, 32);

			label.Frame = new CGRect (x, imageView.Frame.Y, full.Width - x - 15, size.Height);
			sublabel.Frame = new CGRect (x, label.Frame.Y + label.Frame.Y, full.Width - x - 15, size0.Height);
		}
	}

	/// <summary>
	/// Custom element.
	/// </summary>
	public class CustomElement : Element, IElementSizing
	{
		public string Text { get; set; }

		public string SubTitle { get; set; }

		public UIImage Image { get; set; }

		private event Action Tapped = null;

		static NSString Key = new NSString ("CustomElement");

		public CustomElement (string caption, Action tapped) : base (null)
		{
			this.Text = caption;
			Tapped += tapped;
		}

		public override UITableViewCell GetCell (UITableView tv)
		{
			var cell = tv.DequeueReusableCell (Key);

			if (cell == null) {
				cell = new CustomCell (UITableViewCellStyle.Default, Key, Text, SubTitle, Image);
			} else {
				((CustomCell)cell).UpdateCell (Text, SubTitle, Image);
			}

			return cell;
		}

		public override void Selected (DialogViewController dvc, UITableView tableView, NSIndexPath path)
		{
			base.Selected (dvc, tableView, path);
			if (Tapped != null) {
				Tapped ();
			}
		}

		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return 80f;
		}
	}

	#endregion


	#region Custom TextField, TextView

	/// <summary>
	/// Text field.
	/// </summary>
	public class TextField : UIView
	{
		Common common = new Common ();
		UITextField tf;
		UILabel lbl;
		float padding;
		UIColor bgcolor;
		UIView box;

		public TextField (CGRect bounds, UIColor bordercolor, string caption, UIImage icon = null, bool transparent = true)
		{
			this.Frame = bounds;
			BackgroundColor = common.Clear;

			box = new UIView (bounds);

			box.Layer.BorderWidth = 1f;
			box.Layer.BorderColor = bordercolor.CGColor;

			if (transparent)
				bgcolor = UIColor.Clear;
			else
				bgcolor = UIColor.White;

			box.BackgroundColor = bgcolor;

			padding = 5;

			// Label
			lbl = new UILabel (new CGRect (0, 0, bounds.Width, 16)) {
				Text = caption,
				Font = common.Font16F,
				TextColor = common.ColorStyle_1
			};

			box.Frame = new CGRect (0, lbl.Frame.Y + lbl.Frame.Height + 8, bounds.Width, bounds.Height);

			// Textfield
			tf = new UITextField (new CGRect (padding, padding, bounds.Width - padding * 2 - 16, bounds.Height - padding * 2));
			tf.Font = common.Font13F;
			tf.TextColor = bordercolor;
			tf.Placeholder = caption;

			// Icon
			if (icon != null) {
				UIImageView iv = UtilImage.RoundImage (icon, new RectangleF ((float)bounds.Width - 16 - 10, (float)bounds.Height / 2 - 8, 16, 16), false);
				box.AddSubview (iv);
			}

			box.UserInteractionEnabled = true;
			box.AddSubviews (tf);

			this.Frame = new CGRect (bounds.X, bounds.Y, bounds.Width, lbl.Frame.Height + 8 + box.Frame.Height);

			this.AddSubviews (box, lbl);
		}
	}

	#endregion
}