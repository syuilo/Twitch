using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch.HTTP.Twitter;

namespace Twitch.Twitter.APIs.REST
{
	public static class Statuses
	{
		/// <summary>
		/// タイムラインを取得します。
		/// </summary>
		/// <param name="twitterContext">ユーザーのTwitterContext</param>
		/// <param name="count">取得するツイート数</param>
		/// <param name="since_id"></param>
		/// <param name="max_id"></param>
		/// <param name="trim_user"></param>
		/// <param name="exclude_replies"></param>
		/// <param name="contributor_details"></param>
		/// <param name="include_entities"></param>
		/// <returns></returns>
		public static async Task<List<Status>> HomeTimeline(
			TwitterContext twitterContext,
			double count = 0,
			string since_id = null,
			string max_id = null,
			bool trim_user = false,
			bool exclude_replies = false,
			bool contributor_details = false,
			bool include_entities = true)
		{
			StringDictionary query = new StringDictionary();
			query["count"] = count.ToString();
			query["since_id"] = since_id;
			query["max_id"] = max_id;
			query["trim_user"] = trim_user.ToString();
			query["exclude_replies"] = exclude_replies.ToString();
			query["contributor_details"] = contributor_details.ToString();
			query["include_entities"] = include_entities.ToString();

			string source = await new TwitterRequest(twitterContext, HTTP.Request.Method.GET, "https://api.twitter.com/1.1/statuses/home_timeline.json", query).Request();

			dynamic json = Utility.DynamicJson.Parse(source);

			List<Status> statuses = new List<Status>();
			foreach (dynamic status in json)
			{
				statuses.Add(new Status(status.ToString()));
			}

			return statuses;
		}

		/// <summary>
		/// ツイートをポストします。
		/// </summary>
		/// <param name="twitterContext">ユーザーのTwitterContext</param>
		/// <param name="status">本文</param>
		/// <param name="in_reply_to_status_id">返信元になるツイートのID</param>
		/// <param name="media"></param>
		/// <returns></returns>
		public static async Task<string> Update(TwitterContext twitterContext, string status, string in_reply_to_status_id = null)
		{
			StringDictionary query = new StringDictionary();
			query["status"] = status;
			query["in_reply_to_status_id"] = in_reply_to_status_id;

			return await new TwitterRequest(twitterContext, HTTP.Request.Method.POST, "https://api.twitter.com/1.1/statuses/update.json", query).Request();
		}

		/// <summary>
		/// 対象のツイートをリツイートします。
		/// </summary>
		/// <param name="twitterContext">TwitterContext</param>
		/// <param name="id">対象のツイートID</param>
		/// <returns></returns>
		public static async Task<string> Retweet(TwitterContext twitterContext, string id)
		{
			StringDictionary query = new StringDictionary();
			query["id"] = id;

			return await new TwitterRequest(twitterContext, HTTP.Request.Method.POST, "https://api.twitter.com/1.1/statuses/retweet/" + id + ".json", query).Request();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="twitterContext"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public static async Task<Twitter.Status> Show(TwitterContext twitterContext, string id)
		{
			StringDictionary query = new StringDictionary();
			query["id"] = id;

			return new Status(await new TwitterRequest(twitterContext, HTTP.Request.Method.GET, "https://api.twitter.com/1.1/statuses/show.json", query).Request());
		}
	}
}
