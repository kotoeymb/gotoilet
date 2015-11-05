using System;
using UIKit;
using MonoTouch.Dialog;
using Foundation;
using CoreGraphics;
using FontBase;

namespace CustomElements
{
	public class CustomCell : UITableViewCell
	{
		UILabel label;
		UIImageView imageView;
		UIView customColorView = new UIView ();
		Font font = new Font();

		public CustomCell (UITableViewCellStyle style, NSString id, string caption, UIImage img, UIColor textcolor) : base (style, id)
		{
			//SelectionStyle = UITableViewCellSelectionStyle.;
			BackgroundColor = UIColor.Clear;

			// Setting custom cell select color
			customColorView.BackgroundColor = UIColor.FromRGBA(0,0,0, 100);
			SelectedBackgroundView = customColorView;

			label = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = UIColor.Clear,
				TextColor = textcolor,
				Font = Font.Font16F,
				Lines = 0
			};

			imageView = new UIImageView ();
			imageView.Image = img;
			UpdateCell (caption, img, textcolor);

			ContentView.Add (label);
			ContentView.Add (imageView);
		}

		public void UpdateCell (string caption, UIImage img, UIColor textcolor)
		{
			label.Text = caption;
			label.TextColor = textcolor;
			imageView.Image = img;
		}

		public override void LayoutSubviews ()
		{
			base.LayoutSubviews ();
			var full = ContentView.Bounds;

			CGSize size = UIStringDrawing.StringSize (label.Text, Font.Font16F, new CGSize (full.Width, 40), UILineBreakMode.WordWrap);

			var captionFrame = full;
			captionFrame.X = 48;
			captionFrame.Y = 12;
			captionFrame.Height = size.Height;
			captionFrame.Width = full.Width;
			label.Frame = captionFrame;

			imageView.Frame = new CGRect (16, 13, 16, 16);
		}
	}

	public class CustomElement : Element, IElementSizing
	{
		public string Text { get; set; }
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
				cell = new CustomCell (UITableViewCellStyle.Default, Key, Text, Image, TextColor);
			} else {
				((CustomCell)cell).UpdateCell (Text, Image, TextColor);
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

		public nfloat GetHeight(UITableView tableView, NSIndexPath indexPath) {
			return 40f;
		}
	}
}