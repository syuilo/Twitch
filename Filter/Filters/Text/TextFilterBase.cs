namespace Twitch.Filter
{
    public abstract class TextFilterBase : Filter
    {
        public FilterType Type
        {
            get
            {
                return FilterType.Text;
            }
        }

        //public bool Match(string input, string arg, string symbol, string id)
        //{
        //    switch (symbol)
        //    {
        //        // .  大文字小文字問わず、PにQが含まれているか
        //        // :  PにQが含まれているか
        //        // ::  PにQを正規表現として検証した結果
        //        // ==  PとQが等しいか
        //        // !=  PとQが等しくないか
        //        case ".":
        //            return input.ToLower().IndexOf(arg.ToLower()) > -1;
        //        case ":":
        //            return input.IndexOf(arg) > -1;
        //        case "::":
        //            return System.Text.RegularExpressions.Regex.IsMatch(input, arg);
        //        case "==":
        //            return input == arg;
        //        case "!=":
        //            return input != arg;
        //        default:
        //            throw new FilterException("演算子 " + symbol + " はフィルター " + id + " に対して有効ではありません。");
        //    }
        //}
    }
}
