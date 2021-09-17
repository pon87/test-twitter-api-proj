
using twitter_api_service.Entity;
using twitter_api_service.Repository.Interfaces;

using System.Collections.Generic;

namespace twitter_api_service.Repository
{
    public class InMemoryRepository : IRepository<AnalysisData>
    {
        AnalysisData _data;

        List<int> _totalTweetsInMinutesList;

        public InMemoryRepository ()
        {
            _data = new AnalysisData();

            _totalTweetsInMinutesList = new List<int>(1000);
        }        

        public void SaveTotalTweetsInMinute(int totalTweetsInMinute)
        {
            _totalTweetsInMinutesList.Add(totalTweetsInMinute);
        }

        public void SaveTotalTweets(int totalTweets)
        {
            _data.TotalTweets = totalTweets;
        }

        public int GetTotalTweets()
        {
            return _data.TotalTweets;
        }

        public List<int> GetListOfTotalTweetsInMinutes()
        {
            List<int> newList = new List<int>(_totalTweetsInMinutesList);
            return newList;
        }

    }
    
}