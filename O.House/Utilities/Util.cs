﻿using System;
using System.Drawing;
using CoreGraphics;
using UIKit;
using Foundation;

namespace Utils
{
	/// <summary>
	/// This class provide some processing of the image
	/// </summary>
	public class UtilImage
	{
		/// <summary>
		/// Initializes a new instance of UtilImage class.
		/// <summary>
		static UtilImage ()
		{
		}

		/// <summary>
		/// <c>MaxResizeImage</c> is a method in the <c>UtilImage</c> class.
		/// Resize the image keeping aspect ratio
		/// </summary>
		/// <param name="srcImage">the image you want to change.</param>
		/// <param name="maxWidth">the size of width you want.</param>
		/// <param name="maxHeight">the size of height you want.</param>
		public static UIImage ResizeImageKeepAspect (UIImage srcImage, float maxWidth, float maxHeight)
		{
			// 返却画像
			UIImage resultImage;
			// ソースとなる画像のサイズ
			CGSize sourceSize = srcImage.Size;

			double maxResizeFactor = Math.Max (maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

			//System.Console.WriteLine ("maxResizeFactor:" + maxResizeFactor);
			// 最大サイズ(変更したいサイズがソースのサイズのまま)であれば、ソース画像を返却
			if (maxResizeFactor == 1) {
				resultImage = srcImage;
			} else {
				// 変更後の画像サイズを設定
				float width = (float)(maxResizeFactor * sourceSize.Width);
				float height = (float)(maxResizeFactor * sourceSize.Height);

				//画像をリサイズ 
				resultImage = _resizeImage (srcImage, width, height);
			}

			return resultImage;
		}

		public static UIImageView ResizeImageViewKeepAspect (UIImage srcImage, float maxWidth, float maxHeight)
		{
			// 返却画像
			UIImage resultImage;
			UIImageView resultImageView = new UIImageView ();
			// ソースとなる画像のサイズ
			CGSize sourceSize = srcImage.Size;

			double maxResizeFactor = Math.Max (maxWidth / sourceSize.Width, maxHeight / sourceSize.Height);

			//System.Console.WriteLine ("maxResizeFactor:" + maxResizeFactor);
			// 最大サイズ(変更したいサイズがソースのサイズのまま)であれば、ソース画像を返却
			if (maxResizeFactor == 1) {
				resultImage = srcImage;
				resultImageView.Image = resultImage;
			} else {
				// 変更後の画像サイズを設定
				float width = (float)(maxResizeFactor * sourceSize.Width);
				float height = (float)(maxResizeFactor * sourceSize.Height);

				//画像をリサイズ 
				resultImageView = _resizeImageView (srcImage, width, height);
			}

			return resultImageView;
		}

		/// <summary>
		/// <c>_resizeImage</c> is a method in the <c>UtilImage</c> class.
		/// this is a internal function
		/// Resize the image without considering the aspect ratio,
		/// </summary>
		/// <param name="srcImage">the image you want to change.</param>
		/// <param name="width">the size of width you want.</param>
		/// <param name="height">the size of height you want.</param>
		private static UIImage _resizeImage (UIImage srcImage, float width, float height)
		{
			//返却画像
			UIImage resultImage;

			// 指定サイズから、SizeF構造体のインスタンスを作成
			SizeF newSize = new SizeF (width, height);
			// 新しい画像のコンテキストを開始
			// UIGraphics.BeginImageContext (newSize);
			UIGraphics.BeginImageContextWithOptions (newSize, false, 2.0f);

			// 新しい矩形のインスタンスを作成
			RectangleF newRect = new RectangleF (0, 0, width, height);
			// 作成したコンテキストにレンダリング
			srcImage.Draw (newRect);
			//コンテキストにレンダリングした画像を取得する
			resultImage = UIGraphics.GetImageFromCurrentImageContext ();
			// コンテキストを閉じる
			UIGraphics.EndImageContext ();

			return resultImage;

		}

		private static UIImageView _resizeImageView (UIImage srcImage, float width, float height)
		{
			//返却画像
			UIImage resultImage;
			UIImageView resultImageView = new UIImageView ();

			// 指定サイズから、SizeF構造体のインスタンスを作成
			SizeF newSize = new SizeF (width, height);
			// 新しい画像のコンテキストを開始
			// UIGraphics.BeginImageContext (newSize);
			UIGraphics.BeginImageContextWithOptions (newSize, false, 2.0f);

			// 新しい矩形のインスタンスを作成
			RectangleF newRect = new RectangleF (0, 0, width, height);
			CGRect newCGRect = new CGRect (0, 0, width, height);
			// 作成したコンテキストにレンダリング
			srcImage.Draw (newRect);
			//コンテキストにレンダリングした画像を取得する
			resultImage = UIGraphics.GetImageFromCurrentImageContext ();
			resultImageView.Image = resultImage;
			resultImageView.Frame = newCGRect;
			// コンテキストを閉じる
			UIGraphics.EndImageContext ();

			return resultImageView;
		}

		/// <summary>
		/// <c>CropImage</c> is a method in the <c>UtilImage</c> class.
		/// Crop the image with a specified size and postion of rect ,
		/// </summary>
		/// <param name="srcImage">the image you want to change.</param>
		/// <param name="cropX">the position X of RectangleF.</param>
		/// <param name="cropY">the position Y of RectangleF.</param>
		/// <param name="width">the width of RectangleF.</param>
		/// <param name="height">the height of RectangleF.</param>
		public static UIImage CropImage (UIImage srcImage, float cropX, float cropY, float width, float height)
		{
			CGSize imageSize = srcImage.Size;
			float drawRectW = (float)imageSize.Width;
			float drawRectH = (float)imageSize.Height;

			// 指定サイズから、SizeF構造体のインスタンスを作成
			SizeF newSize = new SizeF (width, height);
	
			// 新しい画像のコンテキストを開始
			UIGraphics.BeginImageContextWithOptions (newSize, false, 2.0f);
			// コンテキストを取得
			CGContext context = UIGraphics.GetCurrentContext ();
			// 切り取る矩形のインスタンスを作成
			RectangleF clippedRect = new RectangleF (0, 0, width, height);
			context.ClipToRect (clippedRect);

			RectangleF drawRect = new RectangleF (-cropX, -cropY, drawRectW, drawRectH);
			srcImage.Draw (drawRect);

			// コンテキストにレンダリングした画像を取得
			using (UIImage croppedImage = UIGraphics.GetImageFromCurrentImageContext ()) {
				UIGraphics.EndImageContext ();
				croppedImage.Dispose ();
				return croppedImage;
			}
		}

		/// <summary>
		/// Rounds the image.
		/// </summary>
		/// <returns>The image.</returns>
		/// <param name="srcImage">Source image.</param>
		/// <param name="rect">Rect.</param>
		/// <param name="border">If set to <c>true</c> border.</param>
		public static UIImageView RoundImage (UIImage srcImage, RectangleF rect, bool border)
		{
			if (srcImage == null) {
				throw new System.ArgumentException ("srcImage", "null");
			}
			RectangleF customSize = rect;
			var container = new UIImageView (srcImage);

			container.Frame = customSize;
			container.Layer.CornerRadius = container.Frame.Size.Width / 2;

			if (border) {
				container.Layer.BorderWidth = 3f;
				container.Layer.BorderColor = new CGColor (255f, 255f, 255f);
			}

			container.ClipsToBounds = true;

			return container;
		}

		/// <summary>
		/// Rounds the button.
		/// </summary>
		/// <returns>The button.</returns>
		/// <param name="srcImage">Source image.</param>
		/// <param name="rect">Rect.</param>
		/// <param name="border">If set to <c>true</c> border.</param>
		public static UIButton RoundButton (UIImage srcImage, RectangleF rect, UIColor bgcolor, bool border)
		{
			if (srcImage == null) {
				throw new System.ArgumentException ("srcImage", "null");
			}
			RectangleF customSize = rect;
			var container = new UIButton ();

			container.SetImage (srcImage, UIControlState.Normal);
			container.Frame = customSize;
			container.BackgroundColor = bgcolor;
			container.Layer.CornerRadius = container.Frame.Size.Width / 2;
			if (border) {
				container.Layer.BorderWidth = 1f;
				container.Layer.BorderColor = new CGColor (255f, 255f, 255f);
			}
			container.ClipsToBounds = true;

			return container;
		}

		/// <summary>
		/// Images the button.
		/// </summary>
		/// <returns>The button.</returns>
		/// <param name="srcImage">Source image.</param>
		/// <param name="rect">Rect.</param>
		/// <param name="border">If set to <c>true</c> border.</param>
		/// <param name="alignment">Alignment.</param>
		public static UIButton ImageButton (UIImage srcImage, CGRect rect, bool border, UITextAlignment alignment)
		{
			if (srcImage == null) {
				throw new System.ArgumentException ("srcImage", "null");
			}

			CGRect customSize = rect;
			var container = new UIButton ();

			UIImage src = srcImage;
			src.Scale (new CGSize (customSize.Height, customSize.Height));

			UIImageView icon = new UIImageView ();

			if (customSize.Width <= customSize.Height) {
				icon.Frame = new CGRect (0, 0, customSize.Height, customSize.Height);
			} else {
				if (alignment == UITextAlignment.Left) {
					icon.Frame = new CGRect (0, 0, customSize.Height, customSize.Height);
				} else if (alignment == UITextAlignment.Right) {
					icon.Frame = new CGRect (customSize.Width - customSize.Height, 0, customSize.Height, customSize.Height);
				} else if (alignment == UITextAlignment.Center) {
					icon.Frame = new CGRect (customSize.Width / 2 - customSize.Height / 2, 0, customSize.Height, customSize.Height);
				} else {
					icon.Frame = new CGRect (0, 0, customSize.Height, customSize.Height);
				}
			}
				
			icon.Image = src;
			container.Frame = customSize;
			container.AddSubview (icon);
			//srcImage.Scale(new CGRect (15, 15, customSize.Height, customSize.Height - 30);
			if (border) {
				container.Layer.BorderWidth = 1f;
				container.Layer.BorderColor = new CGColor (255f, 255f, 255f);
			}

			return container;
		}

		/// <summary>
		/// <c>CropImageWithRect</c> is a method in the <c>UtilImage</c> class.
		/// Crop the image with specified rect ,
		/// </summary>
		/// <param name="srcImage">the image you want to change.</param>
		/// <param name="rect">RectangleF youwant to crop.</param>
		public static UIImage CropImageWithRect (UIImage srcImage, RectangleF rect)
		{
			if (srcImage == null) {
				throw new System.ArgumentNullException ("srcImage", "null");
			}
			using (CGImage cgImage = srcImage.CGImage.WithImageInRect (rect)) {
				UIImage croppedImage = UIImage.FromImage (cgImage);
				return croppedImage;
			}
		}

		/// <summary>
		/// Froms the UR.
		/// </summary>
		/// <returns>The UR.</returns>
		/// <param name="uri">URI.</param>
		public static UIImage FromURL (string uri)
		{
			using (var url = new NSUrl (uri)) {
				using (var data = NSData.FromUrl (url)) {
					return UIImage.LoadFromData (data);
				}	
			}
		}

		/// <summary>
		/// Gets the colored image.
		/// </summary>
		/// <returns>The colored image.</returns>
		/// <param name="imageName">Image name.</param>
		/// <param name="color">Color.</param>
		public static UIImage GetColoredImage(string imageName, UIColor color)
		{
			UIImage image = UIImage.FromBundle(imageName);
			UIImage coloredImage = null;

			UIGraphics.BeginImageContext(image.Size);
			using (CGContext context = UIGraphics.GetCurrentContext())
			{
				context.TranslateCTM(0, image.Size.Height);
				context.ScaleCTM(1.0f, -1.0f);

				//var rect = new RectangleF(0, 0, image.Size.Width, image.Size.Height);
				var rect = new CGRect(0, 0, image.Size.Width, image.Size.Height);

				// draw image, (to get transparancy mask)
				context.SetBlendMode(CGBlendMode.Normal);
				context.DrawImage(rect, image.CGImage);

				// draw the color using the sourcein blend mode so its only draw on the non-transparent pixels
				context.SetBlendMode(CGBlendMode.SourceIn);
				context.SetFillColor(color.CGColor);
				context.FillRect(rect);

				coloredImage = UIGraphics.GetImageFromCurrentImageContext();
				UIGraphics.EndImageContext();
			}
			return coloredImage;
		}
	}
}