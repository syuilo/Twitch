namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// ツイートの本文をフィルタします。
    /// </summary>
    public class Text : TextFilterBase, IFilter
    {
        public Text(Twitter.Status status) : base(status) { }

        public string Identification
        {
            get
            {
                return "text";
            }
        }

        public string Description
        {
            get
            {
                return "ツイート本文";
            }
        }

        public bool Verify(string arg, string symbol)
        {
            return this.Match(this.Input.Text, arg, symbol, this.Identification);
        }
    }
}


