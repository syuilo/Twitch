// DEBUG	: デバッグ用文字列を出力するシンボル

#define DEBUG
#undef DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Twitch.HTTP.Twitter.OAuth;

namespace Twitch
{
	[Obsolete]
	public enum StreamType
	{
		User
	}

	[Obsolete]
	public static class oldStreaming
	{
		public static HttpWebResponse Response;
		public static HttpWebRequest Request;
		public static StreamReader sr;

		private static bool StreamingConnected = false;

		public delegate void StreamingEventHandler(string Data);
		public static event StreamingEventHandler StreamingEvent;

		private static bool OnRemoteCertificateValidationCallback(
			Object sender,
			X509Certificate certificate,
			X509Chain chain,
			SslPolicyErrors sslPolicyErrors)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Context"></param>
		/// <param name="Method"></param>
		/// <param name="Url"></param>
		/// <param name="QueryDictionary"></param>
		/// <returns></returns>
		public static async Task<string> StreamingConnect(TwitterContext Context, string Method, string Url, StringDictionary QueryDictionary = null)
		{
			StreamingConnected = true;
			int retry = 250;

		CONNECT:

			string response = String.Empty;

			ServicePointManager.ServerCertificateValidationCallback =
				new RemoteCertificateValidationCallback(
				OnRemoteCertificateValidationCallback);

#if DEBUG
			Console.WriteLine("\r\n--------------------\r\n## リクエストを作成します");
#endif

			// リクエストの作成
			Request = (HttpWebRequest)WebRequest.Create(Url);
			Request.Method = Method;
			Request.ContentType = "application/x-www-form-urlencoded";
			Request.Host = "stream.twitter.com";
			Request.Headers["Authorization"] = OAuthCore.GenerateRequestHeader(Context, Method, Url, QueryDictionary);

			if (QueryDictionary != null)
			{
				// リクエストデータの作成
				var para = from DictionaryEntry k in QueryDictionary
						   select (k.Value != null) ? OAuthCore.UrlEncode((string)k.Key, System.Text.Encoding.UTF8) + "=" + OAuthCore.UrlEncode((string)k.Value, System.Text.Encoding.UTF8) : null;

				string data = String.Join("&", para.ToArray());

#if DEBUG
				Console.WriteLine("## リクエストデータ構築完了 : " + data);
#endif

				// リクエストデータの書き込み
				using (StreamWriter streamWriter = new StreamWriter(await Request.GetRequestStreamAsync()))
				{
					await streamWriter.WriteAsync(data);
				}


			}

#if DEBUG
			Console.WriteLine("## リクエストを送信します 接続を開始します");
#endif

			// リクエストの送信
			Response = (HttpWebResponse)await Request.GetResponseAsync();

			//応答データを受信するためのStreamを取得する
			System.IO.Stream st = Response.GetResponseStream();
			sr = new StreamReader(st);

			try
			{
				while (StreamingConnected)
				{
					string json = await sr.ReadLineAsync();

					if (json != "")
						StreamingEvent(json);

					retry = 250;
				}

			}
			catch (Exception ex)
			{
#if DEBUG
				Console.WriteLine("## エラー : " + ex.Message + "\r\n--------------------");
#endif
				System.Threading.Thread.Sleep(retry);
				retry += 250;
				goto CONNECT;
			}

			Request.Abort();
			Response.Close();
			sr.Close();

			StreamingConnected = false;

#if DEBUG
			Console.WriteLine("## " + Response.StatusCode + " " + Response.StatusDescription + " : " + response + "\r\n--------------------");
#endif

			return response;
		}

		[Obsolete]
		public static void Close()
		{
			if (StreamingConnected)
				StreamingConnected = false;
		}

	}
}
