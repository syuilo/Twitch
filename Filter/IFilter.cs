using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    /// <summary>
    /// フィルタが実装しなければならないインターフェイスです。
    /// </summary>
    public interface IFilter : IFilterObject
    {
        string Identification { get; }
        string Description { get; }
        FilterType Type { get; }
        Operator FilterOperator { get; set; }
        string Argument { get; set; }
        object GetValue(Twitter.Status status);
    }
}
