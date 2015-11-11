using System;
using UIKit;
using CoreGraphics;
using Common;

namespace Common
{
	public class Font
	{
		//		private UIFont _font10f, _font13f, _font16f, _font19f;
		//		private UIColor _white, _blue, _black, _clear;

		private string _fontName = "Helvetica Light";

		public Font ()
		{
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

		public UIColor Clear {
			get { return UIColor.Clear; }
			set { ; }
		}
	}

	public class CustomHdrFtr
	{
		Font font = new Font ();

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
		public UIView CreateHdrFtr (string caption, float width, float height)
		{
			UIView view = new UIView (new CGRect (0, 0, (float)width, height)) {
				BackgroundColor = font.Clear
			};

			UILabel text = new UILabel (new CGRect (15, 0, (float)width - 30, height)) {
				Text = caption,
				TextAlignment = UITextAlignment.Left,
				TextColor = font.White,
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

