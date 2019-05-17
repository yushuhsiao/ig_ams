using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchange.Redis
{
    public class RedisDatabase
    {
        private object _sync = new object();
        private IDatabase _db;
        private bool _busy;

        private string GetConfiguration(string configuration, Func<string> getConfiguration)
        {
            try
            {
                return configuration ?? getConfiguration?.Invoke();
            }
            catch
            {
                SetIdle();
                throw;
            }
        }

        public IDatabase GetDatabase(string configuration = null, Func<string> getConfiguration = null)
        {
            for (; ; Thread.Sleep(1))
            {
                lock (_sync)
                {
                    if (_busy == false)
                    {
                        _busy = true;
                        if (_db != null)
                            return _db;
                        break;
                    }
                }
            }
            configuration = GetConfiguration(configuration, getConfiguration);
            lock (_sync)
            {
                if (_db == null)
                {
                    ConnectionMultiplexer multiplexer;
                    try
                    {
                        multiplexer = ConnectionMultiplexer.Connect(configuration);
                    }
                    catch
                    {
                        _busy = false;
                        throw;
                    }
                    _db = multiplexer.GetDatabase();
                }
                return _db;
            }
        }

        public async Task<IDatabase> GetDatabaseAsync(string configuration = null, Func<string> getConfiguration = null)
        {
            IDatabase db = null;
            for (; ; await Task.Delay(1))
            {
                lock (_sync)
                {
                    if (_busy == false)
                    {
                        _busy = true;
                        db = _db;
                        break;
                    }
                }
            }
            if (db != null)
                return await Task.FromResult(db);
            configuration = GetConfiguration(configuration, getConfiguration);
            try
            {
                var multiplexer = await ConnectionMultiplexer.ConnectAsync(configuration);
                lock (this)
                    return _db = multiplexer.GetDatabase();
            }
            catch
            {
                SetIdle();
                throw;
            }
        }

        public void SetIdle()
        {
            lock (_sync)
                _busy = false;
        }

        public void OnError()
        {
            lock (_sync)
            {
                using (_db?.Multiplexer)
                {
                    _db = null;
                    _busy = false;
                }
            }
        }

        public Exception OnError(ILogger logger, Exception ex, string message, params object[] args)
        {
            OnError();
            logger.LogError(ex, message, args);
            return ex;
        }
    }
}