namespace Twitch.Filter.Filters.Numerical
{
    /// <summary>
    /// フォロワー数をフィルタします。
    /// </summary>
    public class FollowersCount : NumericalFilterBase, IFilter
    {
        public string Identification { get { return "followers_count"; } }
        public string Description { get { return "フォロワー数"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.FollowersCount; }
        public bool Match(Twitter.Status status) { return this.Judge((double)this.GetValue(status), double.Parse(this.Argument), this.FilterOperator, this); }
    }
}
