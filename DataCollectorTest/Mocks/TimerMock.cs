using System;
using DataCollector.Timers;

namespace DataCollectorTest.Mocks
{
    internal class TimerMock : ITimer
    {
        public event Action Elapsed;

        private TimeSpan _interval;

        public DateTime CurrentTime
        {
            get;
            private set;
        }

        public TimerMock(DateTime startTime, TimeSpan interval)
        {
            CurrentTime = startTime;
            _interval = interval;
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void PassIntervals(int number)
        {
            for (var i = 1; i <= number; i++)
            {
                PassInterval();
            }
        }

        public void PassInterval()
        {
            CurrentTime += _interval;
            Elapsed?.Invoke();
        }
    }
}
