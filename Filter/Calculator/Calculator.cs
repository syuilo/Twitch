using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    /// <summary>
    /// カルキュレータはフィルタの一種
    /// </summary>
    public class Calculator : NumericalFilterBase, IFilter
    {
        public Calculator()
        {
            this.Operands = new List<CalculationOperand>();
        }

        /// <summary>
        /// このカルキュレータに含まれるオペランド
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
        /// このカルキュレータにオペランドを追加します。
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
            Arithmetic? opr = Arithmetic.Addition;

            switch (this.Operands.Count)
            {
                case 0:
                    throw new CalculationException("カルキュレータにオペランドがありません。");
                case 1:
                    throw new CalculationException("カルキュレータにオペランドが1つしかありません。計算を行うには、最低でも2つのオペランドが必要です。");
            }

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
                    // 加算
                    case Arithmetic.Addition:
                        value += _value;
                        break;
                    // 減算
                    case Arithmetic.Subtraction:
                        value -= _value;
                        break;
                    // 乗算
                    case Arithmetic.Multiplication:
                        value *= _value;
                        break;
                    // 除算
                    case Arithmetic.Division:
                        value /= _value;
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
