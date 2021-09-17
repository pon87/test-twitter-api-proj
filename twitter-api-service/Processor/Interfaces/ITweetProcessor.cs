namespace twitter_api_service.Processor.Interfaces
{
    public interface ITweetProcessor
    {
        int GetTotalTweets();
        float GetAverageTweetsInMinutes();
    }
}