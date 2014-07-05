using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public abstract class NumericalFilterBase : Filter
    {
        public FilterType Type
        {
            get
            {
                return FilterType.Numerical;
            }
        }

        protected bool Judge(double target, double arg, Operator filterOperator, IFilter f)
        {
            switch (filterOperator)
            {
                case Twitch.Filter.Operator.Include:
                    return target.ToString().IndexOf(arg.ToString()) > -1;
                case Twitch.Filter.Operator.Regex:
                    return System.Text.RegularExpressions.Regex.IsMatch(target.ToString(), arg.ToString());
                case Twitch.Filter.Operator.Equal:
                    return target == arg;
                case Twitch.Filter.Operator.Unequal:
                    return target != arg;
                case Twitch.Filter.Operator.GreaterThan:
                    return target > arg;
                case Twitch.Filter.Operator.LessThan:
                    return target < arg;
                case Twitch.Filter.Operator.GreaterThanOrEqual:
                    return target >= arg;
                case Twitch.Filter.Operator.LessThanOrEqual:
                    return target <= arg;
                default:
                    throw new FilterException("演算子 " + filterOperator + " はフィルター " + f.Identification + " のフィルタ タイプ " + f.Type + " に対して有効ではありません。");
            }
        }
    }
}
