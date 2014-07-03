using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    /// <summary>
    /// フィルタ クラスタ
    /// </summary>
    public class FilterCluster : QueryBase, IFilterObject
    {
        public FilterCluster Parent
        {
            get;
            set;
        }

        public LogicalOperator Operator
        {
            get;
            set;
        }
    }
}
