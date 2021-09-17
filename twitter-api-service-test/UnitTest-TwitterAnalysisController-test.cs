using NUnit.Framework;

using twitter_api_service.Processor.Interfaces;
using twitter_api_service.Processor;
using twitter_api_service.Repository;
using twitter_api_service;

using System;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Configuration;
using twitter_api_service.Utility;
using twitter_api_service.Utility.Interfaces;
using twitter_api_service.Entity;

namespace twitter_api_service_test
{
    public class TestTwitterAnalysisController
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

        [Test(Description="Test API controller")]
        public void TestController()
        {
            AnalysisData respData;

            InMemoryRepository repository = new InMemoryRepository();
            TweetProcessor tweetProcessor;

            MockTweetRetriever tweetRetreiver;

            MockDatetimeUtil timeDiffUtility;            
            
            repository = new InMemoryRepository();
            tweetRetreiver = new MockTweetRetriever();
            timeDiffUtility = new MockDatetimeUtil();         

            tweetProcessor = new TweetProcessor(tweetRetreiver, repository, timeDiffUtility, new NullLogger<TweetProcessor>(), _configuration);

            TwitterAnalysisController apiController = new TwitterAnalysisController(repository, tweetProcessor);

            tweetRetreiver.Tweet("test tweet");
            tweetRetreiver.Tweet("test tweet2");

            // Delay for thread to process the tweet for testing.
            Thread.Sleep(300);

            // Trigger a minute has passed.
            timeDiffUtility.TriggerEvent();   

            respData = apiController.Get().Value;

            Assert.AreEqual(2, respData.AverageTweetsPerMinute);
            Assert.AreEqual(2, respData.TotalTweets);            
        }
    }
}