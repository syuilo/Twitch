namespace Twitch.Filter
{
    public abstract class TextFilterBase : Filter
    {
        public TextFilterBase(Twitter.Status status) : base(status) { }

        public FilterType Type
        {
            get
            {
                return FilterType.Text;
            }
        }

        public bool Match(string input, string arg, string symbol, string id)
        {
            switch (symbol)
            {
                case ":":
                    return System.Text.RegularExpressions.Regex.IsMatch(input, arg);
                case "=":
                    return input == arg;
                case "!=":
                    return input != arg;
                default:
                    throw new FilterException("演算子 " + symbol + " はフィルター " + id + " に対して有効ではありません。");
            }
        }
    }
}
