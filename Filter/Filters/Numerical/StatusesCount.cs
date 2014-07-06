namespace Twitch.Filter.Filters.Numerical
{
    /// <summary>
    /// ツイート数をフィルタします。
    /// </summary>
    public class StatusesCount : NumericalFilterBase, IFilter
    {
        public string Identification { get { return "statuses_count"; } }
        public string Description { get { return "ツイート数"; } }

        public string Argument
        {
            get;
            set;
        }

        public bool Match(Twitter.Status status)
        {
            return this.Judge((double)status.User.StatusesCount, double.Parse(this.Argument), this.FilterOperator, this);
        }

        public object GetValue(Twitter.Status status)
        {
            return (double)status.User.StatusesCount;
        }
    }
}
