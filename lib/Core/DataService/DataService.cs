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
        private IConfiguration _config2;
        //private IConfiguration<DataService> _config;
        //private ISqlConfig<DataService> _sqlConfig;
        //private ISqlConfig _config2;

        internal DataService(IServiceProvider services)
        {
            _services = services;
            //_config = services.GetService<IConfiguration<DataService>>();
            //_sqlConfig = services.GetService<ISqlConfig<DataService>>();
            _config2 = services.GetService<IConfiguration<DataService>>();
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

        public CorpInfoProvider Corps => this.GetService<CorpInfoProvider>();
        public UserDataProvider Users => this.GetService<UserDataProvider>();
        public AgentDataProvider Agents => this.GetService<AgentDataProvider>();
        public AdminDataProvider Admins => this.GetService<AdminDataProvider>();
        public MemberDataProvider Members => this.GetService<MemberDataProvider>();
        public GameTypeInfoProvider GameTypes => this.GetService<GameTypeInfoProvider>();
        public GameInfoProvider Games => this.GetService<GameInfoProvider>();
        public PaymentInfoProvider Payments => this.GetService<PaymentInfoProvider>();
        //public AclDataProvider Acl => this.GetService<AclDataProvider>();
        //public PasswordProvider Password => this.GetService<PasswordProvider>();

        [AppSetting(SectionName = AppSettingAttribute.ConnectionStrings, Key = _Consts.db.CoreDB_R), DefaultValue(_Consts.db.CoreDB_Default)]
        public DbConnectionString _CoreDB_R() => _config2.GetValue<string>();

        [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.CoreDB_W), DefaultValue(_Consts.db.CoreDB_Default)]
        public DbConnectionString _CoreDB_W() => _config2.GetValue<string>();

        [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.UserDB_R)]
        public DbConnectionString _UserDB_R(CorpId id) => _config2.GetValue<string>(id);

        [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.UserDB_W)]
        public DbConnectionString _UserDB_W(CorpId id) => _config2.GetValue<string>(id);

        [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.LogDB_R)]
        public DbConnectionString _LogDB_R(CorpId id) => _config2.GetValue<string>(id);

        [AppSetting(SectionName = _Consts.db.SqlConnection, Key = _Consts.db.LogDB_W)]
        public DbConnectionString _LogDB_W(CorpId id) => _config2.GetValue<string>(id);


        public SqlCmd CoreDB_R(object state = null) => _CoreDB_R().Open(_services, state);
        public SqlCmd CoreDB_W(object state = null) => _CoreDB_W().Open(_services, state);
        public SqlCmd UserDB_R(CorpId id, object state = null) => _UserDB_R(id).Open(_services, state);
        public SqlCmd UserDB_W(CorpId id, object state = null) => _UserDB_W(id).Open(_services, state);
        public SqlCmd LogDB_R(CorpId id, object state = null) => _LogDB_R(id).Open(_services, state);
        public SqlCmd LogDB_W(CorpId id, object state = null) => _LogDB_W(id).Open(_services, state);

        public IDisposable CoreDB_R(ref SqlCmd sqlcmd, object state = null) => _Open(_CoreDB_R, CoreDB_R, ref sqlcmd, state);
        public IDisposable CoreDB_W(ref SqlCmd sqlcmd, object state = null) => _Open(_CoreDB_W, CoreDB_W, ref sqlcmd, state);
        public IDisposable UserDB_R(ref SqlCmd sqlcmd, CorpId id, object state = null) => _Open(_UserDB_R, UserDB_R, ref sqlcmd, id, state);
        public IDisposable UserDB_W(ref SqlCmd sqlcmd, CorpId id, object state = null) => _Open(_UserDB_W, UserDB_W, ref sqlcmd, id, state);
        public IDisposable LogDB_R(ref SqlCmd sqlcmd, CorpId id, object state = null) => _Open(_LogDB_R, LogDB_R, ref sqlcmd, id, state);
        public IDisposable LogDB_W(ref SqlCmd sqlcmd, CorpId id, object state = null) => _Open(_LogDB_W, LogDB_W, ref sqlcmd, id, state);

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
    public static partial class DataServiceExtensions
    {
    }
}