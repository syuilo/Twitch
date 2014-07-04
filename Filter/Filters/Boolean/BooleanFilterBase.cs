namespace Twitch.Filter
{
    public abstract class BooleanFilterBase : Filter
    {
        public FilterType Type
        {
            get
            {
                return FilterType.Boolean;
            }
        }

        public bool Match(bool input, bool arg, string symbol, string id)
        {
            switch (symbol)
            {
                // ==  PとQが等しいか
                // !=  PとQが等しくないか
                case "==":
                    return input == arg;
                case "!=":
                    return input != arg;
                default:
                    throw new FilterException("演算子 " + symbol + " はフィルター " + id + " に対して有効ではありません。");
            }
        }
    }
}
