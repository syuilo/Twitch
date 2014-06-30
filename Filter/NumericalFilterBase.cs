using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public abstract class NumericalFilterBase : Filter
    {
        public NumericalFilterBase(Twitter.Status status) : base(status) { }

        public FilterType Type
        {
            get
            {
                return FilterType.Numerical;
            }
        }

        public bool Match(double input, double arg, string symbol, string id)
        {
            switch (symbol)
            {
                // :  PにQが含まれているか
                // ::  PにQを正規表現として検証した結果
                // ==  PとQが等しいか
                // !=  PとQが等しくないか
                // > PがQより大きいか
                // < PがQより小さいか
                // >= PがQより大きいかまたは等しいか
                // <= PがQより小さいかまたは等しいか
                case ":":
                    return input.ToString().IndexOf(arg.ToString()) > -1;
                case "::":
                    return System.Text.RegularExpressions.Regex.IsMatch(input.ToString(), arg.ToString());
                case "==":
                    return input == arg;
                case "!=":
                    return input != arg;
                case ">":
                    return input > arg;
                case "<":
                    return input < arg;
                case ">=":
                    return input >= arg;
                case "<=":
                    return input <= arg;
                default:
                    throw new FilterException("演算子 " + symbol + " はフィルター " + id + " に対して有効ではありません。");
            }
        }
    }
}
