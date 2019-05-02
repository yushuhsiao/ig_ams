// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

using System.Runtime.CompilerServices;
using System;
using System.Diagnostics;
using System.Globalization;

namespace System.Threading.Tasks
{
    /// <summary>
    /// Helpers for safely using Task libraries. 
    /// </summary>
    public static class TaskHelpers
    {
        private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);
#if !NET40
        public static void RunSync(Action func, bool throwException = true)
        {
            RunSync(() =>
            {
                func();
                return TaskHelpers.FromResult<object>(null);
            }, throwException);
        }
        [DebuggerStepThrough]
        public static T RunSync<T>(Func<Task<T>> func, bool throwException = true)
        {
            CultureInfo cultureUi = CultureInfo.CurrentUICulture;
            CultureInfo culture = CultureInfo.CurrentCulture;
            return _myTaskFactory.StartNew<Task<T>>(() =>
            {
#if NET40
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = cultureUi;
#endif
                try { return func(); }
                catch { if (throwException) throw; }
                return TaskHelpers.FromResult(default(T));

            }).Unwrap<T>().GetAwaiter().GetResult();
        }
#endif
        //public static T Wait<T>(Task<T> task)
        //{
        //    task.Wait();
        //    return task.Result;
        //}

        public static Task<TResult> FromResult<TResult>(TResult result = default(TResult))
        {
#if NET40
            TaskCompletionSource<TResult> t = new TaskCompletionSource<TResult>();
            t.SetResult(result);
            return t.Task;
#else
            return Task.FromResult<TResult>(result);
#endif
        }

        private static readonly Task _defaultCompleted = TaskHelpers.FromResult<AsyncVoid>(default(AsyncVoid));

        private static readonly Task<object> _completedTaskReturningNull = TaskHelpers.FromResult<object>(null);

        /// <summary>
        /// Returns a canceled Task. The task is completed, IsCanceled = True, IsFaulted = False.
        /// </summary>
        public static Task Canceled() => CancelCache<AsyncVoid>.Canceled;

        /// <summary>
        /// Returns a canceled Task of the given type. The task is completed, IsCanceled = True, IsFaulted = False.
        /// </summary>
        public static Task<TResult> Canceled<TResult>() => CancelCache<TResult>.Canceled;

        /// <summary>
        /// Returns a completed task that has no result. 
        /// </summary>        
        public static Task Completed()
        {
            return _defaultCompleted;
        }

#if NET40 || NET452
        public static Task CompletedTask => _defaultCompleted;
#else
        public static Task CompletedTask => Task.CompletedTask;
#endif

        /// <summary>
        /// Returns an error task. The task is Completed, IsCanceled = False, IsFaulted = True
        /// </summary>
        public static Task FromError(Exception exception)
        {
            return FromError<AsyncVoid>(exception);
        }

        /// <summary>
        /// Returns an error task of the given type. The task is Completed, IsCanceled = False, IsFaulted = True
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        public static Task<TResult> FromError<TResult>(Exception exception)
        {
            TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
            tcs.SetException(exception);
            return tcs.Task;
        }

        public static Task<object> NullResult()
        {
            return _completedTaskReturningNull;
        }

        /// <summary>
        /// Used as the T in a "conversion" of a Task into a Task{T}
        /// </summary>
        private struct AsyncVoid
        {
        }

        /// <summary>
        /// This class is a convenient cache for per-type cancelled tasks
        /// </summary>
        private static class CancelCache<TResult>
        {
            public static readonly Task<TResult> Canceled = GetCancelledTask();

            private static Task<TResult> GetCancelledTask()
            {
                TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();
                tcs.SetCanceled();
                return tcs.Task;
            }
        }
    }
}
