using Dapper;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace InnateGlory
{
    public class AclDataProvider : IDataService
    {
        private DataService _dataService;
        //private readonly SqlConfig _config;
        private readonly DbCache<Entity.AclDefine> _cache;

        public AclDataProvider(DataService services, /*SqlConfig config,*/ ILogger<AclDataProvider> logger)
        {
            this._dataService = services;
            //this._config = config;
            this._cache = services.GetDbCache<Entity.AclDefine>(ReadData);
        }

        private IEnumerable<Entity.AclDefine> ReadData(DbCache<Entity.AclDefine>.Entry sender, Entity.AclDefine[] oldValue)
        {
            using (IDbConnection coredb = _dataService.DbConnections.CoreDB_R())
                //return _config.CoreDB_R.ToList<Data.AclDefine>(_config, $"select * from {TableName<Data.AclDefine>.Value}");
                //return coredb.ToList<Data.AclDefine>(SqlBuilder.select_all_from<Data.AclDefine>());
                return coredb.Query<Entity.AclDefine>($"select * from {TableName<Entity.AclDefine>.Value}");
        }

        //public bool CheckCrossCorp(int? aclId, Data.CorpInfo corp, UserId userId)
        //{
        //    if (userId.IsRoot) return true;
        //    if (userId.CorpId == corp.Id) return true;
        //    if (aclId.HasValue)
        //    {
        //        /// todo : <see cref="Data.UserAclDelegate"/>
        //    }
        //    return false;
        //}

        public bool HasPermission(Entity.CorpInfo corp, UserId userId)
        {
            if (userId.IsRoot) return true;
            if (userId.CorpId == corp.Id) return true;
            return false;
        }
    }
    partial class amsExtensions
    {
        public static Entity.AclDefine GetDefine(this Entity.UserAcl obj) => throw new NotImplementedException();
    }
}
