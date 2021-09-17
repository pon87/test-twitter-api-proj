
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using twitter_api_service.Entity;
using twitter_api_service.Processor.Interfaces;
using twitter_api_service.Repository.Interfaces;

using Microsoft.Extensions.Logging;
using twitter_api_service.Utility.Interfaces;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace twitter_api_service.Processor
{
    public class TweetProcessor : ITweetProcessor, IDisposable
    {
        ITweetRetriever _twitterReciever;        

        IRepository<AnalysisData> _respository;

        List<int> _totalTweetsEachMinutesList;       
        int _tweetCounterEachMinute = 0;
        int _totalMinutes = 0;     
        int _totalTweets = 0;   

        object _lock;

        CancellationToken _cancelToken;

        CancellationTokenSource _tokenSource;

        ConcurrentQueue<string> _dataQueue;

        ILogger<TweetProcessor> _logger;

        IDateTimeDiff _dateTimeDiffUtility;

        List<Task> _taskList;        

        public TweetProcessor(ITweetRetriever twitterRetriever, IRepository<AnalysisData> memoryRespository, IDateTimeDiff dateTimeDiff, ILogger<TweetProcessor> logger, IConfiguration configuration)
        {
            _logger = logger;        

            _lock = new Object();

            _totalTweetsEachMinutesList = new List<int>(100);

            _dateTimeDiffUtility = dateTimeDiff;
            _dateTimeDiffUtility.OnMinutePassed += OnMinutePassed;

            _taskList = new List<Task>(3);            

            _tokenSource = new CancellationTokenSource();
            _cancelToken = _tokenSource.Token;

            _respository = memoryRespository;

            _twitterReciever = twitterRetriever;
            _twitterReciever.OnTweet += OnReceiveTwitter;

            _dataQueue = new ConcurrentQueue<string>();    

            Initialize(configuration);
        }

        void Initialize(IConfiguration configuration)
        {
            ServiceSettings serviceSettings;

            Task task;            

            try
            {
                serviceSettings = new ServiceSettings();

                configuration.GetSection(ServiceConstants.SERVICE_SETTINGS).Bind(serviceSettings);              

                // Spawn number of threads as needed base on configuration settings.
                for (int i = 0; i < serviceSettings.TotalProcessingThreads; ++i)
                {
                    task = Task.Run(() => Processing_Thread());
                    _taskList.Add(task);
                }
            }
            catch (Exception err)
            {
                _logger.LogError(err.Message);
            }
        }

        void OnReceiveTwitter(object sender, TwitterEventArgs args)
        {
            // Just enque incoming tweets for processing so not to take up
            // any time in delaying incoming new tweets from Twitter API stream.
            _dataQueue.Enqueue(args.Tweet);
        }

        async Task Processing_Thread()
        {
            string tweet;

            bool isSuccess;

            const int MAX_WAIT = 100;

            int waitInterval = MAX_WAIT;

            while (!_cancelToken.IsCancellationRequested) 
            {
                try
                {
                    isSuccess = _dataQueue.TryDequeue(out tweet);
                    if (isSuccess)
                    {
                        Interlocked.Increment(ref _totalTweets);
                        Interlocked.Increment(ref _tweetCounterEachMinute);

                        lock (_lock)
                        {
                            _respository.SaveTotalTweets(_totalTweets);
                        }                        

                        waitInterval = 0;

                        _logger.LogInformation($"Tweet received: {tweet}");     
                    }
                    else {
                        waitInterval = MAX_WAIT;                        
                    }

                    await Task.Delay(waitInterval, _cancelToken);
                }
                catch (TaskCanceledException)
                {
                    _logger.LogInformation("Thread exited..");
                }
                catch (Exception err)
                {
                    _logger.LogError(err.Message);
                }
            }
        }

        void OnMinutePassed(object sender, EventArgs arg)
        {            
            Interlocked.Increment(ref _totalMinutes);
            
            lock(_lock)
            {
                // When a minute have passed, save the total tweets that was
                // counted and reset it back to zero.
                _respository.SaveTotalTweetsInMinute(_tweetCounterEachMinute);
                _tweetCounterEachMinute = 0;
            }
        }

        public float GetAverageTweetsInMinutes()
        {
            List<int> totalTweetsEachMinutesList;

            int sumTotalTweets;

            float averageTweetsInMinutes;

            lock (_lock) 
            {
                totalTweetsEachMinutesList = _respository.GetListOfTotalTweetsInMinutes();
            }

            if (totalTweetsEachMinutesList.Count > 0)
            {
                // Sum up all the total tweets each minute that was collected.
                sumTotalTweets = totalTweetsEachMinutesList.Sum();

                // Compute the Average tweets per minute.
                averageTweetsInMinutes = (sumTotalTweets / _totalMinutes);
            }
            else
            {
                averageTweetsInMinutes = 0;
            }
            return averageTweetsInMinutes;           
        }

        public int GetTotalTweets()
        {
            return _respository.GetTotalTweets();
        }

        public void Dispose()
        {
            // Clean up all resources.
            // Trigger to cancel and exit all running threads.
            _tokenSource.Cancel();

            Task.WaitAll(_taskList.ToArray(), 100);
        }
    }
}