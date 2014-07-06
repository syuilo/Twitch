namespace Twitch.Filter.Filters.Boolean
{
    /// <summary>
    /// リツイートしているかどうかをフィルタします。
    /// </summary>
    public class Retweeted : BooleanFilterBase, IFilter
    {
        public string Identification { get { return "retweeted"; } }
        public string Description { get { return "リツイートしているか"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.FavoriteCount; }
        public bool Match(Twitter.Status status) { return this.Judge((bool)this.GetValue(status), bool.Parse(this.Argument), this.FilterOperator, this); }
    }
}
