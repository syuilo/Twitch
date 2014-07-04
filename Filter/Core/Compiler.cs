using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

        #region Cursor

        /// <summary>
        /// カーソルの位置を初期状態に戻します。
        /// </summary>
        public static void ResetCursor()
        {
            cursor = 0;
#if DEBUG
            Console.Write(query[cursor]);
#endif
        }

        /// <summary>
        /// カーソルを次に進めます。
        /// </summary>
        public static void Next()
        {
            if (cursor + 1 >= query.Length)
                throw new CompileException("これ以上進められません。");
            else
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
            if (cursor - 1 == -1)
                throw new CompileException("カーソルは既に初期状態に戻っています。これ以上巻き戻せません。");
            else
                cursor--;
#if DEBUG
            Console.Write(query[cursor]);
#endif
        }

        /// <summary>
        /// 現在のカーソル位置にある文字を取得します。
        /// </summary>
        /// <returns>文字</returns>
        public static char ReadChar()
        {
            return query[cursor];
        }

        #endregion

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

                case '\'':
                    return TokenType.SingleQuote;
                case '"':
                    return TokenType.DoubleQuote;

                case '&':
                    return TokenType.ConcatenatorAnd;
                case '|':
                    return TokenType.ConcatenatorOr;
                case '^':
                    return TokenType.ConcatenatorXor;

                case '\\':
                    return TokenType.Escape;

                default:
                    return TokenType.Unknown;
            }
        }

        public static Operator Operatornize(string opr)
        {
            switch (opr)
            {
                case ":":
                    return Operator.Include;
                case ".":
                    return Operator.IncludeTolerance;
                case "::":
                    return Operator.Regex;
                case "==":
                    return Operator.Equal;
                case "!=":
                    return Operator.Unequal;
                case ">":
                    return Operator.GreaterThan;
                case "<":
                    return Operator.LessThan;
                case ">=":
                    return Operator.GreaterThanOrEqual;
                case "<=":
                    return Operator.LessThanOrEqual;

                default:
                    throw new QueryException("不明な演算子 " + opr + " です。");
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

            if (String.IsNullOrEmpty(query))
                return null;

            Debug.Write("> ");

            var q = new Query();

            for (int i = 0; cursor < query.Length; i++, Next())
            {
                switch (Tokenize())
                {
                    case TokenType.Space:
                        break;
                    case TokenType.OpenBracket:
                        q.Add(AnalyzeCluster());
                        Debug.Write(" < COK ");
                        return q;
                    default:
                        throw new QueryException("クエリが不適切です。クエリは必ず { で始まっている必要があります。");
                }
            }

            throw new QueryException("クエリが");

            //return q;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>フィルタ クラスタ</returns>
        private static FilterCluster AnalyzeCluster()
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
                    // 無視する文字
                    case TokenType.Space:
                        break;
                    case TokenType.Tab:
                        break;
                    case TokenType.CarriageReturn:
                        break;
                    case TokenType.LineFeed:
                        break;

                    case TokenType.OpenBracket:
                        var childCluster = AnalyzeCluster();
                        childCluster.Operator = logicalOperator;
                        childCluster.Parent = cluster;
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
                        cluster.Add(AnalyzeFilter());
                        break;
                }
            }
            return cluster;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>フィルタ</returns>
        private static IFilterObject AnalyzeFilter()
        {
            var filter = new object();
            string filterId = String.Empty, filterSymbol = String.Empty, filterArg = String.Empty;

            Back();

            bool findId = false;
            //System.Diagnostics.Debug.WriteLine("# フィルタIdを走査します。");

            while (!findId)  // フィルタIDを走査
            {
                Next();

                if (cursor > query.Length)
                    throw new QueryException("クエリが不適切です。フィルタ " + filterId + " が終了していません。");

                switch (Tokenize())
                {
                    case TokenType.Space:
                        findId = true;
                        break;
                    //case ':':
                    //    findId = true;
                    //    break;
                    case TokenType.CloseBracket:
                        throw new QueryException("フィルタ " + filterId + " に引数がありません。フィルタの引数に出会う前に、オブジェクトが終了しました。");
                    default:
                        if (Regex.IsMatch(ReadChar().ToString(), "[a-z A-Z _]"))
                            filterId += ReadChar();
                        else
                            findId = true;
                        break;
                }
            }

            filter = GetFilterFromId(filterId);
            //System.Diagnostics.Debug.WriteLine("# フィルタIdは " + filterId + " です。演算子の走査を開始します。");
            Back();
            bool findSymbol = false;

            while (!findSymbol)  // フィルタシンボルを走査
            {
                Next();

                if (cursor >= query.Length)
                    throw new QueryException("クエリが不適切です。フィルタ " + filterId + " が終了していません。");

                switch (Tokenize())
                {
                    case TokenType.Space:
                        if (filterSymbol.Length > 0)
                            findSymbol = true;
                        break;
                    case TokenType.DoubleQuote:
                        findSymbol = true;
                        break;
                    case TokenType.CloseBracket:
                        throw new QueryException("フィルタ " + filterId + " に演算子がありません。フィルタの演算子に出会う前に、オブジェクトが終了しました。");
                    default:
                        filterSymbol += ReadChar();
                        break;
                }
            }

            if (String.IsNullOrEmpty(filterSymbol))
                throw new QueryException("フィルタ " + filterId + " に演算子がありません。");

            ((IFilter)filter).FilterOperator = Operatornize(filterSymbol);
            //System.Diagnostics.Debug.WriteLine("# フィルタシンボルは " + filterSymbol + " です。引数の走査を開始します。");
            Back();

            int dcCount = 0;
            bool findArg = false;
            while (!findArg) // フィルタの引数を走査
            {
                Next();

                switch (Tokenize())
                {
                    case TokenType.Space:
                        break;

                    case TokenType.Escape:  // エスケープ文字
                        Next();
                        switch (ReadChar())
                        {
                            case 'n':
                                filterArg += "\n";
                                break;
                            case 'r':
                                filterArg += "\r";
                                break;
                            default:
                                filterArg += ReadChar();
                                break;
                        }
                        break;
                    case TokenType.DoubleQuote:
                        dcCount++;
                        if (dcCount == 2)
                            findArg = true;
                        break;
                    case TokenType.CloseBracket:
                        if (dcCount == 0)
                            throw new QueryException("フィルタに " + filterId + " 引数がありません。フィルタの引数に出会う前に、オブジェクトが終了しました。");
                        else if (dcCount == 1)
                            if (filterArg.Length > 0)
                                throw new QueryException("引数が閉じられていません。引数はダブルクォーテーション '\"' で終わらなければなりません。");
                            else
                                throw new QueryException("引数が不正です。引数はダブルクォーテーション '\"' で始まらなければなりません。");
                        else
                            throw new QueryException("フィルタが不正です。");

                    default:
                        if (dcCount == 1)
                            filterArg += ReadChar();
                        break;
                }
            }

            //Back();
            //Back();

            ((IFilter)filter).Operator = null;
            bool findOpr = false;
            while (!findOpr)
            {
                Next();

                switch (Tokenize())
                {
                    case TokenType.Space:
                        break;

                    case TokenType.CloseBracket:
                        findOpr = true;
                        break;

                    case TokenType.ConcatenatorAnd:
                        ((IFilter)filter).Operator = LogicalOperator.And;
                        findOpr = true;
                        break;
                    case TokenType.ConcatenatorOr:
                        ((IFilter)filter).Operator = LogicalOperator.Or;
                        findOpr = true;
                        break;
                    case TokenType.ConcatenatorXor:
                        ((IFilter)filter).Operator = LogicalOperator.Xor;
                        findOpr = true;
                        break;
                    default:
                        Console.WriteLine(ReadChar());
                        break;
                }
            }

            Back();

            //System.Diagnostics.Debug.WriteLine("# フィルタ " + filterId + " の引数は \"" + filterArg + "\" です。");
            ((IFilter)filter).Argument = filterArg;

            return (IFilterObject)filter;
        }

        public static IFilter GetFilterFromId(string filterId)
        {
            switch (filterId)
            {
                case "text":
                    return new Filters.Text.Text();
                default:
                    throw new QueryException("フィルタが不適切です。ID \"" + filterId + "\" に一致するフィルタがありません。");
            }
        }

    }
}
