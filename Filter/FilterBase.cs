using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
	public abstract class FilterBase : IFilter
	{
		/// <summary>
		/// フィルタを適用します。
		/// </summary>
		/// <returns>フィルタを通過したか</returns>
		public bool Filter(Twitter.Status status)
		{
			return FilterStatus(status) == !Negate;
		}

		protected abstract bool FilterStatus(Twitter.Status status);

		/// <summary>
		/// このフィルタは否定条件です<para />
		/// (Filter()メソッドで考慮されます)
		/// </summary>
		public bool Negate { get; set; }

		/// <summary>
		/// このフィルタを識別する文字列です。
		/// </summary>
		public abstract string Identifier { get; }

		/// <summary>
		/// フィルタの説明
		/// </summary>
		public abstract string Description { get; }
	}
}
