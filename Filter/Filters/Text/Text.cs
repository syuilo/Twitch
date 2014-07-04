namespace Twitch.Filter.Filters.Text
{
    /// <summary>
    /// ツイートの本文をフィルタします。
    /// </summary>
    public class Text : TextFilterBase, IFilter
    {
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

        public string Argument
        {
        get;
        set;
        }

        public Operator FilterOperator
        {
        get;
        set;
        }

        public bool Match(Twitter.Status status)
        {
            return (status.Text.IndexOf(this.Argument) == -1) ? false : true;
            //return this.Match(status.Text, arg, symbol, this.Identification);
        }
    }
}


