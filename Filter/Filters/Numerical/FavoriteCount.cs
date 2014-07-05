namespace Twitch.Filter.Filters.Numerical
{
    /// <summary>
    /// お気に入りに登録されている数をフィルタします。
    /// </summary>
    public class FavoriteCount : NumericalFilterBase, IFilter
    {
        public string Identification
        {
            get
            {
                return "favorite_count";
            }
        }

        public string Description
        {
            get
            {
                return "被お気に入り数";
            }
        }

        public string Argument
        {
            get;
            set;
        }

        public bool Match(Twitter.Status status)
        {
            return this.Judge(status.FavoriteCount, double.Parse(this.Argument), this.FilterOperator, this);
        }
    }
}


