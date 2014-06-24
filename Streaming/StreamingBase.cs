﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Twitch.HTTP.Twitter.OAuth;
using Twitch.Twitter.API;

namespace Twitch.Streaming
{
    /// <summary>
    /// Streamingを扱う基底クラスです。
    /// </summary>
    public abstract class StreamingBase : IDisposable
    {
        #region Events

        #region StreamEvent
        public delegate void StreamEventHandler(object sender, StreamEventArgs e);

        /// <summary>
        /// Streamからデータを受信しました。
        /// </summary>
        public event StreamEventHandler Stream;

        /// <summary>
        /// Stream イベントを発行します。
        /// </summary>
        protected virtual void OnStream(StreamEventArgs e)
        {
            if (Stream != null)
                Stream(this, e);
        }
        #endregion

        #region StreamMessagedEvent
        public delegate void StreamMessagedEventHandler(object sender, StreamEventArgs e);

        /// <summary>
        /// 処理すべきStreamメッセージが発行されました。
        /// </summary>
        public event StreamMessagedEventHandler StreamMessaged;

        /// <summary>
        /// StreamMessaged イベントを発行します。
        /// </summary>
        protected virtual void OnStreamMessaged(StreamEventArgs e)
        {
            if (StreamMessaged != null)
                StreamMessaged(this, e);
        }
        #endregion

        #region ConnectedEvent
        public delegate void ConnectedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Streamに接続しました。
        /// </summary>
        public event ConnectedEventHandler Connected;

        /// <summary>
        /// Connected イベントを発行します。
        /// </summary>
        protected virtual void OnConnected(EventArgs e)
        {
            if (Connected != null)
                Connected(this, e);
        }
        #endregion

        #region DisconnectedEvent
        public delegate void DisconnectedEventHandler(object sender, DisconnectedEventArgs e);

        /// <summary>
        /// Streamから切断されました。
        /// </summary>
        public event DisconnectedEventHandler Disconnected;

        /// <summary>
        /// Disconnected イベントを発行します。
        /// </summary>
        protected virtual void OnDisconnected(DisconnectedEventArgs e)
        {
            if (Disconnected != null)
                Disconnected(this, e);
        }
        #endregion

        #region TerminatedEvent
        public delegate void TerminatedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Streamから切断しました。
        /// </summary>
        public event TerminatedEventHandler Terminated;

        /// <summary>
        /// Terminated イベントを発行します。
        /// </summary>
        protected virtual void OnTerminated(EventArgs e)
        {
            if (Terminated != null)
                Terminated(this, e);
        }
        #endregion

        #region KeepAliveSignaledEvent
        public delegate void KeepAliveSignaledEventHandler(object sender, EventArgs e);

        /// <summary>
        /// 接続を維持するために、空白行が送られました。
        /// </summary>
        public event KeepAliveSignaledEventHandler KeepAliveSignaled;

        /// <summary>
        /// KeepAliveSignaled イベントを発行します。
        /// </summary>
        protected virtual void OnKeepAliveSignaled(EventArgs e)
        {
            if (KeepAliveSignaled != null)
                KeepAliveSignaled(this, e);
        }
        #endregion

        #region StatusDeletionEvent
        public delegate void StatusDeletionEventHandler(object sender, StatusDeletionEventArgs e);

        /// <summary>
        /// ツイートが削除されました。
        /// </summary>
        public event StatusDeletionEventHandler StatusDeletion;

        /// <summary>
        /// StatusDeletion イベントを発行します。
        /// </summary>
        protected virtual void OnStatusDeletion(StatusDeletionEventArgs e)
        {
            if (StatusDeletion != null)
                StatusDeletion(this, e);
        }
        #endregion

        #region ReconnectedEvent
        public delegate void ReconnectedEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Streamへの再接続を試みました。
        /// </summary>
        public event ReconnectedEventHandler Reconnected;

        /// <summary>
        /// Reconnected イベントを発行します。
        /// </summary>
        protected virtual void OnReconnected(EventArgs e)
        {
            if (Reconnected != null)
                Reconnected(this, e);
        }
        #endregion

        #endregion

        /// <summary>
        /// 再接続のためのディレイ
        /// </summary>
        private int Deley = 0;

        /// <summary>
        /// Streamを初期化します。
        /// </summary>
        public StreamingBase()
        {
            this.Stream += new StreamEventHandler(StreamingCallback);
        }

        /// <summary>
        /// Streamに接続されているかを取得します。
        /// </summary>
        public bool IsConnected
        {
            get;
            protected set;
        }

        /// <summary>
        /// 接続に使用するTwitterContextを取得または設定します。
        /// </summary>
        public TwitterContext TwitterContext
        {
            get;
            set;
        }

        /// <summary>
        /// 接続に使用するHTTPメソッドを取得または設定します。
        /// </summary>
        public Methods Method
        {
            get;
            set;
        }

        /// <summary>
        /// Streaming APIのホスト。
        /// </summary>
        public string Host
        {
            get;
            set;
        }

        /// <summary>
        /// Streaming APIのURL。
        /// </summary>
        public string Url
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
        /// Streamからの不明なイベントや予期しないメッセージを許容するかどうかを取得または設定します。
        /// </summary>
        public bool IsTolerance
        {
            get;
            set;
        }

        /// <summary>
        /// この接続をGZipで通信を行うかどうかを取得または設定します。
        /// </summary>
        public bool IsGZip
        {
            get;
            set;
        }

        /// <summary>
        /// Streamから切断されたときに、自動的に再接続を試みるかどうかを示す System.Boolean 値を取得または設定します。
        /// </summary>
        public bool IsAutoReconnect
        {
            get;
            set;
        }

        protected bool OnRemoteCertificateValidationCallback(
            Object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// Streamへ接続します。
        /// </summary>
        public async void Connect()
        {
            this.IsConnected = true;

            string response = String.Empty;

            ServicePointManager.ServerCertificateValidationCallback =
                new RemoteCertificateValidationCallback(
                    OnRemoteCertificateValidationCallback);

            HttpWebRequest request;
            StreamReader sr;

            string reqdata = null;
            string url = this.Url;

            if (this.Parameter != null)
            {
                // Create request data
                var para = from DictionaryEntry k in this.Parameter
                           select (k.Value != null)
                           ? Core.UrlEncode((string)k.Key, Encoding.UTF8) + '=' + Core.UrlEncode((string)k.Value, Encoding.UTF8)
                           : null;

                reqdata = String.Join("&", para.ToArray());

                if (Method == Methods.GET)
                    // リクエストデータをURLクエリとして連結
                    url += '?' + reqdata;
            }

            // Create request
            request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = Method.ToString();
            request.ContentType = "application/x-www-form-urlencoded";
            request.Host = this.Host;
            request.Headers["Authorization"] =
                Core.GenerateRequestHeader(this.TwitterContext, this.Method.ToString(), this.Url, this.Parameter);
            if (this.IsGZip)
                request.Headers["Accept-Encoding"] = "deflate, gzip";

            if (Method == Methods.POST && this.Parameter != null)
            {
                // Write request data
                using (var streamWriter = new StreamWriter(await request.GetRequestStreamAsync()))
                {
                    await streamWriter.WriteAsync(reqdata);
                }
            }

            // Connection start
        ConnectionStart:

            // Send request
            using (var rs = (HttpWebResponse)await request.GetResponseAsync())
            using (var st = rs.GetResponseStream())
            {
                var wr = rs as HttpWebResponse;
                if (wr != null && wr.ContentEncoding.ToLower() == "gzip")
                {
                    // GZip
                    var gzip = new GZipStream(st, CompressionMode.Decompress);
                    sr = new StreamReader(gzip);
                }
                else
                    // text/html
                    sr = new StreamReader(st);

                this.OnConnected(EventArgs.Empty);

                try
                {
                    // Streaming
                    while (this.IsConnected)
                    {
                        string data = await sr.ReadLineAsync();

                        // 接続を維持するために、空白行(Blank lines)が送られてくることがある
                        if (!string.IsNullOrEmpty(data))
                            this.OnStream(new StreamEventArgs(data));
                        else
                            this.OnKeepAliveSignaled(EventArgs.Empty);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);

                    this.IsConnected = false;
                    this.OnDisconnected(null);

                    // 再接続
                    if (this.IsAutoReconnect)
                    {
                        this.OnReconnected(EventArgs.Empty);
                        System.Threading.Thread.Sleep(this.Deley);
                        this.Deley += 250;

                        if (this.Deley < 1000)
                        {
                            this.IsConnected = true;
                            goto ConnectionStart;
                        }
                    }
                }
            }

            request.Abort();
            sr.Close();

            this.OnTerminated(EventArgs.Empty);
        }

        /// <summary>
        /// Streamから切断します。
        /// </summary>
        public void Disconnect()
        {
            this.IsConnected = false;
        }

        /// <summary>
        /// Streamから切断します。
        /// </summary>
        public void Dispose()
        {
            this.IsConnected = false;
        }

        private void StreamingCallback(object sender, StreamEventArgs e)
        {
            this.Deley = 0;
            var json = Twitch.Utility.DynamicJson.Parse(e.Data);

            // Status deleted
            if (json.IsDefined("delete"))
            {
                this.OnStatusDeletion(
                    new StatusDeletionEventArgs
                    {
                        ID = (Int64)json["delete"]["status"]["id"],
                        StringID = json["delete"]["status"]["id_str"],
                        UserID = (Int64)json["delete"]["status"]["user_id"],
                        StringUserID = json["delete"]["status"]["user_id_str"]
                    }
                );
                return;
            }
            // Disconnected
            else if (json.IsDefined("disconnect"))
            {
                this.OnDisconnected(
                    new DisconnectedEventArgs
                    {
                        Code = (int)json["code"],
                        StreamName = json["stream_name"],
                        Reason = json["reason"]
                    }
                );
                return;
            }
            else
            {
                this.OnStreamMessaged(new StreamEventArgs(e.Data));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        ~StreamingBase()
        {
            Debug.WriteLine("close");
        }
    }
}
