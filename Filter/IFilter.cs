using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public interface IFilter : IFilterObject
    {
        string Identification { get; }
        string Description { get; }
        FilterType Type { get; }
        Operator FilterOperator { get; set; }
        string Argument { get; set; }
        bool Match(Twitter.Status status);
    }
}
