using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Streaming;
using twitter_api_service.Processor.Interfaces;

namespace twitter_api_service.Processor
{
    public class TweetRetriever : ITweetRetriever
    {
        ITwitterClient _twitterClient;

        ISampleStream _sampleStream;

        ILogger _logger;
        
        public event EventHandler<TwitterEventArgs> OnTweet;

        public TweetRetriever(ITwitterClient twitterClient, ILogger<TweetRetriever> logger)
        {
            _logger = logger;

            _twitterClient = twitterClient;

            ConnectToTwitter();            
        }

        private void ConnectToTwitter()
        {
            try
            {
                _sampleStream = _twitterClient.Streams.CreateSampleStream();
                _sampleStream.TweetReceived += OnReceiveTweet;

                _sampleStream.StartAsync();
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
            }
        }

        private void OnReceiveTweet(object sender, TweetReceivedEventArgs args)
        {
            string tweet;

            tweet = args.Tweet.Text;
            _logger.LogInformation($"Received Tweet: {tweet}");

            OnTweet?.Invoke(this, new TwitterEventArgs{Tweet = tweet});
        }
    }
}