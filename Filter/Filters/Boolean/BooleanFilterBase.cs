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

        protected bool Judge(bool target, bool arg, Operator filterOperator, IFilter f)
        {
            switch (filterOperator)
            {
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
