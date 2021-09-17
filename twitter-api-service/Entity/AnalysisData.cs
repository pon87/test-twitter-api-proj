using System;

namespace twitter_api_service.Entity
{
    public struct AnalysisData
    {
        public float AverageTweetsPerMinute {get; set;}
        public int TotalTweets {get; set;}
    }
}