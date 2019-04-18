using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace System.Collections.Generic
{
    public class SyncList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable
    {
        private List<T> list1 = new List<T>();
        private T[] _list2;
        private T[] list2
        {
            get
            {
                T[] n = Interlocked.CompareExchange(ref _list2, null, null);
                if (n != null) return n;
                lock (this)
                    return list2 = list1.ToArray();
            }
            set => Interlocked.Exchange(ref _list2, value);
        }



        public T this[int index]
        {
            get => list2[index];
            set => list2[index] = value;
        }

        public int IndexOf(T item) => list2.IndexOf(item);

        public void Insert(int index, T item)
        {
            lock (this)
            {
                list1.Insert(index, item);
                list2 = null;
            }
        }

        public void RemoveAt(int index)
        {
            lock (this)
            {
                list1.RemoveAt(index);
                list2 = null;
            }
        }

        public T[] ToArray() => list2;

        public T Find(Predicate<T> match, Func<T> create)
        {
            T r;
            var list2 = this.list2;
            for (int i = 0, n = list2.Length; i < n; i++)
            {
                r = list2[i];
                if (match(r))
                    return r;
            }
            lock (this)
            {
                for (int i = 0, n = list1.Count; i < n; i++)
                {
                    r = list1[i];
                    if (match(r))
                        return r;
                }
                r = create();
                list1.Add(r);
                list2 = null;
                return r;
            }
        }



        public int Count => list2.Length;

        bool ICollection<T>.IsReadOnly
        {
            get { lock (this) return ((ICollection<T>)list1).IsReadOnly; }
        }

        public void Add(T item) => Add(item, true);
        public void Add(T item, bool allow_duplicate)
        {
            lock (this)
            {
                if (allow_duplicate == false && list1.Contains(item))
                    return;
                list1.Add(item);
                list2 = null;
            }
        }

        public void Clear()
        {
            lock (this)
            {
                list1.Clear();
                list2 = null;
            }
        }

        public bool Contains(T item) => list2.Contains(item);

        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => list2.CopyTo(array, arrayIndex);

        public bool Remove(T item)
        {
            lock (this)
            {
                bool r = list1.Remove(item);
                list2 = null;
                return r;
            }
        }



        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            var list2 = this.list2;
            for (int i = 0, n = list2.Length; i < n; i++)
                yield return list2[i];
        }



        object IList.this[int index]
        {
            get => list2[index];
            set => list2[index] = (T)value;
        }

        bool IList.IsReadOnly
        {
            get { lock (this) return ((IList)list1).IsReadOnly; }
        }

        bool IList.IsFixedSize
        {
            get { lock (this) return ((IList)list1).IsFixedSize; }
        }

        int IList.Add(object value)
        {
            lock (this)
            {
                list1.Add((T)value);
                list2 = null;
                return list2.Length;
            }
        }

        bool IList.Contains(object value) => this.Contains((T)value);

        int IList.IndexOf(object value) => this.IndexOf((T)value);

        void IList.Insert(int index, object value) => this.Insert(index, (T)value);

        void IList.Remove(object value) => this.Remove((T)value);



        object ICollection.SyncRoot
        {
            get { lock (this) return ((ICollection)list1).SyncRoot; }
        }

        bool ICollection.IsSynchronized
        {
            get { lock (this) return ((ICollection)list1).IsSynchronized; }
        }

        void ICollection.CopyTo(Array array, int index) => list2.CopyTo(array, index);



        IEnumerator IEnumerable.GetEnumerator() => list2.GetEnumerator();
    }
}