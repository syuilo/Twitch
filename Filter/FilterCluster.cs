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

        public LogicalOperator? Operator
        {
            get;
            set;
        }

        //public bool Match(Twitter.Status status)
        //{
        //        bool result = this.IsNegate;

        //    foreach (IFilterObject f in this.Filters)
        //    {
        //                bool a = result, b = results[i];

        //                switch (logicalOperators[j])
        //                {
        //                    //case '!':
        //                    //    result = (!result);
        //                    //    break;
        //                    case '&': // and
        //                        result = (a && b);
        //                        break;
        //                    case '|': // or
        //                        result = (a || b);
        //                        break;
        //                    case '^': // xor
        //                        result = (a ^ b);
        //                        break;
        //                }


        //    }
        //        return result;
        //}
    }
}
