namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// ユーザー名をフィルタします。
    /// </summary>
    public class Name : TextFilterBase, IFilter
    {
        public string Identification { get { return "name"; } }
        public string Description { get { return "ユーザー名"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.Name; }
        public bool Match(Twitter.Status status) { return this.Judge((string)this.GetValue(status), this.Argument, this.FilterOperator, this); }
    }
}