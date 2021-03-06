﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Streaming
{
	public class StreamEventEventArgs : EventArgs
	{
		public Twitter.User Target
		{
			get;
			set;
		}

		public Twitter.User Source
		{
			get;
			set;
		}

		public string Event
		{
			get;
			set;
		}

		public object TargetObject
		{
			get;
			set;
		}

		public string CreatedAt
		{
			get;
			set;
		}
	}
}
