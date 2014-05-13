using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twitch.HTTP.Twitter;

namespace Twitch.Twitter.APIs.REST
{
	public static class Account
	{
		public static async Task<Twitter.User> UpdateProfile(TwitterContext twitterContext, string name = null, Uri url = null, string location = null, string description = null)
		{
			StringDictionary query = new StringDictionary();

			if (name != null)
				query["name"] = name;
			if (url != null)
				query["url"] = url.ToString();
			if (location != null)
				query["location"] = location;
			if (description != null)
				query["description"] = description;

			return new User(await new TwitterRequest(twitterContext, HTTP.Request.Method.POST, "https://api.twitter.com/1.1/account/update_profile.json", query).Request());
		}

		/// <summary>
		/// TwitterContextのアイコンを更新します。
		/// </summary>
		/// <param name="twitterContext"></param>
		/// <param name="image">base64エンコードされたgifまたはjpgまたはpngの画像。</param>
		/// <returns></returns>
		public static async Task<Twitter.User> UpdateProfileImage(TwitterContext twitterContext, string image)
		{
			StringDictionary query = new StringDictionary();

			query["image"] = image;

			return new User(await new TwitterRequest(twitterContext, HTTP.Request.Method.POST, "https://api.twitter.com/1.1/account/update_profile_image.json", query).Request());
		}
	}
}
