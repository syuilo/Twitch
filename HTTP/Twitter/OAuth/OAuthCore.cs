using OAuth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;

namespace Twitch.HTTP.Twitter.OAuth
{
	public static class OAuthCore
	{
		/// <summary>
		/// リクエスト ヘッダー文字列を生成します。
		/// </summary>
		/// <param name="Context"></param>
		/// <param name="Method"></param>
		/// <param name="RequestUrl"></param>
		/// <param name="QueryDictionary"></param>
		/// <returns></returns>
		public static string GenerateRequestHeader(
			TwitterContext Context,
			string Method,
			string RequestUrl,
			StringDictionary QueryDictionary = null)
		{
#if DEBUG
			Console.WriteLine("-\t## リクエスト ヘッダーを構築します");
#endif

			string header = String.Empty;
			string headerParams = String.Empty;

			OAuthBase oauth = new OAuthBase();

			string Nonce = oauth.GenerateNonce();
			string TimeStamp = oauth.GenerateTimeStamp();

			SortedDictionary<string, string> paramDictionary = new SortedDictionary<string, string>();

			AddPercentEncodedItem(paramDictionary, "oauth_consumer_key", Context.ConsumerKey);
			AddPercentEncodedItem(paramDictionary, "oauth_nonce", Nonce);

			string signature = GenerateSignature(
					Context,
					Method,
					RequestUrl,
					Nonce,
					"HMAC-SHA1",
					TimeStamp,
					"1.0",
					QueryDictionary
				);

			AddPercentEncodedItem(paramDictionary, "oauth_signature", signature);
			AddPercentEncodedItem(paramDictionary, "oauth_signature_method", "HMAC-SHA1");
			AddPercentEncodedItem(paramDictionary, "oauth_timestamp", TimeStamp);
			AddPercentEncodedItem(paramDictionary, "oauth_token", Context.AccessToken);
			AddPercentEncodedItem(paramDictionary, "oauth_version", "1.0");

			foreach (var kvp in paramDictionary)
			{
				if (kvp.Value != null)
					headerParams += (headerParams.Length > 0 ? ", " : string.Empty) + kvp.Key + "=\"" + kvp.Value + "\"";
			}

			header = "OAuth " + headerParams;

#if DEBUG
			Console.WriteLine("-\t## リクエスト ヘッダー構築完了: [Authorization] " + header);
#endif

			return header;
		}

		/// <summary>
		/// シグネチャ文字列を生成します。
		/// </summary>
		/// <param name="Context"></param>
		/// <param name="Method"></param>
		/// <param name="Url"></param>
		/// <param name="Nonce"></param>
		/// <param name="SignatureMethod"></param>
		/// <param name="TimeStamp"></param>
		/// <param name="OAuthVersion"></param>
		/// <param name="QueryDictionary"></param>
		/// <returns></returns>
		public static string GenerateSignature(
			TwitterContext Context,
			string Method,
			string Url,
			string Nonce,
			string SignatureMethod,
			string TimeStamp,
			string OAuthVersion,
			StringDictionary QueryDictionary = null)
		{
#if DEBUG
			Console.WriteLine("-\t-\t## シグネチャを生成します");
#endif

			SortedDictionary<string, string> Parameters = new SortedDictionary<string, string>();

			Parameters.Add("oauth_consumer_key", Context.ConsumerKey);
			Parameters.Add("oauth_nonce", Nonce);
			Parameters.Add("oauth_signature_method", SignatureMethod);
			Parameters.Add("oauth_timestamp", TimeStamp);
			Parameters.Add("oauth_token", Context.AccessToken);
			Parameters.Add("oauth_version", OAuthVersion);

			if (QueryDictionary != null)
			{
				foreach (DictionaryEntry k in QueryDictionary)
				{
					if (k.Value != null)
						Parameters.Add((string)k.Key, (string)k.Value);
				}
			}

#if DEBUG
			foreach (KeyValuePair<string, string> p in Parameters)
			{
				if (p.Value != null)
					if (p.Value.Length > 1000)
						Console.WriteLine("-\t-\t-\t## [" + p.Key + "] : (1000文字以上)");
					else
						Console.WriteLine("-\t-\t-\t## [" + p.Key + "] : " + p.Value);
			}
#endif

			string StringParameter = string.Empty;

			foreach (var kvp in Parameters)
			{
				if (kvp.Value != null)
					StringParameter += (StringParameter.Length > 0 ? "&" : String.Empty) + UrlEncode(kvp.Key, System.Text.Encoding.UTF8) + "=" + UrlEncode(kvp.Value, System.Text.Encoding.UTF8);
			}

#if DEBUG
			if (StringParameter.Length > 1000)
				Console.WriteLine("-\t-\t-\t## パラメータ生成完了: (1000文字以上)");
			else
				Console.WriteLine("-\t-\t-\t## パラメータ生成完了: " + StringParameter);
#endif

			// シグネチャ ベース ストリングの生成
			string SignatureBaseString = UrlEncode(Method, System.Text.Encoding.UTF8) + "&"
			+ UrlEncode(Url, System.Text.Encoding.UTF8) + "&"
			+ UrlEncode(StringParameter, System.Text.Encoding.UTF8);

#if DEBUG
			if (SignatureBaseString.Length > 1000)
				Console.WriteLine("-\t-\t-\t## シグネチャ ベース ストリング生成完了: (1000文字以上)");
			else
				Console.WriteLine("-\t-\t-\t## シグネチャ ベース ストリング生成完了: " + SignatureBaseString);
#endif

			HMACSHA1 hmacsha1 = new HMACSHA1(Encoding.ASCII.GetBytes(
				UrlEncode(Context.ConsumerSecret, System.Text.Encoding.UTF8)
				+ "&" + ((Context.AccessTokenSecret != null) ? UrlEncode(Context.AccessTokenSecret, System.Text.Encoding.UTF8) : null)
			));

			string Signature = Convert.ToBase64String(hmacsha1.ComputeHash(Encoding.ASCII.GetBytes(SignatureBaseString)));

#if DEBUG
			Console.WriteLine("-\t-\t## シグネチャ生成完了: " + Signature);
#endif

			return Signature;
		}

		/// <summary>
		/// パラメーター名および値をパーセントエンコードしてディクショナリに追加
		/// </summary>
		private static void AddPercentEncodedItem(SortedDictionary<string, string> dictionary, string key, string keyValue)
		{
			if (!string.IsNullOrEmpty(keyValue))
				dictionary.Add(UrlEncode(key, System.Text.Encoding.UTF8), UrlEncode(keyValue, System.Text.Encoding.UTF8));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="encode"></param>
		/// <returns></returns>
		public static string UrlEncode(string value, Encoding encode)
		{
			string unreservedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

			StringBuilder result = new StringBuilder();
			byte[] data = encode.GetBytes(value);
			int len = data.Length;

			for (int i = 0; i < len; i++)
			{
				int c = data[i];

				if (c < 0x80 && unreservedChars.IndexOf((char)c) != -1)
				{
					result.Append((char)c);
				}
				else
				{
					result.Append('%' + String.Format("{0:X2}", (int)data[i]));
				}
			}

			return result.ToString();
		}
	}
}
