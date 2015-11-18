using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

using Foundation;
using UIKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OHouse.DRM
{
	/// <summary>
	/// Data request manager.
	/// </summary>
	public class DataRequestManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="OHouse.DRM.DataRequestManager"/> class.
		/// </summary>
		public DataRequestManager ()
		{
			// Perform any additional setup after loading the view, typically from a nib.
		}

		/// <summary>
		/// Gets the json data.
		/// </summary>
		/// <returns>The json data.</returns>
		/// <param name="link">Link.</param>
		private string GetJsonData (string link)
		{
			var request = HttpWebRequest.Create (string.Format (link, "19440"));
			request.ContentType = "application/json";
			request.Method = "GET";

			using (HttpWebResponse response = request.GetResponse () as HttpWebResponse) {
				if (response.StatusCode != HttpStatusCode.OK) {
					Console.WriteLine ("Error fetching data");
				}

				using (StreamReader reader = new StreamReader (response.GetResponseStream ())) {
					var content = reader.ReadToEnd ();
					if (string.IsNullOrWhiteSpace (content)) {
						return "";
					} else {
						return content;
					}
				}
			}
		}

		/// <summary>
		/// Gets the data list.
		/// </summary>
		/// <returns>The data list.</returns>
		/// <param name="link">Link.</param>
		public List<ToiletsBase> GetDataList(string link) {

			List<ToiletsBase> t = new List<ToiletsBase>();
			string json = link;

			string jsonUn = GetJsonData (link);

			if (jsonUn.IndexOf ("[") == 0 && jsonUn.IndexOf("]") == jsonUn.Length - 1) {
				json = jsonUn.Remove(0, 1);
				json = json.Remove(json.Length - 1);
				json = json.Replace ("},{", "}{");
			}

			Console.WriteLine (json);

			JsonTextReader reader = new JsonTextReader (new StringReader (json));
			reader.SupportMultipleContent = true;
			while (true) {
				if (!reader.Read ())
					break;

				JsonSerializer s = new JsonSerializer ();
				ToiletsBase ba = s.Deserialize<ToiletsBase> (reader);

				t.Add (ba);
			}

			return t;
		}
	}
}

