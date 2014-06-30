namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// ツイートの本文をフィルタします。
    /// </summary>
    public class ScreenName : TextFilterBase, IFilter
    {
        public ScreenName(Twitter.Status status) : base(status) { }

        public string Identification
        {
            get
            {
                return "screen_name";
            }
        }

        public string Description
        {
            get
            {
                return "ユーザーのScreenName";
            }
        }

        public bool Verify(string arg, string symbol)
        {
            return this.Match(this.Input.User.ScreenName, arg, symbol, this.Identification);
        }
    }
}


