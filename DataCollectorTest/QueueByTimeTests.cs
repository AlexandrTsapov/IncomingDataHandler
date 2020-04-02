using System;
using System.Collections.Generic;
using System.Linq;
using DataCollector.Collections;
using DataCollectorTest.Mocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataCollectorTest
{
    [TestClass]
    public class QueueByTimeTests
    {
        private readonly DateTime CurrentTime = new DateTime(2020, 2, 1, 12, 0, 0);
        private readonly TimeSpan Timeout = TimeSpan.FromSeconds(5);
        private readonly TimeSpan Interval = TimeSpan.FromSeconds(1);
        private readonly TimeSpan HalfInterval = TimeSpan.FromSeconds(0.5);

        private TimerMock _timer;
        private List<string> _result;
        private QueueByTime<string> _queue;

        [TestInitialize]
        public void TestInitialize()
        {
            _result = new List<string>();
            _timer = new TimerMock(CurrentTime, Interval);
            _queue = new QueueByTime<string>(
                Timeout,
                Interval,
                () => _timer.CurrentTime,
                _timer);

            _queue.PulledData += Dictionary_SendData;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _queue.PulledData -= Dictionary_SendData;
        }

        [TestMethod]
        public void ExceptionAddTimeLargerCurrent()
        {
            void addTimeLargerCurrent() => _queue.Push(CurrentTime.AddMinutes(1), "A");

            Assert.ThrowsException<ArgumentException>(addTimeLargerCurrent);
        }

        [TestMethod]
        public void ExceptionAddTimeLessTimeout()
        {
            void addTimeLessTimeout() => _queue.Push(CurrentTime.Add(-Timeout.Add(Interval)), "A");

            Assert.ThrowsException<ArgumentException>(addTimeLessTimeout);
        }

        [TestMethod]
        public void ValueInEndQueue()
        {
            var expectedResult = new[] { "A" };

            var firstInterval = CurrentTime.Add(HalfInterval - Timeout);
            _queue.Push(firstInterval, "A");
            _timer.PassInterval();

            CollectionAssert.AreEqual(expectedResult, _result);
        }

        [TestMethod]
        public void ValueInMiddleQueue()
        {
            var expectedResult = new[] { "A" };

            var secondInterval = CurrentTime.Add(Interval + HalfInterval - Timeout);
            _queue.Push(secondInterval, "A");
            _timer.PassInterval();

            Assert.IsFalse(_result.Any());

            _timer.PassInterval();

            CollectionAssert.AreEqual(expectedResult, _result);
        }

        [TestMethod]
        public void AddSomeValue()
        {
            var expectedResult = new[] { "A", "B", "C" };

            var firstInterval = CurrentTime.Add(HalfInterval - Timeout);
            var secondInterval = firstInterval.Add(Interval);
            var thirdInterval = secondInterval.Add(Interval);

            _queue.Push(thirdInterval, "C");
            _queue.Push(firstInterval, "A");
            _queue.Push(secondInterval, "B");
            _timer.PassIntervals(number: 3);

            CollectionAssert.AreEqual(expectedResult, _result);
        }

        [TestMethod]
        public void AddSomeValueInOneInterval()
        {
            var expectedResult = new[] { "A", "B", "C", "D" };

            var halfInterval = CurrentTime.Add(HalfInterval - Timeout);
            var firstInterval = halfInterval.AddSeconds(-0.1);
            var secondInterval = halfInterval.AddSeconds(0.1);
            var thirdInterval = halfInterval.AddSeconds(0.3);

            _queue.Push(thirdInterval, "D");
            _queue.Push(firstInterval, "A");
            _queue.Push(secondInterval, "C");
            _queue.Push(halfInterval, "B");
            _timer.PassInterval();

            CollectionAssert.AreEqual(expectedResult, _result);
        }

        private void Dictionary_SendData(string[] data)
        {
            _result.AddRange(data);
        }
    }
}
