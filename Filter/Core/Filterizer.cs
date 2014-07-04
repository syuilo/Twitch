using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter.Core
{
    public static class Filterizer
    {
        public static bool Filterize(Twitter.Status status, Query query)
        {
            return query.Match(status);
        }
    }
}
