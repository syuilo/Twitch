using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch.Twitter.API;

namespace Twitch.Streaming
{
	public class SiteStream : StreamingBase
	{
		/// <summary>
		/// 受信するユーザーのID。
		/// </summary>
		public List<Int64> Follow
		{
			get;
			set;
		}

		/// <summary>
		/// SiteStream を初期化します。
		/// </summary>
		/// <param name="follow">ツイートを受け取るユーザーのユーザーIDのリスト</param>
		public SiteStream(params string[] follow)
		{
			this.StreamMessaged += new StreamMessagedEventHandler(StreamingCallback);

			this.Url = "https://sitestream.twitter.com/1.1/site.json";
			this.Host = "sitestream.twitter.com";
            this.Method = Methods.GET;

			StringDictionary query = new StringDictionary();

			query["follow"] = string.Join(",", follow);

			this.Parameter = query;
		}

		private void StreamingCallback(object sender, StreamEventArgs e)
		{
			var json = Twitch.Utility.DynamicJson.Parse(e.Data);
			Console.WriteLine(e.Data);
		}


	}
}
