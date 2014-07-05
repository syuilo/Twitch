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

        protected bool Judge(string target, string arg, Operator filterOperator, IFilter f)
        {
            switch (filterOperator)
            {
                // .  大文字小文字問わず、PにQが含まれているか
                // :  PにQが含まれているか
                // ::  PにQを正規表現として検証した結果
                // ==  PとQが等しいか
                // !=  PとQが等しくないか
                case Twitch.Filter.Operator.IncludeTolerance:
                    return target.ToLower().IndexOf(arg.ToLower()) > -1;
                case Twitch.Filter.Operator.Include:
                    return target.IndexOf(arg) > -1;
                case Twitch.Filter.Operator.Regex:
                    return System.Text.RegularExpressions.Regex.IsMatch(target, arg);
                case Twitch.Filter.Operator.Equal:
                    return target == arg;
                case Twitch.Filter.Operator.Unequal:
                    return target != arg;
                default:
                    throw new FilterException("演算子 " + filterOperator + " はフィルター " + f.Identification + " のフィルタ タイプ " + f.Type + " に対して有効ではありません。");
            }
        }
    }
}
