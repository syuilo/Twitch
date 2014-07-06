namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// ユーザーのScreenNameをフィルタします。
    /// </summary>
    public class ScreenName : TextFilterBase, IFilter
    {
        public string Identification { get { return "screen_name"; } }
        public string Description { get { return "ScreenName"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.ScreenName; }
        public bool Match(Twitter.Status status) { return this.Judge((string)this.GetValue(status), this.Argument, this.FilterOperator, this); }
    }
}