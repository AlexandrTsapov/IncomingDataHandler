using System;
using System.Collections.Generic;
using System.Text;

namespace DataSender
{
    public static class DataGenerator
    {
        private const string Chars = "$%#@!*abcdefghijklmnopqrstuvwxyz1234567890?;:ABCDEFGHIJKLMNOPQRSTUVWXYZ^&";
        private const int MinLengthWord = 5;
        private const int MaxLengthWord = 10;

        private static readonly Random _random = new Random();

        public static int GenerateInt()
        {
            return _random.Next();
        }

        public static char GenerateChar()
        {
            var index = _random.Next(Chars.Length);

            return Chars[index];
        }

        public static string GenerateWord()
            => GenerateWord(MinLengthWord, MaxLengthWord);

        public static string GenerateWord(int minLength, int maxLength)
        {
            var length = _random.Next(minLength, maxLength);

            var line = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                line.Append(GenerateChar());
            }

            return line.ToString();
        }

        public static string GenerateLine(int countWord)
            => GenerateLine(countWord, MinLengthWord, MaxLengthWord);

        public static string GenerateLine(int countWord, int minLengthWord, int maxLengthWord)
        {
            var listWords = new List<string>();
            for (var i = 0; i < countWord; i++)
            {
                listWords.Add(GenerateWord(minLengthWord, maxLengthWord));
            }

            return string.Join(" ", listWords);
        }

        public static DateTime GenerateDateTime(TimeSpan timeout)
        {
            var delayMilliseconds = _random.NextDouble() * timeout.TotalMilliseconds;

            return DateTime.Now.AddMilliseconds(-delayMilliseconds);
        }

        public static ReportEvent GenetationReportEvent(TimeSpan timeout)
        {
            var reportEvent = new ReportEvent
            {
                Id = GenerateInt(),
                Name = GenerateWord(),
                Description = GenerateLine(countWord: 10),
                Time = GenerateDateTime(timeout)
            };

            return reportEvent;
        }
    }
}
