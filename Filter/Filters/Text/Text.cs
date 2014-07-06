namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// ツイートの本文をフィルタします。
    /// </summary>
    public class Text : TextFilterBase, IFilter
    {
        public string Identification { get { return "text"; } }
        public string Description { get { return "ツイート本文"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.Text; }
        public bool Match(Twitter.Status status) { return this.Judge((string)this.GetValue(status), this.Argument, this.FilterOperator, this); }
    }
}