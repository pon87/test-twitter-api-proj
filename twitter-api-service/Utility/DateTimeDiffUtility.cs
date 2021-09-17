using System;
using System.Timers;
using twitter_api_service.Utility.Interfaces;

namespace twitter_api_service.Utility
{
    public class DateTimeDiffUtility : IDateTimeDiff
    {
        Timer _timer;
        
        public event EventHandler OnMinutePassed;        

        public DateTimeDiffUtility()
        {
            // Initialize and set timer to trigger every minute.
            _timer = new Timer(60000);
            _timer.Elapsed += OnTimer;
            _timer.Start();
        }

        void OnTimer(object sender, EventArgs arg)
        {
            // Trigger the event that a minute has passed.
            OnMinutePassed?.Invoke(this, arg);
        }
    }
}