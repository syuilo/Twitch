using System;
using System.Drawing;

namespace Twitch.Twitter.Response.Users
{
	/// <summary>
	/// User情報を格納する Twitch.Twitter.TwitterResponse です。
	/// </summary>
	public class User : TwitterResponse
	{
		public User()
			: base() { }

		/// <summary>
		/// Userのコンストラクタです。
		/// Userを初期化します。
		/// </summary>
		/// <param name="source">Jsonソース</param>
		public User(string source)
			: base(source)
		{
			this.ProfileSidebarFillColor = ColorTranslator.FromHtml('#' + this.Json["profile_sidebar_fill_color"]);
			this.ProfileSidebarBorderColor = ColorTranslator.FromHtml('#' + this.Json["profile_sidebar_border_color"]);
			this.IsProfileBackgroundTile = this.Json["profile_background_tile"];
			this.Name = this.Json["name"];
			this.ProfileImageUrl = new Uri(this.Json["profile_image_url"]);
			this.CreatedAt = this.Json["created_at"];
			this.Location = this.Json["location"];
			this.FollowRequestSent = this.Json["follow_request_sent"];
			this.ProfileLinkColor = ColorTranslator.FromHtml('#' + this.Json["profile_link_color"]);
			this.IsTranslator = this.Json["is_translator"];
			this.StringID = this.Json["id_str"];
			this.IsDefaultProfile = this.Json["default_profile"];
			this.ContributorsEnabled = this.Json["contributors_enabled"];
			this.FavouritesCount = this.Json["favourites_count"];
			this.Url = (!String.IsNullOrEmpty(this.Json["url"])) ? new Uri(this.Json["url"]) : null;
			this.ProfileImageUrlHttps = new Uri(this.Json["profile_image_url_https"]);
			this.UtcOffset = (int?)this.Json["utc_offset"];
			this.ID = (Int64)this.Json["id"];
			this.IsProfileUseBackGroundImage = this.Json["profile_use_background_image"];
			this.ListedCount = this.Json["listed_count"];
			this.ProfileTextColor = ColorTranslator.FromHtml('#' + this.Json["profile_text_color"]);
			this.Lang = this.Json["lang"];
			this.FollowersCount = this.Json["followers_count"];
			this.IsProtected = this.Json["protected"];
			this.IsNotifications = this.Json["notifications"];
			this.ProfileBackgroundImageUrlHttps = new Uri(this.Json["profile_background_image_url_https"]);
			this.ProfileBackgroundColor = ColorTranslator.FromHtml('#' + this.Json["profile_background_color"]);
			this.IsVerified = this.Json["verified"];
			this.GeoEnabled = this.Json["geo_enabled"];
			this.TimeZone = this.Json["time_zone"];
			this.Description = this.Json["description"];
			this.IsDefaultProfileImage = this.Json["default_profile_image"];
			this.ProfileBackgroundImageUrl = new Uri(this.Json["profile_background_image_url"]);
			this.Status = (this.Json.IsDefined("status")) ? new Tweets.Status(this.Json["status"].ToString()) : null;
			this.StatusesCount = this.Json["statuses_count"];
			this.FriendsCount = this.Json["friends_count"];
			this.IsFollowing = this.Json["following"];
			this.IsShowAllInlineMedia = (this.Json.IsDefined("show_all_inline_media")) ? this.Json["show_all_inline_media"] : null;
			this.ScreenName = this.Json["screen_name"];
			this.WithheldInCountries = (this.Json.IsDefined("withheld_in_countries")) ? this.Json["withheld_in_countries"] : null;
			this.WithheldScope = (this.Json.IsDefined("withheld_scope")) ? this.Json["withheld_scope"] : null;
		}

		/// <summary>
		/// ユーザーページのサイドバーの色を取得します。
		/// </summary>
		public Color? ProfileSidebarFillColor
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーページのサイドバーの輪郭の色を取得します。
		/// </summary>
		public Color? ProfileSidebarBorderColor
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーページの背景画像をタイルして表示するかどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsProfileBackgroundTile
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザー名を取得します。
		/// </summary>
		public string Name
		{
			get;
			private set;
		}

		/// <summary>
		/// アイコンのURLを取得します。
		/// </summary>
		public Uri ProfileImageUrl
		{
			get;
			private set;
		}

		/// <summary>
		/// アカウントの作成日時を取得します。
		/// </summary>
		public string CreatedAt
		{
			get;
			private set;
		}

		/// <summary>
		/// 場所情報を取得します。
		/// </summary>
		public string Location
		{
			get;
			private set;
		}

		/// <summary>
		/// フォロー申請をしているかどうかを表す System.Boolean 値を取得します。<para />
		/// これは鍵垢の場合にのみ有効なプロパティです。
		/// </summary>
		public bool? FollowRequestSent
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーページのリンクの色を取得します。
		/// </summary>
		public Color? ProfileLinkColor
		{
			get;
			private set;
		}

		/// <summary>
		/// 翻訳者かどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsTranslator
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーID(String)を取得します。
		/// </summary>
		public string StringID
		{
			get;
			private set;
		}

		/// <summary>
		/// プロフィールに自己紹介が設定されていないかどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsDefaultProfile
		{
			get;
			private set;
		}

		/// <summary>
		/// ライター機能を使用しているかどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? ContributorsEnabled
		{
			get;
			private set;
		}

		/// <summary>
		/// お気に入りの数を取得します。
		/// </summary>
		public double? FavouritesCount
		{
			get;
			private set;
		}

		/// <summary>
		/// ウェブサイトURLを取得します。
		/// </summary>
		public Uri Url
		{
			get;
			private set;
		}

		/// <summary>
		/// アイコンのURL (HTTPS)を取得します。
		/// </summary>
		public Uri ProfileImageUrlHttps
		{
			get;
			private set;
		}

		/// <summary>
		/// タイムゾーンとUTC（協定世界時）との差を取得します。
		/// </summary>
		public int? UtcOffset
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーID(Int64)を取得します。
		/// </summary>
		public Int64? ID
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーページの背景画像を設定しているかどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsProfileUseBackGroundImage
		{
			get;
			private set;
		}

		/// <summary>
		/// 被リスト数を取得します。
		/// </summary>
		public double? ListedCount
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーページのテキスト色を取得します。
		/// </summary>
		public Color? ProfileTextColor
		{
			get;
			private set;
		}

		/// <summary>
		/// 言語を取得します。
		/// </summary>
		public string Lang
		{
			get;
			private set;
		}

		/// <summary>
		/// フォロワー数を取得します。
		/// </summary>
		public double? FollowersCount
		{
			get;
			private set;
		}

		/// <summary>
		/// 鍵垢かどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsProtected
		{
			get;
			private set;
		}

		/// <summary>
		/// このユーザーからの通知を受け取るかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsNotifications
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーページの背景画像のURL(HTTPS)を取得します。<para />
		/// (※ヘッダーではありません。Webの公式で見たときに背景に表示されるアレです。ヘッダーを取得したい場合は GET users/profile_banner を使用してください。)
		/// </summary>
		public Uri ProfileBackgroundImageUrlHttps
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーページの背景色を取得します。
		/// </summary>
		public Color? ProfileBackgroundColor
		{
			get;
			private set;
		}

		/// <summary>
		/// 認証済みアカウントかどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsVerified
		{
			get;
			private set;
		}

		/// <summary>
		/// ツイートに位置情報を付加しているかどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? GeoEnabled
		{
			get;
			private set;
		}

		/// <summary>
		/// タイムゾーンを取得します。
		/// </summary>
		public string TimeZone
		{
			get;
			private set;
		}

		/// <summary>
		/// プロフィール (自己紹介)を取得します。
		/// </summary>
		public string Description
		{
			get;
			private set;
		}

		/// <summary>
		/// Twitterのデフォルトのアイコンかどうか
		/// 要するにたまごアイコンかどうか(2014年現在)。
		/// </summary>
		public bool? IsDefaultProfileImage
		{
			get;
			private set;
		}

		/// <summary>
		/// ユーザーページの背景画像のURLを取得します。<para />
		/// (※ヘッダーではありません。Webの公式で見たときに背景に表示されるアレです。ヘッダーを取得したい場合は GET users/profile_banner を使用してください。)
		/// </summary>
		public Uri ProfileBackgroundImageUrl
		{
			get;
			private set;
		}

		/// <summary>
		/// 直近のツイートを取得します。<para />
		/// Null を返す場合もあります。
		/// </summary>
		public Tweets.Status Status
		{
			get;
			private set;
		}

		/// <summary>
		/// ツイート数を取得します。
		/// </summary>
		public double? StatusesCount
		{
			get;
			private set;
		}

		/// <summary>
		/// フォロー数を取得します。
		/// </summary>
		public double? FriendsCount
		{
			get;
			private set;
		}

		/// <summary>
		/// このユーザーをフォローしているかどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsFollowing
		{
			get;
			private set;
		}

		/// <summary>
		/// 投稿された写真とビデオをすべて表示するかどうかを表す System.Boolean 値を取得します。
		/// </summary>
		public bool? IsShowAllInlineMedia
		{
			get;
			private set;
		}

		/// <summary>
		/// スクリーン名 (@以降)を取得します。
		/// </summary>
		public string ScreenName
		{
			get;
			private set;
		}

		public string WithheldInCountries
		{
			get;
			private set;
		}

		public string WithheldScope
		{
			get;
			private set;
		}
	}
}
