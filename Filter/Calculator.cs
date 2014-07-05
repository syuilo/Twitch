using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    /// <summary>
    /// サーキュレータはフィルタの一種
    /// </summary>
    public class Calculator : NumericalFilterBase, IFilter
    {
        public Calculator()
        {
            this.Operands = new List<CalculationOperand>();
        }

        /// <summary>
        /// このサーキュレータに含まれるオペランド
        /// </summary>
        public List<CalculationOperand> Operands
        {
            get;
            set;
        }

        /// <summary>
        /// このクエリの検証結果を否定するか
        /// </summary>
        public bool IsNegate
        {
            get;
            set;
        }

        /// <summary>
        /// このサーキュレータにオペランドを追加します。
        /// </summary>
        /// <param name="o"></param>
        public void Add(CalculationOperand o)
        {
            this.Operands.Add(o);
        }

        public FilterCluster Parent
        {
            get;
            set;
        }

        public string Identification
        {
            get
            {
                return "#calc";
            }
        }

        public string Description
        {
            get
            {
                return null;
            }
        }

        public string Argument
        {
            get;
            set;
        }

        public bool Match(Twitter.Status status)
        {
            double value = 0;
            CalculationOperator? opr = CalculationOperator.Plus;

            foreach (CalculationOperand o in this.Operands)
            {
                double _value = 0;

                switch (o.Type)
                {
                    case CalculationOperandType.Filter:
                        _value = (double)((IFilter)o.Value).GetValue(status);
                        break;
                    case CalculationOperandType.Literal:
                        _value = double.Parse((string)o.Value);
                        break;
                }

                switch (opr)
                {
                    case CalculationOperator.Plus:
                        value += _value;
                        break;
                    case CalculationOperator.Minus:
                        value -= _value;
                        break;
                }

                opr = o.CalcOperator;
            }

            return this.Judge(value, double.Parse(this.Argument), this.FilterOperator, this);
        }

        public object GetValue(Twitter.Status status)
        {
            return status.Text;
        }
    }
}
