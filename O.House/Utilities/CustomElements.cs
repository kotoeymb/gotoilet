using System;
using UIKit;
using MonoTouch.Dialog;
using Foundation;
using CoreGraphics;
using Commons;

namespace CustomElements
{
	public class CustomCell : UITableViewCell
	{
		UILabel label;
		UILabel sublabel;
		UIImageView imageView;
		UIView customColorView = new UIView ();
		Common common= new Common ();

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

			var captionFrame = full;
			var subFrame = full;
			nfloat x = 15 + 32 + 15;
			nfloat centerY = full.Height/2 - size.Height/2;

			captionFrame.X = x;
			captionFrame.Y = centerY - (size.Height/2);
			captionFrame.Height = size.Height;
			captionFrame.Width = full.Width - (x + 15);

			subFrame.X = x;
			subFrame.Y = centerY + (size0.Height/2);
			subFrame.Height = size0.Height;
			subFrame.Width = full.Width - (x + 15);

			label.Frame = captionFrame;
			sublabel.Frame = subFrame;

			imageView.Frame = new CGRect (15, full.Height/2 - (32/2), 32, 32);
		}
	}

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
			return 120f;
		}
	}
}