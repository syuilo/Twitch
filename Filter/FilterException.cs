using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public class FilterException : Exception
    {
        public FilterException(string message)
            : base(message)
        {
        }
    }
}
