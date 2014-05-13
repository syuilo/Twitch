using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Twitter
{
	/// <summary>
	/// Geo情報を格納する Twitch.Twitter.TwitterResponse です。
	/// </summary>
	public class Geo : TwitterResponse
	{
		public Geo(string source)
			: base(source)
		{

		}
	}
}
