// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace OHouse
{
	[Register ("DetailViewController")]
	partial class DetailViewController
	{
		[Outlet]
		UIKit.UIImageView BGImage { get; set; }

		[Outlet]
		UIKit.UIImageView ImageV { get; set; }

		[Outlet]
		UIKit.UILabel Title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (BGImage != null) {
				BGImage.Dispose ();
				BGImage = null;
			}

			if (ImageV != null) {
				ImageV.Dispose ();
				ImageV = null;
			}

			if (Title != null) {
				Title.Dispose ();
				Title = null;
			}
		}
	}
}
