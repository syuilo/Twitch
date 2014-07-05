using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public class CalculationOperand
    {
        public CalculationOperandType Type
        {
            get;
            set;
        }

        public CalculationOperator? CalcOperator
        {
            get;
            set;
        }

        /// <summary>
        /// このオペランドのタイプに基づいてキャストを行ってください。
        /// </summary>
        public object Value
        {
            get;
            set;
        }
    }
}
