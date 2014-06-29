using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public interface IFilter
    {
        string Identification { get; }
        string Description { get; }
        FilterType Type { get; }
        bool Verify(string arg, string symbol);
    }
}
