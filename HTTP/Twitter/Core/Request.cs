using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Twitch.HTTP.Twitter.OAuth;

namespace Twitch.HTTP.Twitter.Core
{
	[Obsolete("TwitterRequestを使用してください。")]
	public static class Request
	{
		/// <summary>
		/// リクエストを送信します。
		/// </summary>
		/// <param name="Context"></param>
		/// <param name="Method"></param>
		/// <param name="Url"></param>
		/// <param name="QueryDictionary"></param>
		/// <returns></returns>
		[Obsolete("TwitterRequestを使用してください。")]
		public static async Task<string> Reuqest(
			TwitterContext Context,
			HTTP.Request.Method Method,
			string Url,
			StringDictionary QueryDictionary = null,
			string Proxy = null)
		{
			string response = string.Empty;
			string data = null;
			string defurl = Url;

#if DEBUG
			Console.WriteLine("\r\n--------------------\r\n## リクエストを作成します > " + Method + " " + Url);
#endif
			if (QueryDictionary != null)
			{
				// リクエストデータの作成
				var para = from DictionaryEntry k in QueryDictionary
						   select (k.Value != null) ? OAuthCore.UrlEncode((string)k.Key, System.Text.Encoding.UTF8) + "=" + OAuthCore.UrlEncode((string)k.Value, System.Text.Encoding.UTF8) : null;

				data = String.Join("&", para.ToArray());

#if DEBUG
				Console.WriteLine("## リクエストデータ構築完了 : " + data);
#endif
				if (Method == HTTP.Request.Method.GET)
				{
					// リクエストデータをURLクエリとして連結
					Url += '?' + data;
					//QueryDictionary = null;
				}
			}

			// リクエストの作成
			HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(Url);

			if (!string.IsNullOrEmpty(Proxy))
			{
				Request.Proxy = new WebProxy(Proxy);
			}

			Request.Method = Method.ToString();
			Request.ContentType = "application/x-www-form-urlencoded";
			Request.Host = "api.twitter.com";
			Request.Headers["Authorization"] = OAuthCore.GenerateRequestHeader(Context, Method.ToString(), defurl, QueryDictionary);

			if (Method == HTTP.Request.Method.POST && QueryDictionary != null)
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
				HttpWebResponse Response = (HttpWebResponse)await Request.GetResponseAsync();

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
