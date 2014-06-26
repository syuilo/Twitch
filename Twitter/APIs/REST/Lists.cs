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
    /// リストに関するAPI
    /// </summary>
    public static class Lists
    {
        /// <summary>
        /// リストを作成します。
        /// </summary>
        /// <param name="twitterContext">リストを作成するユーザー。</param>
        /// <param name="name">リスト名。</param>
        /// <param name="mode">リストの公開状態。 public または private のいずれかを指定します。nullまたは指定しなかった場合はpublic(公開)になります。</param>
        /// <param name="description">リストの説明。</param>
        /// <returns></returns>
        public static async Task<string> Create(TwitterContext twitterContext, string name, string description, string mode = null)
        {
            var query = new StringDictionary();
            query["name"] = name;
            query["mode"] = mode;
            query["description"] = description;

            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Lists_Create), query).Request();
        }

        /// <summary>
        /// リストを削除します。
        /// </summary>
        /// <param name="twitterContext"></param>
        /// <param name="slug"></param>
        /// <param name="owner_screen_name"></param>
        /// <returns></returns>
        public static async Task<string> Destroy(TwitterContext twitterContext, string slug, string owner_screen_name)
        {
            var query = new StringDictionary();
            query["slug"] = slug;
            query["owner_screen_name"] = owner_screen_name;

            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Lists_Destroy), query).Request();
        }

        /// <summary>
        /// リストにメンバーを追加します。
        /// </summary>
        /// <param name="twitterContext"></param>
        /// <param name="slug"></param>
        /// <param name="screen_name"></param>
        /// <param name="owner_screen_name"></param>
        /// <returns></returns>
        public static async Task<string> MembersCreate(TwitterContext twitterContext, string slug, string screen_name, string owner_screen_name)
        {
            var query = new StringDictionary();
            query["slug"] = slug;
            query["screen_name"] = screen_name;
            query["owner_screen_name"] = owner_screen_name;

            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Lists_Members_Create), query).Request();
        }

        /// <summary>
        /// リストからメンバーを削除します。
        /// </summary>
        /// <param name="twitterContext"></param>
        /// <param name="slug"></param>
        /// <param name="screen_name"></param>
        /// <param name="owner_screen_name"></param>
        /// <returns></returns>
        public static async Task<string> MembersDestroy(TwitterContext twitterContext, string slug, string screen_name, string owner_screen_name)
        {
            var query = new StringDictionary();
            query["slug"] = slug;
            query["screen_name"] = screen_name;
            query["owner_screen_name"] = owner_screen_name;

            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Lists_Members_Destroy), query).Request();
        }
    }
}
