using System;
using DataCollector.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataCollectorTest
{
    [TestClass]
    public class PeriodicArrayTests
    {
        [TestMethod]
        public void ExceptionCountNegative()
        {
            Assert.ThrowsException<ArgumentException>(() => new PeriodicArray<int>(-10));
        }

        [TestMethod]
        public void ExceptionFirstIndexNegative()
        {
            var array = new PeriodicArray<int>(10);

            Assert.ThrowsException<ArgumentException>(() => array.FirstIndex = -5);
        }

        [TestMethod]
        public void FirstIndexChange()
        {
            const int count = 5;

            var array = new PeriodicArray<int>(count);

            Assert.AreEqual(0, array.FirstIndex);

            array.FirstIndex = 2;

            Assert.AreEqual(2, array.FirstIndex);

            array.FirstIndex = 2 + count;

            Assert.AreEqual(2, array.FirstIndex);
        }

        [TestMethod]
        public void GetElementByIndex()
        {
            const int count = 5;

            var array = new PeriodicArray<int>(count);

            array[0] = 1;
            array[2] = 5;
            array[4] = 7;

            Assert.AreEqual(1, array[0]);
            Assert.AreEqual(5, array[2]);
            Assert.AreEqual(7, array[4]);
        }

        [TestMethod]
        public void GetElementAfterShift()
        {
            const int count = 5;

            var array = new PeriodicArray<int>(count);

            array[0] = 1;
            array[2] = 5;
            array[4] = 7;
            array.FirstIndex = 2;

            Assert.AreEqual(5, array[0]);
            Assert.AreEqual(7, array[2]);
            Assert.AreEqual(1, array[3]);
        }

        [TestMethod]
        public void RewritingElementAfterShift()
        {
            const int count = 5;

            var array = new PeriodicArray<int>(count);

            array[0] = 1;

            Assert.AreEqual(1, array[0]);

            array.FirstIndex = 2;

            Assert.AreEqual(1, array[3]);

            array[3] = 5;

            Assert.AreEqual(5, array[3]);
        }
    }
}
