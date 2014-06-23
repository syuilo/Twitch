using System.Threading.Tasks;
using System.Collections.Specialized;
using Twitch.Twitter.APIs.REST;

namespace Twitch
{
    /// <summary>
    /// アプリケーションの ConsumerKey, ConsumerSecret、
    /// ユーザーの AccessToken, AccessTokenSecret を格納するクラスです。
    /// </summary>
    public class TwitterContext
    {
        /// <summary>
        /// TwitterContextを初期化します。
        /// </summary>
        /// <param name="consumerKey">アプリケーションの ConsumerKey</param>
        /// <param name="consumerSecret">アプリケーションの ConsumerSecret</param>
        /// <param name="accessToken">ユーザーの AccessToken</param>
        /// <param name="accessTokenSecret">ユーザーの AccessTokenSecret</param>
        public TwitterContext(
                string consumerKey,
                string consumerSecret,
                string accessToken,
                string accessTokenSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            this.AccessToken = accessToken;
            this.AccessTokenSecret = accessTokenSecret;
         }

        /// <summary>
        /// TwitterContextを初期化します。
        /// </summary>
        /// <param name="consumerKey">アプリケーションの ConsumerKey</param>
        /// <param name="consumerSecret">アプリケーションの ConsumerSecret</param>
        public TwitterContext(
                string consumerKey,
                string consumerSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
        }

        /// <summary>
        /// TwitterContextを初期化します。
        /// </summary>
        /// <param name="twitterContext">TwitterContext</param>
        public TwitterContext(
                TwitterContext twitterContext)
        {
            this.ConsumerKey = twitterContext.ConsumerKey;
            this.ConsumerSecret = twitterContext.ConsumerSecret;
            this.AccessToken = twitterContext.AccessToken;
            this.AccessTokenSecret = twitterContext.AccessTokenSecret;
        }

        /// <summary>
        /// アプリケーションの ConsumerKey を取得または設定します。
        /// </summary>
        public string ConsumerKey
        {
            get;
            set;
        }

        /// <summary>
        /// アプリケーションの ConsumerSecret を取得または設定します。
        /// </summary>
        public string ConsumerSecret
        {
            get;
            set;
        }

        /// <summary>
        /// ユーザーの AccessToken を取得または設定します。
        /// </summary>
        public string AccessToken
        {
            get;
            set;
        }

        /// <summary>
        /// ユーザーの AccessTokenSecret を取得または設定します。
        /// </summary>
        public string AccessTokenSecret
        {
            get;
            set;
        }

        /// <summary>
        /// このTwitterContextで、リクエストを送信します。
        /// </summary>
        /// <param name="method"></param>
        /// <param name="url"></param>
        /// <param name="parameter"></param>
        /// <returns>レスポンス</returns>
        public async Task<string> Reuqest(
            Twitter.API.Methods method, System.Uri url, StringDictionary parameter = null)
        {
            return await new HTTP.Twitter.TwitterRequest(
                this, method, url, parameter).Request();
        }

        /// <summary>
        /// 指定ユーザーをフォローします。
        /// </summary>
        /// <param name="id">フォローするユーザーのid</param>
        /// <returns>成功したかどうか</returns>
        public async Task<bool> Follow(string id)
        {
            return await Friendships.Create(this, id: id) != null;
        }

        /// <summary>
        /// 指定ユーザーのフォローを解除します。
        /// </summary>
        /// <param name="id">フォローを解除するユーザーのid</param>
        /// <returns>成功したかどうか</returns>
        public async Task<bool> Remove(string id)
        {
            return await Friendships.Destory(this, id: id) != null;
        }

        /// <summary>
        /// 指定ユーザーのフォローを逆転します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Reverse(string id)
        {
            return await Friendships.Destory(this, id: id) != null;
        }

        /// <summary>
        /// このTwitterContextからツイートを行います。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> Tweet(string status)
        {
            return await Statuses.Update(this, status) != null;
        }
    }
}
