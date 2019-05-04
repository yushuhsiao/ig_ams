using System.Diagnostics;

namespace System.Threading
{
    [DebuggerStepThrough]
    public static class _Monitor
    {
        public static void ThreadSleep(object obj, int millisecondsTimeout, ref bool lockTaken)
        {
            if (lockTaken)
            {
                _Monitor.ExitN(obj, ref lockTaken);
                Thread.Sleep(millisecondsTimeout);
                _Monitor.EnterN(obj, ref lockTaken);
            }
            else
                Thread.Sleep(millisecondsTimeout);
        }

        public static bool EnterN(object obj, ref bool lockTaken)
        {
            if (false == lockTaken)
                Monitor.Enter(obj, ref lockTaken);
            return lockTaken;
        }

        //public static bool EnterN(object obj, ref bool lockTaken, out double timesWait)
        //{
        //    DateTime t = DateTime.Now;
        //    if (lockTaken == false)
        //        Monitor.Enter(obj, ref lockTaken);
        //    timesWait = (DateTime.Now - t).TotalMilliseconds;
        //    return lockTaken;
        //}

        public static bool EnterN(object obj, int millisecondsTimeout, ref bool lockTaken, out int retyrCount)
        {
            for (retyrCount = 0; lockTaken == false; retyrCount++)
                Monitor.TryEnter(obj, millisecondsTimeout, ref lockTaken);
            return lockTaken;
        }

        public static bool EnterN(object obj, int millisecondsTimeout, ref bool lockTaken) => EnterN(obj, millisecondsTimeout, ref lockTaken, out int count);

        public static bool TryEnterN(object obj, ref bool lockTaken)
        {
            if (false == lockTaken)
                Monitor.TryEnter(obj, ref lockTaken);
            return lockTaken;
        }

        public static bool TryEnterN(object obj, int millisecondsTimeout, ref bool lockTaken)
        {
            if (false == lockTaken)
                Monitor.TryEnter(obj, millisecondsTimeout, ref lockTaken);
            return lockTaken;
        }



        /// <summary> 取得指定物件的獨佔鎖定。</summary>
        /// <param name="obj">要從其上取得監視器鎖定的物件。</param>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        public static bool Enter(object obj)
        {
            Monitor.Enter(obj);
            return true;
        }

        /// <summary>取得指定之物件的獨佔鎖定，並且完整設定值，指出是否採用鎖定。</summary>
        /// <param name="obj">要等候的物件。</param>
        /// <param name="lockTaken">嘗試取得鎖定的結果 (以傳址方式傳遞)。輸入必須是 false。如果已取得鎖定，輸出就是 true，否則輸出為 false。嘗試取得鎖定期間，即使發生例外狀況，仍然會設定輸出。Note如果沒有發生例外狀況，這個方法的輸出永遠是true。</param>
        /// <exception cref="System.ArgumentException">lockTaken 的輸入為 true。</exception>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        public static bool Enter(object obj, ref bool lockTaken)
        {
            Monitor.Enter(obj, ref lockTaken);
            return lockTaken;
        }

        //public static void Enter(object obj, out bool lockTaken)
        //{
        //    lockTaken = false;
        //    Monitor.Enter(obj, ref lockTaken);
        //}

#if NETCORE

        /// <summary>判斷目前執行緒是否保持鎖定指定的物件。</summary>
        /// <param name="obj">要測試的物件。</param>
        /// <returns>如果目前的執行緒持有 obj 的鎖定，則為 true；否則為 false。</returns>
        /// <exception cref="System.ArgumentNullException">obj 為 null。</exception>
        public static bool IsEntered(object obj) => Monitor.IsEntered(obj);
#endif
        /// <summary>釋出指定物件的獨佔鎖定。</summary>
        /// <param name="obj">要從其上釋出鎖定的物件。</param>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">目前執行緒沒有指定物件的鎖定。</exception>
        public static bool Exit(object obj)
        {
            Monitor.Exit(obj);
            return false;
        }

        /// <summary>釋出指定物件的獨佔鎖定。</summary>
        /// <param name="obj">要從其上釋出鎖定的物件。</param>
        /// <param name="lockTaken">嘗試取得鎖定的結果 (以傳址方式傳遞)。Note如果沒有發生例外狀況，這個方法的輸出永遠是false。</param>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">目前執行緒沒有指定物件的鎖定。</exception>
        public static bool Exit(object obj, ref bool lockTaken)
        {
            Monitor.Exit(obj);
            return lockTaken = false;
        }

        /// <summary>通知等候佇列中的執行緒，鎖定物件的狀態有所變更。</summary>
        /// <param name="obj">執行緒正等候的物件。</param>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">呼叫執行緒沒有指定物件的鎖定。</exception>
        public static void Pulse(object obj) => Monitor.Pulse(obj);

        /// <summary>通知所有等候中的執行緒，物件的狀態有所變更。</summary>
        /// <param name="obj">送出 Pulse 的物件。</param>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">呼叫執行緒沒有指定物件的鎖定。</exception>
        public static void PulseAll(object obj) => Monitor.PulseAll(obj);

        /// <summary>嘗試取得指定物件的獨佔鎖定。</summary>
        /// <param name="obj">要取得鎖定的物件。</param>
        /// <returns>如果目前執行緒取得鎖定，則為 true，否則為 false。</returns>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        public static bool TryEnter(object obj) => Monitor.TryEnter(obj);

        /// <summary>嘗試取得指定之物件的獨佔鎖定，並且完整設定值，指出是否採用鎖定。</summary>
        /// <param name="obj">要取得鎖定的物件。</param>
        /// <param name="lockTaken">嘗試取得鎖定的結果 (以傳址方式傳遞)。輸入必須是 false。如果已取得鎖定，輸出就是 true，否則輸出為 false。嘗試取得鎖定期間，即使發生例外狀況，仍然會設定輸出。</param>
        /// <exception cref="System.ArgumentException">lockTaken 的輸入為 true。</exception>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        public static bool TryEnter(object obj, ref bool lockTaken)
        {
            Monitor.TryEnter(obj, ref lockTaken);
            return lockTaken;
        }

        /// <summary>嘗試取得指定物件的獨佔鎖定 (在指定的毫秒數時間內)。</summary>
        /// <param name="obj">要取得鎖定的物件。</param>
        /// <param name="millisecondsTimeout">等候鎖定的毫秒數。</param>
        /// <returns>如果目前執行緒取得鎖定，則為 true，否則為 false。</returns>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">millisecondsTimeout 為負，且不等於 System.Threading.Timeout.Infinite。</exception>
        public static bool TryEnter(object obj, int millisecondsTimeout) => Monitor.TryEnter(obj, millisecondsTimeout);

        /// <summary>嘗試取得指定物件的獨佔鎖定 (在指定的時間內)。</summary>
        /// <param name="obj">要取得鎖定的物件。</param>
        /// <param name="timeout">System.TimeSpan，代表等候鎖定的時間量。-1 毫秒的值會指定無限期等候。</param>
        /// <returns>如果目前執行緒不封鎖而取得鎖定，則為 true，否則為 false。</returns>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">timeout 的毫秒值為負且不等於 System.Threading.Timeout.Infinite (-1 毫秒) 或大於 System.Int32.MaxValue。</exception>
        public static bool TryEnter(object obj, TimeSpan timeout) => Monitor.TryEnter(obj, timeout);

        /// <summary>嘗試在指定的毫秒數內取得指定之物件的獨佔鎖定，並且完整設定值，指出是否採用鎖定。/// </summary>
        /// <param name="obj">要取得鎖定的物件。</param>
        /// <param name="millisecondsTimeout">等候鎖定的毫秒數。</param>
        /// <param name="lockTaken">嘗試取得鎖定的結果 (以傳址方式傳遞)。輸入必須是 false。如果已取得鎖定，輸出就是 true，否則輸出為 false。嘗試取得鎖定期間，即使發生例外狀況，仍然會設定輸出。</param>
        /// <exception cref="System.ArgumentException">lockTaken 的輸入為 true。</exception>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">millisecondsTimeout 為負，且不等於 System.Threading.Timeout.Infinite。</exception>
        public static bool TryEnter(object obj, int millisecondsTimeout, ref bool lockTaken)
        {
            Monitor.TryEnter(obj, millisecondsTimeout, ref lockTaken);
            return lockTaken;
        }

        /// <summary>嘗試在指定的時間內取得指定之物件的獨佔鎖定，並且完整設定值，指出是否採用鎖定。</summary>
        /// <param name="obj">要取得鎖定的物件。</param>
        /// <param name="timeout">等候鎖定的時間長度。-1 毫秒的值會指定無限期等候。</param>
        /// <param name="lockTaken">嘗試取得鎖定的結果 (以傳址方式傳遞)。輸入必須是 false。如果已取得鎖定，輸出就是 true，否則輸出為 false。嘗試取得鎖定期間，即使發生例外狀況，仍然會設定輸出。</param>
        /// <exception cref="System.ArgumentException">lockTaken 的輸入為 true。</exception>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">timeout 的毫秒值為負且不等於 System.Threading.Timeout.Infinite (-1 毫秒) 或大於 System.Int32.MaxValue。</exception>
        public static void TryEnter(object obj, TimeSpan timeout, ref bool lockTaken) => Monitor.TryEnter(obj, timeout, ref lockTaken);

        public static bool ExitN(object obj, ref bool lockTaken)
        {
            if (lockTaken) _Monitor.Exit(obj);
            return lockTaken = false;
        }

        /// <summary>釋出物件的鎖並且封鎖目前的執行緒，直到這個執行緒重新取得鎖定為止。</summary>
        /// <param name="obj">要等候的物件。</param>
        /// <returns>如果因為呼叫端重新取得指定物件的鎖定所以呼叫被傳回，則為 true。如果鎖定不被重新取得，則這個方法不會傳回。</returns>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">呼叫執行緒沒有指定物件的鎖定。</exception>
        /// <exception cref="System.Threading.ThreadInterruptedException">叫用 Wait 的執行緒稍後會從等候狀態被插斷。這會當另一個執行緒呼叫這個執行緒的 System.Threading.Thread.Interrupt 方法時發生。</exception>
        public static bool Wait(object obj) => Monitor.Wait(obj);

        /// <summary>釋出物件的鎖並且封鎖目前的執行緒，直到這個執行緒重新取得鎖定為止。如果超過指定的逾時間隔時間，執行緒會進入就緒序列。</summary>
        /// <param name="obj">要等候的物件。</param>
        /// <param name="millisecondsTimeout">在執行緒進入就緒序列之前要等候的毫秒數。</param>
        /// <returns>如果在經過指定的時間之前重新取得鎖定則為 true，如果在經過指定的時間之後重新取得鎖定則為 false。要等到重新取得鎖定之後，此方法才會傳回。</returns>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">呼叫執行緒沒有指定物件的鎖定。</exception>
        /// <exception cref="System.Threading.ThreadInterruptedException">叫用 Wait 的執行緒稍後會從等候狀態被插斷。這會當另一個執行緒呼叫這個執行緒的 System.Threading.Thread.Interrupt 方法時發生。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">millisecondsTimeout 參數的值為負，且不等於 System.Threading.Timeout.Infinite。</exception>
        public static bool Wait(object obj, int millisecondsTimeout) => Monitor.Wait(obj, millisecondsTimeout);

        /// <summary>釋出物件的鎖並且封鎖目前的執行緒，直到這個執行緒重新取得鎖定為止。如果超過指定的逾時間隔時間，執行緒會進入就緒序列。</summary>
        /// <param name="obj">要等候的物件。</param>
        /// <param name="timeout">System.TimeSpan 表示在執行緒進入就緒序列之前要等候的時間量。</param>
        /// <returns>如果在經過指定的時間之前重新取得鎖定則為 true，如果在經過指定的時間之後重新取得鎖定則為 false。要等到重新取得鎖定之後，此方法才會傳回。</returns>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">呼叫執行緒沒有指定物件的鎖定。</exception>
        /// <exception cref="System.Threading.ThreadInterruptedException">叫用 Wait 的執行緒稍後會從等候狀態被插斷。這會當另一個執行緒呼叫這個執行緒的 System.Threading.Thread.Interrupt 方法時發生。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">timeout 參數的毫秒值為負，且不表示 System.Threading.Timeout.Infinite (-1 毫秒)，或大於 System.Int32.MaxValue。</exception>
        public static bool Wait(object obj, TimeSpan timeout) => Monitor.Wait(obj, timeout);

#if netfx
        /// <summary>釋出物件的鎖並且封鎖目前的執行緒，直到這個執行緒重新取得鎖定為止。如果超過指定的逾時間隔時間，執行緒會進入就緒序列。這個方法也會指定等候之前和重新取得之後，是否要離開內容(Context) 的同步處理領域 (如果在同步化內容中的話)。</summary>
        /// <param name="obj">要等候的物件。</param>
        /// <param name="millisecondsTimeout">在執行緒進入就緒序列之前要等候的毫秒數。</param>
        /// <param name="exitContext">在等候前離開內容的同步化領域 (如果在同步化內容中) 並重新取得它，則為 true，否則為 false。</param>
        /// <returns>如果在經過指定的時間之前重新取得鎖定則為 true，如果在經過指定的時間之後重新取得鎖定則為 false。要等到重新取得鎖定之後，此方法才會傳回。</returns>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">Wait 不是從同步化的程式碼區塊中叫用出來的。</exception>
        /// <exception cref="System.Threading.ThreadInterruptedException">叫用 Wait 的執行緒稍後會從等候狀態被插斷。這會當另一個執行緒呼叫這個執行緒的 System.Threading.Thread.Interrupt 方法時發生。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">millisecondsTimeout 參數的值為負，且不等於 System.Threading.Timeout.Infinite。</exception>
        public static bool Wait(object obj, int millisecondsTimeout, bool exitContext) => Monitor.Wait(obj, millisecondsTimeout, exitContext);

        /// <summary>釋出物件的鎖並且封鎖目前的執行緒，直到這個執行緒重新取得鎖定為止。如果超過指定的逾時間隔時間，執行緒會進入就緒序列。在等候之前和重新取得領域之後，可選擇性地結束同步化內容的同步處理領域。</summary>
        /// <param name="obj">要等候的物件。</param>
        /// <param name="timeout">System.TimeSpan 表示在執行緒進入就緒序列之前要等候的時間量。</param>
        /// <param name="exitContext">在等候前離開內容的同步化領域 (如果在同步化內容中) 並重新取得它，則為 true，否則為 false。</param>
        /// <returns>如果在經過指定的時間之前重新取得鎖定則為 true，如果在經過指定的時間之後重新取得鎖定則為 false。要等到重新取得鎖定之後，此方法才會傳回。</returns>
        /// <exception cref="System.ArgumentNullException">obj 參數為 null。</exception>
        /// <exception cref="System.Threading.SynchronizationLockException">Wait 不是從同步化的程式碼區塊中叫用出來的。</exception>
        /// <exception cref="System.Threading.ThreadInterruptedException">叫用 Wait 的執行緒稍後會從等候狀態被插斷。這會當另一個執行緒呼叫這個執行緒的 System.Threading.Thread.Interrupt 方法時發生。</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">timeout 參數為負，且不表示 System.Threading.Timeout.Infinite (-1 毫秒)，或大於 System.Int32.MaxValue。</exception>
        public static bool Wait(object obj, TimeSpan timeout, bool exitContext) => Monitor.Wait(obj, timeout, exitContext);
#endif
    }
}
