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
    /// ミュート機能に関するAPI
    /// </summary>
    public static class Mutes
    {
        /// <summary>
        /// 指定されたユーザーをミュートします。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="screen_name">ミュートするユーザーのScreenName。</param>
        /// <param name="id">ミュートするユーザーのID。</param>
        /// <returns>ミュートされたユーザー</returns>
        public static async Task<Twitter.User> UsersCreate(TwitterContext twitterContext, string screen_name = null, string id = null)
        {
            StringDictionary query = new StringDictionary();
            query["screen_name"] = screen_name;
            query["user_id"] = id;

            return new Twitter.User(
                await new TwitterRequest(
                    twitterContext, API.Methods.POST,
                    new Uri(API.Urls.Mutes_Users_Create), query).Request());
        }

        /// <summary>
        /// 指定したユーザーのミュートを解除します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="screen_name">ミュートを解除するユーザーのScreenName。</param>
        /// <param name="id">ミュートを解除するユーザーのID。</param>
        /// <returns>ミュートを解除されたユーザー</returns>
        public static async Task<Twitter.User> UsersDestroy(TwitterContext twitterContext, string screen_name = null, string id = null)
        {
            StringDictionary query = new StringDictionary();
            query["screen_name"] = screen_name;
            query["user_id"] = id;

            return new Twitter.User(
                await new TwitterRequest(
                    twitterContext, API.Methods.POST,
                    new Uri(API.Urls.Mutes_Users_Destroy), query).Request());
        }
    }
}
