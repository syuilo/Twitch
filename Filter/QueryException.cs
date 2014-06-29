using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public class QueryException : Exception
    {
        public QueryException(string message)
            : base(message)
        {
        }
    }
}
