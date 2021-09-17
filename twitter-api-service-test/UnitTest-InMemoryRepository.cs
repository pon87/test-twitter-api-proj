using NUnit.Framework;
using System.Collections.Generic;

using twitter_api_service.Processor.Interfaces;
using twitter_api_service.Processor;
using twitter_api_service.Repository;

using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using twitter_api_service.Utility;
using twitter_api_service.Utility.Interfaces;
using twitter_api_service.Entity;

namespace twitter_api_service_test
{
    public class TestInMemoryRepository
    {
        [Test]
        public void TestSaveTotalTweets()
        {
            InMemoryRepository repository = new InMemoryRepository();

            int expectTotalTweets = 30;
            int returnTotalTweets;            

            repository.SaveTotalTweets(expectTotalTweets);
            returnTotalTweets = repository.GetTotalTweets();

            Assert.AreEqual(expectTotalTweets, returnTotalTweets);
        }

        [Test]
        public void TestSaveTweetsPerMinute()
        {
            InMemoryRepository repository = new InMemoryRepository();

            List<int> totalTweetsPerMinuteList;

            repository.SaveTotalTweetsInMinute(90);
            totalTweetsPerMinuteList = repository.GetListOfTotalTweetsInMinutes();

            Assert.AreEqual(1, totalTweetsPerMinuteList.Count);
        }
    }
}