namespace Twitch.Filter.Filters.Boolean
{
    /// <summary>
    /// ふぁぼっているかどうかをフィルタします。
    /// </summary>
    public class Favorited : BooleanFilterBase, IFilter
    {
        public string Identification { get { return "favorited"; } }
        public string Description { get { return "ふぁぼっているか"; } }
        public string Argument { get; set; }

        public object GetValue(Twitter.Status status) { return status.IsFavorited; }
        public bool Match(Twitter.Status status) { return this.Judge((bool)this.GetValue(status), bool.Parse(this.Argument), this.FilterOperator, this); }
    }
}
