using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using _DebuggerStepThroughAttribute = System.Diagnostics.DebuggerStepThroughAttribute;
using Microsoft.Extensions.Logging;

namespace System.Threading
{
    [_DebuggerStepThrough]
    public static class Tick
    {
        private static ILogger _logger;

        public static bool WriteLog { get; set; } = false;
        private static void writelog(string msg)
        {
            //if (WriteLog)
            //    LoggerHelper.LoggerFactory.GetLogger("Tick").Log(LogLevel.Debug, 0, msg);
            _logger?.Log(LogLevel.Debug, 0, msg);
            //LoggerHelper.LoggerFactory.GetLogger("Tick").LogInformation(msg);
        }

        public delegate bool Handler();

        [_DebuggerStepThrough]
        class TickItem : TimeCounter, IDisposable
        {
            Handler _handler;
            TickAttribute _options;
            //int _maxThread;
            //double _interval;
            int thread_count;

            public TickItem() : base(false) { }

            //public int IncrementCount() => Interlocked.Increment(ref thread_count);
            //public int DecrementCount() => Interlocked.Decrement(ref thread_count);
            public Handler Handler => Interlocked.CompareExchange(ref _handler, null, null);
            public TickAttribute Options => Interlocked.CompareExchange(ref _options, null, null);
            //public int MaxThread => Interlocked.CompareExchange(ref _maxThread, 0, 0);
            //public double Interval => Interlocked.CompareExchange(ref _interval, 0, 0);
            public static bool Alloc(Handler handler, int maxThread, double interval)
            {
                if (handler == null)
                    return false;
                TickAttribute options;
                lock (Tick._options)
                {
                    if (!Tick._options.TryGetValue(handler.Method, out options))
                        options = Tick._options[handler.Method] = handler.Method.GetCustomAttribute<TickAttribute>() ?? new TickAttribute();
                }
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
                    item.Reset();
                    Interlocked.Exchange(ref item._options, options);
                    //Interlocked.Exchange(ref item._interval, interval);
                    //Interlocked.Exchange(ref item._maxThread, maxThread);
                    Interlocked.Exchange(ref item._handler, handler);
                    _items.Add(item);
                }
                return true;
            }
            void IDisposable.Dispose() => Interlocked.Exchange(ref _handler, null);

            //public void Invoke()
            //{
            //    try { if ((this.Handler ?? _null.noop<bool>)()) return; }
            //    catch (Exception ex) { writelog(ex.Message); }
            //    using (this) return;
            //}

            public void Invoke()
            {
                try
                {
                    int thread_count = Interlocked.Increment(ref this.thread_count);
                    Handler handler = this.Handler;
                    if (handler == null)
                    {
                        lock (_items)
                        {
                            if (!_pooling.Contains(this))
                                _pooling.Enqueue(this);
                            return;
                        }
                    }
                    else
                    {
                        TickAttribute options = this.Options;
                        //int maxThread = this.MaxThread;
                        //double interval = this.Interval;
                        if ((options.MaxThread == 1) && (options.Interval > 0) && (false == this.IsTimeout(options.Interval)))
                            return;
                        else if ((options.MaxThread > 0) && (options.MaxThread < thread_count))
                            return;
                        Tick.writelog($"{thread_count}\t{DateTime.Now.ToString(StringFormatWith.DateTimeFormatEx)}");
                        try
                        {
                            bool next = handler();
                            if (false == next)
                                using (this)
                                    return;
                        }
                        catch { }
                        finally
                        {
                            this.Reset();
                        }
                    }
                }
                finally
                {
                    Interlocked.Decrement(ref this.thread_count);
                }
            }
        }

        static Dictionary<MethodInfo, TickAttribute> _options = new Dictionary<MethodInfo, TickAttribute>();
        static Queue<TickItem> _pooling = new Queue<TickItem>();
        static List<TickItem> _items = new List<TickItem>();
        static int tick_index;
        static int thread_count;

        static Timer timer1 = new Timer(tick_proc, null, 1, 1);
        static void tick_proc(object state)
        {
            if (Monitor.TryEnter(_items))
            {
                TickItem item;
                try
                {
                    Interlocked.Increment(ref thread_count);
                    int n = Interlocked.Increment(ref tick_index);
                    n &= 0x7fffffff;
                    n %= _items.Count;
                    item = _items[n];
                }
                finally
                {
                    Interlocked.Decrement(ref thread_count);
                    Monitor.Exit(_items);
                }
                item.Invoke();
            }
        }
        //static void tick_proc1(object state)
        //{
        //    bool locked = false;
        //    TickItem item = null;
        //    try
        //    {
        //        Interlocked.Increment(ref thread_count);
        //        Monitor.TryEnter(_items, ref locked);
        //        int cnt1 = _items.Count;
        //        if (locked && cnt1 > 0)
        //        {
        //            int n = Interlocked.Increment(ref tick_index);
        //            n &= 0x7fffffff;
        //            n %= cnt1;
        //            item = _items[n];
        //            int cnt2 = item.IncrementCount();
        //            if (item.Handler == null)
        //            {
        //                _items.RemoveAt(n);
        //                _pooling.Enqueue(item);
        //            }
        //            else
        //            {
        //                int max = item.MaxThread;
        //                if (max <= 0 || cnt2 <= max)
        //                {
        //                    Monitor.Exit(_items);
        //                    locked = false;
        //                    double interval = item.Interval;
        //                    if (interval > 0)
        //                        item.Timeout(interval, item.Invoke);
        //                    else
        //                        item.Invoke();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex) { writelog(ex.Message); }
        //    finally
        //    {
        //        item?.DecrementCount();
        //        if (locked) Monitor.Exit(_items);
        //        Interlocked.Decrement(ref thread_count);
        //    }
        //}

        //public static bool Add(Handler handler, int maxThread = 0, double interval = 0) => TickItem.Alloc(handler, maxThread, interval);

        public static event Handler OnTick
        {
            add { TickItem.Alloc(value, 1, 0); }
            remove
            {
                if (value == null)
                    return;
                lock (_items)
                {
                    for (int i = _items.Count - 1; i >= 0; i--)
                        if (object.ReferenceEquals(value, _items[i].Handler))
                            using (_items[i])
                                _items.RemoveAt(i);
                }
            }
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class TickAttribute : Attribute
    {
        public int MaxThread { get; set; } = 1;
        /// <summary>
        /// Active at MaxThread = 1
        /// </summary>
        public double Interval { get; set; }
    }
}