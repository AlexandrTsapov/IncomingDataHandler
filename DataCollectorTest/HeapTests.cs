using System;
using System.Collections.Generic;
using DataCollector.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataCollectorTest
{
    [TestClass]
    public class HeapTests
    {
        [TestMethod]
        public void CountEmpty()
        {
            var heap = new Heap<int, string>();

            Assert.AreEqual(0, heap.Count);
            Assert.IsFalse(heap.Any());
        }

        [TestMethod]
        public void CountEmptyAfterPushPull()
        {
            var heap = new Heap<int, string>();

            heap.Push(1, "A");
            heap.Pull();

            Assert.AreEqual(0, heap.Count);
            Assert.IsFalse(heap.Any());
        }

        [TestMethod]
        public void CountAfterPush()
        {
            var heap = new Heap<int, string>();

            heap.Push(1, "A");
            heap.Push(2, "B");

            Assert.AreEqual(2, heap.Count);
            Assert.IsTrue(heap.Any());
        }

        [TestMethod]
        public void CountAfterPushPull()
        {
            var heap = new Heap<int, string>();

            heap.Push(1, "A");
            heap.Push(2, "B");
            heap.Pull();

            Assert.AreEqual(1, heap.Count);
            Assert.IsTrue(heap.Any());
        }

        [TestMethod]
        public void ExceptionPullEmpty()
        {
            var heap = new Heap<int, string>();

            Assert.ThrowsException<IndexOutOfRangeException>(() => heap.Pull());
        }

        [TestMethod]
        public void ExceptionPeekEmpty()
        {
            var heap = new Heap<int, string>();

            Assert.ThrowsException<IndexOutOfRangeException>(() => heap.Peek());
        }

        [TestMethod]
        public void ExceptionPullEmptyAfterPush()
        {
            var heap = new Heap<int, string>();

            heap.Push(1, "A");
            heap.Pull();

            Assert.ThrowsException<IndexOutOfRangeException>(() => heap.Peek());
        }

        [TestMethod]
        public void PeekAfterPush()
        {
            const int key = 1;
            const string value = "A";

            var heap = new Heap<int, string>();

            heap.Push(key, value);
            var pair = heap.Peek();

            Assert.AreEqual(key, pair.Key);
            Assert.AreEqual(value, pair.Value);
        }

        [TestMethod]
        public void PeekAfterPull()
        {
            const int key = 2;
            const string value = "B";

            var heap = new Heap<int, string>();

            heap.Push(key - 1, $"after {value}");
            heap.Push(key, value);
            heap.Push(key + 1, $"before {value}");
            heap.Pull();
            var pair = heap.Peek();

            Assert.AreEqual(key, pair.Key);
            Assert.AreEqual(value, pair.Value);
        }

        [TestMethod]
        public void PullAfterPush()
        {
            const int key = 1;
            const string value = "A";

            var heap = new Heap<int, string>();

            heap.Push(key, value);
            var pair = heap.Pull();

            Assert.AreEqual(key, pair.Key);
            Assert.AreEqual(value, pair.Value);
        }

        [TestMethod]
        public void PullRandomSequence()
        {
            var startSequence = new List<int> { 4, 6, 6, 1, 9, 2, 6, 3, 3, 9 };
            var expectedSequence = new List<int> { 1, 2, 3, 3, 4, 6, 6, 6, 9, 9 };

            var heap = new Heap<int, int>();
            startSequence.ForEach(item => heap.Push(item, item));
            var actualSequence = PullAllValue(heap);

            CollectionAssert.AreEqual(expectedSequence, actualSequence);
        }

        [TestMethod]
        public void PullIncreasingSequence()
        {
            var startSequence = new List<int> { 1, 2, 2, 2, 4, 5, 5, 6, 7, 7 };

            var heap = new Heap<int, int>();
            startSequence.ForEach(item => heap.Push(item, item));
            var actualSequence = PullAllValue(heap);

            CollectionAssert.AreEqual(startSequence, actualSequence);
        }

        [TestMethod]
        public void PullDecreaseSequence()
        {
            var startSequence = new List<int> { 9, 9, 8, 6, 6, 5, 5, 4, 3, 1 };
            var expectedSequence = new List<int> { 1, 3, 4, 5, 5, 6, 6, 8, 9, 9 };

            var heap = new Heap<int, int>();
            startSequence.ForEach(item => heap.Push(item, item));
            var actualSequence = PullAllValue(heap);

            CollectionAssert.AreEqual(expectedSequence, actualSequence);
        }

        private List<TValue> PullAllValue<TKey, TValue>(Heap<TKey, TValue> heap) where TKey : IComparable<TKey>
        {
            var allValue = new List<TValue>();
            while (heap.Any())
            {
                allValue.Add(heap.Pull().Value);
            }

            return allValue;
        }
    }
}
