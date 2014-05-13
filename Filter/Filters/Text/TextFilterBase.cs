using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter.Filters.Text
{
	public abstract class TextFilterBase : FilterBase
	{
		public string Needle
		{
			get { return this.needle; }
			set
			{
				this.needle = value;
			}
		}
		protected string needle = String.Empty;

		protected virtual bool Match(string haystack, string needle, bool isCaseSensitive)
		{
			return true;
		}
	}
}
