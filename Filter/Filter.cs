using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch.Twitter;

namespace Twitch.Filter
{
    public class Filter
    {
        public Status Input
        {
            get;
            set;
        }

        public Filter(Status status)
        {
            this.Input = status;
        }
    }
}
