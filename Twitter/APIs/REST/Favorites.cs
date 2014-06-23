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
    /// お気に入りに関するAPI
    /// </summary>
    public static class Favorites
    {
        /// <summary>
        /// 対象のツイートをお気に入りに登録します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="id">対象のツイートのID。</param>
        /// <returns>対象のツイート</returns>
        public static async Task<Twitter.Status> Create(TwitterContext twitterContext, string id)
        {
            StringDictionary query = new StringDictionary();
            query["id"] = id;

            string res = await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Favorites_Create), query).Request();
            return res != null ? new Status(res) : null;
        }

        /// <summary>
        /// 対象のツイートをお気に入りから削除します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="id">対象のツイートのID。</param>
        /// <returns>対象のツイート</returns>
        public static async Task<Twitter.Status> Destroy(TwitterContext twitterContext, string id)
        {
            StringDictionary query = new StringDictionary();
            query["id"] = id;

            string res = await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Favorites_Destroy), query).Request();
            return res != null ? new Status(res) : null;
        }
    }
}
