namespace Twitch.Filter.Filters.Numerical
{
    /// <summary>
    /// 被リスト数をフィルタします。
    /// </summary>
    public class ListedCount : NumericalFilterBase, IFilter
    {
        public string Identification { get { return "listed_count"; } }
        public string Description { get { return "被リスト数"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.ListedCount; }
        public bool Match(Twitter.Status status) { return this.Judge((double)this.GetValue(status), double.Parse(this.Argument), this.FilterOperator, this); }
    }
}
