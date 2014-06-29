namespace Twitch.Filter
{
    public abstract class TextFilterBase : Filter
    {
        public TextFilterBase(Twitter.Status status) : base(status) { }

        public bool Match(string input, string arg)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, arg);
        }
    }
}
