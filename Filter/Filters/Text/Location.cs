namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// 場所をフィルタします。
    /// </summary>
    public class Location : TextFilterBase, IFilter
    {
        public string Identification { get { return "location"; } }
        public string Description { get { return "場所"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.Location; }
        public bool Match(Twitter.Status status) { return this.Judge((string)this.GetValue(status), this.Argument, this.FilterOperator, this); }
    }
}
