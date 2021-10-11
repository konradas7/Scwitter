using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scwitter
{
    public class Scwitter
    {
        private string guestToken = string.Empty;
        private DateTime guestTokenCreatedAt = DateTime.Now;
        public Scwitter()
        {
            RefreshGuestToken();
        }

        private void RefreshGuestToken()
        {
            if (guestToken == string.Empty || guestTokenCreatedAt - DateTime.Now > new TimeSpan(3, 0, 0))
            {
                guestToken = Api.GetGuestToken();
                guestTokenCreatedAt = DateTime.Now;
            }
        }

        public IEnumerable<Tweet> SearchForTweets(string query, int count = 25)
        {
            RefreshGuestToken();
            var result = Api.SearchTweets("https://twitter.com/i/api/2/search/adaptive.json", guestToken, query, count);
            return ParseTweets(result);
        }

        public IEnumerable<Tweet> GetUserTweets(long userid, int count = 25)
        {
            RefreshGuestToken();
            var result = Api.UserTweets(userid, $"https://api.twitter.com/2/timeline/profile/{userid}.json", guestToken, count);
            return ParseTweets(result);
        }

        public Tweet GetSingleTweet(long id)
        {
            RefreshGuestToken();
            var result = Api.SingleTweet($"https://twitter.com/i/api/2/timeline/conversation/{id}.json", guestToken);
            return ParseTweets(result).First();
        }

        public IEnumerable<Trend> GetCurrentTrends()
        {
            RefreshGuestToken();
            var result = Api.GetTrends("https://twitter.com/i/api/2/guide.json", guestToken);
            return ParseTrends(result);
        }

        public Profile GetUserProfile(string username)
        {
            RefreshGuestToken();
            var result = Api.GetUserProfile($"https://api.twitter.com/graphql/4S2ihIKfF3xhp-ENxvUAfQ/UserByScreenName?variables=%7B%22screen_name%22%3A%22{username}%22%2C%22withHighlightedLabel%22%3Atrue%7D", guestToken);
            return new Profile(result);
        }

        #region Response parse methods
        private IEnumerable<Tweet> ParseTweets(JsonObject result)
        {
            if(result.TryGetValue("globalObjects", out object globalObj))
            {
                if(((JsonObject)globalObj).TryGetValue("tweets", out object tweetsObj))
                {
                    var tweets = (JsonObject)tweetsObj;
                    foreach(var tweetKey in tweets.Keys)
                    {
                        if (tweets.TryGetValue(tweetKey, out object tweet))
                        {
                            yield return new Tweet((JsonObject)tweet);
                        }
                    }
                }
            }
        }

        private IEnumerable<Trend> ParseTrends(JsonObject result)
        {
            if(!result.TryGetValue("timeline", out object timeline))
            {
                throw new Exception("Incorrect trend response body");
            }
            if(!((JsonObject)timeline).TryGetValue("instructions", out object instructionArr))
            {
                throw new Exception("Incorrect trend response body");
            }
            var entryInstruction = ((JsonArray)instructionArr).ElementAt(1);
            if (!((JsonObject)entryInstruction).TryGetValue("addEntries", out object addEntries))
            {
                throw new Exception("Incorrect trend response body");
            }
            if (!((JsonObject)addEntries).TryGetValue("entries", out object entries))
            {
                throw new Exception("Incorrect trend response body");
            }
            var entryArray = (JsonArray)entries;
            var trends = entryArray.Where(obj => IsTrendEntry(obj)).First();
            if (!((JsonObject)trends).TryGetValue("content", out object content))
            {
                throw new Exception("Incorrect trend response body");
            }
            if (!((JsonObject)content).TryGetValue("timelineModule", out object tlModule))
            {
                throw new Exception("Incorrect trend response body");
            }
            if (!((JsonObject)tlModule).TryGetValue("items", out object items))
            {
                throw new Exception("Incorrect trend response body");
            }

            foreach (var contentEntry in (JsonArray)items)
            {
                if (((JsonObject)contentEntry).TryGetValue("item", out object item))
                {
                    yield return new Trend((JsonObject)item);
                }
            }
        }

        private bool IsTrendEntry(object obj)
        {
            if (((JsonObject)obj).TryGetValue("entryId", out object id))
            {
                if ((string)id == "trends")
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
