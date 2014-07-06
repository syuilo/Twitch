namespace Twitch.Filter.Filters.Boolean
{
    /// <summary>
    /// 鍵垢かどうかをフィルタします。
    /// </summary>
    public class Protected : BooleanFilterBase, IFilter
    {
        public string Identification { get { return "protected"; } }
        public string Description { get { return "鍵垢かどうか"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.IsProtected; }
        public bool Match(Twitter.Status status) { return this.Judge((bool)this.GetValue(status), bool.Parse(this.Argument), this.FilterOperator, this); }
    }
}
