using System;
using System.Timers;

namespace DataCollector.Timers
{
    public class SystemTimer : ITimer
    {
        private readonly Timer _timer;

        public event Action Elapsed;

        public SystemTimer(TimeSpan interval)
        {
            _timer = new Timer(interval.TotalMilliseconds);
            _timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Elapsed?.Invoke();
        }
    }
}
