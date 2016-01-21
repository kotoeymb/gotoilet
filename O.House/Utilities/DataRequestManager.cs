using System;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;

using Foundation;
using UIKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using AssetsLibrary;

namespace OHouse.DRM
{
	/// <summary>
	/// Data request manager.
	/// </summary>
	public class DataRequestManager
	{

		private static readonly Encoding encoding = Encoding.UTF8;

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

		/// <summary>
		/// Gets the toilet list.
		/// From plist file name
		/// </summary>
		/// <returns>The toilet list.</returns>
		/// <param name="plistFileName">Plist file name.</param>
		public List<ToiletsBase> GetToiletList (string plistFileName)
		{
			List<ToiletsBase> toiletsList = new List<ToiletsBase> ();

			var path = NSBundle.MainBundle.PathForResource (plistFileName, "plist");
			var toilets = NSDictionary.FromFile (path);

			foreach (var toilet in toilets) {
				var obj = toilet.Value;
				var idKey = obj.ValueForKey ((NSString)"idKey").ToString ();
				var titleKey = obj.ValueForKey ((NSString)"titleKey").ToString ();
				var subtitleKey = obj.ValueForKey ((NSString)"subtitleKey").ToString ();
				var latitudeKey = obj.ValueForKey ((NSString)"latitudeKey").ToString ();
				var longitudeKey = obj.ValueForKey ((NSString)"longitudeKey").ToString ();
				var voteKey = obj.ValueForKey ((NSString)"voteKey").ToString ();
				double distance = 0.0;

				toiletsList.Add (
					new ToiletsBase (Int32.Parse (idKey), Int32.Parse (voteKey), titleKey, subtitleKey, "", double.Parse (longitudeKey), double.Parse (latitudeKey), distance, true)	
				);
			}
			return toiletsList;
		}

		/// <summary>
		/// Gets the spot info.
		/// </summary>
		/// <returns>The spot info.</returns>
		/// <param name="spot_id">Spot identifier.</param>
		public List<ToiletsBase> GetSpotInfoFromLocal (string plistFileName, int spot_id)
		{
			List<ToiletsBase> toiletsList = new List<ToiletsBase> ();

			var path = NSBundle.MainBundle.PathForResource (plistFileName, "plist");
			var toilets = NSDictionary.FromFile (path);

			foreach (var toilet in toilets) {

				var obj = toilet.Value;

				var idKey = obj.ValueForKey ((NSString)"idKey").ToString ();

				if (idKey != spot_id.ToString ()) {
					continue;
				}

				idKey = obj.ValueForKey ((NSString)"idKey").ToString ();
				var titleKey = obj.ValueForKey ((NSString)"titleKey").ToString ();
				var subtitleKey = obj.ValueForKey ((NSString)"subtitleKey").ToString ();
				var latitudeKey = obj.ValueForKey ((NSString)"latitudeKey").ToString ();
				var longitudeKey = obj.ValueForKey ((NSString)"longitudeKey").ToString ();
				var voteKey = obj.ValueForKey ((NSString)"voteKey").ToString ();
				double distance = 0.0;

				toiletsList.Add (
					new ToiletsBase (Int32.Parse (idKey), Int32.Parse (voteKey), titleKey, subtitleKey, "", double.Parse (longitudeKey), double.Parse (latitudeKey), distance, true)	
				);
			}
			return toiletsList;
		}

		#endregion

		#region Setters

		public bool RegisterVote ()
		{
			return true;
		}

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

		/// <summary>
		/// Registers the spot.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="modalViewController">Modal view controller.</param>
		public void RegisterSpot (ToiletsBase info, UIViewController modalViewController)
		{
			Uri setUrl = new Uri ("http://gstore.pcp.jp/api/reg_spot.php");
		
			WebClient request = new WebClient ();
			NameValueCollection postParameters = new NameValueCollection ();
		
			postParameters.Add ("user_id", "default");
			postParameters.Add ("title", info.title);
			postParameters.Add ("sub_title", info.sub_title);
			postParameters.Add ("latitude", info.latitude.ToString ());
			postParameters.Add ("longitude", info.longitude.ToString ());

			request.UploadValuesCompleted += (object sender, UploadValuesCompletedEventArgs e) => {
				string result = Encoding.UTF8.GetString (e.Result);
				var json = JObject.Parse (result);
				bool responseStatus = (bool)json ["status"];
		
				Console.WriteLine (info.picture + ":" + responseStatus.ToString ());
		
				if (responseStatus) {
					UIAlertView av = new UIAlertView (
						"Thank you!",
						"Your support has been registered!",
						null,
						"Got it!",
						null
					);

					av.Show ();

				} else {
					UIAlertView av = new UIAlertView (
						                 "Error",
						                 "Sorry for any incovenience, the data post has occured an error. Please try again!",
						                 null,
						                 "OK",
						                 null
					                 );
		
					av.Show ();
				}
			};
		
			request.UploadValuesAsync (setUrl, postParameters);
		
			modalViewController.DismissModalViewController (true);
		}

		#endregion
	}
}