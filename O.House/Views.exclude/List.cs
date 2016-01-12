using System;
using System.Collections.Generic;

namespace O.House
{
	public class List
	{
		public List ()
		{
			List<Dictionary<string, string>> itemList = new List<Dictionary<string, string>> ();

			itemList.Add (
				new Dictionary<string, string> () {
					{"ItemID", "1"}, 
					{"ItemName", "Potato" }, 
					{"ItemCode", "FRU00001"},	
					{"ItemDescription", "Ism mouri jaki bur"},

				}
			);

			itemList.Add (
				new Dictionary<string, string> () {
					{"ItemID", "2"}, 
					{"ItemName", "Cucumber" }, 
					{"ItemCode", "FRU00002"},
					{"ItemDescription", "Jaki mouri bur"}
				}
			);

			// Print out
			for (var i = 0; i < itemList.Count; i++) {
				Console.WriteLine(
					"\nItemID : {0} \nItemName : {1} \nItemCode : {2} \nItemDescription : {3}", 
					itemList[i]["ItemID"], 
					itemList[i]["ItemName"], 
					itemList[i]["ItemCode"],
					itemList[i]["ItemDescription"]
				);
			}

			// Print with condition
			for (var i = 0; i < itemList.Count; i++) {
				if (itemList [i] ["ItemID"] == "3") {
					Console.WriteLine (
						"\nItemID : {0} \nItemName : {1} \nItemCode : {2} \nItemDescription : {3}", 
						itemList [i] ["ItemID"], 
						itemList [i] ["ItemName"], 
						itemList [i] ["ItemCode"],
						itemList [i] ["ItemDescription"]
					);
				}
			}

		}
	}
}