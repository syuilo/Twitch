using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public class CalculationException : Exception
    {
        public CalculationException(string message)
            : base(message)
        {
        }
    }
}
