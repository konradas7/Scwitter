using RestSharp;
using System;

namespace Scwitter
{
    public class Profile
    {
        private const string _twitterDateTimeFormat = "ddd MMM dd HH:mm:ss +ffff yyyy";
        public long Id { get; private set; }
        public string Username { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public long Followers { get; private set; }
        public long Friends { get; private set; }
        public string Location { get; private set; }
        public DateTime CreatedAt { get; private set; }

        internal Profile(JsonObject response)
        {
            if(!response.TryGetValue("data", out object data))
            {
                throw new Exception("No data in profile response body");
            }
            if (!((JsonObject)data).TryGetValue("user", out object user))
            {
                throw new Exception("No data in profile response body");
            }
            if (((JsonObject)user).TryGetValue("rest_id", out object id))
            {
                Id = long.Parse((string)id);
            }
            if (((JsonObject)user).TryGetValue("legacy", out object legacy))
            {
                var legacyNode = (JsonObject)legacy;
                if (legacyNode.TryGetValue("created_at", out object created))
                {
                    CreatedAt = DateTime.ParseExact((string)created, _twitterDateTimeFormat, new System.Globalization.CultureInfo("en-US"));
                }
                if (legacyNode.TryGetValue("description", out object description))
                {
                    Description = (string)description;
                }
                if (legacyNode.TryGetValue("followers_count", out object followers))
                {
                    Followers = (long)followers;
                }
                if (legacyNode.TryGetValue("friends_count", out object friends))
                {
                    Friends = (long)friends;
                }
                if (legacyNode.TryGetValue("location", out object location))
                {
                    Location = (string)location;
                }
                if (legacyNode.TryGetValue("name", out object name))
                {
                    Name = (string)name;
                }
                if (legacyNode.TryGetValue("screen_name", out object username))
                {
                    Username = (string)username;
                }
            }
        }
    }
}