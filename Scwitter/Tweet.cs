using RestSharp;
using System;
using System.Collections.Generic;

namespace Scwitter
{
    public class Tweet
    {
        private const string _twitterDateTimeFormat = "ddd MMM dd HH:mm:ss +ffff yyyy";
        public DateTime CreatedAt { get; private set; }
        public long Id { get; private set; }
        public string Text { get; private set; }
        public bool IsTruncated { get; private set; }
        public List<string> Hashtags { get; private set; }
        public long UserId { get; private set; }
        public long Retweets { get; private set; }
        public long Favourites { get; private set; }
        public long Replies { get; private set; }
        public long Quotes { get; private set; }

        internal Tweet(JsonObject json)
        {
            if (json.TryGetValue("created_at", out object created))
            {
                CreatedAt = DateTime.ParseExact((string)created, _twitterDateTimeFormat, new System.Globalization.CultureInfo("en-US"));
            }
            if (json.TryGetValue("id_str", out object id))
            {
                Id = long.Parse((string)id);
            }
            if (json.TryGetValue("text", out object text))
            {
                Text = (string)text;
            }
            if (json.TryGetValue("truncated", out object trunc))
            {
                IsTruncated = (bool)trunc;
            }
            if (json.TryGetValue("entities", out object entities) &&
                ((JsonObject)entities).TryGetValue("hashtags", out object hashtagEntities))
            {
                Hashtags = new List<string>();
                foreach(var hashtag in (JsonArray)hashtagEntities)
                {
                    if (((JsonObject)hashtag).TryGetValue("text", out object hText))
                    {
                        Hashtags.Add((string)hText);
                    }
                }               
            }
            if (json.TryGetValue("user_id_str", out object userid))
            {
                UserId = long.Parse((string)userid);
            }
            if (json.TryGetValue("retweet_count", out object retweets))
            {
                Retweets = (long)retweets;
            }
            if (json.TryGetValue("favorite_count", out object favourites))
            {
                Favourites = (long)favourites;
            }
            if (json.TryGetValue("reply_count", out object replies))
            {
                Replies = (long)replies;
            }
            if (json.TryGetValue("quote_count", out object quotes))
            {
                Quotes = (long)quotes;
            }
        }
    }
}
