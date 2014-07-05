using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    /// <summary>
    /// サーキュレータはフィルタの一種
    /// </summary>
    public class CalculationCluster : CalculationBase, IFilterObject
    {
        public FilterCluster Parent
        {
            get;
            set;
        }

        public LogicalOperator? Operator
        {
            get;
            set;
        }

        public bool Match(Twitter.Status status)
        {
            return false;
        }
    }
}
