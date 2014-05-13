using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
	/// <summary>
	/// フィルタが実装すべきインターフェイスです。
	/// </summary>
	public interface IFilter
	{
		/// <summary>
		/// フィルタID
		/// </summary>
		string Identifier
		{
			get;
		}

		/// <summary>
		/// フィルタの説明
		/// </summary>
		string Description
		{
			get;
		}

		/// <summary>
		/// 否定条件であるか
		/// </summary>
		bool Negate { get; set; }

		/// <summary>
		/// フィルタを適用します。
		/// フィルタを通過したならばtrueを返し、それ以外ならfalseを返します。
		/// </summary>
		/// <param name="source"></param>
		/// <returns>フィルタを通過したか</returns>
		bool Filter(Twitter.Status status);
	}
}
