using System;
using System.Collections.Generic;
using System.Text;
using RestSharp;

namespace Scwitter
{   
    internal static class Api
    {
        private static readonly RestClient client = new RestClient();
        private const string bearerToken = "AAAAAAAAAAAAAAAAAAAAANRILgAAAAAAnNwIzUejRCOuH5E6I8xnZz4puTs%3D1Zv7ttfk8LF81IUq16cHjhLTvJu4FA33AGWWjCpTnA";

        internal static string GetGuestToken()
        {
            var request = new RestRequest("https://api.twitter.com/1.1/guest/activate.json");
            request.AddHeader("Authorization", "Bearer " + bearerToken);
            var response = client.Post(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Could not get guest token. HTTP Error: " + response.StatusCode + "; " + response.ErrorMessage);
            }
            var responseBody = (JsonObject) SimpleJson.DeserializeObject(response.Content);
            if(responseBody.TryGetValue("guest_token", out object token))
            {
                return (string)token;
            }
            throw new Exception("Could not get guest token from response body: response contains no guest_token key.");
        }

        internal static JsonObject UserTweets(long userId, string url, string guestToken, int count)
        {
            var request = RequestWithStandartParameters(url, guestToken);
            request.AddParameter("count", count.ToString());
            request.AddParameter("userId", userId);
            var response = client.Get(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Could not get user tweets. HTTP Error: " + response.StatusCode + "; " + response.ErrorMessage);
            }
            return SimpleJson.DeserializeObject<JsonObject>(response.Content);
        }

        internal static JsonObject GetTrends(string url, string guestToken)
        {
            var request = RequestWithStandartParameters(url, guestToken);
            request.AddParameter("count", "20");
            request.AddParameter("candidate_source", "trends");
            request.AddParameter("include_page_configuration", "false");
            request.AddParameter("entity_tokens", "false");
            var response = client.Get(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Could not get trends by id. HTTP Error: " + response.StatusCode + "; " + response.ErrorMessage);
            }
            return SimpleJson.DeserializeObject<JsonObject>(response.Content);
        }

        internal static JsonObject GetUserProfile(string url, string guestToken)
        {
            var request = RequestWithStandartParameters(url, guestToken);
            var response = client.Get(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Could not get trends by id. HTTP Error: " + response.StatusCode + "; " + response.ErrorMessage);
            }
            return SimpleJson.DeserializeObject<JsonObject>(response.Content);
        }

        internal static JsonObject SingleTweet(string url, string guestToken)
        {
            var request = RequestWithStandartParameters(url, guestToken);
            var response = client.Get(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Could not get tweet by id. HTTP Error: " + response.StatusCode + "; " + response.ErrorMessage);
            }
            return SimpleJson.DeserializeObject<JsonObject>(response.Content);
        }

        internal static JsonObject SearchTweets(string url, string guestToken, string query, int count)
        {
            var request = RequestWithStandartParameters(url, guestToken);
            request.AddParameter("q", query);
            request.AddParameter("count", count.ToString());
            request.AddParameter("query_source", "typed_query");
            request.AddParameter("pc", "1");
            request.AddParameter("spelling_corrections", "1");
            var response = client.Get(request);
            if (!response.IsSuccessful)
            {
                throw new Exception("Could not search for tweets. HTTP Error: " + response.StatusCode + "; " + response.ErrorMessage);
            }
            return SimpleJson.DeserializeObject<JsonObject>(response.Content);
        }

        private static RestRequest RequestWithStandartParameters(string url, string guestToken)
        {
            var request = new RestRequest(url);
            request.AddHeader("Authorization", "Bearer " + bearerToken);
            request.AddHeader("X-Guest-Token", guestToken);
            request.AddQueryParameter("include_profile_interstitial_type", "1");
            request.AddQueryParameter("include_blocking", "1");
            request.AddQueryParameter("include_blocked_by", "1");
            request.AddQueryParameter("include_followed_by", "1");
            request.AddQueryParameter("include_want_retweets", "1");
            request.AddQueryParameter("include_mute_edge", "1");
            request.AddQueryParameter("include_can_dm", "1");
            request.AddQueryParameter("include_can_media_tag", "1");
            request.AddQueryParameter("skip_status", "1");
            request.AddQueryParameter("include_cards", "1");
            request.AddQueryParameter("include_ext_alt_text", "true");
            request.AddQueryParameter("include_quote_count", "true");
            request.AddQueryParameter("include_reply_count", "1");
            request.AddQueryParameter("include_entities", "true");
            request.AddQueryParameter("include_user_entities", "true");
            request.AddQueryParameter("include_ext_media_color", "true");
            request.AddQueryParameter("include_ext_media_availability", "true");
            request.AddQueryParameter("send_error_codes", "true");
            request.AddQueryParameter("simple_quoted_tweet", "true");
            request.AddQueryParameter("simple_quoted_tweet", "true");
            request.AddQueryParameter("include_tweet_replies", "true");
            request.AddQueryParameter("ext", "mediaStats,highlightedLabel");
            return request;
        }
    }
}
