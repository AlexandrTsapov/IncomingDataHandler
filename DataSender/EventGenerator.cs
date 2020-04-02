using System;
using System.Timers;
using Newtonsoft.Json;
using NLog;

namespace DataSender
{
    public class EventGenerator<T>
    {
        private static readonly Logger Logger = LogManager.GetLogger("DataSenderLogger");

        private static readonly Random _random = new Random();

        private readonly Timer _timer;
        private readonly TimeSpan _timeForGeneration;
        private readonly Func<T> _generator;

        public event Action<T> Generated;

        public EventGenerator(TimeSpan timeForGeneration, Func<T> generator)
        {
            _timeForGeneration = timeForGeneration;
            _generator = generator;

            _timer = new Timer();
            _timer.Elapsed += Timer_Elapsed;
        }

        public void Start()
        {
            UpdateTimerInterval();
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void UpdateTimerInterval()
        {
            var interval = _random.NextDouble() * _timeForGeneration.TotalMilliseconds;
            _timer.Interval = interval;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateTimerInterval();
            GenetateData();
        }

        private void GenetateData()
        {
            var generatedData = _generator();
            var json = JsonConvert.SerializeObject(generatedData);
            Logger.Info($"Generate object with type: [{generatedData.GetType()}], object: [{json}]");

            Generated?.Invoke(generatedData);
        }
    }
}
