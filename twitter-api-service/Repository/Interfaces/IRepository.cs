
using System.Collections;
using System.Collections.Generic;

namespace twitter_api_service.Repository.Interfaces
{
    public interface IRepository<T> where T: struct
    {
        void SaveTotalTweets(int totalTweets);

        int GetTotalTweets();

        void SaveTotalTweetsInMinute(int totalTweetsInMinute);

        List<int> GetListOfTotalTweetsInMinutes();
    }
}