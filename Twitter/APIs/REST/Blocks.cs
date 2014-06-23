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
    /// ユーザーのブロックに関するAPI
    /// </summary>
    public static class Blocks
    {
        /// <summary>
        /// 指定したユーザーをブロックします。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="screen_name">ブロックするユーザーのScreenName。</param>
        /// <param name="id">ブロックするユーザーのID。</param>
        /// <returns>ブロックされたユーザー</returns>
        public static async Task<Twitter.User> Create(TwitterContext twitterContext, string screen_name = null, string id = null)
        {
            StringDictionary query = new StringDictionary();
            query["screen_name"] = screen_name;
            query["user_id"] = id;

            return new Twitter.User(
                await new TwitterRequest(
                    twitterContext, API.Methods.POST,
                    new Uri(API.Urls.Blocks_Create), query).Request());
        }

        /// <summary>
        /// 指定したユーザーのブロックを解除します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="screen_name">ブロックを解除するユーザーのScreenName。</param>
        /// <param name="id">ブロックを解除するユーザーのID。</param>
        /// <returns>ブロックを解除されたユーザー</returns>
        public static async Task<Twitter.User> Destroy(TwitterContext twitterContext, string screen_name = null, string id = null)
        {
            StringDictionary query = new StringDictionary();
            query["screen_name"] = screen_name;
            query["user_id"] = id;

            return new Twitter.User(
                await new TwitterRequest(
                    twitterContext, API.Methods.POST,
                    new Uri(API.Urls.Blocks_Destroy), query).Request());
        }
    }
}
