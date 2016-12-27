using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Diagnostics;

namespace System.Threading
{
#if NET40
    using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;
    public static partial class Extensions
    {
        [_DebuggerStepThrough]
        public static MethodBase GetCallingMethod(this Thread thread, int stackLevel)
        {
            if (stackLevel >= 0)
            {
                MethodBase m1 = MethodBase.GetCurrentMethod();
                StackTrace s = new StackTrace(thread, false);
                for (int i = 0; i < s.FrameCount; i++)
                {
                    MethodBase m2 = s.GetFrame(i).GetMethod();
                    if (m1 != m2) continue;
                    for (int n1 = 0, n2 = i + 1; (n1 <= stackLevel) && (n2 < s.FrameCount); n1++, n2++)
                        m2 = s.GetFrame(n2).GetMethod();
                    return m2;
                }
            }
            return null;
        }
    }
#endif

    //[DebuggerStepThrough]
    //public class SyncLock : IDisposable
    //{
    //    static Queue<SyncLock> p = new Queue<SyncLock>();

    //    static SyncLock Alloc(object obj)
    //    {
    //        SyncLock s;
    //        lock (p)
    //            if (p.Count == 0)
    //                s = new SyncLock();
    //            else
    //                s = p.Dequeue();
    //        Interlocked.Exchange(ref s.obj0, obj);
    //        Interlocked.Exchange(ref s.obj1, obj);
    //        return s;
    //    }

    //    public static IEnumerable<SyncLock> TryLock(object obj)
    //    {
    //        if (Monitor.TryEnter(obj))
    //            using (SyncLock s = SyncLock.Alloc(obj))
    //                yield return s;
    //    }

    //    public void Unlock()
    //    {
    //        object obj1 = Interlocked.Exchange(ref this.obj1, null);
    //        if (obj1 != null)
    //            Monitor.Exit(obj1);
    //    }

    //    public void Lock()
    //    {
    //        object obj0 = Interlocked.CompareExchange(ref this.obj0, null, null);
    //        if (obj0 == null) return;
    //        if (Interlocked.CompareExchange(ref this.obj1, null, obj0) == null)
    //            Monitor.Enter(obj0);
    //    }

    //    object obj0;
    //    object obj1;

    //    SyncLock() { }

    //    void IDisposable.Dispose()
    //    {
    //        this.Unlock();
    //        Interlocked.Exchange(ref this.obj0, null);
    //        lock (p)
    //            if (p.Contains(this) == false)
    //                p.Enqueue(this);
    //    }
    //}

    public class SyncLock : IDisposable
    {
        private SyncLock() { }

        static Queue<SyncLock> _pooling = new Queue<SyncLock>();

        object _obj;
        object _Locked;
        object obj
        {
            get { return Interlocked.CompareExchange(ref this._obj, null, null); }
            set { Interlocked.Exchange(ref this._obj, value); }
        }
        public bool Locked
        {
            get { return Interlocked.CompareExchange(ref this._Locked, null, null) != null; }
            private set
            {
                if (value)
                    Interlocked.Exchange(ref this._Locked, this.obj);
                else
                    Interlocked.Exchange(ref this._Locked, null);
            }
        }

        public static IEnumerable<SyncLock> TryLock2(object obj)
        {
            using (SyncLock sync = SyncLock.TryLock(obj))
                if (sync != null)
                    yield return sync;
        }

        public static SyncLock TryLock(object obj)
        {
            if (obj == null) return null;
            if (Monitor.TryEnter(obj))
            {
                SyncLock sync;
                lock (_pooling)
                {
                    if (_pooling.Count > 0)
                        sync = _pooling.Dequeue();
                    else
                        sync = new SyncLock();
                }
                sync.obj = obj;
                Interlocked.Exchange(ref sync._Locked, obj);
                return sync;
            }
            return null;
        }

        public void Unlock()
        {
            object locked = Interlocked.Exchange(ref this._Locked, null);
            if (locked != null)
                Monitor.Exit(locked);
        }

        void IDisposable.Dispose()
        {
            object obj = Interlocked.Exchange(ref this._obj, null);
            this.Unlock();
            lock (_pooling) _pooling.Enqueue(this);
        }
    }
}