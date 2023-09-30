using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurvaLauncher.Utilities
{
    public class SortedCollection<T, TKey> : Collection<T>
        where TKey : IComparable<TKey>
    {
        public bool Descending { get; set; } = false;

        public Func<T, TKey>? SortingRoot { get; set; }

        public SortedCollection()
        {
        }

        public SortedCollection(List<T> list) : base(list)
        {
        }

        protected override void InsertItem(int index, T item)
        {
            bool IsRightOfLast(int index, T current)
            {
                if (index == 0)
                    return true;
                if (SortingRoot == null)
                    return true;

                var last = this[index - 1];
                var compareResult = SortingRoot.Invoke(current).CompareTo(SortingRoot.Invoke(last));

                if (Descending)
                    compareResult = -compareResult;

                bool rightOfNext = compareResult >= 0;
                return rightOfNext;
            }

            bool IsLeftOfNext(int index, T current)
            {
                if (index == Count ||
                    index == Count - 1)
                    return true;
                if (SortingRoot == null)
                    return true;

                var next = this[index + 1];
                var compareResult = SortingRoot.Invoke(current).CompareTo(SortingRoot.Invoke(next));

                if (Descending)
                    compareResult = -compareResult;

                bool rightOfNext = compareResult <= 0;
                return rightOfNext;
            }

            while (true)
            {
                bool isRightOfLast = IsRightOfLast(index, item);
                bool isLeftOrNext = IsLeftOfNext(index, item);

                if (!isRightOfLast)
                {
                    index--;
                    continue;
                }

                if (!isLeftOrNext)
                {
                    index++;
                    continue;
                }

                break;
            }

            base.InsertItem(index, item);
        }
    }
}
