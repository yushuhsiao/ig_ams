using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace StackExchange.Redis
{

    public class RedisDatabaseWithLock : IDatabase
    {
        //public Func<string> GetConfiguration { get; set; }
        //public Func<int> GetDatabase { get; set; }
        //private readonly ILogger _logger;
        //private IDatabase _instance1; /// Use for <see cref="RedisSubscriber{TMessage}"/>
        //private IDatabase _instance2;

        //public RedisDatabaseWithLock()
        //{
        //    _logger = InitLogger(null);
        //}
        //public RedisDatabaseWithLock(IServiceProvider serviceProvider)
        //{
        //    this._logger = InitLogger(serviceProvider.GetService<ILogger<RedisDatabaseWithLock>>());
        //}
        //public RedisDatabaseWithLock(ILogger<RedisDatabaseWithLock> logger)
        //{
        //    this._logger = InitLogger(logger);
        //}

        //private ILogger InitLogger(ILogger logger) => logger ?? Microsoft.Extensions.Logging.Abstractions.NullLogger.Instance;

        //private IDatabase GetInstance()
        //{
        //    if (_instance1 != null)
        //        return _instance1;
        //    if (_instance2 == null)
        //        return this.Connect();
        //    else
        //        return _instance2;
        //}

        ///// Use for <see cref="RedisSubscriber{TMessage}"/>
        //internal bool InitInstance(ConnectionMultiplexer multiplexer, int index)
        //{
        //    lock (this)
        //    {
        //        if (_instance1 == null)
        //            _instance1 = multiplexer?.GetDatabase(index);
        //        else
        //        {
        //            if (_instance1.Multiplexer == multiplexer)
        //            {
        //                if (_instance1.Database != index)
        //                    _instance1 = multiplexer?.GetDatabase(index);
        //            }
        //            else
        //            {
        //                using (_instance1.Multiplexer)
        //                    _instance1 = multiplexer?.GetDatabase(index);
        //            }
        //        }
        //        return _instance1 != null;
        //    }
        //}

        //public IDatabase Connect(string configuration = null, int db = -1)
        //{
        //    lock (this)
        //    {
        //        try
        //        {
        //            configuration = configuration ?? GetConfiguration?.Invoke();
        //            if (db == -1)
        //                db = GetDatabase?.Invoke() ?? -1;
        //            if (string.IsNullOrEmpty(configuration))
        //                _logger.LogWarning($"Multiplexer : Configuration is empty");
        //            else
        //            {
        //                _logger.LogInformation($"Multiplexer : {configuration}...");
        //                using (_instance2?.Multiplexer)
        //                    _instance2 = ConnectionMultiplexer.Connect(configuration).GetDatabase(db);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Multiplexer");
        //            using (_instance2?.Multiplexer)
        //                _instance2 = null;
        //        }
        //        return _instance2;
        //    }
        //}

        //public void ResetMultiplexer()
        //{
        //    lock (this)
        //        using (_instance2?.Multiplexer)
        //            _instance2 = null;
        //}

        public IDatabase Instance { get; }

        public RedisDatabaseWithLock(IDatabase instance)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public ConnectionMultiplexer Multiplexer => this.Instance.Multiplexer;

        int IDatabase.Database => this.Instance.Database;

        public IBatch CreateBatch(object asyncState = null)
        {
            lock (this) return this.Instance.CreateBatch(asyncState);
        }

        public ITransaction CreateTransaction(object asyncState = null)
        {
            lock (this) return this.Instance.CreateTransaction(asyncState);
        }

        public RedisValue DebugObject(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.DebugObject(key, flags);
        }

        public Task<RedisValue> DebugObjectAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.DebugObjectAsync(key, flags);
        }

        public RedisResult Execute(string command, params object[] args)
        {
            lock (this) return this.Instance.Execute(command, args);
        }

        public RedisResult Execute(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.Execute(command, args, flags);
        }

        public Task<RedisResult> ExecuteAsync(string command, params object[] args)
        {
            lock (this) return this.Instance.ExecuteAsync(command, args);
        }

        public Task<RedisResult> ExecuteAsync(string command, ICollection<object> args, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ExecuteAsync(command, args, flags);
        }

        public bool GeoAdd(RedisKey key, double longitude, double latitude, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoAdd(key, longitude, latitude, member, flags);
        }

        public bool GeoAdd(RedisKey key, GeoEntry value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoAdd(key, value, flags);
        }

        public long GeoAdd(RedisKey key, GeoEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoAdd(key, values, flags);
        }

        public Task<bool> GeoAddAsync(RedisKey key, double longitude, double latitude, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoAddAsync(key, longitude, latitude, member, flags);
        }

        public Task<bool> GeoAddAsync(RedisKey key, GeoEntry value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoAddAsync(key, value, flags);
        }

        public Task<long> GeoAddAsync(RedisKey key, GeoEntry[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoAddAsync(key, values, flags);
        }

        public double? GeoDistance(RedisKey key, RedisValue member1, RedisValue member2, GeoUnit unit = GeoUnit.Meters, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoDistance(key, member1, member2, unit, flags);
        }

        public Task<double?> GeoDistanceAsync(RedisKey key, RedisValue member1, RedisValue member2, GeoUnit unit = GeoUnit.Meters, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoDistanceAsync(key, member1, member2, unit, flags);
        }

        public string[] GeoHash(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoHash(key, members, flags);
        }

        public string GeoHash(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoHash(key, member, flags);
        }

        public Task<string[]> GeoHashAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoHashAsync(key, members, flags);
        }

        public Task<string> GeoHashAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoHashAsync(key, member, flags);
        }

        public GeoPosition?[] GeoPosition(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoPosition(key, members, flags);
        }

        public GeoPosition? GeoPosition(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoPosition(key, member, flags);
        }

        public Task<GeoPosition?[]> GeoPositionAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoPositionAsync(key, members, flags);
        }

        public Task<GeoPosition?> GeoPositionAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoPositionAsync(key, member, flags);
        }

        public GeoRadiusResult[] GeoRadius(RedisKey key, RedisValue member, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoRadius(key, member, radius, unit, count, order, options, flags);
        }

        public GeoRadiusResult[] GeoRadius(RedisKey key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoRadius(key, longitude, latitude, radius, unit, count, order, options, flags);
        }

        public Task<GeoRadiusResult[]> GeoRadiusAsync(RedisKey key, RedisValue member, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoRadiusAsync(key, member, radius, unit, count, order, options, flags);
        }

        public Task<GeoRadiusResult[]> GeoRadiusAsync(RedisKey key, double longitude, double latitude, double radius, GeoUnit unit = GeoUnit.Meters, int count = -1, Order? order = null, GeoRadiusOptions options = GeoRadiusOptions.Default, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoRadiusAsync(key, longitude, latitude, radius, unit, count, order, options, flags);
        }

        public bool GeoRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoRemove(key, member, flags);
        }

        public Task<bool> GeoRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.GeoRemoveAsync(key, member, flags);
        }

        public long HashDecrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashDecrement(key, hashField, value, flags);
        }

        public double HashDecrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashDecrement(key, hashField, value, flags);
        }

        public Task<long> HashDecrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashDecrementAsync(key, hashField, value, flags);
        }

        public Task<double> HashDecrementAsync(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashDecrementAsync(key, hashField, value, flags);
        }

        public bool HashDelete(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashDelete(key, hashField, flags);
        }

        public long HashDelete(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashDelete(key, hashFields, flags);
        }

        public Task<bool> HashDeleteAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashDeleteAsync(key, hashField, flags);
        }

        public Task<long> HashDeleteAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashDeleteAsync(key, hashFields, flags);
        }

        public bool HashExists(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashExists(key, hashField, flags);
        }

        public Task<bool> HashExistsAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashExistsAsync(key, hashField, flags);
        }

        public RedisValue HashGet(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashGet(key, hashField, flags);
        }

        public RedisValue[] HashGet(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashGet(key, hashFields, flags);
        }

        public HashEntry[] HashGetAll(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashGetAll(key, flags);
        }

        public Task<HashEntry[]> HashGetAllAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashGetAllAsync(key, flags);
        }

        public Task<RedisValue> HashGetAsync(RedisKey key, RedisValue hashField, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashGetAsync(key, hashField, flags);
        }

        public Task<RedisValue[]> HashGetAsync(RedisKey key, RedisValue[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashGetAsync(key, hashFields, flags);
        }

        public long HashIncrement(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashIncrement(key, hashField, value, flags);
        }

        public double HashIncrement(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashIncrement(key, hashField, value, flags);
        }

        public Task<long> HashIncrementAsync(RedisKey key, RedisValue hashField, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashIncrementAsync(key, hashField, value, flags);
        }

        public Task<double> HashIncrementAsync(RedisKey key, RedisValue hashField, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashIncrementAsync(key, hashField, value, flags);
        }

        public RedisValue[] HashKeys(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashKeys(key, flags);
        }

        public Task<RedisValue[]> HashKeysAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashKeysAsync(key, flags);
        }

        public long HashLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashLength(key, flags);
        }

        public Task<long> HashLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashLengthAsync(key, flags);
        }

        public IEnumerable<HashEntry> HashScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            lock (this) return this.Instance.HashScan(key, pattern, pageSize, flags);
        }

        public IEnumerable<HashEntry> HashScan(RedisKey key, RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashScan(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public void HashSet(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            lock (this) this.Instance.HashSet(key, hashFields, flags);
        }

        public bool HashSet(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashSet(key, hashField, value, when, flags);
        }

        public Task HashSetAsync(RedisKey key, HashEntry[] hashFields, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashSetAsync(key, hashFields, flags);
        }

        public Task<bool> HashSetAsync(RedisKey key, RedisValue hashField, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashSetAsync(key, hashField, value, when, flags);
        }

        public RedisValue[] HashValues(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashValues(key, flags);
        }

        public Task<RedisValue[]> HashValuesAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HashValuesAsync(key, flags);
        }

        public bool HyperLogLogAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogAdd(key, value, flags);
        }

        public bool HyperLogLogAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogAdd(key, values, flags);
        }

        public Task<bool> HyperLogLogAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogAddAsync(key, value, flags);
        }

        public Task<bool> HyperLogLogAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogAddAsync(key, values, flags);
        }

        public long HyperLogLogLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogLength(key, flags);
        }

        public long HyperLogLogLength(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogLength(keys, flags);
        }

        public Task<long> HyperLogLogLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogLengthAsync(key, flags);
        }

        public Task<long> HyperLogLogLengthAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogLengthAsync(keys, flags);
        }

        public void HyperLogLogMerge(RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            lock (this) this.Instance.HyperLogLogMerge(destination, first, second, flags);
        }

        public void HyperLogLogMerge(RedisKey destination, RedisKey[] sourceKeys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) this.Instance.HyperLogLogMerge(destination, sourceKeys, flags);
        }

        public Task HyperLogLogMergeAsync(RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogMergeAsync(destination, first, second, flags);
        }

        public Task HyperLogLogMergeAsync(RedisKey destination, RedisKey[] sourceKeys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.HyperLogLogMergeAsync(destination, sourceKeys, flags);
        }

        public EndPoint IdentifyEndpoint(RedisKey key = default(RedisKey), CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.IdentifyEndpoint(key, flags);
        }

        public Task<EndPoint> IdentifyEndpointAsync(RedisKey key = default(RedisKey), CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.IdentifyEndpointAsync(key, flags);
        }

        public bool IsConnected(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.IsConnected(key, flags);
        }

        public bool KeyDelete(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyDelete(key, flags);
        }

        public long KeyDelete(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyDelete(keys, flags);
        }

        public Task<bool> KeyDeleteAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyDeleteAsync(key, flags);
        }

        public Task<long> KeyDeleteAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyDeleteAsync(keys, flags);
        }

        public byte[] KeyDump(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyDump(key, flags);
        }

        public Task<byte[]> KeyDumpAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyDumpAsync(key, flags);
        }

        public bool KeyExists(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyExists(key, flags);
        }

        public Task<bool> KeyExistsAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyExistsAsync(key, flags);
        }

        public bool KeyExpire(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyExpire(key, expiry, flags);
        }

        public bool KeyExpire(RedisKey key, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyExpire(key, expiry, flags);
        }

        public Task<bool> KeyExpireAsync(RedisKey key, TimeSpan? expiry, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyExpireAsync(key, expiry, flags);
        }

        public Task<bool> KeyExpireAsync(RedisKey key, DateTime? expiry, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyExpireAsync(key, expiry, flags);
        }

        public void KeyMigrate(RedisKey key, EndPoint toServer, int toDatabase = 0, int timeoutMilliseconds = 0, MigrateOptions migrateOptions = MigrateOptions.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) this.Instance.KeyMigrate(key, toServer, toDatabase, timeoutMilliseconds, migrateOptions, flags);
        }

        public Task KeyMigrateAsync(RedisKey key, EndPoint toServer, int toDatabase = 0, int timeoutMilliseconds = 0, MigrateOptions migrateOptions = MigrateOptions.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyMigrateAsync(key, toServer, toDatabase, timeoutMilliseconds, migrateOptions, flags);
        }

        public bool KeyMove(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyMove(key, database, flags);
        }

        public Task<bool> KeyMoveAsync(RedisKey key, int database, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyMoveAsync(key, database, flags);
        }

        public bool KeyPersist(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyPersist(key, flags);
        }

        public Task<bool> KeyPersistAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyPersistAsync(key, flags);
        }

        public RedisKey KeyRandom(CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyRandom(flags);
        }

        public Task<RedisKey> KeyRandomAsync(CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyRandomAsync(flags);
        }

        public bool KeyRename(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyRename(key, newKey, when, flags);
        }

        public Task<bool> KeyRenameAsync(RedisKey key, RedisKey newKey, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyRenameAsync(key, newKey, when, flags);
        }

        public void KeyRestore(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) this.Instance.KeyRestore(key, value, expiry, flags);
        }

        public Task KeyRestoreAsync(RedisKey key, byte[] value, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyRestoreAsync(key, value, expiry, flags);
        }

        public TimeSpan? KeyTimeToLive(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyTimeToLive(key, flags);
        }

        public Task<TimeSpan?> KeyTimeToLiveAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyTimeToLiveAsync(key, flags);
        }

        public RedisType KeyType(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyType(key, flags);
        }

        public Task<RedisType> KeyTypeAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.KeyTypeAsync(key, flags);
        }

        public RedisValue ListGetByIndex(RedisKey key, long index, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListGetByIndex(key, index, flags);
        }

        public Task<RedisValue> ListGetByIndexAsync(RedisKey key, long index, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListGetByIndexAsync(key, index, flags);
        }

        public long ListInsertAfter(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListInsertAfter(key, pivot, value, flags);
        }

        public Task<long> ListInsertAfterAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListInsertAfterAsync(key, pivot, value, flags);
        }

        public long ListInsertBefore(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListInsertBefore(key, pivot, value, flags);
        }

        public Task<long> ListInsertBeforeAsync(RedisKey key, RedisValue pivot, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListInsertBeforeAsync(key, pivot, value, flags);
        }

        public RedisValue ListLeftPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListLeftPop(key, flags);
        }

        public Task<RedisValue> ListLeftPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListLeftPopAsync(key, flags);
        }

        public long ListLeftPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListLeftPush(key, value, when, flags);
        }

        public long ListLeftPush(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListLeftPush(key, values, flags);
        }

        public Task<long> ListLeftPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListLeftPushAsync(key, value, when, flags);
        }

        public Task<long> ListLeftPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListLeftPushAsync(key, values, flags);
        }

        public long ListLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListLength(key, flags);
        }

        public Task<long> ListLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListLengthAsync(key, flags);
        }

        public RedisValue[] ListRange(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRange(key, start, stop, flags);
        }

        public Task<RedisValue[]> ListRangeAsync(RedisKey key, long start = 0, long stop = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRangeAsync(key, start, stop, flags);
        }

        public long ListRemove(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRemove(key, value, count, flags);
        }

        public Task<long> ListRemoveAsync(RedisKey key, RedisValue value, long count = 0, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRemoveAsync(key, value, count, flags);
        }

        public RedisValue ListRightPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRightPop(key, flags);
        }

        public Task<RedisValue> ListRightPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRightPopAsync(key, flags);
        }

        public RedisValue ListRightPopLeftPush(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRightPopLeftPush(source, destination, flags);
        }

        public Task<RedisValue> ListRightPopLeftPushAsync(RedisKey source, RedisKey destination, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRightPopLeftPushAsync(source, destination, flags);
        }

        public long ListRightPush(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRightPush(key, value, when, flags);
        }

        public long ListRightPush(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRightPush(key, values, flags);
        }

        public Task<long> ListRightPushAsync(RedisKey key, RedisValue value, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRightPushAsync(key, value, when, flags);
        }

        public Task<long> ListRightPushAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListRightPushAsync(key, values, flags);
        }

        public void ListSetByIndex(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) this.Instance.ListSetByIndex(key, index, value, flags);
        }

        public Task ListSetByIndexAsync(RedisKey key, long index, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListSetByIndexAsync(key, index, value, flags);
        }

        public void ListTrim(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            lock (this) this.Instance.ListTrim(key, start, stop, flags);
        }

        public Task ListTrimAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ListTrimAsync(key, start, stop, flags);
        }

        public bool LockExtend(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.LockExtend(key, value, expiry, flags);
        }

        public Task<bool> LockExtendAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.LockExtendAsync(key, value, expiry, flags);
        }

        public RedisValue LockQuery(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.LockQuery(key, flags);
        }

        public Task<RedisValue> LockQueryAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.LockQueryAsync(key, flags);
        }

        public bool LockRelease(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.LockRelease(key, value, flags);
        }

        public Task<bool> LockReleaseAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.LockReleaseAsync(key, value, flags);
        }

        public bool LockTake(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.LockTake(key, value, expiry, flags);
        }

        public Task<bool> LockTakeAsync(RedisKey key, RedisValue value, TimeSpan expiry, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.LockTakeAsync(key, value, expiry, flags);
        }

        public TimeSpan Ping(CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.Ping(flags);
        }

        public Task<TimeSpan> PingAsync(CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.PingAsync(flags);
        }

        public long Publish(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.Publish(channel, message, flags);
        }

        public Task<long> PublishAsync(RedisChannel channel, RedisValue message, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.PublishAsync(channel, message, flags);
        }

        public RedisResult ScriptEvaluate(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ScriptEvaluate(script, keys, values, flags);
        }

        public RedisResult ScriptEvaluate(byte[] hash, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ScriptEvaluate(hash, keys, values, flags);
        }

        public RedisResult ScriptEvaluate(LuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ScriptEvaluate(script, parameters, flags);
        }

        public RedisResult ScriptEvaluate(LoadedLuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ScriptEvaluate(script, parameters, flags);
        }

        public Task<RedisResult> ScriptEvaluateAsync(string script, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ScriptEvaluateAsync(script, keys, values, flags);
        }

        public Task<RedisResult> ScriptEvaluateAsync(byte[] hash, RedisKey[] keys = null, RedisValue[] values = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ScriptEvaluateAsync(hash, keys, values, flags);
        }

        public Task<RedisResult> ScriptEvaluateAsync(LuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ScriptEvaluateAsync(script, parameters, flags);
        }

        public Task<RedisResult> ScriptEvaluateAsync(LoadedLuaScript script, object parameters = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.ScriptEvaluateAsync(script, parameters, flags);
        }

        public bool SetAdd(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetAdd(key, value, flags);
        }

        public long SetAdd(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetAdd(key, values, flags);
        }

        public Task<bool> SetAddAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetAddAsync(key, value, flags);
        }

        public Task<long> SetAddAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetAddAsync(key, values, flags);
        }

        public RedisValue[] SetCombine(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetCombine(operation, first, second, flags);
        }

        public RedisValue[] SetCombine(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetCombine(operation, keys, flags);
        }

        public long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetCombineAndStore(operation, destination, first, second, flags);
        }

        public long SetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetCombineAndStore(operation, destination, keys, flags);
        }

        public Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetCombineAndStoreAsync(operation, destination, first, second, flags);
        }

        public Task<long> SetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetCombineAndStoreAsync(operation, destination, keys, flags);
        }

        public Task<RedisValue[]> SetCombineAsync(SetOperation operation, RedisKey first, RedisKey second, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetCombineAsync(operation, first, second, flags);
        }

        public Task<RedisValue[]> SetCombineAsync(SetOperation operation, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetCombineAsync(operation, keys, flags);
        }

        public bool SetContains(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetContains(key, value, flags);
        }

        public Task<bool> SetContainsAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetContainsAsync(key, value, flags);
        }

        public long SetLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetLength(key, flags);
        }

        public Task<long> SetLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetLengthAsync(key, flags);
        }

        public RedisValue[] SetMembers(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetMembers(key, flags);
        }

        public Task<RedisValue[]> SetMembersAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetMembersAsync(key, flags);
        }

        public bool SetMove(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetMove(source, destination, value, flags);
        }

        public Task<bool> SetMoveAsync(RedisKey source, RedisKey destination, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetMoveAsync(source, destination, value, flags);
        }

        public RedisValue SetPop(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetPop(key, flags);
        }

        public Task<RedisValue> SetPopAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetPopAsync(key, flags);
        }

        public RedisValue SetRandomMember(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetRandomMember(key, flags);
        }

        public Task<RedisValue> SetRandomMemberAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetRandomMemberAsync(key, flags);
        }

        public RedisValue[] SetRandomMembers(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetRandomMembers(key, count, flags);
        }

        public Task<RedisValue[]> SetRandomMembersAsync(RedisKey key, long count, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetRandomMembersAsync(key, count, flags);
        }

        public bool SetRemove(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetRemove(key, value, flags);
        }

        public long SetRemove(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetRemove(key, values, flags);
        }

        public Task<bool> SetRemoveAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetRemoveAsync(key, value, flags);
        }

        public Task<long> SetRemoveAsync(RedisKey key, RedisValue[] values, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetRemoveAsync(key, values, flags);
        }

        public IEnumerable<RedisValue> SetScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            lock (this) return this.Instance.SetScan(key, pattern, pageSize, flags);
        }

        public IEnumerable<RedisValue> SetScan(RedisKey key, RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SetScan(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public RedisValue[] Sort(RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.Sort(key, skip, take, order, sortType, by, get, flags);
        }

        public long SortAndStore(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortAndStore(destination, key, skip, take, order, sortType, by, get, flags);
        }

        public Task<long> SortAndStoreAsync(RedisKey destination, RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortAndStoreAsync(destination, key, skip, take, order, sortType, by, get, flags);
        }

        public Task<RedisValue[]> SortAsync(RedisKey key, long skip = 0, long take = -1, Order order = Order.Ascending, SortType sortType = SortType.Numeric, RedisValue by = default(RedisValue), RedisValue[] get = null, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortAsync(key, skip, take, order, sortType, by, get, flags);
        }

        public bool SortedSetAdd(RedisKey key, RedisValue member, double score, CommandFlags flags)
        {
            lock (this) return this.Instance.SortedSetAdd(key, member, score, flags);
        }

        public bool SortedSetAdd(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetAdd(key, member, score, when, flags);
        }

        public long SortedSetAdd(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
        {
            lock (this) return this.Instance.SortedSetAdd(key, values, flags);
        }

        public long SortedSetAdd(RedisKey key, SortedSetEntry[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetAdd(key, values, when, flags);
        }

        public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, CommandFlags flags)
        {
            lock (this) return this.Instance.SortedSetAddAsync(key, member, score, flags);
        }

        public Task<bool> SortedSetAddAsync(RedisKey key, RedisValue member, double score, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetAddAsync(key, member, score, when, flags);
        }

        public Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, CommandFlags flags)
        {
            lock (this) return this.Instance.SortedSetAddAsync(key, values, flags);
        }

        public Task<long> SortedSetAddAsync(RedisKey key, SortedSetEntry[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetAddAsync(key, values, when, flags);
        }

        public long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetCombineAndStore(operation, destination, first, second, aggregate, flags);
        }

        public long SortedSetCombineAndStore(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetCombineAndStore(operation, destination, keys, weights, aggregate, flags);
        }

        public Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey first, RedisKey second, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetCombineAndStoreAsync(operation, destination, first, second, aggregate, flags);
        }

        public Task<long> SortedSetCombineAndStoreAsync(SetOperation operation, RedisKey destination, RedisKey[] keys, double[] weights = null, Aggregate aggregate = Aggregate.Sum, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetCombineAndStoreAsync(operation, destination, keys, weights, aggregate, flags);
        }

        public double SortedSetDecrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetDecrement(key, member, value, flags);
        }

        public Task<double> SortedSetDecrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetDecrementAsync(key, member, value, flags);
        }

        public double SortedSetIncrement(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetIncrement(key, member, value, flags);
        }

        public Task<double> SortedSetIncrementAsync(RedisKey key, RedisValue member, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetIncrementAsync(key, member, value, flags);
        }

        public long SortedSetLength(RedisKey key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetLength(key, min, max, exclude, flags);
        }

        public Task<long> SortedSetLengthAsync(RedisKey key, double min = double.NegativeInfinity, double max = double.PositiveInfinity, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetLengthAsync(key, min, max, exclude, flags);
        }

        public long SortedSetLengthByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetLengthByValue(key, min, max, exclude, flags);
        }

        public Task<long> SortedSetLengthByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetLengthByValueAsync(key, min, max, exclude, flags);
        }

        public RedisValue[] SortedSetRangeByRank(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByRank(key, start, stop, order, flags);
        }

        public Task<RedisValue[]> SortedSetRangeByRankAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByRankAsync(key, start, stop, order, flags);
        }

        public SortedSetEntry[] SortedSetRangeByRankWithScores(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByRankWithScores(key, start, stop, order, flags);
        }

        public Task<SortedSetEntry[]> SortedSetRangeByRankWithScoresAsync(RedisKey key, long start = 0, long stop = -1, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByRankWithScoresAsync(key, start, stop, order, flags);
        }

        public RedisValue[] SortedSetRangeByScore(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByScore(key, start, stop, exclude, order, skip, take, flags);
        }

        public Task<RedisValue[]> SortedSetRangeByScoreAsync(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByScoreAsync(key, start, stop, exclude, order, skip, take, flags);
        }

        public SortedSetEntry[] SortedSetRangeByScoreWithScores(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByScoreWithScores(key, start, stop, exclude, order, skip, take, flags);
        }

        public Task<SortedSetEntry[]> SortedSetRangeByScoreWithScoresAsync(RedisKey key, double start = double.NegativeInfinity, double stop = double.PositiveInfinity, Exclude exclude = Exclude.None, Order order = Order.Ascending, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByScoreWithScoresAsync(key, start, stop, exclude, order, skip, take, flags);
        }

        public RedisValue[] SortedSetRangeByValue(RedisKey key, RedisValue min = default(RedisValue), RedisValue max = default(RedisValue), Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByValue(key, min, max, exclude, skip, take, flags);
        }

        public Task<RedisValue[]> SortedSetRangeByValueAsync(RedisKey key, RedisValue min = default(RedisValue), RedisValue max = default(RedisValue), Exclude exclude = Exclude.None, long skip = 0, long take = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRangeByValueAsync(key, min, max, exclude, skip, take, flags);
        }

        public long? SortedSetRank(RedisKey key, RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRank(key, member, order, flags);
        }

        public Task<long?> SortedSetRankAsync(RedisKey key, RedisValue member, Order order = Order.Ascending, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRankAsync(key, member, order, flags);
        }

        public bool SortedSetRemove(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemove(key, member, flags);
        }

        public long SortedSetRemove(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemove(key, members, flags);
        }

        public Task<bool> SortedSetRemoveAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemoveAsync(key, member, flags);
        }

        public Task<long> SortedSetRemoveAsync(RedisKey key, RedisValue[] members, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemoveAsync(key, members, flags);
        }

        public long SortedSetRemoveRangeByRank(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemoveRangeByRank(key, start, stop, flags);
        }

        public Task<long> SortedSetRemoveRangeByRankAsync(RedisKey key, long start, long stop, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemoveRangeByRankAsync(key, start, stop, flags);
        }

        public long SortedSetRemoveRangeByScore(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemoveRangeByScore(key, start, stop, exclude, flags);
        }

        public Task<long> SortedSetRemoveRangeByScoreAsync(RedisKey key, double start, double stop, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemoveRangeByScoreAsync(key, start, stop, exclude, flags);
        }

        public long SortedSetRemoveRangeByValue(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemoveRangeByValue(key, min, max, exclude, flags);
        }

        public Task<long> SortedSetRemoveRangeByValueAsync(RedisKey key, RedisValue min, RedisValue max, Exclude exclude = Exclude.None, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetRemoveRangeByValueAsync(key, min, max, exclude, flags);
        }

        public IEnumerable<SortedSetEntry> SortedSetScan(RedisKey key, RedisValue pattern, int pageSize, CommandFlags flags)
        {
            lock (this) return this.Instance.SortedSetScan(key, pattern, pageSize, flags);
        }

        public IEnumerable<SortedSetEntry> SortedSetScan(RedisKey key, RedisValue pattern = default(RedisValue), int pageSize = 10, long cursor = 0, int pageOffset = 0, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetScan(key, pattern, pageSize, cursor, pageOffset, flags);
        }

        public double? SortedSetScore(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetScore(key, member, flags);
        }

        public Task<double?> SortedSetScoreAsync(RedisKey key, RedisValue member, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.SortedSetScoreAsync(key, member, flags);
        }

        public long StringAppend(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringAppend(key, value, flags);
        }

        public Task<long> StringAppendAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringAppendAsync(key, value, flags);
        }

        public long StringBitCount(RedisKey key, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringBitCount(key, start, end, flags);
        }

        public Task<long> StringBitCountAsync(RedisKey key, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringBitCountAsync(key, start, end, flags);
        }

        public long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default(RedisKey), CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringBitOperation(operation, destination, first, second, flags);
        }

        public long StringBitOperation(Bitwise operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringBitOperation(operation, destination, keys, flags);
        }

        public Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey first, RedisKey second = default(RedisKey), CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringBitOperationAsync(operation, destination, first, second, flags);
        }

        public Task<long> StringBitOperationAsync(Bitwise operation, RedisKey destination, RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringBitOperationAsync(operation, destination, keys, flags);
        }

        public long StringBitPosition(RedisKey key, bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringBitPosition(key, bit, start, end, flags);
        }

        public Task<long> StringBitPositionAsync(RedisKey key, bool bit, long start = 0, long end = -1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringBitPositionAsync(key, bit, start, end, flags);
        }

        public long StringDecrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringDecrement(key, value, flags);
        }

        public double StringDecrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringDecrement(key, value, flags);
        }

        public Task<long> StringDecrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringDecrementAsync(key, value, flags);
        }

        public Task<double> StringDecrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringDecrementAsync(key, value, flags);
        }

        public RedisValue StringGet(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGet(key, flags);
        }

        public RedisValue[] StringGet(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGet(keys, flags);
        }

        public Task<RedisValue> StringGetAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetAsync(key, flags);
        }

        public Task<RedisValue[]> StringGetAsync(RedisKey[] keys, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetAsync(keys, flags);
        }

        public bool StringGetBit(RedisKey key, long offset, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetBit(key, offset, flags);
        }

        public Task<bool> StringGetBitAsync(RedisKey key, long offset, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetBitAsync(key, offset, flags);
        }

        public RedisValue StringGetRange(RedisKey key, long start, long end, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetRange(key, start, end, flags);
        }

        public Task<RedisValue> StringGetRangeAsync(RedisKey key, long start, long end, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetRangeAsync(key, start, end, flags);
        }

        public RedisValue StringGetSet(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetSet(key, value, flags);
        }

        public Task<RedisValue> StringGetSetAsync(RedisKey key, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetSetAsync(key, value, flags);
        }

        public RedisValueWithExpiry StringGetWithExpiry(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetWithExpiry(key, flags);
        }

        public Task<RedisValueWithExpiry> StringGetWithExpiryAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringGetWithExpiryAsync(key, flags);
        }

        public long StringIncrement(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringIncrement(key, value, flags);
        }

        public double StringIncrement(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringIncrement(key, value, flags);
        }

        public Task<long> StringIncrementAsync(RedisKey key, long value = 1, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringIncrementAsync(key, value, flags);
        }

        public Task<double> StringIncrementAsync(RedisKey key, double value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringIncrementAsync(key, value, flags);
        }

        public long StringLength(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringLength(key, flags);
        }

        public Task<long> StringLengthAsync(RedisKey key, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringLengthAsync(key, flags);
        }

        public bool StringSet(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringSet(key, value, expiry, when, flags);
        }

        public bool StringSet(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringSet(values, when, flags);
        }

        public Task<bool> StringSetAsync(RedisKey key, RedisValue value, TimeSpan? expiry = null, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringSetAsync(key, value, expiry, when, flags);
        }

        public Task<bool> StringSetAsync(KeyValuePair<RedisKey, RedisValue>[] values, When when = When.Always, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringSetAsync(values, when, flags);
        }

        public bool StringSetBit(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringSetBit(key, offset, bit, flags);
        }

        public Task<bool> StringSetBitAsync(RedisKey key, long offset, bool bit, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringSetBitAsync(key, offset, bit, flags);
        }

        public RedisValue StringSetRange(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringSetRange(key, offset, value, flags);
        }

        public Task<RedisValue> StringSetRangeAsync(RedisKey key, long offset, RedisValue value, CommandFlags flags = CommandFlags.None)
        {
            lock (this) return this.Instance.StringSetRangeAsync(key, offset, value, flags);
        }

        public bool TryWait(Task task)
        {
            lock (this) return this.Instance.TryWait(task);
        }

        public void Wait(Task task)
        {
            lock (this) this.Instance.Wait(task);
        }

        public T Wait<T>(Task<T> task)
        {
            lock (this) return this.Instance.Wait(task);
        }

        public void WaitAll(params Task[] tasks)
        {
            lock (this) this.Instance.WaitAll(tasks);
        }
    }
}