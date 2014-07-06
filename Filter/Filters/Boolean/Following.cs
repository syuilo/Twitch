namespace Twitch.Filter.Filters.Boolean
{
    /// <summary>
    /// フォローしているかどうかをフィルタします。
    /// </summary>
    public class Following : BooleanFilterBase, IFilter
    {
        public string Identification { get { return "following"; } }
        public string Description { get { return "フォローしているか"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.IsFollowing; }
        public bool Match(Twitter.Status status) { return this.Judge((bool)this.GetValue(status), bool.Parse(this.Argument), this.FilterOperator, this); }
    }
}
