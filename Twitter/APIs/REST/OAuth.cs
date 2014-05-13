using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch.HTTP.Twitter;

namespace Twitch.Twitter.APIs.REST
{
	public static class OAuth
	{
		//namespace Utility
		//{
		//	public static class API
		//	{
		//		public static async Task<string> GetAuthorizeUrl(string oauth_consumer_key, string oauth_consumer_secret)
		//		{
		//			TwitterContext tw = new TwitterContext()
		//			{
		//				ConsumerKey = oauth_consumer_key,
		//				ConsumerSecret = oauth_consumer_secret
		//			};

		//			string res = await Twitter.API.REST.oauth.request_token(tw);

		//			string oauth_token = res.Substring(res.IndexOf("oauth_token") + "oauth_token".Length + 1, res.IndexOf("&") - (res.IndexOf("oauth_token") + "oauth_token".Length + 1));
		//			string oauth_token_secret = res.Substring(res.IndexOf("oauth_token_secret") + "oauth_token_secret".Length + 1, res.IndexOf("&", res.IndexOf("&") + 1) - (res.IndexOf("oauth_token_secret") + "oauth_token_secret".Length + 1));

		//			return "https://api.twitter.com/oauth/authorize?oauth_token=" + oauth_token;
		//		}
		//	}
		//}

		public static async Task<string> authorize(TwitterContext twitterContext)
		{
			return await new TwitterRequest(twitterContext, HTTP.Request.Method.GET, "https://api.twitter.com/oauth/authorize").Request();
		}

		public static async Task<string> access_token(TwitterContext twitterContext, string oauth_verifier)
		{
			StringDictionary query = new StringDictionary();
			query["oauth_verifier"] = oauth_verifier;

			return await new TwitterRequest(twitterContext, HTTP.Request.Method.POST, "https://api.twitter.com/oauth/access_token", query).Request();
		}

		public static async Task<string> request_token(TwitterContext twitterContext)
		{
			return await new TwitterRequest(twitterContext, HTTP.Request.Method.POST, "https://api.twitter.com/oauth/request_token").Request();
		}
	}
}
