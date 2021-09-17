using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using twitter_api_service.Entity;
using twitter_api_service.Repository.Interfaces;
using twitter_api_service.Processor.Interfaces;

namespace twitter_api_service
{
    [Route("v1/[controller]")]
    public class TwitterAnalysisController : ControllerBase
    {
        IRepository<AnalysisData> _inMemoryRepository;

        ITweetProcessor _tweetProcessor;

        public TwitterAnalysisController(IRepository<AnalysisData> memoryRepository, ITweetProcessor processor)
        {
            _inMemoryRepository = memoryRepository;

            _tweetProcessor = processor;
        }

        [HttpGet]
        public ActionResult<AnalysisData> Get()
        {
            AnalysisData data; 

            data = new AnalysisData();
            
            data.AverageTweetsPerMinute = _tweetProcessor.GetAverageTweetsInMinutes();
            data.TotalTweets = _tweetProcessor.GetTotalTweets();
            return data;
        }
        
    }
}