using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public abstract class CalculationBase
    {
        public CalculationBase()
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
        /// このクエリにフィルタまたはフィルタ クラスタを追加します。
        /// </summary>
        /// <param name="o"></param>
        public void Add(CalculationOperand o)
        {
            this.Operands.Add(o);
        }

        /// <summary>
        /// 与えられたステータスをクエリに基づいて検証します。
        /// </summary>
        /// <param name="status"></param>
        /// <returns>検証結果</returns>
        public int Calculation(Twitter.Status status)
        {
            bool result = this.IsNegate;
            LogicalOperator? opr = null;

            switch (this.Operands.Count)
            {
                case 0:
                    throw new CalculationException("サーキュレータにオペランドがありません。");
                case 1:
                    throw new CalculationException("サーキュレータにオペランドが1つしかありません。計算を行うには、最低でも2つのオペランドが必要です。");
                default:
                    foreach (IFilterObject f in this.Operands)
                    {
                        bool _result = f.Match(status);

                        switch (opr)
                        {
                            case LogicalOperator.And: // and
                                result &= _result;
                                break;
                            case LogicalOperator.Or: // or
                                result |= _result;
                                break;
                            case LogicalOperator.Xor: // xor
                                result ^= _result;
                                break;

                            default:
                                result = _result;
                                break;
                        }

                        opr = f.Operator;
                    }

                    return 100;
            }
        }
    }
}
