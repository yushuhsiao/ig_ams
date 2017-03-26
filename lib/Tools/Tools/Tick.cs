using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace Tools
{
    //[DebuggerStepThrough]
    public static class Tick
    {
        public delegate bool Handler();

        //[DebuggerStepThrough]
        class TickItem : TimeCounter, IDisposable
        {
            Handler _handler;
            int _maxThread;
            double _interval;
            int thread_count;

            public TickItem() : base(false) { }

            public int IncrementCount() => Interlocked.Increment(ref thread_count);
            public int DecrementCount() => Interlocked.Decrement(ref thread_count);
            public Handler Handler => Interlocked.CompareExchange(ref _handler, null, null);
            public int MaxThread => Interlocked.CompareExchange(ref _maxThread, 0, 0);
            public double Interval => Interlocked.CompareExchange(ref _interval, 0, 0);
            public static bool Alloc(Handler handler, int maxThread, double interval)
            {
                if (handler == null)
                    return false;
                lock (_items)
                {
                    //for (int i = _items.Count - 1; i >= 0; i--)
                    //    if (object.ReferenceEquals(handler, _items[i].Handler))
                    //        return false;
                    TickItem item;
                    if (_pooling.Count == 0)
                        item = new TickItem();
                    else
                        item = _pooling.Dequeue();
                    item.Enabled = true;
                    item.SetTimeout(-1);
                    Interlocked.Exchange(ref item._interval, interval);
                    Interlocked.Exchange(ref item._maxThread, maxThread);
                    Interlocked.Exchange(ref item._handler, handler);
                    _items.Add(item);
                }
                return true;
            }
            void IDisposable.Dispose() => Interlocked.Exchange(ref _handler, null);

            public void Invoke()
            {
                try { if ((this.Handler ?? _null.noop<bool>)()) return; }
                catch (Exception ex) { log.message("tick", ex.Message); }
                using (this) return;
            }
        }

        static Queue<TickItem> _pooling = new Queue<TickItem>();
        static List<TickItem> _items = new List<TickItem>();
        static int tick_index;
        static int thread_count;

        static Timer timer1 = new Timer(tick_proc, null, 1, 1);
        static void tick_proc(object state)
        {
            bool locked = false;
            TickItem item = null;
            try
            {
                Interlocked.Increment(ref thread_count);
                Monitor.TryEnter(_items, ref locked);
                int cnt1 = _items.Count;
                if (locked && cnt1 > 0)
                {
                    int n = Interlocked.Increment(ref tick_index);
                    n &= 0x7fffffff;
                    n %= cnt1;
                    item = _items[n];
                    int cnt2 = item.IncrementCount();
                    if (item.Handler == null)
                    {
                        _items.RemoveAt(n);
                        _pooling.Enqueue(item);
                    }
                    else
                    {
                        int max = item.MaxThread;
                        if (max <= 0 || cnt2 <= max)
                        {
                            Monitor.Exit(_items);
                            locked = false;
                            double interval = item.Interval;
                            if (interval > 0)
                                item.Timeout(interval, item.Invoke);
                            else
                                item.Invoke();
                        }
                    }
                }
            }
            catch (Exception ex) { log.message("tick", ex.Message); }
            finally
            {
                item?.DecrementCount();
                if (locked) Monitor.Exit(_items);
                Interlocked.Decrement(ref thread_count);
            }
        }

        public static bool Add(Handler handler, int maxThread = 0, double interval = 0) => TickItem.Alloc(handler, maxThread, interval);

        public static event Handler OnTick
        {
            add { TickItem.Alloc(value, 0, 0); }
            remove
            {
                if (value == null)
                    return;
                lock (_items)
                {
                    for (int i = _items.Count - 1; i >= 0; i--)
                        if (object.ReferenceEquals(value, _items[i].Handler))
                            using (_items[i])
                                continue;
                }
            }
        }
    }
}