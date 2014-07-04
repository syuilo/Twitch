using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Twitch.Filter
{
    public class Filterize
    {
        private int pos = 0;

        /// <summary>
        /// 対象の Twitter.Status 。
        /// </summary>
        public Twitter.Status Input
        {
            get;
            private set;
        }

        /// <summary>
        /// ツイートに対するクエリ文字列。
        /// </summary>
        public string Query
        {
            get;
            set;
        }

        public Hashtable Filters
        {
            get;
            set;
        }

        public Filterize(Twitter.Status input, string query)
        {
            this.Input = input;
            this.Query = query;

            this.Filters = new Hashtable();
            //this.Filters["text"] = new Filters.Text.Text(this.Input);
            //this.Filters["name"] = new Filters.Text.Name(this.Input);
            //this.Filters["screen_name"] = new Filters.Text.ScreenName(this.Input);
            //this.Filters["favorite_count"] = new Filters.Numerical.FavoriteCount(this.Input);

        }

        /// <summary>
        /// クエリを検証します。
        /// </summary>
        /// <returns>結果</returns>
        public bool Analyze()
        {
            this.pos = 0;

            for (int i = 0; pos < this.Query.Length; i++, pos++)
            {
                //System.Diagnostics.Debug.WriteLine(this.Query[pos]);

                switch (this.Query[pos])
                {
                    case ' ':
                        break;
                    case '{':
                        return AnalyzeObject();
                    default:
                        throw new QueryException("クエリが不適切です。クエリは必ず { で始まっている必要があります。");
                }
            }

            throw new QueryException("1つ以上のフィルターを検証しませんでした。");
        }

        private bool AnalyzeObject()
        {
            var results = new List<bool>();
            var logicalOperators = new List<char>();

            bool endPoint = false;

            while (!endPoint)
            {
                pos++;

                if (pos >= this.Query.Length)
                    throw new QueryException("クエリが不適切です。オブジェクトが終了していません。");

                System.Diagnostics.Debug.Write(this.Query[pos]);

                switch (this.Query[pos])
                {
                    // 無視する文字
                    case ' ':
                        break;
                    case '\t':
                        break;
                    case '\r':
                        break;
                    case '\n':
                        break;

                    case '{':
                        results.Add(AnalyzeObject());
                        break;
                    case '}':
                        endPoint = true;
                        break;
                    case ':':
                        break;

                    case '!': // not
                        logicalOperators.Add('!');
                        break;
                    case '&': // and
                        logicalOperators.Add('&');
                        break;
                    case '|': // or
                        logicalOperators.Add('|');
                        break;
                    case '^': // xor
                        logicalOperators.Add('^');
                        break;

                    default:
                        results.Add(AnalyzeFilter());
                        break;
                }
            }

            if (results.Count == 0)
                return true;

            bool result = results[0];
            if (results.Count > 1)
            {
                for (int i = 1, j = 0; i < results.Count; i++, j++)
                {
                    bool a = result, b = results[i];

                    switch (logicalOperators[j])
                    {
                        //case '!':
                        //    result = (!result);
                        //    break;
                        case '&': // and
                            result = (a && b);
                            break;
                        case '|': // or
                            result = (a || b);
                            break;
                        case '^': // xor
                            result = (a ^ b);
                            break;
                    }
                }
            }

            return result;
        }

        private bool AnalyzeFilter()
        {
            string filterId = String.Empty, filterSymbol = String.Empty, filterArg = String.Empty;

            bool findId = false;
            System.Diagnostics.Debug.WriteLine("# フィルタIdを走査します。");

            while (!findId)  // フィルタIDを走査
            {
                pos++;

                if (pos > this.Query.Length)
                    throw new QueryException("クエリが不適切です。フィルタ " + filterId + " が終了していません。");

                switch (this.Query[pos - 1])
                {
                    case ' ':
                        findId = true;
                        break;
                    case ':':
                        findId = true;
                        break;
                    case '}':
                        throw new QueryException("フィルタ " + filterId + " に引数がありません。フィルタの引数に出会う前に、オブジェクトが終了しました。");
                    default:
                        if (Regex.IsMatch(this.Query[pos - 1].ToString(), "[a-z A-Z _]"))
                            filterId += this.Query[pos - 1];
                        else
                            findId = true;
                        break;
                }
            }

            System.Diagnostics.Debug.WriteLine("# フィルタIdは " + filterId + " です。演算子の走査を開始します。");
            pos--;

            bool findSymbol = false;

            while (!findSymbol)  // フィルタシンボルを走査
            {
                pos++;

                if (pos >= this.Query.Length)
                    throw new QueryException("クエリが不適切です。フィルタ " + filterId + " が終了していません。");

                switch (this.Query[pos - 1])
                {
                    case ' ':
                        if (filterSymbol.Length > 0)
                            findSymbol = true;
                        break;
                    case '"':
                        findSymbol = true;
                        break;
                    case '}':
                        throw new QueryException("フィルタ " + filterId + " に演算子がありません。フィルタの演算子に出会う前に、オブジェクトが終了しました。");
                    default:
                        filterSymbol += this.Query[pos - 1];
                        break;
                }
            }

            if (String.IsNullOrEmpty(filterSymbol))
                throw new QueryException("フィルタ " + filterId + " に演算子がありません。");

            System.Diagnostics.Debug.WriteLine("# フィルタシンボルは " + filterSymbol + " です。引数の走査を開始します。");
            pos--;
            pos--;

            int dcCount = 0;
            bool findArg = false;
            while (!findArg) // フィルタの引数を走査
            {
                pos++;

                switch (this.Query[pos])
                {
                    case ' ':
                        break;

                    case '\\':  // エスケープ文字
                        pos++;
                        switch (this.Query[pos])
                        {
                            case 'n':
                                filterArg += "\n";
                                break;
                            case 'r':
                                filterArg += "\r";
                                break;
                            default:
                                filterArg += this.Query[pos];
                                break;
                        }
                        break;
                    case '"':
                        dcCount++;
                        if (dcCount == 2)
                            findArg = true;
                        break;
                    case '}':
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
                            filterArg += this.Query[pos];
                        break;
                }
            }

            System.Diagnostics.Debug.WriteLine("# フィルタ " + filterId + " の引数は \"" + filterArg + "\" です。検証を開始します。");

            //// Filterize
            //if (this.Filters.ContainsKey(filterId))
            //    return ((IFilter)this.Filters[filterId]).Match(Input, filterArg, filterSymbol);
            //else
            return false;
            //    throw new QueryException("フィルタが不適切です。ID \"" + filterId + "\" に一致するフィルタがありません。");

            //switch (filterId) // フィルタに通す(作成中)
            //{
            //    case "text":
            //        return new Filters.Text.Text(this.Input).Verify(filterArg, filterSymbol);
            //    case "screen_name":
            //        return new Filters.Text.ScreenName(this.Input).Verify(filterArg, filterSymbol);
            //    case "name":
            //        return new Filters.Text.Name(this.Input).Verify(filterArg, filterSymbol);
            //    case "favorite_count":
            //        return new Filters.Numerical.FavoriteCount(this.Input).Verify(filterArg, filterSymbol);
            //    default:
            //        throw new QueryException("フィルタが不適切です。ID \"" + filterId + "\" に一致するフィルタがありません。");
            //}
        }
    }
}
