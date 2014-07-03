namespace Twitch.Filter
{
    public enum TokenType
    {
        /// <summary>
        /// ピリオド
        /// </summary>
        Period,

        /// <summary>
        /// カンマ
        /// </summary>
        Comma,

        /// <summary>
        /// コロン記号
        /// </summary>
        Collon,

        /// <summary>
        /// エクスクラメーション マーク
        /// </summary>
        Exclamation,

        /// <summary>
        /// リテラル(べた書き文字列,数値など)
        /// </summary>
        Literal,

        /// <summary>
        /// &amp;記号
        /// </summary>
        ConcatenatorAnd,

        /// <summary>
        /// |記号
        /// </summary>
        ConcatenatorOr,

        /// <summary>
        /// ^記号
        /// </summary>
        ConcatenatorXor,

        /// <summary>
        /// (記号
        /// </summary>
        OpenBracket,

        /// <summary>
        /// )記号
        /// </summary>
        CloseBracket,

        /// <summary>
        /// 文字列
        /// </summary>
        String,

        /// <summary>
        /// 空白
        /// </summary>
        Space,

        /// <summary>
        /// タブ
        /// </summary>
        Tab,

        /// <summary>
        /// キャリッジリターン
        /// </summary>
        CarriageReturn,

        /// <summary>
        /// ラインフィード
        /// </summary>
        LineFeed,

        /// <summary>
        /// 不明(その他)
        /// </summary>
        Unknown
    }
}
