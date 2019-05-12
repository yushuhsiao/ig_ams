// $Id: OrderedEnumerable.cs 71137f497bf2 2012/04/16 20:01:27 azizatif $

namespace LinqBridge
{
    #region Imports

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    #endregion

    internal sealed class OrderedEnumerable<T, K> : IOrderedEnumerable<T>
    {
        private readonly IEnumerable<T> _source;
        private readonly Func<T[], IComparer<int>, IComparer<int>> _comparerComposer;

        public OrderedEnumerable(IEnumerable<T> source,
            Func<T, K> keySelector, IComparer<K> comparer, bool descending) :
            this(source, (_, next) => next, keySelector, comparer, descending) { }

        private OrderedEnumerable(IEnumerable<T> source,
            Func<T[], IComparer<int>, IComparer<int>> parent,
            Func<T, K> keySelector, IComparer<K> comparer, bool descending)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            Debug.Assert(parent != null);

            _source = source;

            comparer = comparer ?? Comparer<K>.Default;
            var direction = descending ? -1 : 1;

            _comparerComposer = (items, next) =>
            {
                Debug.Assert(items != null);
                Debug.Assert(next != null);

                var keys = new K[items.Length];
                for (var i = 0; i < items.Length; i++)
                    keys[i] = keySelector(items[i]);

                return parent(items, new DelegatingComparer<int>((i, j) =>
                {
                    var result = direction * comparer.Compare(keys[i], keys[j]);
                    return result != 0 ? result : next.Compare(i, j);
                }));
            };
        }

        public IOrderedEnumerable<T> CreateOrderedEnumerable<KK>(
            Func<T, KK> keySelector, IComparer<KK> comparer, bool descending)
        {
            return new OrderedEnumerable<T, KK>(_source, _comparerComposer, keySelector, comparer, descending);
        }

        public IEnumerator<T> GetEnumerator()
        {
            //
            // Sort using Array.Sort but docs say that it performs an 
            // unstable sort. LINQ, on the other hand, says OrderBy performs 
            // a stable sort. Use the item position then as a tie 
            // breaker when all keys compare equal, thus making the sort 
            // stable.
            //

            var items = _source.ToArray();
            var positionComparer = new DelegatingComparer<int>((i, j) => i.CompareTo(j));
            var comparer = _comparerComposer(items, positionComparer);
            var keys = new int[items.Length];
            for (var i = 0; i < keys.Length; i++)
                keys[i] = i;
            Array.Sort(keys, items, comparer);
            return ((IEnumerable<T>)items).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

