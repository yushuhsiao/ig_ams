namespace System.Linq
{
    using LinqBridge;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Threading;

    internal sealed class Lookup<TKey, TElement> : ILookup<TKey, TElement>, IEnumerable<IGrouping<TKey, TElement>>, IEnumerable
    {
        private readonly Dictionary<Key<TKey>, IGrouping<TKey, TElement>> _map;
        private readonly List<Key<TKey>> _orderedKeys;

        internal Lookup(IEqualityComparer<TKey> comparer)
        {
            this._map = new Dictionary<Key<TKey>, IGrouping<TKey, TElement>>(new KeyComparer<TKey>(comparer));
            this._orderedKeys = new List<Key<TKey>>();
        }

        internal void Add(IGrouping<TKey, TElement> item)
        {
            Key<TKey> key = new Key<TKey>(item.Key);
            this._map.Add(key, item);
            this._orderedKeys.Add(key);
        }

        public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        {
            if (this.resultSelector == null)
            {
                throw new ArgumentNullException("resultSelector");
            }
            Dictionary<Key<TKey>, IGrouping<TKey, TElement>>.Enumerator enumerator = this._map.GetEnumerator();
            while (enumerator.MoveNext())
            {
                KeyValuePair<Key<TKey>, IGrouping<TKey, TElement>> current = enumerator.Current;
                yield return this.resultSelector(current.Key.Value, current.Value);
            }
            this.<>m__Finally3();
        }

        public bool Contains(TKey key)
        {
            return this._map.ContainsKey(new Key<TKey>(key));
        }

        internal IEnumerable<TElement> Find(TKey key)
        {
            IGrouping<TKey, TElement> grouping;
            if (!this._map.TryGetValue(new Key<TKey>(key), out grouping))
            {
                return null;
            }
            return grouping;
        }

        public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
        {
            foreach (Key<TKey> iteratorVariable0 in this._orderedKeys)
            {
                yield return this._map[iteratorVariable0];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public int Count
        {
            get
            {
                return this._map.Count;
            }
        }

        public IEnumerable<TElement> this[TKey key]
        {
            get
            {
                IGrouping<TKey, TElement> grouping;
                if (!this._map.TryGetValue(new Key<TKey>(key), out grouping))
                {
                    return Enumerable.Empty<TElement>();
                }
                return grouping;
            }
        }

        [CompilerGenerated]
        private sealed class <ApplyResultSelector>d__0<TResult> : IEnumerable<TResult>, IEnumerable, IEnumerator<TResult>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private TResult <>2__current;
            public Func<TKey, IEnumerable<TElement>, TResult> <>3__resultSelector;
            public Lookup<TKey, TElement> <>4__this;
            public Dictionary<Key<TKey>, IGrouping<TKey, TElement>>.Enumerator <>7__wrap2;
            private int <>l__initialThreadId;
            public KeyValuePair<Key<TKey>, IGrouping<TKey, TElement>> <pair>5__1;
            public Func<TKey, IEnumerable<TElement>, TResult> resultSelector;

            [DebuggerHidden]
            public <ApplyResultSelector>d__0(int <>1__state)
            {
                this.<>1__state = <>1__state;
                this.<>l__initialThreadId = Thread.CurrentThread.ManagedThreadId;
            }

            private void <>m__Finally3()
            {
                this.<>1__state = -1;
                this.<>7__wrap2.Dispose();
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            if (this.resultSelector == null)
                            {
                                throw new ArgumentNullException("resultSelector");
                            }
                            break;

                        case 2:
                            goto Label_00A9;

                        default:
                            goto Label_00C3;
                    }
                    this.<>7__wrap2 = this.<>4__this._map.GetEnumerator();
                    this.<>1__state = 1;
                    while (this.<>7__wrap2.MoveNext())
                    {
                        this.<pair>5__1 = this.<>7__wrap2.Current;
                        this.<>2__current = this.resultSelector(this.<pair>5__1.Key.Value, this.<pair>5__1.Value);
                        this.<>1__state = 2;
                        return true;
                    Label_00A9:
                        this.<>1__state = 1;
                    }
                    this.<>m__Finally3();
                Label_00C3:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            IEnumerator<TResult> IEnumerable<TResult>.GetEnumerator()
            {
                Lookup<TKey, TElement>.<ApplyResultSelector>d__0<TResult> d__;
                if ((Thread.CurrentThread.ManagedThreadId == this.<>l__initialThreadId) && (this.<>1__state == -2))
                {
                    this.<>1__state = 0;
                    d__ = (Lookup<TKey, TElement>.<ApplyResultSelector>d__0<TResult>) this;
                }
                else
                {
                    d__ = new Lookup<TKey, TElement>.<ApplyResultSelector>d__0<TResult>(0) {
                        <>4__this = this.<>4__this
                    };
                }
                d__.resultSelector = this.<>3__resultSelector;
                return d__;
            }

            [DebuggerHidden]
            IEnumerator IEnumerable.GetEnumerator()
            {
                return this.System.Collections.Generic.IEnumerable<TResult>.GetEnumerator();
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally3();
                        }
                        return;
                }
            }

            TResult IEnumerator<TResult>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }

        [CompilerGenerated]
        private sealed class <GetEnumerator>d__6 : IEnumerator<IGrouping<TKey, TElement>>, IEnumerator, IDisposable
        {
            private int <>1__state;
            private IGrouping<TKey, TElement> <>2__current;
            public Lookup<TKey, TElement> <>4__this;
            public List<Key<TKey>>.Enumerator <>7__wrap8;
            public Key<TKey> <key>5__7;

            [DebuggerHidden]
            public <GetEnumerator>d__6(int <>1__state)
            {
                this.<>1__state = <>1__state;
            }

            private void <>m__Finally9()
            {
                this.<>1__state = -1;
                this.<>7__wrap8.Dispose();
            }

            private bool MoveNext()
            {
                bool flag;
                try
                {
                    switch (this.<>1__state)
                    {
                        case 0:
                            this.<>1__state = -1;
                            this.<>7__wrap8 = this.<>4__this._orderedKeys.GetEnumerator();
                            this.<>1__state = 1;
                            goto Label_0080;

                        case 2:
                            this.<>1__state = 1;
                            goto Label_0080;

                        default:
                            goto Label_0093;
                    }
                Label_0041:
                    this.<key>5__7 = this.<>7__wrap8.Current;
                    this.<>2__current = this.<>4__this._map[this.<key>5__7];
                    this.<>1__state = 2;
                    return true;
                Label_0080:
                    if (this.<>7__wrap8.MoveNext())
                    {
                        goto Label_0041;
                    }
                    this.<>m__Finally9();
                Label_0093:
                    flag = false;
                }
                fault
                {
                    this.System.IDisposable.Dispose();
                }
                return flag;
            }

            [DebuggerHidden]
            void IEnumerator.Reset()
            {
                throw new NotSupportedException();
            }

            void IDisposable.Dispose()
            {
                switch (this.<>1__state)
                {
                    case 1:
                    case 2:
                        try
                        {
                        }
                        finally
                        {
                            this.<>m__Finally9();
                        }
                        return;
                }
            }

            IGrouping<TKey, TElement> IEnumerator<IGrouping<TKey, TElement>>.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }

            object IEnumerator.Current
            {
                [DebuggerHidden]
                get
                {
                    return this.<>2__current;
                }
            }
        }
    }
}

