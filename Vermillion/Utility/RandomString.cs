using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Vermillion.Utility
{
	public static class RandomString
	{
		public static async Task<string> Generate()
		{
			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("http://ja.wikipedia.org/w/api.php?action=query&list=random&rnnamespace=0&rnlimit=1&format=json");
			System.Net.WebResponse res = await Request.GetResponseAsync();
			System.IO.Stream resStream = res.GetResponseStream();
			System.IO.StreamReader sr = new System.IO.StreamReader(resStream);
			string strJson = sr.ReadToEnd();
			dynamic json = Twitch.Utility.DynamicJson.Parse(strJson);
			return json.query.random[0].title;
		}
	}
}
