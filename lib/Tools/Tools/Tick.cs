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

        static List<Handler> item1 = new List<Handler>();
        static List<Handler> item2 = new List<Handler>();
        static int tick_index;

        static Timer timer = new Timer(tick_proc, null, 1, 1);
        static void tick_proc(object state)
        {
            //for (int i = 0; i < 1; i++)
            {
                try
                {
                    if (!tick_proc())
                        return;
                }
                finally
                {
                    Thread.Sleep(1);
#if NET40
                    Thread.SpinWait(1);
#endif
                }
            }
        }
        static bool tick_proc()
        {
            if (!Monitor.TryEnter(item1))
                return false;
            Handler item;
            bool single_thread;
            try
            {
                if (item1.Count == 0) return false;
                int n = Interlocked.Increment(ref tick_index);
                n &= 0x7fffffff;
                n %= item1.Count;
                item = item1[n];
                single_thread = item2.Contains(item);
            }
            finally { Monitor.Exit(item1); }
            try
            {
                if (single_thread)
                {
                    if (Monitor.TryEnter(item))
                        try { if (item()) return true; }
                        finally { Monitor.Exit(item); }
                }
                else
                {
                    if (item()) return true;
                }
            }
            catch { }
            Tick.OnTick -= item;
            return true;
        }

        public static void Add(Handler handler, bool single_thread)
        {
            if (handler == null)
                return;
            lock (item1)
            {
                if (item1.Contains(handler))
                    return;
                item1.Add(handler);
                if (single_thread)
                    item2.Add(handler);
            }

        }

        public static event Handler OnTick
        {
            add { Add(value, false); }
            remove
            {
                if (value == null)
                    return;
                lock (item1)
                {
                    while (item1.Remove(value)) continue;
                    while (item2.Remove(value)) continue;
                }
            }
        }
    }
}