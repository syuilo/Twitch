using System;

namespace Twitch.Twitter.API
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property)]
    public class ApiAttribute : Attribute
    {
        private string Summary;
        public ApiAttribute(string summary) { this.Summary = summary; }
    }
}