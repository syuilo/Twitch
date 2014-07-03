using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twitch.Filter.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class Compiler
    {
        private static int cursor;
        private static string query;

        /// <summary>
        /// カーソルの位置を初期状態に戻します。
        /// </summary>
        public static void ResetCursor()
        {
            cursor = 0;
        }

        /// <summary>
        /// カーソルを次に進めます。
        /// </summary>
        public static void Next()
        {
            cursor++;
#if DEBUG
            Console.Write(query[cursor]);
#endif
        }

        /// <summary>
        /// カーソルを前に戻します。
        /// </summary>
        public static void Back()
        {
            if (cursor-- == -1)
                throw new CompileException("カーソルは既に初期状態に戻っています。これ以上巻き戻せません。");
            else
                cursor--;
#if DEBUG
            Console.Write(query[cursor]);
#endif
        }

        public static TokenType Tokenize()
        {
            switch (query[cursor])
            {
                case ' ':
                    return TokenType.Space;
                case '\t':
                    return TokenType.Tab;
                case '\r':
                    return TokenType.CarriageReturn;
                case '\n':
                    return TokenType.LineFeed;

                case '(':
                    return TokenType.OpenBracket;
                case ')':
                    return TokenType.CloseBracket;
                case '{':
                    return TokenType.OpenBracket;
                case '}':
                    return TokenType.CloseBracket;

                case '&':
                    return TokenType.ConcatenatorAnd;
                case '|':
                    return TokenType.ConcatenatorOr;
                case '^':
                    return TokenType.ConcatenatorXor;

                default:
                    return TokenType.Unknown;
            }
        }

        /// <summary>
        /// 与えられたクエリ文字列を元にQueryオブジェクトを生成します。
        /// </summary>
        /// <param name="_query">クエリ文字列</param>
        /// <returns>Query オブジェクト</returns>
        public static Query Compile(string _query)
        {
            query = _query;
            ResetCursor();

            var q = new Query();

            for (int i = 0; cursor < query.Length; i++, Next())
            {
                //System.Diagnostics.Debug.WriteLine(this.Query[pos]);

                switch (Tokenize())
                {
                    case TokenType.Space:
                        break;
                    case TokenType.OpenBracket:
                        q.Add(AnalyzeObject());
                        break;
                    default:
                        throw new QueryException("クエリが不適切です。クエリは必ず { で始まっている必要があります。");
                }
            }

            return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>フィルタ クラスタ</returns>
        private static FilterCluster AnalyzeObject()
        {
            var cluster = new FilterCluster();

            bool endPoint = false;
            while (!endPoint)
            {
                Next();

                if (cursor >= query.Length)
                    throw new QueryException("クエリが不適切です。オブジェクトが終了していません。");

                var logicalOperator = new LogicalOperator();

                switch (Tokenize())
                {
                    //// 無視する文字
                    //case ' ':
                    //    break;
                    //case '\t':
                    //    break;
                    //case '\r':
                    //    break;
                    //case '\n':
                    //    break;

                    case TokenType.OpenBracket:
                        var childCluster = AnalyzeObject();
                        childCluster.Operator = logicalOperator;
                        cluster.Add(childCluster);
                        break;
                    case TokenType.CloseBracket:
                        endPoint = true;
                        break;
                    //case ':':
                    //    break;

                    //case '!': // not
                    //    logicalOperators.Add('!');
                    //    break;
                    case TokenType.ConcatenatorAnd: // and
                        logicalOperator = LogicalOperator.And;
                        break;
                    case TokenType.ConcatenatorOr: // or
                        logicalOperator = LogicalOperator.Or;
                        break;
                    case TokenType.ConcatenatorXor: // xor
                        logicalOperator = LogicalOperator.Xor;
                        break;

                    default:
                        //results.Add(AnalyzeFilter());
                        break;
                }
            }
            return null;

        }

    }
}
