using System;
using NLog;

namespace DataCollector.Collections
{
    public class PeriodicArray<T>
    {
        private static readonly Logger Logger = LogManager.GetLogger("DataCollectorLogger");

        private readonly T[] _array;
        private int _firstIndex;

        public PeriodicArray(int count)
        {
            if (count < 0)
            {
                Logger.Info("Negative number of elements");
                throw new ArgumentException("Negative number of elements");
            }

            _array = new T[count];
        }

        public int FirstIndex
        {
            get
            {
                return _firstIndex;
            }
            set
            {
                if (value < 0)
                {
                    Logger.Info("Negative index");
                    throw new ArgumentException("Negative index");
                }

                _firstIndex = value % _array.Length;
                Logger.Info($"First index set [{_firstIndex}]");
            }
        }

        public T this[int index]
        {
            get
            {
                return _array[GetIndexInData(index)];
            }
            set
            {
                var indexInData = GetIndexInData(index);
                _array[indexInData] = value;

                Logger.Info($"By periodic index [{index}] and non-periodic [{indexInData}] element set: [{value}]");
            }
        }

        public T First()
        {
            return this[0];
        }

        private int GetIndexInData(int index)
        {
            var indexInData = (FirstIndex + index) % _array.Length;

            return indexInData >= 0
                ? indexInData
                : _array.Length + indexInData;
        }
    }
}
