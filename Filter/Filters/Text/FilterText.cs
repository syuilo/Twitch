using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter.Filters.Text
{
	public class FilterText : TextFilterBase
	{
		public override string Identifier
		{
			get
			{
				return "text";
			}
		}

		public override string Description
		{
			get
			{
				return "ツイートの本文";
			}
		}

		protected override bool FilterStatus(Twitter.Status status)
		{
			//return this.Match(status.Text);
			return false;
		}
	}
}
