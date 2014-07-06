namespace Twitch.Filter.Filters.Numerical
{
    /// <summary>
    /// リツイート数をフィルタします。
    /// </summary>
    public class RetweetCount : NumericalFilterBase, IFilter
    {
        public string Identification { get { return "retweet_count"; } }
        public string Description { get { return "リツイート数"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.RetweetCount; }
        public bool Match(Twitter.Status status) { return this.Judge((double)this.GetValue(status), double.Parse(this.Argument), this.FilterOperator, this); }
    }
}
