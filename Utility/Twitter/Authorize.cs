using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Twitch.Utility.Twitter
{
	/// <summary>
	/// OAuthを利用した一連のアカウント認証処理を行うクラスです。
	/// </summary>
	public class Authorize
	{
		/// <summary>
		/// アプリケーションのConsumerKey。
		/// </summary>
		private string ConsumerKey
		{
			get;
			set;
		}

		/// <summary>
		/// アプリケーションのConsumerSecret。
		/// </summary>
		private string ConsumerSecret
		{
			get;
			set;
		}

		/// <summary>
		/// RequestToken
		/// </summary>
		private string OAuthToken
		{
			get;
			set;
		}

		/// <summary>
		/// RequestTokenSecret
		/// </summary>
		private string OAuthTokenSecret
		{
			get;
			set;
		}

		/// <summary>
		/// クラスを初期化します。
		/// </summary>
		/// <param name="ConsumerKey"></param>
		/// <param name="ConsumerSecret"></param>
		public Authorize(string ConsumerKey, string ConsumerSecret)
		{
			this.ConsumerKey = ConsumerKey;
			this.ConsumerSecret = ConsumerSecret;
		}

		/// <summary>
		/// RequestToken,RequestTokenSecretを取得します。
		/// </summary>
		/// <returns>正常に取得できた場合はtrueを、それ以外の場合はfalseを返します。</returns>
		public async Task<bool> GetRequestToken()
		{
			Twitch.TwitterContext tw = new Twitch.TwitterContext(this.ConsumerKey, this.ConsumerSecret);

			try
			{
				string res = await Twitch.Twitter.APIs.REST.OAuth.request_token(tw);

				if (!string.IsNullOrEmpty(res))
				{
					this.OAuthToken = Utility.AnalyzeUrlQuery.Analyze(res, "oauth_token");
					this.OAuthTokenSecret = Utility.AnalyzeUrlQuery.Analyze(res, "oauth_token_secret");

					return true;
				}
				else
					return false;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Authorize用URLを取得します。
		/// </summary>
		/// <returns>URL</returns>
		public Uri GetAuthorizeUrl()
		{
			if (this.OAuthToken != null)
				return new Uri("https://api.twitter.com/oauth/authorize?oauth_token=" + this.OAuthToken);
			else
				throw new NullReferenceException("リクエスト トークンが設定されていません。");
		}

		/// <summary>
		/// Authorizeページを既定のウェブ ブラウザーで表示します。
		/// </summary>
		public void ShowAuthorizeBrowser()
		{
			Uri url = GetAuthorizeUrl();

			System.Diagnostics.Process.Start(url.ToString());
		}

		/// <summary>
		/// 認証ページをスクレイピングし、ScreenName,PasswordからAccessToken,AccessTokenSecretを取得します。<para />
		/// ただし、このメソッドは、恒常的に使用できることを保証するものではありません。
		/// Twitterの仕様変更によって、今後使用できなくなる可能性があります。
		/// </summary>
		/// <returns>TwitterContext。失敗した場合はNull</returns>
		[CannotGuarantee("Twitterの仕様変更によって、今後使用できなくなる可能性があります。")]
		public async Task<TwitterContext> GetAccessTokenFromScreenNameAndPassword(
			string ScreenName, string Password)
		{
			System.Net.WebClient wc = new System.Net.WebClient();
			string sorce = wc.DownloadString("https://twitter.com");
			wc.Dispose();

			Regex reg = new Regex("<input type=\"hidden\" name=\"authenticity_token\" value=\"(?<token>.*?)\">",
							RegexOptions.IgnoreCase | RegexOptions.Singleline);

			Match m = reg.Match(sorce);
			Console.WriteLine("authenticity_token: " + m.Groups["token"].Value);

			string url = "https://api.twitter.com/oauth/authorize";

			wc = new System.Net.WebClient();

			System.Collections.Specialized.NameValueCollection ps =
				new System.Collections.Specialized.NameValueCollection();

			ps.Add("authenticity_token", m.Groups["token"].Value);
			ps.Add("oauth_token", OAuthToken);
			ps.Add("session[username_or_email]", ScreenName);
			ps.Add("session[password]", Password);

			byte[] resData = wc.UploadValues(url, ps);
			wc.Dispose();

			string resText = System.Text.Encoding.UTF8.GetString(resData);

			reg = new Regex("<code>(?<pin>.*?)</code>",
				RegexOptions.IgnoreCase | RegexOptions.Singleline);

			m = reg.Match(resText);
			Console.WriteLine("pin: " + m.Groups["pin"].Value);

			return await GetAccessTokenFromPinCode(m.Groups["pin"].Value);
		}

		/// <summary>
		/// PINコードからAccessToken,AccessTokenSecretを取得します。
		/// </summary>
		/// <param name="PIN">PINコード</param>
		/// <returns>TwitterContext。失敗した場合はNull</returns>
		public async Task<TwitterContext> GetAccessTokenFromPinCode(string PIN)
		{
			Twitch.TwitterContext tw = new Twitch.TwitterContext()
			{
				ConsumerKey = ConsumerKey,
				ConsumerSecret = ConsumerSecret,
				AccessToken = OAuthToken,
				AccessTokenSecret = OAuthTokenSecret,
			};

			string res = await Twitch.Twitter.APIs.REST.OAuth.access_token(tw, PIN);

			if (!string.IsNullOrEmpty(res))
			{
				string access_token = Utility.AnalyzeUrlQuery.Analyze(res, "oauth_token");
				string access_token_secret = Utility.AnalyzeUrlQuery.Analyze(res, "oauth_token_secret");

				return new Twitch.TwitterContext()
				{
					ConsumerKey = ConsumerKey,
					ConsumerSecret = ConsumerSecret,
					AccessToken = access_token,
					AccessTokenSecret = access_token_secret
				};
			}
			else
				return null;
		}
	}
}
