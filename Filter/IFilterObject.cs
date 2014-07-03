using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public interface IFilterObject
    {
        LogicalOperator Operator
        {
            get;
            set;
        }

        bool Match(Twitter.Status status);
    }
}
