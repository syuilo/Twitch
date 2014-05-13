using System.Threading.Tasks;
using System.Collections.Specialized;

namespace Twitch
{
	/// <summary>
	/// アプリケーションの ConsumerKey, ConsumerSecret、
	/// ユーザーの AccessToken, AccessTokenSecret を格納するクラスです。
	/// </summary>
	public class TwitterContext
	{
		/// <summary>
		/// TwitterContextのコンストラクタです。
		/// TwitterContextの作成を行います。
		/// </summary>
		/// <param name="consumerKey">アプリケーションの ConsumerKey</param>
		/// <param name="consumerSecret">アプリケーションの ConsumerSecret</param>
		/// <param name="accessToken">ユーザーの AccessToken</param>
		/// <param name="accessTokenSecret">ユーザーの AccessTokenSecret</param>
		public TwitterContext(string consumerKey = null,
							  string consumerSecret = null,
							  string accessToken = null,
							  string accessTokenSecret = null)
		{
			this.ConsumerKey = consumerKey;
			this.ConsumerSecret = consumerSecret;
			this.AccessToken = accessToken;
			this.AccessTokenSecret = accessTokenSecret;
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
			HTTP.Request.Method method, string url, StringDictionary parameter = null)
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
			return await Twitch.Twitter.APIs.REST.Friendships.Create(this, id: id) != null;
		}

		/// <summary>
		/// 指定ユーザーのフォローを解除します。
		/// </summary>
		/// <param name="id">フォローを解除するユーザーのid</param>
		/// <returns>成功したかどうか</returns>
		public async Task<bool> Remove(string id)
		{
			return await Twitch.Twitter.APIs.REST.Friendships.Destory(this, id: id) != null;
		}

		/// <summary>
		/// 指定ユーザーのフォローを逆転します。
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<bool> Reverse(string id)
		{
			return await Twitch.Twitter.APIs.REST.Friendships.Destory(this, id: id) != null;
		}

		/// <summary>
		/// このTwitterContextからツイートを行います。
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<bool> Tweet(string status)
		{
			return await Twitch.Twitter.APIs.REST.Statuses.Update(this, status) != null;
		}
	}
}
