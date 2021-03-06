﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter
{
    public abstract class QueryBase
    {
        public QueryBase()
        {
            this.Filters = new List<IFilterObject>();
        }

        /// <summary>
        /// このクエリに含まれるフィルタまたはフィルタ クラスタのコレクション
        /// </summary>
        public List<IFilterObject> Filters
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
        public void Add(IFilterObject o)
        {
            this.Filters.Add(o);
        }

        /// <summary>
        /// 与えられたステータスをクエリに基づいて検証します。
        /// </summary>
        /// <param name="status"></param>
        /// <returns>検証結果</returns>
        public bool Match(Twitter.Status status)
        {
            bool result = this.IsNegate;
            LogicalOperator? opr = null;

            switch (this.Filters.Count)
            {
                case 0:
                    return !this.IsNegate;
                case 1:
                    if (!this.IsNegate)
                        return this.Filters[0].Match(status);
                    else
                        return !this.Filters[0].Match(status);
                default:
                    foreach (IFilterObject f in this.Filters)
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

                    return result;
            }
        }
    }
}
