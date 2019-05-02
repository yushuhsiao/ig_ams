using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace InnateGlory
{
    [_DebuggerStepThrough]
    public sealed class DataService : IServiceProvider
    {
        private IServiceProvider _services;
        private IConfiguration<DataService> _config;
        private ISqlConfig<DataService> _sqlConfig;
        //private ISqlConfig _config2;

        internal DataService(IServiceProvider services)
        {
            _services = services;
            _config = services.GetService<IConfiguration<DataService>>();
            _sqlConfig = services.GetService<ISqlConfig<DataService>>();
            //_config2 = services.GetSqlConfig(this);
        }

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(DataService))
                return this;
            return _services.GetService(serviceType);
        }

        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        private T GetInstance<T>()
        {
            T result = _services.GetService<T>();
            if (result == null)
            {
                lock (_instances)
                {
                    if (_instances.TryGetValue(typeof(T), out object tmp))
                        result = (T)tmp;
                    else
                        _instances[typeof(T)] = result = _services.GetServiceOrCreateInstance<T>();
                }
            }
            return result;
        }

        public CorpInfoProvider Corps => GetInstance<CorpInfoProvider>(); //_corps.Value;
        public UserDataProvider Users => GetInstance<UserDataProvider>(); //_users.Value;
        public AgentDataProvider Agents => GetInstance<AgentDataProvider>(); //_agents.Value;
        public AdminDataProvider Admins => GetInstance<AdminDataProvider>(); //_admins.Value;
        public MemberDataProvider Members => GetInstance<MemberDataProvider>(); //_members.Value;
        public GamePlatformInfoProvider GamePlatforms => GetInstance<GamePlatformInfoProvider>(); //_platforms.Value;
        public GameTypeInfoProvider GameTypes => GetInstance<GameTypeInfoProvider>();
        public GameInfoProvider Games => GetInstance<GameInfoProvider>(); //_games.Value;
        public PaymentInfoProvider Payments => GetInstance<PaymentInfoProvider>(); //_payments.Value;
        public AclDataProvider Acl => GetInstance<AclDataProvider>(); //_acl.Value;
        public PasswordProvider Password => GetInstance<PasswordProvider>();
        public TranService Tran => GetInstance<TranService>();


        [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
        public DbConnectionString _CoreDB_R
        {
            //[DebuggerStepThrough]
            get => _config.GetValue<string>();
        }

        [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.CoreDB_W), DefaultValue(_Consts.db.CoreDB_Default)]
        public DbConnectionString _CoreDB_W
        {
            //[DebuggerStepThrough]
            get => _config.GetValue<string>();
        }

        [SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.UserDB_R)]
        public DbConnectionString _UserDB_R(CorpId id) => _sqlConfig.GetValue<string>(id);

        [SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.UserDB_W)]
        public DbConnectionString _UserDB_W(CorpId id) => _sqlConfig.GetValue<string>(id);

        [SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.LogDB_R)]
        public DbConnectionString _LogDB_R(CorpId id) => _sqlConfig.GetValue<string>(id);

        [SqlConfig(Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.LogDB_W)]
        public DbConnectionString _LogDB_W(CorpId id) => _sqlConfig.GetValue<string>(id);


        public SqlCmd CoreDB_R(object state = null) => _CoreDB_R.Open(_services, state);
        public SqlCmd CoreDB_W(object state = null) => _CoreDB_W.Open(_services, state);
        public SqlCmd UserDB_R(CorpId id, object state = null) => _UserDB_R(id).Open(_services, state);
        public SqlCmd UserDB_W(CorpId id, object state = null) => _UserDB_W(id).Open(_services, state);
        public SqlCmd LogDB_R(CorpId id, object state = null) => _LogDB_R(id).Open(_services, state);
        public SqlCmd LogDB_W(CorpId id, object state = null) => _LogDB_W(id).Open(_services, state);

        public IDisposable CoreDB_R(ref SqlCmd sqlcmd, object state = null)
        {
            if (sqlcmd == null || sqlcmd.ConnectionString != _CoreDB_R)
                return sqlcmd = CoreDB_R(state);
            else
                return null;
        }
        public IDisposable CoreDB_W(ref SqlCmd sqlcmd, object state = null)
        {
            if (sqlcmd == null || sqlcmd.ConnectionString != _CoreDB_W)
                return sqlcmd = CoreDB_W(state);
            else
                return null;
        }
        public IDisposable UserDB_R(ref SqlCmd sqlcmd, CorpId id, object state = null)
        {
            if (sqlcmd == null || sqlcmd.ConnectionString != _UserDB_R(id))
                return sqlcmd = UserDB_R(id, state);
            else
                return null;
        }
        public IDisposable UserDB_W(ref SqlCmd sqlcmd, CorpId id, object state = null)
        {
            if (sqlcmd == null || sqlcmd.ConnectionString != _UserDB_W(id))
                return sqlcmd = UserDB_W(id, state);
            else
                return null;
        }
        public IDisposable LogDB_R(ref SqlCmd sqlcmd, CorpId id, object state = null)
        {
            if (sqlcmd == null || sqlcmd.ConnectionString != _LogDB_R(id))
                return sqlcmd = LogDB_R(id, state);
            else
                return null;
        }
        public IDisposable LogDB_W(ref SqlCmd sqlcmd, CorpId id, object state = null)
        {
            if (sqlcmd == null || sqlcmd.ConnectionString != _LogDB_W(id))
                return sqlcmd = LogDB_W(id, state);
            else
                return null;
        }
    }
    public static partial class DataServiceExtensions
    {
    }
}