using System;
using UIKit;
using CoreGraphics;

namespace Commons
{
	public class Common
	{
		//		private UIFont _font10f, _font13f, _font16f, _font19f;
		//		private UIColor _white, _blue, _black, _clear;

		private string _fontName = "Helvetica Light";

		public Common ()
		{
		}

		public float PopUpDistance {
			get { return 200f; }
			set { ; }
		}

		public UIFont Font13F {
			get { return UIFont.FromName (_fontName, 13f); }
			set { ; }
		}

		public UIFont Font16F {
			get { return UIFont.FromName (_fontName, 16f); }
			set { ; }
		}

		public UIFont Font19F {
			get { return UIFont.FromName (_fontName, 19f); }
			set { ; }
		}

		public UIFont Font10F {
			get { return UIFont.FromName (_fontName, 10f); }
			set { ; }
		}

		public UIColor White {
			get { return UIColor.White; }
			set { ; }
		}

		public UIColor Blue {
			get { return UIColor.Blue; }
			set { ; }
		}

		public UIColor Black {
			get { return UIColor.Black; }
			set { ; }
		}

		public UIColor Blackish {
			get { return UIColor.FromRGB (52, 52, 52); }
			set { ; }
		}

		public UIColor BlackishWithAlpha {
			get { return UIColor.FromRGBA (52, 52, 52, 230); }
			set { ; }
		}

		public UIColor Clear {
			get { return UIColor.Clear; }
			set { ; }
		}

		public UIColor ColorStyle_1 {
			get { return UIColor.FromRGB (255, 162, 69); }
			set { ; }
		}

		public UITextAttributes commonStyle {
			get {
				UITextAttributes attri = new UITextAttributes () {
					Font = this.Font16F,
					TextColor = this.White
				};

				return attri;
				
			}
			set { ; }
		}
	}

	public class CustomHdrFtr
	{
		Common font = new Common ();

		public CustomHdrFtr ()
		{

		}

		/// <summary>
		/// Creates the hdr ftr.
		/// </summary>
		/// <returns>The hdr ftr.</returns>
		/// <param name="caption">Caption.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="fontcolor">Fontcolor. Null for default white color font.</param>
		public UIView CreateHdrFtr (string caption, float width, float height, UIColor fontcolor)
		{
			UIView view = new UIView (new CGRect (0, 0, (float)width, height)) {
				BackgroundColor = UIColor.Clear
			};

			if (fontcolor == null) {
				fontcolor = font.White;
			}

			UILabel text = new UILabel (new CGRect (15, 0, (float)width - 30, height)) {
				Text = caption,
				TextAlignment = UITextAlignment.Left,
				//TextColor = font.White,
				TextColor = fontcolor,
				Font = font.Font13F,
				BackgroundColor = font.Clear,
				AdjustsFontSizeToFitWidth = false,
				Lines = 2
			};

			//CGSize size = UIStringDrawing.StringSize (caption, font.Font16F, new CGSize (width, height), UILineBreakMode.WordWrap);
			text.Frame = new CGRect (15, 0, width - 30, height);

			view.AddSubview (text);

			return view;
		}
	}
}

