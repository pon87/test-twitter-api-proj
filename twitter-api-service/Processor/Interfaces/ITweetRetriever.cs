using System;

namespace twitter_api_service.Processor.Interfaces
{
    public interface ITweetRetriever
    {
        event EventHandler<TwitterEventArgs> OnTweet;
    }
}