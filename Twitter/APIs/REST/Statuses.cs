using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twitch.HTTP.Twitter;

namespace Twitch.Twitter.APIs.REST
{
    /// <summary>
    /// Status(ツイート)に関するAPI
    /// </summary>
    public static class Statuses
    {
        /// <summary>
        /// ホーム タイムラインを取得します。
        /// </summary>
        /// <param name="twitterContext">タイムラインを取得するユーザー。</param>
        /// <param name="count">取得するツイートの数。200以下でなければなりません。</param>
        /// <param name="since_id">指定したIDより大きなIDを持つ結果のみを返します(つまり、より新しい)。APIを介してアクセスできるツイートの数には制限があります。ツイートの制限がsince_id以来発生している場合は、since_idは、利用可能な最も古いIDに強制されます。</param>
        /// <param name="max_id">指定されたIDと同じか、それより古いIDを持つ結果のみを返します(つまり、より古い)。</param>
        /// <param name="trim_user">trueに設定すると、タイムライン上で返される各ツイートはステータスのみ作者数値IDを含むユーザーオブジェクトが含まれます。完全なユーザーオブジェクトを受け取るためには、このパラメータを省略します。</param>
        /// <param name="exclude_replies">trueに設定すると、取得するタイムラインからリツイートやリプライは除外されます。</param>
        /// <param name="contributor_details"></param>
        /// <param name="include_entities"></param>
        /// <returns>ツイートのList</returns>
        public static async Task<List<Status>> HomeTimeline(TwitterContext twitterContext,
            double count = 0,
            string since_id = null,
            string max_id = null,
            bool trim_user = false,
            bool exclude_replies = false,
            bool contributor_details = false,
            bool include_entities = true)
        {
            StringDictionary query = new StringDictionary();
            query["count"] = count.ToString();
            query["since_id"] = since_id;
            query["max_id"] = max_id;
            query["trim_user"] = trim_user.ToString();
            query["exclude_replies"] = exclude_replies.ToString();
            query["contributor_details"] = contributor_details.ToString();
            query["include_entities"] = include_entities.ToString();

            string source = await new TwitterRequest(
                twitterContext, API.Methods.GET, new Uri(API.Urls.Statuses_HomeTimeline), query).Request();

            dynamic json = Utility.DynamicJson.Parse(source);

            var statuses = new List<Status>();
            foreach (dynamic status in json)
            {
                statuses.Add(new Status(status.ToString()));
            }
            return statuses;
        }

        /// <summary>
        /// 指定したユーザーのタイムラインを取得します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="user_id"></param>
        /// <param name="screen_name"></param>
        /// <param name="count">取得するツイートの数。200以下でなければなりません。</param>
        /// <param name="since_id">指定したIDより大きなIDを持つ結果のみを返します(つまり、より新しい)。APIを介してアクセスできるツイートの数には制限があります。ツイートの制限がsince_id以来発生している場合は、since_idは、利用可能な最も古いIDに強制されます。</param>
        /// <param name="max_id">指定されたIDと同じか、それより古いIDを持つ結果のみを返します(つまり、より古い)。</param>
        /// <param name="trim_user">trueに設定すると、タイムライン上で返される各ツイートはステータスのみ作者数値IDを含むユーザーオブジェクトが含まれます。完全なユーザーオブジェクトを受け取るためには、このパラメータを省略します。</param>
        /// <param name="exclude_replies">trueに設定すると、取得するタイムラインからリツイートやリプライは除外されます。</param>
        /// <param name="contributor_details"></param>
        /// <param name="include_rts"></param>
        /// <returns></returns>
        public static async Task<List<Status>> UserTimeline(TwitterContext twitterContext,
            string user_id = null,
            string screen_name = null,
            double count = 0,
            string since_id = null,
            string max_id = null,
            bool trim_user = false,
            bool exclude_replies = false,
            bool contributor_details = false,
            bool include_rts = false)
        {
            StringDictionary query = new StringDictionary();
            if (user_id != string.Empty)
                query["user_id"] = user_id;
            else if (screen_name != string.Empty)
                query["screen_name"] = screen_name;
            query["count"] = count.ToString();
            query["since_id"] = since_id;
            query["max_id"] = max_id;
            query["trim_user"] = trim_user.ToString();
            query["exclude_replies"] = exclude_replies.ToString();
            query["contributor_details"] = contributor_details.ToString();
            query["include_rts"] = include_rts.ToString();

            string source = await new TwitterRequest(
                twitterContext, API.Methods.GET, new Uri(API.Urls.Statuses_UserTimeline), query).Request();

            dynamic json = Utility.DynamicJson.Parse(source);

            var statuses = new List<Status>();
            foreach (dynamic status in json)
            {
                statuses.Add(new Status(status.ToString()));
            }
            return statuses;
        }


        /// <summary>
        /// メンション タイムラインを取得します。
        /// </summary>
        /// <param name="twitterContext">タイムラインを取得するユーザー。</param>
        /// <param name="count">取得するツイートの数。200以下でなければなりません。</param>
        /// <param name="since_id">指定したIDより大きなIDを持つ結果のみを返します(つまり、より新しい)。APIを介してアクセスできるツイートの数には制限があります。ツイートの制限がsince_id以来発生している場合は、since_idは、利用可能な最も古いIDに強制されます。</param>
        /// <param name="max_id">指定されたIDと同じか、それより古いIDを持つ結果のみを返します(つまり、より古い)。</param>
        /// <param name="trim_user">trueに設定すると、タイムライン上で返される各ツイートはステータスのみ作者数値IDを含むユーザーオブジェクトが含まれます。完全なユーザーオブジェクトを受け取るためには、このパラメータを省略します。</param>
        /// <param name="contributor_details"></param>
        /// <param name="include_entities"></param>
        /// <returns>ツイートのList</returns>
        public static async Task<List<Status>> MentionsTimeline(TwitterContext twitterContext,
            double count = 0,
            string since_id = null,
            string max_id = null,
            bool trim_user = false,
            bool contributor_details = true,
            bool include_entities = true)
        {
            StringDictionary query = new StringDictionary();
            query["count"] = count.ToString();
            query["since_id"] = since_id;
            query["max_id"] = max_id;
            query["trim_user"] = trim_user.ToString();
            query["contributor_details"] = contributor_details.ToString();
            query["include_entities"] = include_entities.ToString();

            string source = await new TwitterRequest(
                twitterContext, API.Methods.GET, new Uri(API.Urls.Statuses_MentionsTimeline), query).Request();

            dynamic json = Utility.DynamicJson.Parse(source);

            var statuses = new List<Status>();
            foreach (dynamic status in json)
            {
                statuses.Add(new Status(status.ToString()));
            }
            return statuses;
        }

        /// <summary>
        /// 新しいツイートを投稿します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="status">本文。</param>
        /// <param name="in_reply_to_status_id">返信元になるツイートのID。</param>
        /// <param name="media"></param>
        /// <returns></returns>
        public static async Task<string> Update(TwitterContext twitterContext,
            string status,
            string in_reply_to_status_id = null)
        {
            StringDictionary query = new StringDictionary();
            query["status"] = status;
            query["in_reply_to_status_id"] = in_reply_to_status_id;

            return await new TwitterRequest(
                twitterContext, API.Methods.POST, new Uri(API.Urls.Statuses_Update), query).Request();
        }

        /// <summary>
        /// 対象のツイートをリツイートします。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="id">リツイートするツイートのID。</param>
        /// <returns></returns>
        public static async Task<string> Retweet(TwitterContext twitterContext,
            string id)
        {
            StringDictionary query = new StringDictionary();
            query["id"] = id;

            return await new TwitterRequest(twitterContext, API.Methods.POST, new Uri(API.Urls.Statuses_Retweet + id + ".json"), query).Request();
        }

        /// <summary>
        /// ツイートを取得します。
        /// </summary>
        /// <param name="twitterContext">自分。</param>
        /// <param name="id">取得するツイートのID。</param>
        /// <returns></returns>
        public static async Task<Twitter.Status> Show(TwitterContext twitterContext,
            string id)
        {
            StringDictionary query = new StringDictionary();
            query["id"] = id;

            return new Status(await new TwitterRequest(twitterContext, API.Methods.GET, new Uri(API.Urls.Statuses_Show), query).Request());
        }
    }
}
