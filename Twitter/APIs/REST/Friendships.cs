using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twitch.HTTP.Twitter;

namespace Twitch.Twitter.APIs.REST
{
	/// <summary>
	/// 
	/// </summary>
	public static class Friendships
	{
		/// <summary>
		/// 指定ユーザーをフォローします。
		/// </summary>
		/// <param name="twitterContext"></param>
		/// <param name="screen_name"></param>
		/// <param name="id"></param>
		/// <param name="follow"></param>
		/// <returns>指定ユーザーの Twitch.User オブジェクト</returns>
		public static async Task<Twitter.User> Create(TwitterContext twitterContext, string screen_name = null, string id = null, bool follow = false)
		{
			StringDictionary query = new StringDictionary();
			query["screen_name"] = screen_name;
			query["user_id"] = id;
			query["follow"] = follow.ToString();

			return new Twitter.User(
				await new TwitterRequest(
					twitterContext, HTTP.Request.Method.POST,
					"https://api.twitter.com/1.1/friendships/create.json", query).Request());
		}

		/// <summary>
		/// 指定ユーザーのフォローを解除します。
		/// </summary>
		/// <param name="twitterContext"></param>
		/// <param name="screen_name"></param>
		/// <param name="id"></param>
		/// <returns>指定ユーザーの Twitch.User オブジェクト</returns>
		public static async Task<Twitter.User> Destory(TwitterContext twitterContext, string screen_name = null, string id = null)
		{
			StringDictionary query = new StringDictionary();
			query["screen_name"] = screen_name;
			query["user_id"] = id;

			return new Twitter.User(
				await new TwitterRequest(
					twitterContext, HTTP.Request.Method.POST,
					"https://api.twitter.com/1.1/friendships/destroy.json", query).Request());
		}
	}
}
