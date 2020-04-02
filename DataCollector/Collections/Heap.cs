using System;
using System.Collections.Generic;
using NLog;

namespace DataCollector.Collections
{
    public class Heap<TKey, TValue> where TKey : IComparable<TKey>
    {
        private static readonly Logger Logger = LogManager.GetLogger("DataCollectorLogger");

        private const int MinLength = 1;

        private KeyValuePair<TKey, TValue>[] _data;

        public Heap()
        {
            _data = new KeyValuePair<TKey, TValue>[MinLength];
        }

        public int Count { get; private set; }

        public bool Any() => Count > 0;

        public void Push(TKey key, TValue value)
        {
            if (Count == _data.Length)
            {
                Array.Resize(ref _data, 2 * _data.Length);
                Logger.Info($"Increase heap size to [{_data.Length}]");
            }

            _data[Count++] = new KeyValuePair<TKey, TValue>(key, value);

            SiftUp();

            Logger.Info($"Push with key: [{key}]");
        }

        public KeyValuePair<TKey, TValue> Pull()
        {
            if (Count == 0)
            {
                Logger.Info("Pull element from empty heap");
                throw new IndexOutOfRangeException("Pull element from empty heap");
            }

            var value = _data[0];
            Count--;

            SiftDown();

            Logger.Info($"Pull with key: [{value.Key}]");

            return value;
        }

        public KeyValuePair<TKey, TValue> Peek()
        {
            if (Count == 0)
            {
                Logger.Info("Peek element from empty heap");
                throw new IndexOutOfRangeException("Peek element from empty heap");
            }

            return _data[0];
        }

        private void Swap(int indexA, int indexB)
        {
            var temp = _data[indexA];
            _data[indexA] = _data[indexB];
            _data[indexB] = temp;
        }

        private bool Compare(int indexA, int indexB)
        {
            return _data[indexA].Key.CompareTo(_data[indexB].Key) < 0;
        }

        private void SiftUp()
        {
            var index = Count - 1;

            while (index > 0)
            {
                var indexParent = (index - 1) / 2;
                if (Compare(index, indexParent))
                {
                    Swap(index, indexParent);
                    index = indexParent;
                }
                else
                {
                    break;
                }
            }
        }

        private void SiftDown()
        {
            _data[0] = _data[Count];
            _data[Count] = default;

            var index = 0;

            while (true)
            {
                var indexMin = index;
                var leftChild = 2 * index + 1;
                var rightChild = 2 * index + 2;

                if (leftChild < Count
                    && Compare(leftChild, indexMin))
                {
                    indexMin = leftChild;
                }

                if (rightChild < Count
                    && Compare(rightChild, indexMin))
                {
                    indexMin = rightChild;
                }

                if (indexMin == index)
                {
                    break;
                }
                else
                {
                    Swap(index, indexMin);
                    index = indexMin;
                }
            }
        }
    }
}
