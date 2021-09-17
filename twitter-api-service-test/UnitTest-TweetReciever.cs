using NUnit.Framework;

using twitter_api_service.Processor.Interfaces;
using twitter_api_service.Processor;
using twitter_api_service.Repository;

using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Tweetinvi;
using Tweetinvi.Events;
using Tweetinvi.Streaming;

using twitter_api_service.Utility;
using twitter_api_service.Utility.Interfaces;
using twitter_api_service.Entity;
using Tweetinvi.Client;
using Tweetinvi.Client.V2;
using Tweetinvi.Models;
using Tweetinvi.Core.Events;
using Tweetinvi.Client.Tools;
using Tweetinvi.Core.Client.Validators;
using Tweetinvi.Core.Client;
using Tweetinvi.Parameters;
using Tweetinvi.Core.Streaming;
using Tweetinvi.Streaming.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tweetinvi.Models.DTO;
using Tweetinvi.Models.Entities;

namespace twitter_api_service_test
{
    class MockTweet : ITweet
    {
        static string _tweet;

        public MockTweet(string tweet)
        {
            _tweet = tweet;
        }

        public ITwitterClient Client { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DateTimeOffset CreatedAt => throw new NotImplementedException();

        public string Text => _tweet;

        public string Prefix => throw new NotImplementedException();

        public string Suffix => throw new NotImplementedException();

        public string FullText => throw new NotImplementedException();

        public int[] DisplayTextRange => throw new NotImplementedException();

        public int[] SafeDisplayTextRange => throw new NotImplementedException();

        public IExtendedTweet ExtendedTweet => throw new NotImplementedException();

        public ICoordinates Coordinates => throw new NotImplementedException();

        public string Source => throw new NotImplementedException();

        public bool Truncated => throw new NotImplementedException();

        public int? ReplyCount => throw new NotImplementedException();

        public long? InReplyToStatusId => throw new NotImplementedException();

        public string InReplyToStatusIdStr => throw new NotImplementedException();

        public long? InReplyToUserId => throw new NotImplementedException();

        public string InReplyToUserIdStr => throw new NotImplementedException();

        public string InReplyToScreenName => throw new NotImplementedException();

        public IUser CreatedBy => throw new NotImplementedException();

        public ITweetIdentifier CurrentUserRetweetIdentifier => throw new NotImplementedException();

        public int[] ContributorsIds => throw new NotImplementedException();

        public IEnumerable<long> Contributors => throw new NotImplementedException();

        public int RetweetCount => throw new NotImplementedException();

        public ITweetEntities Entities => throw new NotImplementedException();

        public bool Favorited => throw new NotImplementedException();

        public int FavoriteCount => throw new NotImplementedException();

        public bool Retweeted => throw new NotImplementedException();

        public bool PossiblySensitive => throw new NotImplementedException();

        public Language? Language => throw new NotImplementedException();

        public IPlace Place => throw new NotImplementedException();

        public Dictionary<string, object> Scopes => throw new NotImplementedException();

        public string FilterLevel => throw new NotImplementedException();

        public bool WithheldCopyright => throw new NotImplementedException();

        public IEnumerable<string> WithheldInCountries => throw new NotImplementedException();

        public string WithheldScope => throw new NotImplementedException();

        public ITweetDTO TweetDTO => throw new NotImplementedException();

        public List<IHashtagEntity> Hashtags => throw new NotImplementedException();

        public List<IUrlEntity> Urls => throw new NotImplementedException();

        public List<IMediaEntity> Media => throw new NotImplementedException();

        public List<IUserMentionEntity> UserMentions => throw new NotImplementedException();

        public bool IsRetweet => throw new NotImplementedException();

        public ITweet RetweetedTweet => throw new NotImplementedException();

        public int? QuoteCount => throw new NotImplementedException();

        public long? QuotedStatusId => throw new NotImplementedException();

        public string QuotedStatusIdStr => throw new NotImplementedException();

        public ITweet QuotedTweet => throw new NotImplementedException();

        public string Url => throw new NotImplementedException();

        public TweetMode TweetMode => throw new NotImplementedException();

        public long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string IdStr { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task DestroyAsync()
        {
            throw new NotImplementedException();
        }

        public Task DestroyRetweetAsync()
        {
            throw new NotImplementedException();
        }

        public bool Equals(ITweet other)
        {
            throw new NotImplementedException();
        }

        public Task FavoriteAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IOEmbedTweet> GenerateOEmbedTweetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ITweet[]> GetRetweetsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ITweet> PublishRetweetAsync()
        {
            throw new NotImplementedException();
        }

        public Task UnfavoriteAsync()
        {
            throw new NotImplementedException();
        }
    }

    class MockSampleStream : ISampleStream
    {
        public TweetMode? TweetMode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StreamState StreamState => throw new NotImplementedException();

        public string[] LanguageFilters => throw new NotImplementedException();

        public bool? StallWarnings { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public StreamFilterLevel? FilterLevel { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<Tuple<string, string>> CustomQueryParameters => throw new NotImplementedException();

        public string FormattedCustomQueryParameters => throw new NotImplementedException();

        public ITwitterExecutionContext ExecutionContext { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void TriggerTweetReceivedEvent(string tweet)
        {
            MockTweet mockTweet = new MockTweet(tweet);

            TweetReceived?.Invoke(this, new TweetReceivedEventArgs(mockTweet, null));
        }

        public event EventHandler<TweetReceivedEventArgs> TweetReceived;
        public event EventHandler StreamStarted;
        public event EventHandler StreamResumed;
        public event EventHandler StreamPaused;
        public event EventHandler<StreamStoppedEventArgs> StreamStopped;
        public event EventHandler KeepAliveReceived;
        public event EventHandler<TweetDeletedEvent> TweetDeleted;
        public event EventHandler<TweetLocationDeletedEventArgs> TweetLocationInfoRemoved;
        public event EventHandler<DisconnectedEventArgs> DisconnectMessageReceived;
        public event EventHandler<TweetWitheldEventArgs> TweetWitheld;
        public event EventHandler<UserWitheldEventArgs> UserWitheld;
        public event EventHandler<LimitReachedEventArgs> LimitReached;
        public event EventHandler<WarningFallingBehindEventArgs> WarningFallingBehindDetected;
        public event EventHandler<UnsupportedMessageReceivedEvent> UnmanagedEventReceived;
        public event EventHandler<StreamEventReceivedArgs> EventReceived;

        public void AddCustomQueryParameter(string name, string value)
        {
            throw new NotImplementedException();
        }

        public void AddLanguageFilter(string language)
        {
            throw new NotImplementedException();
        }

        public void AddLanguageFilter(LanguageFilter language)
        {
            throw new NotImplementedException();
        }

        public void ClearCustomQueryParameters()
        {
            throw new NotImplementedException();
        }

        public void ClearLanguageFilters()
        {
            throw new NotImplementedException();
        }

        public void Pause()
        {
            throw new NotImplementedException();
        }

        public void RemoveLanguageFilter(string language)
        {
            throw new NotImplementedException();
        }

        public void RemoveLanguageFilter(LanguageFilter language)
        {
            throw new NotImplementedException();
        }

        public void Resume()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync()
        {
            return null;
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }

    class MockStream : IStreamsClient
    {
        public MockSampleStream _mockSampleStream;

        public MockStream()
        {
            _mockSampleStream = new MockSampleStream();
        }

        public IFilteredStream CreateFilteredStream()
        {
            throw new NotImplementedException();
        }

        public IFilteredStream CreateFilteredStream(ICreateFilteredTweetStreamParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ISampleStream CreateSampleStream()
        {
            return _mockSampleStream;
        }

        public ISampleStream CreateSampleStream(ICreateSampleStreamParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ITrackedStream CreateTrackedTweetStream()
        {
            throw new NotImplementedException();
        }

        public ITrackedStream CreateTrackedTweetStream(ICreateTrackedTweetStreamParameters parameters)
        {
            throw new NotImplementedException();
        }

        public ITweetStream CreateTweetStream()
        {
            throw new NotImplementedException();
        }

        public ITweetStream CreateTweetStream(ICreateTweetStreamParameters parameters)
        {
            throw new NotImplementedException();
        }
    }

    class MockTwitterClient : ITwitterClient
    {
        public MockStream _mockStream;

        public MockTwitterClient()
        {
            _mockStream = new MockStream();
        }

        public IAccountSettingsClient AccountSettings => throw new NotImplementedException();

        public IAuthClient Auth => throw new NotImplementedException();

        public IHelpClient Help => throw new NotImplementedException();

        public IExecuteClient Execute => throw new NotImplementedException();

        public IListsClient Lists => throw new NotImplementedException();

        public IMessagesClient Messages => throw new NotImplementedException();

        public IRateLimitsClient RateLimits => throw new NotImplementedException();

        public ISearchClient Search => throw new NotImplementedException();

        public IStreamsClient Streams => _mockStream;

        public ITimelinesClient Timelines => throw new NotImplementedException();

        public ITrendsClient Trends => throw new NotImplementedException();

        public ITweetsClient Tweets => throw new NotImplementedException();

        public IUsersClient Users => throw new NotImplementedException();

        public IUploadClient Upload => throw new NotImplementedException();

        public IAccountActivityClient AccountActivity => throw new NotImplementedException();

        public ISearchV2Client SearchV2 => throw new NotImplementedException();

        public ITweetsV2Client TweetsV2 => throw new NotImplementedException();

        public IUsersV2Client UsersV2 => throw new NotImplementedException();

        public IStreamsV2Client StreamsV2 => throw new NotImplementedException();

        public IRawExecutors Raw => throw new NotImplementedException();

        public ITweetinviSettings Config => throw new NotImplementedException();

        public IReadOnlyTwitterCredentials Credentials => throw new NotImplementedException();

        public IExternalClientEvents Events => throw new NotImplementedException();

        public ITwitterClientFactories Factories => throw new NotImplementedException();

        public IJsonClient Json => throw new NotImplementedException();

        public IParametersValidator ParametersValidator => throw new NotImplementedException();

        public ITwitterRequest CreateRequest()
        {
            throw new NotImplementedException();
        }

        public ITwitterExecutionContext CreateTwitterExecutionContext()
        {
            throw new NotImplementedException();
        }
    }

    public class TestTweetReciever
    {
        [Test(Description="Test tweet retriever")]
        public void TestTweet()
        {
            MockTwitterClient mockTwitterClient = new MockTwitterClient();

            MockSampleStream mockSampleStream;
            MockStream mockStream;

            TweetRetriever tweetRetriever = new TweetRetriever(mockTwitterClient, new NullLogger<TweetRetriever>());

            string sentTweet = "senttweet";
            string recTweet;

            tweetRetriever.OnTweet += (sender, args) => {
                
                recTweet = args.Tweet;

                // Test if it was the tweet that was sent.
                Assert.AreEqual(sentTweet, recTweet);
            };

            mockStream = mockTwitterClient._mockStream;
            mockSampleStream = mockStream._mockSampleStream;

            mockSampleStream.TriggerTweetReceivedEvent(sentTweet);
        }
    }
}