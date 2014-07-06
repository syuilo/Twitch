namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// BIOをフィルタします。
    /// </summary>
    public class Bio : TextFilterBase, IFilter
    {
        public string Identification { get { return "bio"; } }
        public string Description { get { return "BIO"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.Description; }
        public bool Match(Twitter.Status status) { return this.Judge((string)this.GetValue(status), this.Argument, this.FilterOperator, this); }
    }
}
