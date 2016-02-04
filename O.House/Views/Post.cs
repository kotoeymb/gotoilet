using System;

namespace OHouse
{
	public class Post
	{
		public string postTitle { get; set; }
		public string postDescp { get; set; }

		public Post(string title, string descp) {
			this.postTitle = title;
			this.postDescp = descp;
		}
	}
}

