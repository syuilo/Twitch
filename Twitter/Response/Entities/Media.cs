using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Twitter.Response.Entities
{
	/// <summary>
	/// Media情報を格納する Twitch.Twitter.TwitterResponse です。
	/// </summary>
	public class Media : TwitterResponse
	{
		public Media(string source)
			: base(source)
		{
			dynamic json;

			try
			{
				json = Utility.DynamicJson.Parse(source);
			}
			catch (Exception e)
			{
				throw new FormatException(
					"Jsonの解析に失敗しました。Jsonの形式が正しくない可能性があります。" +
					Environment.NewLine +
					e.Message);
			}

			//try
			//{
				this.ExpandedUrl = new Uri(json["expanded_url"]);
				this.Url = new Uri(json["url"]);
				this.Indices = json["indices"];
				this.DisplayUrl = json["display_url"];
				this.ID = json["id"];
				this.StringID = json["id_str"];
				this.MediaUrl = new Uri(json["media_url"]);
				this.MediaUrlHttps = new Uri(json["media_url_https"]);
				this.Sizes = new Sizes(json["sizes"].ToString());
				this.SourceStatusID = (json.IsDefined("source_status_id")) ? (Int64?)json["source_status_id"] : null;
				this.SourceStatusStringID = (json.IsDefined("source_status_id_str")) ? json["source_status_id_str"] : null;
				this.Type = json["type"];
			//}
			//catch (Exception e)
			//{
			//	throw new FormatException(
			//		"初期化に失敗しました。Jsonの形式が正しくない可能性があります。" +
			//		Environment.NewLine +
			//		e.Message);
			//}
		}

		/// <summary>
		/// MediaのID。
		/// </summary>
		public double ID
		{
			get;
			private set;
		}

		/// <summary>
		/// MediaのID。
		/// </summary>
		public string StringID
		{
			get;
			private set;
		}

		/// <summary>
		/// 元のURL。
		/// </summary>
		public Uri ExpandedUrl
		{
			get;
			private set;
		}

		/// <summary>
		/// t.coにより短縮されたURL。
		/// </summary>
		public Uri Url
		{
			get;
			private set;
		}

		/// <summary>
		/// ツイート内の、このURLのオフセットを表す整数の配列。
		/// 最初の整数は、ツイート内のURLの最初の文字の位置を表しています。
		/// 第二の整数は、URLの末尾の後の最初の非URL文字の位置を表しています。
		/// </summary>
		public double[] Indices
		{
			get;
			private set;
		}

		/// <summary>
		/// クライアントに表示されるURL。
		/// </summary>
		public string DisplayUrl
		{
			get;
			private set;
		}

		/// <summary>
		/// 
		/// </summary>
		public Uri MediaUrl
		{
			get;
			private set;
		}

		/// <summary>
		/// 
		/// </summary>
		public Uri MediaUrlHttps
		{
			get;
			private set;
		}

		/// <summary>
		/// 
		/// </summary>
		public Sizes Sizes
		{
			get;
			private set;
		}

		/// <summary>
		/// 
		/// </summary>
		public Int64? SourceStatusID
		{
			get;
			private set;
		}

		/// <summary>
		/// 
		/// </summary>
		public string SourceStatusStringID
		{
			get;
			private set;
		}

		/// <summary>
		/// 
		/// </summary>
		public string Type
		{
			get;
			private set;
		}
	}
}
