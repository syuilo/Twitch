using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Twitch.Twitter.API;

namespace Twitch.Twitter
{
    public class Api
    {
        public Api(Methods method, string url)
        {
            this.Method = method;
            try
            {
                this.Url = new Uri(url);
            }
            catch
            {
                throw new ApplicationException("APIの初期化に失敗しました。URLが正しくありません。");
            }
        }

        /// <summary>
        /// リクエストに使用するHTTPメソッド。
        /// </summary>
        public Methods Method
        {
            get;
            protected set;
        }

        /// <summary>
        /// リクエスト先のURL。
        /// </summary>
        public Uri Url
        {
            get;
            protected set;
        }
    }
}
