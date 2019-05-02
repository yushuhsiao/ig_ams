namespace LinqBridge
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class OrderedEnumerable<T, K> : IOrderedEnumerable<T>, IEnumerable<T>, IEnumerable
    {
        private readonly Func<T[], IComparer<int>, IComparer<int>> _comparerComposer;
        private readonly IEnumerable<T> _source;

        public OrderedEnumerable(IEnumerable<T> source, Func<T, K> keySelector, IComparer<K> comparer, bool descending) : this(source, (_, next) => next, keySelector, comparer, descending)
        {
        }

        private OrderedEnumerable(IEnumerable<T> source, Func<T[], IComparer<int>, IComparer<int>> parent, Func<T, K> keySelector, IComparer<K> comparer, bool descending)
        {
            <>c__DisplayClass4<T, K> class3;
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            this._source = source;
            comparer = (IComparer<K>) (comparer ?? Comparer<K>.Default);
            int direction = descending ? -1 : 1;
            this._comparerComposer = delegate (T[] items, IComparer<int> next) {
                <>c__DisplayClass4<T, K> class1 = class3;
                K[] keys = new K[items.Length];
                for (int j = 0; j < items.Length; j++)
                {
                    keys[j] = keySelector(items[j]);
                }
                return parent(items, new DelegatingComparer<int>(delegate (int i, int j) {
                    int num = direction * class1.comparer.Compare(keys[i], keys[j]);
                    if (num == 0)
                    {
                        return next.Compare(i, j);
                    }
                    return num;
                }));
            };
        }

        public IOrderedEnumerable<T> CreateOrderedEnumerable<KK>(Func<T, KK> keySelector, IComparer<KK> comparer, bool descending)
        {
            return new OrderedEnumerable<T, KK>(this._source, this._comparerComposer, keySelector, comparer, descending);
        }

        public IEnumerator<T> GetEnumerator()
        {
            T[] localArray = this._source.ToArray<T>();
            DelegatingComparer<int> comparer = new DelegatingComparer<int>((i, j) => i.CompareTo(j));
            IComparer<int> comparer2 = this._comparerComposer(localArray, comparer);
            int[] keys = new int[localArray.Length];
            for (int k = 0; k < keys.Length; k++)
            {
                keys[k] = k;
            }
            Array.Sort<int, T>(keys, localArray, comparer2);
            return ((IEnumerable<T>) localArray).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}

