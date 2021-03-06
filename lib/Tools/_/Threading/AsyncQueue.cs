#if !NET40
// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace System.Collections.Generic
{
    /// <summary>
    /// A queue whose enqueue and dequeue operations can be performed in parallel.
    /// </summary>
    /// <typeparam name="TElement">The type of values kept by the queue.</typeparam>
    internal sealed class AsyncQueue<TElement>
    {
        private readonly TaskCompletionSource<bool> _whenCompleted = new TaskCompletionSource<bool>();

        // Note: All of the below fields are accessed in parallel and may only be accessed
        // when protected by lock (SyncObject)
        private readonly Queue<TElement> _data = new Queue<TElement>();
        private Queue<TaskCompletionSource<TElement>> _waiters;

        private object SyncObject
        {
            get { return _data; }
        }

        /// <summary>
        /// The number of unconsumed elements in the queue.
        /// </summary>
        public int Count
        {
            get
            {
                lock (SyncObject)
                {
                    return _data.Count;
                }
            }
        }

        /// <summary>
        /// Adds an element to the tail of the queue.  This method will throw if the queue 
        /// is completed.
        /// </summary>
        /// <exception cref="InvalidOperationException">The queue is already completed.</exception>
        /// <param name="value">The value to add.</param>
        public void Enqueue(TElement value)
        {
            if (!EnqueueCore(value))
            {
                throw new InvalidOperationException($"Cannot call {nameof(Enqueue)} when the queue is already completed.");
            }
        }

        /// <summary>
        /// Tries to add an element to the tail of the queue.  This method will return false if the queue
        /// is completed.
        /// </summary>
        /// <param name="value">The value to add.</param>
        public bool TryEnqueue(TElement value)
        {
            return EnqueueCore(value);
        }

        private bool EnqueueCore(TElement value)
        {
            TaskCompletionSource<TElement> waiter;
            lock (SyncObject)
            {
                if (_waiters == null || _waiters.Count == 0)
                {
                    _data.Enqueue(value);
                    return true;
                }

                Debug.Assert(_data.Count == 0);
                waiter = _waiters.Dequeue();
            }

            // Invoke SetResult on a separate task, as this invocation could cause the underlying task to executing,
            // which could be a long running operation that can potentially cause a deadlock if executed on the current thread.
            Task.Run(() => waiter.SetResult(value));

            return true;
        }

        /// <summary>
        /// Attempts to dequeue an existing item and return whether or not it was available.
        /// </summary>
        public bool TryDequeue(out TElement d)
        {
            lock (SyncObject)
            {
                if (_data.Count == 0)
                {
                    d = default(TElement);
                    return false;
                }

                d = _data.Dequeue();
                return true;
            }
        }

        /// <summary>
        /// Same operation as <see cref="AsyncQueue{TElement}.Complete"/> except it will not
        /// throw if the queue is already completed.
        /// </summary>
        /// <returns>Whether or not the operation succeeded.</returns>
        /// <summary>
        /// Gets a task that transitions to a completed state when <see cref="Complete"/> or
        /// <see cref="TryComplete"/> is called.  This transition will not happen synchronously.
        /// 
        /// This Task will not complete until it has completed all existing values returned
        /// from <see cref="DequeueAsync"/>.
        /// </summary>
        public Task WhenCompletedTask
        {
            get
            {
                return _whenCompleted.Task;
            }
        }

        /// <summary>
        /// Gets a task whose result is the element at the head of the queue. If the queue
        /// is empty, the returned task waits for an element to be enqueued. If <see cref="Complete"/> 
        /// is called before an element becomes available, the returned task is cancelled.
        /// </summary>
        //[PerformanceSensitive("https://github.com/dotnet/roslyn/issues/23582", OftenCompletesSynchronously = true)]
        public Task<TElement> DequeueAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return WithCancellation(DequeueAsyncCore(), cancellationToken);
        }

        /// <summary>
        /// 
        /// Note: The early cancellation behavior is intentional.
        /// </summary>
        //[PerformanceSensitive("https://github.com/dotnet/roslyn/issues/23582", OftenCompletesSynchronously = true)]
        private static Task<T> WithCancellation<T>(Task<T> task, CancellationToken cancellationToken)
        {
            if (task.IsCompleted || !cancellationToken.CanBeCanceled)
            {
                return task;
            }

            if (cancellationToken.IsCancellationRequested)
            {
                return new Task<T>(() => default(T), cancellationToken);
            }

            return task.ContinueWith(t => t, cancellationToken, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default).Unwrap();
        }

        //[PerformanceSensitive("https://github.com/dotnet/roslyn/issues/23582", OftenCompletesSynchronously = true)]
        private Task<TElement> DequeueAsyncCore()
        {
            lock (SyncObject)
            {
                // No matter what the state we allow DequeueAsync to drain the existing items 
                // in the queue.  This keeps the behavior in line with TryDequeue
                if (_data.Count > 0)
                {
                    return Task.FromResult(_data.Dequeue());
                }

                if (_waiters == null)
                {
                    _waiters = new Queue<TaskCompletionSource<TElement>>();
                }

                var waiter = new TaskCompletionSource<TElement>();
                _waiters.Enqueue(waiter);
                return waiter.Task;
            }
        }
    }
}
#endif