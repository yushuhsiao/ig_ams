namespace LinqBridge
{
    using System;
    using System.Collections.Generic;

    internal sealed class KeyComparer<T> : IEqualityComparer<Key<T>>
    {
        private readonly IEqualityComparer<T> _innerComparer;

        public KeyComparer(IEqualityComparer<T> innerComparer)
        {
            this._innerComparer = innerComparer ?? EqualityComparer<T>.Default;
        }

        public bool Equals(Key<T> x, Key<T> y)
        {
            return this._innerComparer.Equals(x.Value, y.Value);
        }

        public int GetHashCode(Key<T> obj)
        {
            if (obj.Value != null)
            {
                return this._innerComparer.GetHashCode(obj.Value);
            }
            return 0;
        }
    }
}

