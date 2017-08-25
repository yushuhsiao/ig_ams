using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Web;

namespace casino
{
	partial class User
	{
        partial void Init_CommandQueue()
        {
            this.tick_timer = new Timer(tick_proc, null, 1, 1);
            this.CommandQueue = new _CommandQueue();
        }

        Timer tick_timer;
        void tick_proc(object state)
        {
            if (Interlocked.CompareExchange(ref this.alive, 2, 1) == 1)
            {
                using (tick_timer)
                    this.CommandQueue.ClearQueue();
            }
            else
            {
                CommandQueue.Tick();
            }
        }

        #region CommandQueue

        internal void QueueCommand(apiRoute route, apiRoute.Handler command)
        {
            this.CommandQueue.AddCommand(route, command);
        }

        readonly _CommandQueue CommandQueue;

        class _CommandQueue : Dictionary<apiRoute, Queue<apiRoute.Handler>>
        {
            const int MAX_THREAD = 1;
            Queue<apiRoute.Handler> _default = new Queue<apiRoute.Handler>();
            List<Queue<apiRoute.Handler>> _queue2 = new List<Queue<apiRoute.Handler>>();

            public _CommandQueue()
            {
                _queue2.Add(_default);
            }

            public void ClearQueue()
            {
                lock (this)
                {
                    foreach (var _q in this.Values)
                    {
                        while (_q.Count > 0)
                            using (_q.Dequeue())
                                continue;
                    }
                    this.Clear();
                    this._queue2.Clear();
                }
            }

            public void AddCommand(apiRoute route, apiRoute.Handler command)
            {
                lock (this)
                {
                    Queue<apiRoute.Handler> queue;
                    if (route == null)
                        queue = this._queue2[0];
                    else if (!this.TryGetValue(route, out queue))
                    {
                        queue = new Queue<apiRoute.Handler>();
                        this[route] = queue;
                        this._queue2.Add(queue);
                    }
                    queue.Enqueue(command);
                }
            }

            int tick_index;
            int thread_count;
            public void Tick()
            {
                try
                {
                    int t = Interlocked.Increment(ref thread_count);
                    if (t > MAX_THREAD) return;
                    Queue<apiRoute.Handler> queue;
                    if (!Monitor.TryEnter(this)) return;
                    apiRoute.Handler command;
                    try
                    {
                        if (this._queue2.Count == 0) return;
                        int node_index = Interlocked.Increment(ref this.tick_index);
                        node_index &= 0x7fffffff;
                        node_index %= this._queue2.Count;
                        queue = this._queue2[node_index];
                        if (queue.Count == 0) return;
                        command = queue.Dequeue();
                    }
                    catch { return; }
                    finally { Monitor.Exit(this); }
                    command.ProcessRequest();
                }
                finally
                {
                    Interlocked.Decrement(ref thread_count);
                }
            }
        }

        #endregion
    }
}