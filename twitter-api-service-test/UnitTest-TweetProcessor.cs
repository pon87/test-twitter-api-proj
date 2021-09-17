using NUnit.Framework;

using twitter_api_service.Processor.Interfaces;
using twitter_api_service.Processor;
using twitter_api_service.Repository;

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using twitter_api_service.Utility;
using twitter_api_service.Utility.Interfaces;
using twitter_api_service.Entity;

namespace twitter_api_service_test
{
    class MockTweetRetriever : ITweetRetriever
    {
        public event EventHandler<TwitterEventArgs> OnTweet;

        public void Tweet(string tweet)
        {
            OnTweet?.Invoke(this, new TwitterEventArgs{Tweet = "tweet"});
        }
    }

    class MockDatetimeUtil : IDateTimeDiff
    {
        public event EventHandler OnMinutePassed;

        public void TriggerEvent()
        {
            OnMinutePassed?.Invoke(this, new EventArgs());
        }
    }

    public class TestProcessor
    {
        Dictionary<string, string> _inMemoryConfig;

        IConfiguration _configuration;

        [SetUp]
        public void Setup()
        {
            _inMemoryConfig = new Dictionary<string, string>{
                {"ServiceSettings:TotalProcessingThreads", "1"}
            };

            _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(_inMemoryConfig)
            .Build();
        }

        [Test(Description="Testing Tweet Processor: TotalTweet")]
        public void TestTotalTweet()
        {
            InMemoryRepository repository;

            TweetProcessor tweetProcessor;

            MockTweetRetriever tweetRetreiver;

            DateTimeDiffUtility timeDiffUtility;            

            int totalTweet;

            repository = new InMemoryRepository();
            tweetRetreiver = new MockTweetRetriever();
            timeDiffUtility = new DateTimeDiffUtility();            

            tweetProcessor = new TweetProcessor(tweetRetreiver, repository, timeDiffUtility, new NullLogger<TweetProcessor>(), _configuration);

            tweetRetreiver.Tweet("this tweet");

            // Very little delay time for processor to spawn up thread and start processing..
            Thread.Sleep(10);

            totalTweet = tweetProcessor.GetTotalTweets();

            Assert.AreEqual(totalTweet, 1);
        }

        [Test(Description="Testing Tweet Processor: AverageTweet")]
        public void TestAverageTweet()
        {
            InMemoryRepository repository;

            TweetProcessor tweetProcessor;

            MockDatetimeUtil timeDiffUtil;

            MockTweetRetriever tweetRetreiver;

            float averageTweet;

            repository = new InMemoryRepository();
            tweetRetreiver = new MockTweetRetriever();
            timeDiffUtil = new MockDatetimeUtil();

            tweetProcessor = new TweetProcessor(tweetRetreiver, repository, timeDiffUtil, new NullLogger<TweetProcessor>(), _configuration);

            tweetRetreiver.Tweet("this tweet");
            tweetRetreiver.Tweet("this tweet2");

            // Very little delay time for processor to spawn up thread and start processing..
            Thread.Sleep(10);

            // Trigger a minute has passed.
            timeDiffUtil.TriggerEvent();            

            averageTweet = tweetProcessor.GetAverageTweetsInMinutes();
            Assert.AreEqual(averageTweet, 2f);
        }
    }
}