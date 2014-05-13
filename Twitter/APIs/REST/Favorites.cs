using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch.HTTP.Twitter;

namespace Twitch.Twitter.APIs.REST
{
	public static class Favorites
	{
		/// <summary>
		/// 対象のツイートをお気に入りに登録します。
		/// </summary>
		/// <param name="twitterContext">TwitterContext</param>
		/// <param name="id">対象のツイートID</param>
		/// <returns></returns>
		public static async Task<Twitter.Status> Create(TwitterContext twitterContext, string id)
		{
			StringDictionary query = new StringDictionary();
			query["id"] = id;

			string res = await new TwitterRequest(twitterContext, HTTP.Request.Method.POST, "https://api.twitter.com/1.1/favorites/create.json", query).Request();
			
			return res != null ? new Status(res) : null;
		}

		/// <summary>
		/// 対象のツイートをお気に入りから削除します。
		/// </summary>
		/// <param name="twitterContext">TwitterContext</param>
		/// <param name="id">対象のツイートID</param>
		/// <returns></returns>
		public static async Task<Twitter.Status> Destroy(TwitterContext twitterContext, string id)
		{
			StringDictionary query = new StringDictionary();
			query["id"] = id;

			string res = await new TwitterRequest(twitterContext, HTTP.Request.Method.POST, "https://api.twitter.com/1.1/favorites/destroy.json", query).Request();

			return res != null ? new Status(res) : null;
		}
	}
}
