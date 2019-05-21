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
        private IConfiguration _config;

        public DataService(IServiceProvider services)
        {
            _services = services;
            _config = services.GetService<IConfiguration<DataService>>();
            this.Connections = new _Connections(this);
            this.SqlCmds = new _SqlCmds(this);
        }

        private Dictionary<Type, object> _instances = new Dictionary<Type, object>();

        object IServiceProvider.GetService(Type serviceType)
        {
            if (serviceType == typeof(DataService))
                return this;

            var result = _services.GetService(serviceType);
            if (result != null)
                return result;

            if (serviceType.HasInterface<IDataService>())
            {
                lock (_instances)
                {
                    if (_instances.TryGetValue(serviceType, out result))
                        return result;
                    else
                        return _instances[serviceType] = _services.CreateInstance(serviceType);
                }
            }
            return result;
        }

        public _Connections Connections { get; }
        public _SqlCmds SqlCmds { get; }

        public CorpInfoProvider Corps => this.GetService<CorpInfoProvider>();
        public UserDataProvider Users => this.GetService<UserDataProvider>();
        public AgentDataProvider Agents => this.GetService<AgentDataProvider>();
        public AdminDataProvider Admins => this.GetService<AdminDataProvider>();
        public MemberDataProvider Members => this.GetService<MemberDataProvider>();
        public GameTypeInfoProvider GameTypes => this.GetService<GameTypeInfoProvider>();
        public GameInfoProvider Games => this.GetService<GameInfoProvider>();
        public PaymentInfoProvider Payments => this.GetService<PaymentInfoProvider>();



        public sealed class _Connections
        {
            private DataService _services;
            private IConfiguration _config;

            public _Connections(DataService services)
            {
                _services = services;
                _config = services.GetService<IConfiguration<_Connections>>();
            }

            [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
            public DbConnectionString CoreDB_R() => _config.GetValue<string>();

            [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.CoreDB_W), DefaultValue(_Consts.db.CoreDB_Default)]
            public DbConnectionString CoreDB_W() => _config.GetValue<string>();

            [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.UserDB_R)]
            public DbConnectionString UserDB_R(CorpId id) => _config.GetValue<string>(id);

            [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.UserDB_W)]
            public DbConnectionString UserDB_W(CorpId id) => _config.GetValue<string>(id);

            [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.LogDB_R)]
            public DbConnectionString LogDB_R(CorpId id) => _config.GetValue<string>(id);

            [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.LogDB_W)]
            public DbConnectionString LogDB_W(CorpId id) => _config.GetValue<string>(id);
        }

        public sealed class _SqlCmds
        {
            private DataService _services;
            private IConfiguration _config;

            public _SqlCmds(DataService services)
            {
                _services = services;
                _config = services.GetService<IConfiguration<_SqlCmds>>();
            }

            public SqlCmd CoreDB_R(object state = null) => _services.Connections.CoreDB_R().Open(_services, state);
            public SqlCmd CoreDB_W(object state = null) => _services.Connections.CoreDB_W().Open(_services, state);
            public SqlCmd UserDB_R(CorpId id, object state = null) => _services.Connections.UserDB_R(id).Open(_services, state);
            public SqlCmd UserDB_W(CorpId id, object state = null) => _services.Connections.UserDB_W(id).Open(_services, state);
            public SqlCmd LogDB_R(CorpId id, object state = null) => _services.Connections.LogDB_R(id).Open(_services, state);
            public SqlCmd LogDB_W(CorpId id, object state = null) => _services.Connections.LogDB_W(id).Open(_services, state);

            public IDisposable CoreDB_R(ref SqlCmd sqlcmd, object state = null) => _Open(_services.Connections.CoreDB_R, CoreDB_R, ref sqlcmd, state);
            public IDisposable CoreDB_W(ref SqlCmd sqlcmd, object state = null) => _Open(_services.Connections.CoreDB_W, CoreDB_W, ref sqlcmd, state);
            public IDisposable UserDB_R(ref SqlCmd sqlcmd, CorpId id, object state = null) => _Open(_services.Connections.UserDB_R, UserDB_R, ref sqlcmd, id, state);
            public IDisposable UserDB_W(ref SqlCmd sqlcmd, CorpId id, object state = null) => _Open(_services.Connections.UserDB_W, UserDB_W, ref sqlcmd, id, state);
            public IDisposable LogDB_R(ref SqlCmd sqlcmd, CorpId id, object state = null) => _Open(_services.Connections.LogDB_R, LogDB_R, ref sqlcmd, id, state);
            public IDisposable LogDB_W(ref SqlCmd sqlcmd, CorpId id, object state = null) => _Open(_services.Connections.LogDB_W, LogDB_W, ref sqlcmd, id, state);

            private IDisposable _Open(Func<DbConnectionString> _conn, Func<object, SqlCmd> _db, ref SqlCmd sqlcmd, object state = null)
            {
                if (sqlcmd == null || sqlcmd.ConnectionString != _conn())
                    return sqlcmd = _db(state);
                else
                    return null;
            }

            private IDisposable _Open(Func<CorpId, DbConnectionString> _conn, Func<CorpId, object, SqlCmd> _db, ref SqlCmd sqlcmd, CorpId id, object state = null)
            {
                if (sqlcmd == null || sqlcmd.ConnectionString != _conn(id))
                    return sqlcmd = _db(id, state);
                else
                    return null;
            }
        }
    }
    public static partial class DataServiceExtensions
    {
    }
}