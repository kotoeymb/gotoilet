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

		#region Getters

		/// <summary>
		/// Gets the json data.
		/// </summary>
		/// <returns>The json data.</returns>
		/// <param name="link">Link.</param>
		private string GetJsonData (string link)
		{
			var request = HttpWebRequest.Create (string.Format (link, ""));
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
		public List<ToiletsBase> GetDataList (string link)
		{
			List<ToiletsBase> t = new List<ToiletsBase> ();
			string json = "";

			string jsonUn = GetJsonData (link);

			if (jsonUn.IndexOf ("[") == 0 && jsonUn.IndexOf ("]") == jsonUn.Length - 1) {
				json = jsonUn.Remove (0, 1);
				json = json.Remove (json.Length - 1);
				json = json.Replace ("},{", "}{");
			} else {
				json = jsonUn;
			}

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

		/// <summary>
		/// Gets the user info.
		/// </summary>
		/// <returns>The user info.</returns>
		/// <param name="link">Link.</param>
		public List<UserBase> GetUserInfo (string facebook_id)
		{
			List<UserBase> u = new List<UserBase> ();

			string get = "http://gstore.pcp.jp/api/get_user.php?facebook_id=" + facebook_id;

			string json = GetJsonData (get);

			JsonTextReader reader = new JsonTextReader (new StringReader (json));
			reader.SupportMultipleContent = true;
			while (true) {
				if (!reader.Read ())
					break;

				JsonSerializer s = new JsonSerializer ();
				UserBase ba = s.Deserialize<UserBase> (reader);

				u.Add (ba);
			}

			return u;
		}

		/// <summary>
		/// Gets the spot info.
		/// </summary>
		/// <returns>The spot info.</returns>
		/// <param name="spot_id">Spot identifier.</param>
		public List<ToiletsBase> GetSpotInfo (int spot_id)
		{
			List<ToiletsBase> u = new List<ToiletsBase> ();

			string get = "http://gstore.pcp.jp/api/get_spots_info.php?spot_id=" + spot_id;

			string json = GetJsonData (get);

			if (json.IndexOf ("[") == 0 && json.IndexOf ("]") == json.Length - 1) {
				json = json.Remove (0, 1);
				json = json.Remove (json.Length - 1);
				json = json.Replace ("},{", "}{");
			}

			JsonTextReader reader = new JsonTextReader (new StringReader (json));
			reader.SupportMultipleContent = true;
			while (true) {
				if (!reader.Read ())
					break;

				JsonSerializer s = new JsonSerializer ();
				ToiletsBase ba = s.Deserialize<ToiletsBase> (reader);

				u.Add (ba);
			}

			return u;
		}

		#endregion

		#region Setters

		/// <summary>
		/// Registers the user.
		/// If user exists or user successfully register, return true
		/// If user not exists (will create), error, return false
		/// </summary>
		/// <param name="link">Link. Notice, include starting from ? e.g, www.serverlink.com?id=</param>
		/// <param name="facebook_id">Facebook identifier.</param>
		public bool RegisterUser (string facebook_id)
		{
			List<UserBase> u = new List<UserBase> ();

			string set = "http://gstore.pcp.jp/api/reg_user.php?facebook_id=" + facebook_id;
			string get = "http://gstore.pcp.jp/api/get_user.php?facebook_id=" + facebook_id;

			u = GetUserInfo (get);

			foreach (var i in u) {
				/// check user exists or not
				/// if i.status is true, exist, simply return false, user not register
				/// if i.status is false, not exist, create user
				if (!i.status) {
					// register user
					var request = HttpWebRequest.Create (string.Format (set, ""));
					request.Method = "POST";
					request.Headers.Add ("facebook_id", facebook_id);

					using (HttpWebResponse response = request.GetResponse () as HttpWebResponse) {
						if (response.StatusCode != HttpStatusCode.OK) {
							// not successful
							return false;
						}
						// successfully created
						return true;
					}
				}
				return true;
			}
			return false;
		}

		#endregion
	}
}