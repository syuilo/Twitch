using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Twitter
{
	/// <summary>
	/// TwitterのStatus(ツイート,つぶやき,投稿)です。
	/// </summary>
	public class Status : Twitter.Response.Tweets.Status
	{
		public Status(string json)
			: base(json) { }

		/// <summary>
		/// お気に入りに登録します。
		/// </summary>
		/// <returns></returns>
		public bool Favorite()
		{
			return false;
		}

		/// <summary>
		/// リツイートします。
		/// </summary>
		/// <returns></returns>
		public bool Retweet()
		{
			return false;
		}
	}
}
