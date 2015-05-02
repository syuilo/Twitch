using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch.Net;

namespace Twitch.Twitter.APIs.REST
{
    /// <summary>
    /// OAuthに関するAPI
    /// </summary>
	public static class Oauth
	{
		public static async Task<string> Authorize(TwitterContext twitterContext)
		{
            return await new TwitterRequest(twitterContext, API.Methods.GET, new Uri(API.Urls.Oauth_Authorize)).Request();
		}

		public static async Task<string> AccessToken(TwitterContext twitterContext, string oauth_verifier)
		{
			StringDictionary query = new StringDictionary();
			query["oauth_verifier"] = oauth_verifier;

            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Oauth_AccessToken), query).Request();
		}
		
		public static async Task<string> AccessToken(TwitterContext twitterContext, string x_auth_username, string x_auth_password)
		{
			StringDictionary query = new StringDictionary();
			query["x_auth_username"] = x_auth_username;
			query["x_auth_password"] = x_auth_password;
			query["x_auth_mode"] = "client_auth";

            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Oauth_AccessToken), query).Request();
		}

		public static async Task<string> RequestToken(TwitterContext twitterContext)
		{
            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Oauth_RequestToken)).Request();
		}
	}
}
