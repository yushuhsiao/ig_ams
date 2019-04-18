using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Tools
{
    [DebuggerStepThrough]
    public static class Tick
    {
        public delegate bool Handler();

        static Queue<Handler> queue = new Queue<Handler>();
        static List<Handler> all = new List<Handler>();

        static Timer timer = new Timer(tick_proc, null, 1, 1);
        static void tick_proc(object state)
        {
            for (int i = 0; i < 1000; i++)
            {
                if (!Monitor.TryEnter(all))
                    return;
                Handler item;
                try
                {
                    if (queue.Count == 0)
                        return;
                    item = queue.Dequeue();
                    if (all.Contains(item))
                        queue.Enqueue(item);
                    else
                        return;
                }
                finally { Monitor.Exit(all); }

                try
                {
                    if (item())
                        return;
                }
                catch { }
                Tick.OnTick -= item;
            }
        }

        public static event Handler OnTick
        {
            add
            {
                if (value == null)
                    return;
                lock (all)
                    if (all.Contains(value))
                        return;
                    else
                        all.Add(value);
                lock (queue)
                    queue.Enqueue(value);
            }
            remove
            {
                if (value == null)
                    return;
                lock (all)
                    while (all.Remove(value)) ;
            }
        }
    }

    //    static class Tick
    //{
    //    static Queue<ThreadStart> ticks = new Queue<ThreadStart>();
    //    static List<ThreadStart> stop = new List<ThreadStart>();
    //    static System.Threading.Timer timer = new System.Threading.Timer(proc, null, 1, 1);
    //    static void proc(object state)
    //    {
    //        for (int i = 0; i < 100; i++)
    //        {
    //            ThreadStart tick;
    //            lock (ticks)
    //            {
    //                if (ticks.Count == 0) return;
    //                tick = ticks.Dequeue();
    //            }

    //            lock (stop)
    //            {
    //                if (stop.Contains(tick))
    //                {
    //                    while (stop.Remove(tick)) { }
    //                    return;
    //                }
    //            }

    //            try { tick(); }
    //            catch { }
    //            finally { lock (ticks) ticks.Enqueue(tick); }
    //        }
    //    }

    //    public static event ThreadStart OnTick
    //    {
    //        add { if (value != null) lock (ticks) ticks.Enqueue(value); }
    //        remove { if (value != null) lock (stop) stop.Add(value); }
    //    }

    //    public static class Timer
    //    {
    //        class item : EventWaitHandle
    //        {
    //            public item() : base(false, EventResetMode.AutoReset) { }

    //            WaitOrTimerCallback callback;

    //            static int msgID_alloc = 0;

    //            int msgID;
    //            public int? Register(WaitOrTimerCallback callback, int millisecondsTimeOutInterval)
    //            {
    //                if (Interlocked.CompareExchange(ref this.callback, callback, null) == null)
    //                {
    //                    ThreadPool.RegisterWaitForSingleObject(this, proc, null, millisecondsTimeOutInterval, true);
    //                    int msgID = Interlocked.Increment(ref item.msgID_alloc);
    //                    Interlocked.Exchange(ref this.msgID, msgID);
    //                    return msgID;
    //                }
    //                return null;
    //            }

    //            public void proc(object state, bool timedOut)
    //            {
    //                WaitOrTimerCallback callback = Interlocked.Exchange(ref this.callback, null);
    //                if (callback != null)
    //                    callback(state, timedOut);
    //            }

    //            public int MsgID
    //            {
    //                get { return Interlocked.CompareExchange(ref this.msgID, 0, 0); }
    //            }
    //        }

    //        static List<item> items = new List<item>();

    //        public static int Register(WaitOrTimerCallback callback, int millisecondsTimeOutInterval)
    //        {
    //            while (true)
    //            {
    //                lock (items)
    //                {
    //                    foreach (item n in items)
    //                    {
    //                        int? msgID = n.Register(callback, millisecondsTimeOutInterval);
    //                        if (msgID.HasValue)
    //                            return msgID.Value;
    //                    }
    //                    item item = new item();
    //                    items.Add(item);
    //                    return item.Register(callback, millisecondsTimeOutInterval).Value;
    //                }
    //            }
    //        }

    //        public static void SendState(int msgID, object state)
    //        {
    //            item item = null;
    //            lock (items)
    //            {
    //                foreach (item n in items)
    //                {
    //                    if (n.MsgID == msgID)
    //                    {
    //                        item = n;
    //                        break;
    //                    }
    //                }
    //            }
    //            if (item != null)
    //            {
    //                item.proc(state, false);
    //                item.Set();
    //            }
    //        }
    //        public static void SendState(WaitOrTimerCallback callback, object state)
    //        {
    //        }

    //        public static void Unregister(int msgID)
    //        {
    //        }
    //        public static void Unregister(WaitOrTimerCallback callback)
    //        {
    //        }
    //    }


    //    //public static class Wait
    //    //{
    //    //    static int MsgID = 0;
    //    //    static int Index = 0;

    //    //    class WaitItem : EventWaitHandle
    //    //    {
    //    //        public readonly int MsgID = Interlocked.Increment(ref Wait.MsgID);
    //    //        public WaitItem() : base(false, EventResetMode.AutoReset) { }
    //    //        public WaitOrTimerCallback Callback;
    //    //        public int Index;

    //    //        public void proc(object state, bool timedOut)
    //    //        {
    //    //            int cnt = (int)state;
    //    //            lock (this)
    //    //                if (this.Index != cnt)
    //    //                    return;
    //    //            SendState(null, timedOut);
    //    //            lock (pool) pool.Enqueue(this);
    //    //        }

    //    //        public void SendState(object state, bool timedOut)
    //    //        {
    //    //            WaitOrTimerCallback callback;
    //    //            lock (this)
    //    //            {
    //    //                callback = this.Callback;
    //    //                this.Callback = null;
    //    //                this.Set();
    //    //            }
    //    //            if (callback == null)
    //    //                return;
    //    //            try { callback(state, timedOut); }
    //    //            catch { }
    //    //        }
    //    //    }

    //    //    static Queue<WaitItem> pool = new Queue<WaitItem>();
    //    //    static List<WaitItem> items = new List<WaitItem>();

    //    //    static WaitItem GetItem(int msgID)
    //    //    {
    //    //        lock (items)
    //    //            foreach (WaitItem item in items)
    //    //                if (item.MsgID == msgID)
    //    //                    return item;
    //    //        return null;
    //    //    }

    //    //    public static int Register(WaitOrTimerCallback callback, int millisecondsTimeOutInterval)
    //    //    {
    //    //        WaitItem item;
    //    //        lock (pool)
    //    //            if (pool.Count > 0)
    //    //                item = pool.Dequeue();
    //    //        lock (items)
    //    //            items.Add(item = new WaitItem());
    //    //        lock (item)
    //    //        {
    //    //            item.Callback = callback;
    //    //            item.Index = Interlocked.Increment(ref Wait.Index);
    //    //            ThreadPool.RegisterWaitForSingleObject(item, item.proc, item.Index, millisecondsTimeOutInterval, true);
    //    //        }
    //    //        return item.MsgID;
    //    //    }

    //    //    public static void ChangeTimeout(int msgID, int millisecondsTimeOutInterval)
    //    //    {
    //    //        WaitItem item = GetItem(msgID);
    //    //        if (item == null) return;
    //    //        lock (item)
    //    //        {
    //    //            item.Index = Interlocked.Increment(ref Wait.Index);
    //    //            item.Set();
    //    //            ThreadPool.RegisterWaitForSingleObject(item, item.proc, item.Index, millisecondsTimeOutInterval, true);
    //    //        }
    //    //    }


    //    //    public static void SendState(int msgID, object state)
    //    //    {
    //    //        WaitItem item = GetItem(msgID);
    //    //        if (item == null) return;
    //    //        item.SendState(state, false);
    //    //    }

    //    //    public static void CancelWait(int msgID)
    //    //    {
    //    //        WaitItem item = GetItem(msgID);
    //    //        if (item == null) return;
    //    //        item.Set();
    //    //    }
    //    //}
    //}
}