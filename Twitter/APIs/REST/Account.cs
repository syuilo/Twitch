using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

using Twitch.Net;

namespace Twitch.Twitter.APIs.REST
{
    public static class Account
    {
        /// <summary>
        /// アカウントのプロフィールを各種変更します。
        /// </summary>
        /// <param name="twitterContext">更新するアカウント。</param>
        /// <param name="name">名前。nullまたは指定しなかった場合はこのパラメータは更新されません。</param>
        /// <param name="url">ウェブサイト URL。nullまたは指定しなかった場合はこのパラメータは更新されません。</param>
        /// <param name="location">場所。nullまたは指定しなかった場合はこのパラメータは更新されません。</param>
        /// <param name="description">自己紹介。nullまたは指定しなかった場合はこのパラメータは更新されません。</param>
        /// <returns>更新されたユーザー。</returns>
        public static async Task<Twitter.User> UpdateProfile(TwitterContext twitterContext, string name = null, Uri url = null, string location = null, string description = null)
        {
            StringDictionary query = new StringDictionary();
            query["name"] = name;
            query["url"] = url.ToString();
            query["location"] = location;
            query["description"] = description;

            return new User(await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Account_UpdateProfile), query).Request());
        }

        /// <summary>
        /// TwitterContextのアイコンを更新します。
        /// </summary>
        /// <param name="twitterContext">更新するアカウント。</param>
        /// <param name="image">base64エンコードされたgifまたはjpgまたはpngの画像。</param>
        /// <returns>更新されたユーザー。</returns>
        public static async Task<Twitter.User> UpdateProfileImage(TwitterContext twitterContext, string image)
        {
            StringDictionary query = new StringDictionary();
            query["image"] = image;

            return new User(await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Account_UpdateProfileImage), query).Request());
        }
    }
}
