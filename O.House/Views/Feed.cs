using System;
using System.Collections.Generic;
using Foundation;

namespace OHouse
{
	public class Feed {

		//NSMutableArray posts;
		List<Post> posts;

		public Feed() {

		}

		//public NSMutableArray GetFeeds() {
		public List<Post> GetFeeds() {
			//posts = new NSMutableArray ();
			posts = new List<Post>();
			for (var i = 0; i <= 5; i++) {
				Post p = new Post("title" + i, "descp" + i);
				//posts.Add (NSObject.FromObject(p));
				posts.Add(p);
			}

			return posts;
		}
	}
}