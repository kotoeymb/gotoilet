using System;
using System.Drawing;
using UIKit;
using MonoTouch.Dialog;
using Foundation;
using CoreGraphics;
using Commons;
using Utils;

namespace CustomElements
{
	public class TimelineCell : UITableViewCell
	{
		private UIButton ShareBtn;

		private UILabel Title;
		private UILabel Info;

		private UILabel Count;

		private UIButton LikeBtn;

		private UIView Container;
		private UIImageView MapViewView;

		private UIView customColorView = new UIView();

		Common common = new Common ();

		public TimelineCell (UITableViewCellStyle style, NSString id, UIImage mapView, string title, string info, int count) : base (style, id)
		{
			BackgroundColor = UIColor.Clear;

			customColorView.BackgroundColor = UIColor.Clear;
			SelectedBackgroundView = customColorView;

			Title = new UILabel () {
				Font = common.Font13F,
				TextColor = common.Blue,
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear
			};

			Info = new UILabel () {
				Font = common.Font13F,
				TextColor = common.Blackish,
				TextAlignment = UITextAlignment.Left,
				Lines = 3,
				AdjustsFontSizeToFitWidth = false,
				BackgroundColor = UIColor.Clear
					
			};

			Count = new UILabel () {
				Font = common.Font13F,
				TextColor = common.Blackish,
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear
			};

			ShareBtn = new UIButton () {
				BackgroundColor = UIColor.Clear
			};

			ShareBtn.SetImage(UtilImage.ResizeImageKeepAspect(UIImage.FromBundle ("images/icons/icon-share"), 16, 16), UIControlState.Normal);
			ShareBtn.TouchUpInside += (s, e) => {
				UIAlertView alert = new UIAlertView("hi", count.ToString(), null, "ok");
				alert.Show();
			};
					
			LikeBtn = new UIButton () {
				BackgroundColor = UIColor.Clear
			};

			LikeBtn.SetImage (UtilImage.ResizeImageKeepAspect (UIImage.FromBundle ("images/icons/icon-heart"), 16, 16), UIControlState.Normal);

			MapViewView = new UIImageView () { 
				Image = mapView,
				BackgroundColor = UIColor.FromRGB (25, 25, 25)
			};

			UpdateCell (mapView, title, info, count);
			Container = new UIView () {
				BackgroundColor = UIColor.White
			};
			Container.AddSubviews (MapViewView, Title, Info, Count, ShareBtn, LikeBtn);

			ContentView.Add (Container);
		}

		public void UpdateCell (UIImage mapview, string title, string info, int count)
		{
			MapViewView.Image = mapview;
			Title.Text = title;
			Info.Text = info;
			Count.Text = count.ToString();
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();

			//CGRect full = ContentView.Bounds;
			CGRect full = ContentView.Frame;
			float padding = 15;

			// UIView - container
			Container.Frame = new CGRect (padding, padding, full.Width - (padding * 2), full.Height - padding);
			CGRect CFull = Container.Frame;

			// UIImageView mapView
			MapViewView.Frame = new CGRect (10, 10, CFull.Width - 20, 128);

			// UILabel title
			Title.Frame = new CGRect (10, MapViewView.Frame.Height + MapViewView.Frame.Y + 10, CFull.Width - 20, 24);

			// UILabe info
			CGSize size = UIStringDrawing.StringSize (Info.Text, common.Font13F, new CGSize (CFull.Width, 60), UILineBreakMode.WordWrap);
			Info.Frame = new CGRect (10, Title.Frame.Y + Title.Frame.Height, CFull.Width - 20, size.Height);

			// UIImage likeBtn
			//LikeBtn.Frame = new CGRect (10, Info.Frame.Y + size.Height + 15, 16, 16);
			LikeBtn.Frame = new CGRect (10, full.Height - 16 - 30, 16, 16);

			// UILabel count
			//Count.Frame = new CGRect (10 + LikeBtn.Frame.Width + 5, LikeBtn.Frame.Y, 16, 16);
			Count.Frame = new CGRect (10 + LikeBtn.Frame.Width + 5, LikeBtn.Frame.Y, 16, 16);

			// UIButton shareBtn
			ShareBtn.Frame = new CGRect (CFull.Width - 10 - 16, LikeBtn.Frame.Y, 16, 16);
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
			if (Tapped != null) {
				Tapped ();
			}
		}

		public nfloat GetHeight (UITableView tableView, NSIndexPath indexPath)
		{
			return 280f;
		}
	}

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

		public CustomCell (UITableViewCellStyle style, NSString id, string caption, string subtitle, UIImage img, UIColor textcolor) : base (style, id)
		{
			//SelectionStyle = UITableViewCellSelectionStyle.;
			BackgroundColor = UIColor.Clear;

			// Setting custom cell select color
			customColorView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 100);
			SelectedBackgroundView = customColorView;

			label = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = common.Clear,
				TextColor = textcolor,
				Font = common.Font16F,
				Lines = 0
			};

			sublabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = common.Clear,
				TextColor = textcolor,
				Font = common.Font13F,
				AdjustsFontSizeToFitWidth = false,
				Lines = 2,
			};

			imageView = new UIImageView ();
			imageView.Image = img;
			UpdateCell (caption, subtitle, img, textcolor);

			ContentView.Add (label);
			ContentView.Add (sublabel);
			ContentView.Add (imageView);
		}

		public void UpdateCell (string caption, string subtitle, UIImage img, UIColor textcolor)
		{
			label.Text = caption;
			label.TextColor = textcolor;
			sublabel.Text = subtitle;
			sublabel.TextColor = textcolor;

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

		public UIColor TextColor { get; set; }

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
				cell = new CustomCell (UITableViewCellStyle.Default, Key, Text, SubTitle, Image, TextColor);
			} else {
				((CustomCell)cell).UpdateCell (Text, SubTitle, Image, TextColor);
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
}