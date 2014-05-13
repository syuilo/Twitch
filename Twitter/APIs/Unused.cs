using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Twitter.APIs
{
	public class Unused : Attribute
	{
		public Unused(string msg)
		{
			this.Message = msg;
		}

		public string Message { get; set; }
	}
}
