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
        private static int _cursor;
        private static int cursor
        {
            get
            {
                return _cursor;
            }
            set
            {
                _cursor = value;
#if DEBUG
                Console.Write(query[cursor]);
#endif
            }
        }

        private static string query;

        #region Cursor

        /// <summary>
        /// カーソルの位置を初期状態に戻します。
        /// </summary>
        public static void ResetCursor()
        {
            cursor = new int();
            cursor = 0;
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
                case '[':
                    return TokenType.OpenBracket;
                case ']':
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

                case '#':
                    return TokenType.Sharp;

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
        public static Query CompileToObject(string _query)
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
                        foreach (IFilterObject f in AnalyzeCluster().Filters)
                        {
                            q.Add(f);
                        }
                        Debug.Write(" < COK コンパイルは正常に完了しました。 ");
                        return q;
                    default:
                        throw new QueryException("クエリが不適切です。クエリは必ず { で始まっている必要があります。");
                }
            }

            throw new QueryException("");

            //return q;
        }

        /// <summary>
        /// 与えられたQueryオブジェクトを元にクエリ文字列を生成します。
        /// </summary>
        /// <param name="_query">Query オブジェクト</param>
        /// <returns>クエリ文字列</returns>
        public static string CompileToString(Query _query)
        {
            return "";
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
                        childCluster.Parent = cluster;
                        cluster.Add(childCluster);
                        break;
                    case TokenType.CloseBracket:
                        endPoint = true;
                        break;

                    case TokenType.Sharp:
                        Next();
                        switch (Tokenize())
                        {
                            case TokenType.OpenBracket:
                                var childCalculator = AnalyzeCalculator();
                                childCalculator.Parent = cluster;
                                cluster.Add(childCalculator);
                                break;
                        }
                        break;

                    case TokenType.ConcatenatorAnd: // and
                        cluster.Filters.Last().Operator = LogicalOperator.And;
                        break;
                    case TokenType.ConcatenatorOr: // or
                        cluster.Filters.Last().Operator = LogicalOperator.Or;
                        break;
                    case TokenType.ConcatenatorXor: // xor
                        cluster.Filters.Last().Operator = LogicalOperator.Xor;
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
        /// <returns></returns>
        private static CalculationCluster AnalyzeCalculator()
        {
            var calculator = new CalculationCluster();

            bool endPoint = false;
            while (!endPoint)
            {
                Next();

                if (cursor >= query.Length)
                    throw new QueryException("サーキュレータが終了していません。");

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
                        break;
                    case TokenType.CloseBracket:
                        endPoint = true;
                        break;

                    default:
                        calculator.Add(AnalyzeOperand());
                        break;
                }
            }

            Back();
            return calculator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static CalculationOperand AnalyzeOperand()
        {
            var operand = new object();
            string filterId = String.Empty;
            string literal = String.Empty;
            CalculationOperator? copr = null;

            bool end = false;
            while (!end)
            {
                switch (Tokenize())
                {
                    case TokenType.Space:
                        break;
                    case TokenType.CloseBracket:
                        Back();
                        end = true;
                        break;
                    default:
                        if (Regex.IsMatch(ReadChar().ToString(), "[a-z A-Z _]"))
                            filterId += ReadChar();
                        else if (Regex.IsMatch(ReadChar().ToString(), "[0-9]"))
                            literal += ReadChar();
                        else
                        {
                            switch (ReadChar())
                            {
                                case '+':
                                    copr = CalculationOperator.Plus;
                                    break;
                                case '-':
                                    copr = CalculationOperator.Minus;
                                    break;
                                default:
                                    break;
                            }
                            end = true;
                        }
                        break;
                }

                Next();
            }

            if (string.IsNullOrEmpty(literal))
            {
                operand = GetFilterFromId(filterId);

                if (((IFilter)operand).Type != FilterType.Numerical)
                    throw new QueryException("フィルタ " + filterId + " のフィルタ タイプ " + ((IFilter)operand).Type + " をサーキュレータのオペランドとして使用することはできません。サーキュレータのオペランドとして使用できるのは、 Numerical タイプのフィルタだけです。");

                return new CalculationOperand()
               {
                   Type = CalculationOperandType.Filter,
                   Value = (IFilter)operand,
                   CalcOperator = copr
               };
            }
            else if (string.IsNullOrEmpty(filterId))
                return new CalculationOperand()
                {
                    Type = CalculationOperandType.Literal,
                    Value = literal,
                    CalcOperator = copr
                };

            return null;
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

            while (!findId)
            {
                Next();

                if (cursor > query.Length)
                    throw new QueryException("クエリが不適切です。フィルタ " + filterId + " が終了していません。");

                switch (Tokenize())
                {
                    case TokenType.Space:
                        findId = true;
                        break;
                    case TokenType.CloseBracket:
                        throw new QueryException("フィルタ " + filterId + " に演算子がありません。フィルタの演算子に出会う前に、オブジェクトが終了しました。");
                    default:
                        if (Regex.IsMatch(ReadChar().ToString(), "[a-z A-Z _]"))
                            filterId += ReadChar();
                        else
                            findId = true;
                        break;
                }
            }

            filter = GetFilterFromId(filterId);
            Back();

            ((IFilter)filter).FilterOperator = GetFilterOperator(filterId);
            ((IFilter)filter).Argument = GetFilterArgument(filterId);
            ((IFilter)filter).Operator = GetLogicalOperator();

            return (IFilterObject)filter;
        }

        private static LogicalOperator GetLogicalOperator()
        {
            var opr = new LogicalOperator();

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
                        opr = LogicalOperator.And;
                        findOpr = true;
                        break;
                    case TokenType.ConcatenatorOr:
                        opr = LogicalOperator.Or;
                        findOpr = true;
                        break;
                    case TokenType.ConcatenatorXor:
                        opr = LogicalOperator.Xor;
                        findOpr = true;
                        break;
                    default:
                        Console.WriteLine(ReadChar());
                        break;
                }
            }

            Back();
            return opr;
        }

        private static string GetFilterArgument(string filterId)
        {
            string filterArg = String.Empty;
            int dcCount = 0;
            bool findArg = false;
            while (!findArg)
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
                            throw new QueryException("フィルタ " + filterId + " に引数がありません。フィルタの引数に出会う前に、オブジェクトが終了しました。");
                        else if (dcCount == 1)
                            if (filterArg.Length == 0)
                                throw new QueryException("フィルタ " + filterId + "の引数が不正です。引数はダブルクォーテーション '\"' で始まらなければなりません。");
                            else
                                throw new QueryException("フィルタ " + filterId + "の引数が閉じられていません。引数はダブルクォーテーション '\"' で終わらなければなりません。");
                        else
                            throw new QueryException("フィルタが不正です。");

                    default:
                        if (dcCount == 1)
                            filterArg += ReadChar();
                        break;
                }
            }

            return filterArg;
        }

        private static Operator GetFilterOperator(string filterId)
        {
            string filterSymbol = String.Empty;
            bool findSymbol = false;

            while (!findSymbol)
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
                        if (filterSymbol.Length > 0)
                            throw new QueryException("フィルタ " + filterId + " に引数がありません。フィルタの引数に出会う前に、オブジェクトが終了しました。");
                        else
                            throw new QueryException("フィルタ " + filterId + " に演算子がありません。フィルタの演算子に出会う前に、オブジェクトが終了しました。");
                    default:
                        if (!Regex.IsMatch(ReadChar().ToString(), "[a-z A-Z _]"))
                            filterSymbol += ReadChar();
                        else
                            findSymbol = true;
                        break;
                }
            }

            if (String.IsNullOrEmpty(filterSymbol))
                throw new QueryException("フィルタ " + filterId + " に演算子がありません。");

            Back();
            return Operatornize(filterSymbol);
        }

        public static IFilter GetFilterFromId(string filterId)
        {
            switch (filterId)
            {
                case "text":
                    return new Filters.Text.Text();
                case "favorite_count":
                    return new Filters.Numerical.FavoriteCount();
                default:
                    throw new QueryException("フィルタが不適切です。ID \"" + filterId + "\" に一致するフィルタがありません。");
            }
        }

    }
}
