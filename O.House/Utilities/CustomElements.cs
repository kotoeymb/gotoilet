using System;
using System.Drawing;
using UIKit;
using MonoTouch.Dialog;
using Foundation;
using CoreGraphics;
using CoreAnimation;
using Commons;
using Utils;
using Facebook.ShareKit;

namespace CustomElements
{
	#region Timeline element


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
		

		public CustomCell (UITableViewCellStyle style, NSString id, string caption, string subtitle, UIImage img) : base (style, id)
		{
			//SelectionStyle = UITableViewCellSelectionStyle.;
			BackgroundColor = UIColor.Clear;

			// Setting custom cell select color
			customColorView.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 100);
			SelectedBackgroundView = customColorView;

			label = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = Common.Clear,
				TextColor = Common.ColorStyle_1,
				Font = Common.Font16F,
				Lines = 0
			};

			sublabel = new UILabel () {
				TextAlignment = UITextAlignment.Left,
				BackgroundColor = Common.Clear,
				TextColor = Common.White,
				Font = Common.Font13F,
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

			CGSize size = UIStringDrawing.StringSize (label.Text, Common.Font16F, new CGSize (full.Width, 40), UILineBreakMode.WordWrap);
			CGSize size0 = UIStringDrawing.StringSize (sublabel.Text, Common.Font16F, new CGSize (full.Width, 40), UILineBreakMode.WordWrap);

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
		UITextField tf;
		UILabel lbl;
		float padding;
		UIColor bgcolor;
		UIView box;

		public TextField (CGRect bounds, UIColor bordercolor, string caption, UIImage icon = null, bool transparent = true)
		{
			this.Frame = bounds;
			BackgroundColor = Common.Clear;

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
				Font = Common.Font16F,
				TextColor = Common.ColorStyle_1
			};

			box.Frame = new CGRect (0, lbl.Frame.Y + lbl.Frame.Height + 8, bounds.Width, bounds.Height);

			// Textfield
			tf = new UITextField (new CGRect (padding, padding, bounds.Width - padding * 2 - 16, bounds.Height - padding * 2));
			tf.Font = Common.Font13F;
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