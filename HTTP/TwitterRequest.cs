using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Twitch.HTTP.Twitter.OAuth;

namespace Twitch.HTTP.Twitter
{
	/// <summary>
	/// Twitterへのリクエストを作成します。
	/// </summary>
	public class TwitterRequest
	{
		/// <summary>
		/// Twitterへのリクエストを作成します。
		/// </summary>
		/// <param name="twitterContext">リクエストを行うTwitterContext。</param>
		/// <param name="method">APIのリクエストに使用するHTTPメソッド。</param>
		/// <param name="url">APIのURL。</param>
		/// <param name="query">リクエストのパラメータ。</param>
		public TwitterRequest(
			TwitterContext twitterContext = null,
			HTTP.Request.Method method = HTTP.Request.Method.POST,
			string url = null,
			StringDictionary query = null,
			string proxy = null)
		{
			this.TwitterContext = twitterContext;
			this.Method = method;
			this.Url = new Uri(url);
			this.Parameter = query;
			this.Proxy = proxy;
		}

		/// <summary>
		/// リクエストを行うTwitterContext。
		/// </summary>
		public TwitterContext TwitterContext
		{
			get;
			set;
		}

		/// <summary>
		/// APIのリクエストに使用するHTTPメソッド。
		/// 適当なメソッドについては各種APIドキュメントを参照してください。
		/// </summary>
		public HTTP.Request.Method Method
		{
			get;
			set;
		}

		/// <summary>
		/// APIのURL。
		/// </summary>
		public Uri Url
		{
			get;
			set;
		}

		/// <summary>
		/// リクエストのパラメータ。
		/// </summary>
		public StringDictionary Parameter
		{
			get;
			set;
		}

		/// <summary>
		/// リクエストに使用するプロキシ サーバー。
		/// </summary>
		public string Proxy
		{
			get;
			set;
		}

		/// <summary>
		/// リクエストを送信し、レスポンスを取得します。
		/// </summary>
		/// <returns></returns>
		public async Task<string> Request()
		{
			string response = string.Empty;
			string data = null;

			string url = this.Url.ToString();

#if DEBUG
			Console.WriteLine("\r\n--------------------\r\n## リクエストを作成します > " + this.Method + " " + this.Url);
#endif
			if (this.Parameter != null)
			{
				// リクエストデータの作成
				var para = from DictionaryEntry k in this.Parameter
						   select (k.Value != null) ? OAuthCore.UrlEncode((string)k.Key, System.Text.Encoding.UTF8) + "=" + OAuthCore.UrlEncode((string)k.Value, System.Text.Encoding.UTF8) : null;

				data = String.Join("&", para.ToArray());

#if DEBUG
				if (data.Length > 1000)
					Console.WriteLine("## リクエストデータ構築完了 : (1000文字以上)");
				else
					Console.WriteLine("## リクエストデータ構築完了 : " + data);
#endif
				if (Method == HTTP.Request.Method.GET)
				{
					// リクエストデータをURLクエリとして連結
					url += '?' + data;
					//QueryDictionary = null;
				}
			}

			// リクエストの作成
			var Request = (HttpWebRequest)WebRequest.Create(url);

			if (this.Proxy != null)
				Request.Proxy = new WebProxy(this.Proxy);

			Request.Method = Method.ToString();
			Request.ContentType = "application/x-www-form-urlencoded";
			Request.Host = "api.twitter.com";
			Request.Headers["Authorization"] = OAuthCore.GenerateRequestHeader(this.TwitterContext, Method.ToString(), this.Url.ToString(), this.Parameter);

			if (Method == HTTP.Request.Method.POST && this.Parameter != null)
			{
				// リクエストデータの書き込み
				using (StreamWriter streamWriter = new StreamWriter(await Request.GetRequestStreamAsync()))
				{
					await streamWriter.WriteAsync(data);
				}
			}

#if DEBUG
			Console.WriteLine("## リクエストを送信します...");
#endif

			try
			{
				// リクエストの送信
				var Response = (HttpWebResponse)await Request.GetResponseAsync();

				using (StreamReader ResponseDataStream = new StreamReader(Response.GetResponseStream()))
				{
					response = await ResponseDataStream.ReadToEndAsync();
				}

#if DEBUG
				Console.WriteLine("## " + Response.StatusCode + " " + Response.StatusDescription + " : " + response + "\r\n--------------------");
#endif

				return response;
			}
			catch (System.Net.WebException ex)
			{
#if DEBUG
				Console.WriteLine("## エラー : " + ex.Message + "\r\n--------------------");
#endif
				return null;
			}

		}
	}
}
