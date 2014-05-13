using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch
{
	public static class twitch
	{
		public delegate void MessageEventHandler(string Data);
		public static event MessageEventHandler MessageEvent;

		public static void Message(string msg)
		{
			if (MessageEvent != null)
				MessageEvent(msg);
		}
	}
}
