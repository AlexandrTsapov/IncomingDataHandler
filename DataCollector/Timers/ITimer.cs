using System;

namespace DataCollector.Timers
{
    public interface ITimer
    {
        event Action Elapsed;

        void Start();

        void Stop();
    }
}
