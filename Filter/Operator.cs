namespace Twitch.Filter
{
    public enum Operator
    {
        /// <summary>
        /// :<para />
        /// 含む
        /// </summary>
        Include,

        /// <summary>
        /// .<para />
        /// 大文字小文字問わず含む
        /// </summary>
        IncludeTolerance,

        /// <summary>
        /// ::<para />
        /// 正規表現
        /// </summary>
        Regex,

        /// <summary>
        /// ==<para />
        /// 等しい
        /// </summary>
        Equal,

        /// <summary>
        /// !=<para />
        /// 等しくない
        /// </summary>
        Unequal,

        /// <summary>
        /// &gt;<para />
        /// 大きい
        /// </summary>
        GreaterThan,

        /// <summary>
        /// &lt;<para />
        /// 小さい
        /// </summary>
        LessThan,

        /// <summary>
        /// &gt;=<para />
        /// 大きいか等しい
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// &lt;=<para />
        /// 小さいか等しい
        /// </summary>
        LessThanOrEqual
    }
}
