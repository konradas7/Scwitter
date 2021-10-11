# Scwitter
A simple twitter scraper for .NET Core 5.0. Implements the Twitter front-end API, so no rate limits and no tokens required.

# Dependencies
• RestSharp

# Usage

Create an instance of the scraper:
    
    var scwitter = new Scwitter.Scwitter();
    
Search for tweets:

    var tweets = scwitter.SearchForTweets(query);
    
Get a single tweet from ID:

    var tweet = scwitter.GetSingleTweet(tweetId);
    
        
Get latest user tweets:

    var tweets = scwitter.GetUserTweets(userId);
    
Get user profile from display name:

    var profile = scwitter.GetUserProfile(displayName);
    
Get trends:

    var trends = scwitter.GetCurrentTrends();
