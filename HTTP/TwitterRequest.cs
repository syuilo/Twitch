using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Twitch.Twitter.API;

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
            Methods method = Methods.POST,
            Uri url = null,
            StringDictionary query = null,
            string proxy = null,
            string userAgent = null)
        {
            this.TwitterContext = twitterContext;
            this.Method = method;
            this.Url = url;
            this.Parameter = query;
            this.Proxy = proxy;
            this.UserAgent = userAgent;
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
        public Methods Method
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

        public string UserAgent
        {
            get;
            set;
        }

        /// <summary>
        /// 非同期でリクエストを送信し、レスポンスを取得します。
        /// </summary>
        /// <returns>レスポンス</returns>
        public async Task<string> Request()
        {
            string response = string.Empty;
            string data = null;

            string url = this.Url.ToString();

            Debug.WriteLine("\r\n--------------------\r\n## リクエストを作成します > " + this.Method + " " + this.Url);

            if (this.Parameter != null)
            {
                var para = from DictionaryEntry k in this.Parameter
                           select (k.Value != null)
                           ? OAuth.Core.UrlEncode((string)k.Key, Encoding.UTF8) + '=' + OAuth.Core.UrlEncode((string)k.Value, Encoding.UTF8)
                           : null;

                data = String.Join("&", para.ToArray());

                Debug.WriteLine(data.Length > 1000 ? "## リクエストデータ構築完了 : (1000文字以上)" : "## リクエストデータ構築完了 : " + data);

                if (Method == Methods.GET)
                    url += '?' + data;
            }

            // Create request
            var Request = (HttpWebRequest)WebRequest.Create(url);

            if (this.Proxy != null)
                Request.Proxy = new WebProxy(this.Proxy);

            Request.Method = Method.ToString();
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.Host = "api.twitter.com";
            Request.Headers["Authorization"] =
                OAuth.Core.GenerateRequestHeader(
                    this.TwitterContext, Method.ToString(), this.Url.ToString(), this.Parameter);

            if (!String.IsNullOrEmpty(this.UserAgent))
                Request.UserAgent = this.UserAgent;

            if (Method == Methods.POST && this.Parameter != null)
            {
                // Write request data
                using (StreamWriter streamWriter = new StreamWriter(await Request.GetRequestStreamAsync()))
                    await streamWriter.WriteAsync(data);
            }

            Debug.WriteLine("## リクエストを送信します...");

            try
            {
                // Send request
                var Response = (HttpWebResponse)await Request.GetResponseAsync();

                // Read response
                using (StreamReader ResponseDataStream = new StreamReader(Response.GetResponseStream()))
                    response = await ResponseDataStream.ReadToEndAsync();

                Debug.WriteLine("## " + Response.StatusCode + " " + Response.StatusDescription + " : " + response + "\r\n--------------------");

                return response;
            }
            catch (System.Net.WebException ex)
            {
                Debug.WriteLine("## エラー : " + ex.Message + "\r\n--------------------");
                return null;
            }

        }
    }
}
