namespace Twitch.Filter.Filters.Numerical
{
    /// <summary>
    /// お気に入りに登録されている数をフィルタします。
    /// </summary>
    public class FavoriteCount : NumericalFilterBase, IFilter
    {
        public FavoriteCount(Twitter.Status status) : base(status) { }

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

        public bool Verify(string arg, string symbol)
        {
            return this.Match(this.Input.FavoriteCount, double.Parse(arg), symbol, this.Identification);
        }
    }
}


