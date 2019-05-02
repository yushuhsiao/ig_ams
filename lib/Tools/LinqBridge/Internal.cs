// $Id: Internal.cs 1567e00f1a20 2012/04/17 16:09:51 azizatif $

namespace LinqBridge
{
    #region Imports

    using System;
    using System.Collections.Generic;

    #endregion

    /// <remarks>
    /// This type is not intended to be used directly from user code.
    /// It may be removed or changed in a future version without notice.
    /// </remarks>

    sealed class DelegatingComparer<T> : IComparer<T>
    {
        private readonly Func<T, T, int> _comparer;

        public DelegatingComparer(Func<T, T, int> comparer)
        {
            if (comparer == null) throw new ArgumentNullException("comparer");
            _comparer = comparer;
        }

        public int Compare(T x, T y) { return _comparer(x, y); }
    }

    /// <remarks>
    /// This type is not intended to be used directly from user code.
    /// It may be removed or changed in a future version without notice.
    /// </remarks>

    struct Key<T>
    {
        public Key(T value) : this() { Value = value; }
        public T Value { get; private set; }
    }

    /// <remarks>
    /// This type is not intended to be used directly from user code.
    /// It may be removed or changed in a future version without notice.
    /// </remarks>

    sealed class KeyComparer<T> : IEqualityComparer<Key<T>>
    {
        private readonly IEqualityComparer<T> _innerComparer;

        public KeyComparer(IEqualityComparer<T> innerComparer)
        {
            _innerComparer = innerComparer ?? EqualityComparer<T>.Default;
        }

        public bool Equals(Key<T> x, Key<T> y)
        {
            return _innerComparer.Equals(x.Value, y.Value);
        }

        public int GetHashCode(Key<T> obj)
        {
            return obj.Value == null ? 0 : _innerComparer.GetHashCode(obj.Value);
        }
    }
}

