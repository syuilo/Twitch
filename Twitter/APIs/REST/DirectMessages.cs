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
    /// ダイレクト メッセージに関するAPI
    /// </summary>
    public static class DirectMessages
    {
        /// <summary>
        /// ダイレクト メッセージを送信します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="text">ダイレクト メッセージの本文。</param>
        /// <param name="screen_name">宛先のユーザーのScreenName。</param>
        /// <param name="id">宛先のユーザーのID。</param>
        /// <returns></returns>
        public static async Task<string> New(TwitterContext twitterContext, string text, string screen_name = null, string id = null)
        {
            var query = new StringDictionary();
            query["text"] = text;
            query["screen_name"] = screen_name;
            query["user_id"] = id;

            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.DirectMessages_New), query).Request();
        }
    }
}
