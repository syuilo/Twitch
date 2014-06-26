using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twitch.Net;
using Twitch.Twitter;

namespace Twitch.Twitter.APIs.REST
{
    /// <summary>
    /// ユーザー(アカウント)に関するAPI
    /// </summary>
    public static class Users
    {
        /// <summary>
        /// ユーザーを取得します。
        /// 
        /// Returns a variety of information about the user specified by the required user_id or screen_name parameter.
        /// The author's most recent Tweet will be returned inline when possible.
        ///
        /// GET users/lookup is used to retrieve a bulk collection of user objects.
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="user_id">取得するユーザーのID。</param>
        /// <param name="screen_name">取得するユーザーのScreenName。</param>
        /// <returns>ユーザー</returns>
        public static async Task<Twitter.User> Show(TwitterContext twitterContext, string user_id = null, string screen_name = null)
        {
            StringDictionary query = new StringDictionary();

            if (!string.IsNullOrEmpty(user_id))
                query["user_id"] = user_id;
            else if (!string.IsNullOrEmpty(screen_name))
                query["screen_name"] = screen_name;

            return new User(
                await new TwitterRequest(
                    twitterContext, API.Methods.GET,
                    new Uri(API.Urls.Users_Show), query).Request());
        }

        /// <summary>
        /// 対象のアカウントをスパムとして報告します。
        /// 
        /// Report the specified user as a spam account to Twitter.
        /// Additionally performs the equivalent of POST blocks/create on behalf of the authenticated user.
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="user_id">スパム報告されるユーザーのID。</param>
        /// <param name="screen_name">スパム報告されるユーザーのScreenName。</param>
        /// <returns>スパム報告されたユーザー</returns>
        public static async Task<Twitter.User> ReportSpam(TwitterContext twitterContext,
            string user_id = null,
            string screen_name = null)
        {
            StringDictionary query = new StringDictionary();

            if (user_id != string.Empty)
                query["user_id"] = user_id;
            else if (screen_name != string.Empty)
                query["screen_name"] = screen_name;

            return new User(
                await new TwitterRequest(
                    twitterContext, API.Methods.POST,
                    new Uri(API.Urls.Users_ReportSpam), query).Request());
        }
    }
}
