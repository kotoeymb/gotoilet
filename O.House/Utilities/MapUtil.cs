using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

using MonoTouch.Dialog;
using MonoTouch.SlideoutNavigation;

using Foundation;
using MapKit;
using UIKit;
using Utils;
using CoreGraphics;
using CoreLocation;
using OHouse;

namespace MapUtils
{
	/// <summary>
	/// Map util.
	/// </summary>
	public class MapUtil : MKAnnotation
	{
		// Declaration
		string _title;
		string _subtitle;
		CLLocationCoordinate2D _coord;

		/// <summary>
		/// Initializes a new instance of the <see cref="MapUtils.MapUtil"/> class.
		/// </summary>
		/// <param name="coord">Coordinate.</param>
		/// <param name="title">Title.</param>
		/// <param name="subtitle">Subtitle.</param>
		public MapUtil (CLLocationCoordinate2D coord, string title, string subtitle) : base ()
		{
			_coord = coord;
			_title = title;
			_subtitle = subtitle;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MapUtils.MapUtil"/> class.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="coord">Coordinate.</param>
		public MapUtil (string title, CLLocationCoordinate2D coord)
		{
			_title = title;
			_coord = coord;
		}

		/// <summary>
		/// Gets the title.
		/// </summary>
		/// <value>The title.</value>
		public override string Title {
			get {
				return _title;
			}
		}

		/// <summary>
		/// Gets the subtitle.
		/// </summary>
		/// <value>The subtitle.</value>
		public override string Subtitle {
			get {

				return _subtitle;
			}
		}

		/// <summary>
		/// Gets the coordinate.
		/// </summary>
		/// <value>The coordinate.</value>
		public override CLLocationCoordinate2D Coordinate {
			get {
				return _coord;
			}
		}
	}
}

