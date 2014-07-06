namespace Twitch.Filter.Filters.Boolean
{
    /// <summary>
    /// 公式アカウントかどうかをフィルタします。
    /// </summary>
    public class Verified : BooleanFilterBase, IFilter
    {
        public string Identification { get { return "verified"; } }
        public string Description { get { return "公式アカウントか"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.User.IsVerified; }
        public bool Match(Twitter.Status status) { return this.Judge((bool)this.GetValue(status), bool.Parse(this.Argument), this.FilterOperator, this); }
    }
}
