using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Twitter
{
	public class UnOfficial : Attribute
	{
		public UnOfficial(string msg)
		{
			this.Message = msg;
		}

		public string Message { get; set; }
	}
}
