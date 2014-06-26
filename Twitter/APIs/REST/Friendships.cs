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
    /// ユーザーのフォローに関するAPI
    /// </summary>
    public static class Friendships
    {
        /// <summary>
        /// 指定したユーザーをフォローします。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="screen_name">フォローするユーザーのScreenName。</param>
        /// <param name="id">フォローするユーザーのID。</param>
        /// <param name="follow">このユーザーからの通知を受け取るかどうかを示す System.Boolean 値。</param>
        /// <returns>フォローされたユーザー</returns>
        public static async Task<Twitter.User> Create(TwitterContext twitterContext, string screen_name = null, string id = null, bool follow = false)
        {
            StringDictionary query = new StringDictionary();
            query["screen_name"] = screen_name;
            query["user_id"] = id;
            query["follow"] = follow.ToString();

            return new Twitter.User(
                await new TwitterRequest(
                    twitterContext, API.Methods.POST,
                    new Uri(API.Urls.Friendships_Create), query).Request());
        }

        /// <summary>
        /// 指定したユーザーのフォローを解除します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="screen_name">フォローを解除するユーザーのScreenName。</param>
        /// <param name="id">フォローを解除するユーザーのID。</param>
        /// <returns>フォローを解除されたユーザー</returns>
        public static async Task<Twitter.User> Destory(TwitterContext twitterContext, string screen_name = null, string id = null)
        {
            StringDictionary query = new StringDictionary();
            query["screen_name"] = screen_name;
            query["user_id"] = id;

            return new Twitter.User(
                await new TwitterRequest(
                    twitterContext, API.Methods.POST,
                    new Uri(API.Urls.Friendships_Destroy), query).Request());
        }
    }
}
