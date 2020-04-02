using System;
using System.Collections.Generic;
using System.Linq;
using DataCollector.Timers;
using NLog;

namespace DataCollector.Collections
{
    public class QueueByTime<T>
    {
        private static readonly Logger Logger = LogManager.GetLogger("DataCollectorLogger");

        private readonly PeriodicArray<Heap<DateTime, T>> _data;
        private readonly Func<DateTime> _timeGetter;
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _interval;

        public event Action<T[]> PulledData;

        public QueueByTime(TimeSpan timeout, TimeSpan interval)
            : this(timeout, interval, () => DateTime.Now, new SystemTimer(interval))
        {

        }

        public QueueByTime(
            TimeSpan timeout,
            TimeSpan interval,
            Func<DateTime> timeGetter,
            ITimer timer)
        {
            _timeout = timeout;
            _interval = interval;
            _timeGetter = timeGetter;
            var count = (int)Math.Ceiling((double)timeout.Ticks / interval.Ticks);

            _data = new PeriodicArray<Heap<DateTime, T>>(count);
            timer.Start();
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed()
        {
            var firstHeap = _data.First();

            if (firstHeap != null && firstHeap.Any())
            {
                var data = new List<KeyValuePair<DateTime, T>>();
                while (firstHeap.Any())
                {
                    data.Add(firstHeap.Pull());
                }

                Logger.Info($"Pulled data with times: {string.Join(", ", data.Select(pair => pair.Key))}");

                PulledData?.Invoke(data.Select(pair => pair.Value).ToArray());
            }

            _data.FirstIndex++;
            Logger.Info($"First index is index [{_data.FirstIndex}]");
        }

        public void Push(DateTime dateTime, T value)
        {
            var currentTime = _timeGetter();

            if (currentTime > dateTime + _timeout)
            {
                var message = $"Event time: [{dateTime}] lags behind current [{currentTime}] more than allowed timeout [{_timeout}] second";
                Logger.Error(message);

                throw new ArgumentException(message);
            }

            if (currentTime < dateTime)
            {
                var message = $"Event time [{dateTime}] is longer than current [{currentTime}]";
                Logger.Error(message);

                throw new ArgumentException(message);
            }

            var index = (int)Math.Floor((double)(dateTime + _timeout - currentTime).Ticks / _interval.Ticks);

            if (_data[index] == null)
            {
                _data[index] = new Heap<DateTime, T>();
            }

            _data[index].Push(dateTime, value);

            Logger.Info($"Object with time [{dateTime}] added by index [{index}]");
        }
    }
}
