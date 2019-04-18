namespace System.Threading
{
    public class AsyncResult : IAsyncResult
    {
        public static readonly AsyncResult NotCompleted = new AsyncResult() { CompletedSynchronously = false, IsCompleted = false };
        static readonly AsyncResult _01 = new AsyncResult() { CompletedSynchronously = false, IsCompleted = true };
        static readonly AsyncResult _10 = new AsyncResult() { CompletedSynchronously = true, IsCompleted = false };
        public static readonly AsyncResult Completed = new AsyncResult() { CompletedSynchronously = true, IsCompleted = true };
        public static AsyncResult Default(bool completedSynchronously, bool isCompleted)
        {
            if (completedSynchronously)
                return isCompleted ? Completed : _10;
            else
                return isCompleted ? _01 : NotCompleted;
        }

        public object AsyncState { get; set; }
        public WaitHandle AsyncWaitHandle { get; set; }
        public bool CompletedSynchronously { get; set; }
        public bool IsCompleted { get; set; }
    }
}
