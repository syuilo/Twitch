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

        public bool Match(Twitter.Status status)
        {
            return this.Judge(status.Text, this.Argument, this.FilterOperator, this);
        }

        public object GetValue(Twitter.Status status)
        {
            return status.Text;
        }
    }
}


