using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

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

		//private static readonly Encoding encoding = Encoding.UTF8;

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
	    //to release data from server..posts = drm.GetDataList ("http://gstore.pcp.jp/api/get_spots.php", 0, 10, false);
		public List<ToiletsBase> GetDataList (string link, int startIndex = 0, int length = 10, bool loadAll = true)
		{
			List<ToiletsBase> t = new List<ToiletsBase> ();
			List<ToiletsBase> orderedList = new List<ToiletsBase> ();
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

			if (loadAll) {
				orderedList = t.OrderByDescending(o => o.spot_id).ToList();
			} else {
				orderedList = t.OrderByDescending(o => o.spot_id).Skip(startIndex).Take(length).ToList();
			}

			return orderedList;
		}

		/// <summary>
		/// Gets the data list.
		/// </summary>
		/// <returns>The data list.</returns>
		/// <param name="json">Json.</param>
		/// <param name="startIndex">Start index.</param>
		/// <param name="length">Length.</param>
		/// <param name="loadAll">If set to <c>true</c> load all.</param>
		public List<ToiletsBase> GetDataListJSON (string json, int startIndex = 0, int length = 10, bool loadAll = true)
		{
			List<ToiletsBase> t = new List<ToiletsBase> ();
			List<ToiletsBase> orderedList = new List<ToiletsBase> ();

			string jsonUn = json;

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

			if (loadAll) {
				orderedList = t.OrderByDescending(o => o.spot_id).ToList();
			} else {
				orderedList = t.OrderByDescending(o => o.spot_id).Skip(startIndex).Take(length).ToList();
			}

			return orderedList;
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

		public List<ToiletsBase> GetSpotInfoJSON (string json)
		{
			List<ToiletsBase> u = new List<ToiletsBase> ();

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
		public List<ToiletsBase> GetToiletList (string plistFileName, int startIndex = 0, int length = 10, bool loadAll = true)
		{
			List<ToiletsBase> toiletsList = new List<ToiletsBase> ();
			List<ToiletsBase> orderedList = new List<ToiletsBase> ();

			NSFileManager fileMgn = NSFileManager.DefaultManager;
			NSUrl[] paths = fileMgn.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
			string documentsDirectory = paths [0].Path;
			string fileName = Path.Combine (documentsDirectory, plistFileName);
			//var path = NSBundle.MainBundle.PathForResource (plistFileName, "plist");
			var toilets = NSDictionary.FromFile(fileName);

			foreach (var toilet in toilets) {
				var obj = toilet.Value;
				var idKey = obj.ValueForKey ((NSString)"spot_id").ToString ();
				var titleKey = obj.ValueForKey ((NSString)"title").ToString ();
				var subtitleKey = obj.ValueForKey ((NSString)"sub_title").ToString ();
				var latitudeKey = obj.ValueForKey ((NSString)"latitude").ToString ();
				var longitudeKey = obj.ValueForKey ((NSString)"longitude").ToString ();
				var voteKey = obj.ValueForKey ((NSString)"vote_cnt").ToString ();
				double distance = 0.0;

				toiletsList.Add (
					new ToiletsBase (Int32.Parse (idKey), Int32.Parse (voteKey), titleKey, subtitleKey, "", double.Parse (longitudeKey), double.Parse (latitudeKey), distance, true)	
				);
			}

			if (loadAll) {
				orderedList = toiletsList.OrderByDescending(o => o.spot_id).ToList();
			} else {
				orderedList = toiletsList.OrderByDescending(o => o.spot_id).Skip(startIndex).Take(length).ToList();
			}

			return orderedList;
		}

		/// <summary>
		/// Gets the spot info.
		/// </summary>
		/// <returns>The spot info.</returns>
		/// <param name="spot_id">Spot identifier.</param>
		public List<ToiletsBase> GetSpotInfoFromLocal (string plistFileName, int spot_id)
		{
			List<ToiletsBase> toiletsList = new List<ToiletsBase> ();

			NSFileManager fileMgn = NSFileManager.DefaultManager;
			NSUrl[] paths = fileMgn.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User);
			string documentsDirectory = paths [0].Path;
			string fileName = Path.Combine (documentsDirectory, plistFileName);
			var toilets = NSDictionary.FromFile(fileName);

			foreach (var toilet in toilets) {

				var obj = toilet.Value;

				var idKey = obj.ValueForKey ((NSString)"spot_id").ToString ();

				if (idKey != spot_id.ToString ()) {
					continue;
				}

				var titleKey = obj.ValueForKey ((NSString)"title").ToString ();
				var subtitleKey = obj.ValueForKey ((NSString)"sub_title").ToString ();
				var latitudeKey = obj.ValueForKey ((NSString)"latitude").ToString ();
				var longitudeKey = obj.ValueForKey ((NSString)"longitude").ToString ();
				var voteKey = obj.ValueForKey ((NSString)"vote_cnt").ToString ();
				double distance = 0.0;

				toiletsList.Add (
					new ToiletsBase (Int32.Parse (idKey), Int32.Parse (voteKey), titleKey, subtitleKey, "", double.Parse (longitudeKey), double.Parse (latitudeKey), distance, true)	
				);
			}
			return toiletsList;
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

		/// <summary>
		/// Registers the spot.
		/// </summary>
		/// <param name="info">Info.</param>
		/// <param name="modalViewController">Modal view controller.</param>
		public bool RegisterSpot (ToiletsBase info)
		{
			Uri setUrl = new Uri ("http://gstore.pcp.jp/api/reg_spot.php");
			bool responseStatus = false;
		
			WebClient request = new WebClient ();
			NameValueCollection postParameters = new NameValueCollection ();
		
			postParameters.Add ("user_id", "default");
			postParameters.Add ("title", info.title);
			postParameters.Add ("sub_title", info.sub_title);
			postParameters.Add ("latitude", info.latitude.ToString ());
			postParameters.Add ("longitude", info.longitude.ToString ());

			request.UploadValuesAsync (setUrl, postParameters);
			request.UploadValuesCompleted += (object sender, UploadValuesCompletedEventArgs e) => {
				string result = Encoding.UTF8.GetString (e.Result);
				var json = JObject.Parse (result);
				responseStatus = (bool)json ["status"];
			};

			return responseStatus;
		}

		public bool RegisterVote (int spotid)
		{
			Uri setUrl = new Uri ("http://gstore.pcp.jp/api/vote_spot.php");
			bool responseStatus = false;

			WebClient request = new WebClient ();
			NameValueCollection postParameters = new NameValueCollection ();

			postParameters.Add ("spot_id", spotid.ToString ());
			postParameters.Add ("user_id", "9cd18824abbdeaf5d7ca0d71b99a0267");

			Console.WriteLine (spotid);

			request.UploadValuesAsync (setUrl, postParameters);
			request.UploadValuesCompleted += (object sender, UploadValuesCompletedEventArgs e) => {
				string result = Encoding.UTF8.GetString (e.Result);
				var json = JObject.Parse (result);
				responseStatus = (bool)json ["status"];
			};

			return responseStatus;
		}

		#endregion
	}
}