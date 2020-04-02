using System;
using System.Threading.Tasks;
using DataCollector.Collections;
using DataReceiver;
using DataSender;
using NDesk.Options;

namespace IncomingDataHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan interval = default;
            TimeSpan timeout = default;
            TimeSpan period = default;
            TimeSpan frequency = default;

            bool needHelp = false;

            TimeSpan ConvertStringToSeconds(string s) => TimeSpan.FromSeconds(Convert.ToDouble(s));

            var option = new OptionSet
            {
                {
                    "i|interval=",
                    "duration of a unit time {interval} for data storage",
                    v => interval = ConvertStringToSeconds(v)
                },
                {
                    "t|timeout=",
                    "events {timeout}",
                    v => timeout = ConvertStringToSeconds(v)
                },
                {
                    "p|period=",
                    "data handling {period}",
                    v => period = ConvertStringToSeconds(v)
                },
                {
                    "f|frequency=",
                    "{frequency} of generation of a new event",
                    v => frequency = ConvertStringToSeconds(v)
                },
                {
                    "h|help",
                    "show information",
                    v => needHelp = v != null
                },
            };

            try
            {
                option.Parse(args);
            }
            catch (OptionException exception)
            {
                Console.WriteLine($"An error occurred during startup: {exception.Message}");
                Console.WriteLine("Use --help for more information.");
                return;
            }

            if (needHelp)
            {
                ShowHelp(option);
                Console.ReadLine();
                return;
            }

            if (interval == default
                || timeout == default
                || period == default
                || frequency == default)
            {
                Console.WriteLine($"Not all parameters are set");
                Console.WriteLine("Use --help for more information.");
                return;
            }

            try
            {
                RunHandler(interval, timeout, period, frequency);

                Console.WriteLine("Event handling completed");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"An error occurred during execution: {exception.Message}");
            }
        }

        private static void RunHandler(
            TimeSpan interval,
            TimeSpan timeout,
            TimeSpan period,
            TimeSpan frequency)
        {
            var receiver = new Receiver();
            var generator = new EventGenerator<ReportEvent>(
                frequency,
                () => DataGenerator.GenetationReportEvent(timeout));

            void receiveData(ReportEvent[] data) => receiver.Receive(data);

            var dictionary = new QueueByTime<ReportEvent>(timeout, interval);
            dictionary.PulledData += receiveData;

            void addData(ReportEvent data) => dictionary.Push(data.Time, data);
            generator.Generated += addData;

            generator.Start();

            Task.Delay(period).Wait();

            generator.Stop();

            dictionary.PulledData -= receiveData;
            generator.Generated -= addData;
        }

        private static void ShowHelp(OptionSet option)
        {
            Console.WriteLine("Application handles events that have a random time stamp and returns them sorted by time stamp");
            Console.WriteLine();
            Console.WriteLine("Options:");
            option.WriteOptionDescriptions(Console.Out);
        }
    }
}
