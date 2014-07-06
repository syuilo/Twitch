namespace Twitch.Filter.Filters.Numerical
{
    /// <summary>
    /// フォロー数をフィルタします。
    /// </summary>
    public class FriendsCount : NumericalFilterBase, IFilter
    {
        public string Identification { get { return "friends_count"; } }
        public string Description { get { return "フォロー数"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.FriendsCount; }
        public bool Match(Twitter.Status status) { return this.Judge((double)this.GetValue(status), double.Parse(this.Argument), this.FilterOperator, this); }
    }
}
