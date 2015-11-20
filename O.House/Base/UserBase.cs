using System;

namespace OHouse
{
	public class UserBase
	{
		public string user_id { get; set; }

		public string facebook_id { get; set; }

		public string push_id{ get; set; }

		public string update_at{ get; set; }

		public bool status { get; set; }

		public UserBase (string user_id, string facebook_id, string push_id, string update_at, bool status)
		{
			this.user_id = user_id;
			this.facebook_id = facebook_id;
			this.push_id = push_id;
			this.update_at = update_at;
			this.status = status;
		}
	}
}

