using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch.Twitter;

namespace Twitch.Filter
{
    public abstract class Filter
    {
        public LogicalOperator? Operator
        {
            get;
            set;
        }

        public Operator FilterOperator
        {
            get;
            set;
        }

        //public bool Match(Twitter.Status status)
        //{
        //    return false;
        //}
    }
}
