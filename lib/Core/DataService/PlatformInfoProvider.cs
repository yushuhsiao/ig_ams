using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace InnateGlory
{
    public class PlatformInfoProvider : IDataService
    {
        private DataService _dataService;
        //private SqlConfig2 _config;
        private DbCache<Entity.PlatformInfo> _cache;

        public PlatformInfoProvider(DataService dataService)
        {
            this._dataService = dataService;
            //this._config = dataService.GetService<SqlConfig2>();
            this._cache = dataService.GetDbCache<Entity.PlatformInfo>(ReadData);
        }

        private IEnumerable<Entity.PlatformInfo> ReadData(DbCache<Entity.PlatformInfo>.Entry sender, Entity.PlatformInfo[] oldValue)
        {
            string sql1 = $"select * from {TableName<Entity.PlatformInfo>.Value} nolock";
            using (SqlCmd coredb = _dataService.SqlCmds.CoreDB_R())
            {
                foreach (SqlDataReader r in coredb.ExecuteReaderEach(sql1))
                {
                    PlatformType t1 = (PlatformType)r.GetInt32("PlatformType");
                    Platform t2 = GetInstance(t1) ?? GetInstance(PlatformType.Main);
                    yield return (Entity.PlatformInfo)r.ToObject(t2.PlatformInfoType);
                }
            }
            //var values = _config.Root.CoreDB_R.ToList<Data.PlatformInfo>(_dataService, sql1, create:create);
            //return values;
        }

        private List<Platform> _instances = new List<Platform>();
        private Platform GetInstance(PlatformType platformType)
        {
            lock (_instances)
            {
                if (_instances.Count == 0)
                    _instances.Add(_dataService.CreateInstance<MainPlatform>());
                for (int i = 0; i < _instances.Count; i++)
                {
                    if (_instances[i].PlatformType == platformType)
                        return _instances[i];
                }
                //IEnumerable<ApplicationPart> parts = DefaultAssemblyPartDiscoveryProvider.DiscoverAssemblyParts(Assembly.GetEntryAssembly().GetName().Name);
                IEnumerable<ApplicationPart> parts = _dataService.GetService<ApplicationPartManager>().ApplicationParts;
                foreach (var n1 in parts.OfType<AssemblyPart>())
                {
                    if (n1.Assembly.IsDefined<PlatformInfoAttribute>() || n1.Assembly.Equals(Assembly.GetExecutingAssembly()))
                    {
                        foreach (var n2 in n1.Types)
                        {
                            if (n2.IsSubclassOf<Platform>() && n2.GetCustomAttribute<PlatformInfoAttribute>(out var a))
                            {
                                if (n2.IsAbstract) continue;
                                if (a.PlatformType == platformType)
                                    return (Platform)_dataService.CreateInstance(n2);
                            }
                        }
                    }
                }
            }
            return null;
        }

        public void test()
        {
            var x = _cache.GetValues();
        }

        public Entity.PlatformInfo this[PlatformId id]
        {
            get
            {
                var p = _cache.GetValues();
                for (int i = 0; i < p.Length; i++)
                    if (p[i].Id == id)
                        return p[i];
                return null;
            }
        }
        public Entity.PlatformInfo this[UserName name]
        {
            get
            {
                var p = _cache.GetValues();
                for (int i = 0; i < p.Length; i++)
                    if (p[i].Name == name)
                        return p[i];
                return null;
            }
        }
        internal Platform this[PlatformType platformType] => GetInstance(platformType);
    }

    public abstract class Platform
    {
        internal abstract Type PlatformInfoType { get; }
        internal abstract Type MemberPlatformType { get; }
        internal PlatformType PlatformType { get; }

        internal Platform()
        {
            this.PlatformType = this.GetType().GetCustomAttribute<PlatformInfoAttribute>().PlatformType;
        }

        // *platforms
        // create
        // update

        // *members
        // create
        // update
        // get balance
        // cash-in
        // cash-out
        // auth
        // forward game
    }
    public abstract class Platform<TPlatformInfo, TMemberPlatform> : Platform
        where TPlatformInfo : Entity.PlatformInfo
        where TMemberPlatform : Entity.MemberPlatform
    {
        internal override Type MemberPlatformType => typeof(TMemberPlatform);
        internal override Type PlatformInfoType => typeof(TPlatformInfo);

        public Platform()
        {
        }
    }

    [PlatformInfo(PlatformType = PlatformType.Main)]
    public class MainPlatform : Platform<Entity.PlatformInfo, Entity.MemberPlatform>
    {
    }
}