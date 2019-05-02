namespace LinqBridge
{
    using System;
    using System.Collections.Generic;

    internal sealed class DelegatingComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _comparer;

        public DelegatingComparer(Func<T, T, int> comparer)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }
            this._comparer = comparer;
        }

        public int Compare(T x, T y)
        {
            return this._comparer(x, y);
        }
    }
}

