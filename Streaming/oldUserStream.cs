using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Twitch.HTTP.Twitter.OAuth;

namespace Twitch.Streaming
{
	[Obsolete]
	public static class oldUserStream
	{
		public static HttpWebResponse Response;
		public static HttpWebRequest Request;
		public static StreamReader sr;

		private static bool UserStreamConnected = false;

		public delegate void UserStreamEventHandler(string Data);

		/// <summary>
		/// ストリームからデータを受信した時にこのイベントが発生し、受信したデータが流し込まれます。
		/// </summary>
		public static event UserStreamEventHandler UserStreamEvent;

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
		public static async void UserStreamConnect(TwitterContext Context, string Method, string Url, StringDictionary QueryDictionary = null)
		{
			UserStreamConnected = true;
			int retry = 250;

			twitch.Message("UserStreamに接続します");

		CONNECT:

			string response = String.Empty;

			ServicePointManager.ServerCertificateValidationCallback =
				new RemoteCertificateValidationCallback(
				OnRemoteCertificateValidationCallback);

#if DEBUG
			Console.WriteLine("\r\n--------------------\r\n## リクエストを作成します");
#endif

			try
			{
				// リクエストの作成
				Request = (HttpWebRequest)WebRequest.Create(Url);
				Request.Method = Method;
				Request.ContentType = "application/x-www-form-urlencoded";
				Request.Host = "userstream.twitter.com";
				Request.Headers["Authorization"] = OAuthCore.GenerateRequestHeader(Context, Method, Url, QueryDictionary);
			}
			catch (Exception e)
			{
				twitch.Message(
					"UserStreamリクエストを作成時にエラーが発生しました。" +
					Environment.NewLine +
					e.Message);
			}

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

			try
			{
				// リクエストの送信
				Response = (HttpWebResponse)await Request.GetResponseAsync();

				//応答データを受信するためのStreamを取得する
				System.IO.Stream st = Response.GetResponseStream();
				sr = new StreamReader(st);
			}
			catch (Exception e)
			{
				twitch.Message(
					"UserStreamへ接続するときに、エラーが発生しました。" +
					Environment.NewLine +
					e.Message);
			}

			try
			{
				string testjson = await sr.ReadLineAsync();

				if (testjson != "")
				{
					twitch.Message("UserStreamに接続しました。");
					UserStreamEvent(testjson);
				}

				while (UserStreamConnected)
				{
					// Jsonを取得
					string json = await sr.ReadLineAsync();

					if (json != "")
						UserStreamEvent(json);

					retry = 250;
				}

			}
			catch (Exception ex)
			{
#if DEBUG
				Console.WriteLine("## エラー : " + ex.Message + "\r\n--------------------");
#endif
				twitch.Message("UserStreamから切断されました。再接続を試みます... (" + retry.ToString() + "sec)");

				System.Threading.Thread.Sleep(retry);
				retry += 250;

				if (retry > 1000)
				{
					twitch.Message("UserStreamへ接続できません : 再試行回数が一定を超えました。");
				}
				else
				{
					goto CONNECT;
				}
			}

			Request.Abort();
			Response.Close();
			sr.Close();

			twitch.Message("UserStreamから切断しました");

			UserStreamConnected = false;

#if DEBUG
			Console.WriteLine("## " + Response.StatusCode + " " + Response.StatusDescription + " : " + response + "\r\n--------------------");
#endif
		}

		public static void Close()
		{
			if (UserStreamConnected)
				UserStreamConnected = false;
		}
	}
}
