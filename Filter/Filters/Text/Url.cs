namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// ユーザーのウェブサイトURLをフィルタします。
    /// </summary>
    public class Url : TextFilterBase, IFilter
    {
        public string Identification { get { return "url"; } }
        public string Description { get { return "ユーザーのウェブサイトURL"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.Url; }
        public bool Match(Twitter.Status status) { return this.Judge((string)this.GetValue(status), this.Argument, this.FilterOperator, this); }
    }
}
