using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Twitter
{
	/// <summary>
	/// Twitterのユーザー(アカウント)です。
	/// </summary>
	public class User : Response.Users.User
	{
		public User(string json)
			: base(json) { }


		/// <summary>
		/// @を含むScreenNameを取得します。
		/// </summary>
		public string DisplayScreenName
		{
			get
			{
				return '@' + this.ScreenName;
			}
			private set
			{
				this.DisplayScreenName = value;
			}
		}


		/// <summary>
		/// フォローします。
		/// </summary>
		/// <param name="twCtx"></param>
		/// <returns>成功したかどうか</returns>
		public async Task<bool> Follow(TwitterContext twCtx)
		{
			return await APIs.REST.Friendships.Create(twCtx, id: this.StringID) != null;
		}

		/// <summary>
		/// フォローを解除します。
		/// </summary>
		/// <param name="twCtx"></param>
		/// <returns>成功したかどうか</returns>
		public async Task<bool> Remove(TwitterContext twCtx)
		{
			return await APIs.REST.Friendships.Destory(twCtx, id: this.StringID) != null;
		}

		/// <summary>
		/// スパムとして報告します。
		/// </summary>
		/// <param name="twCtx"></param>
		/// <returns></returns>
		public async Task<bool> SpamAndBlock(TwitterContext twCtx)
		{
			return await APIs.REST.Users.ReportSpam(twCtx, user_id: this.StringID) != null;
		}

		// リストに追加します。
		// 
		// ブロックします。


	}
}
