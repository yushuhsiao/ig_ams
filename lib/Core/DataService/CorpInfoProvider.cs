using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory
{
    public class CorpInfoProvider : IDataService
    {
        private DataService _dataService;
        //private SqlConfig _config;
        private IConfiguration _config;
        private DbCache<Entity.CorpInfo> _cache;

        public CorpInfoProvider(DataService dataService)
        {
            this._dataService = dataService;
            this._config = dataService.GetService<IConfiguration<CorpInfoProvider>>();// .GetSqlConfig(this);
            this._cache = dataService.GetDbCache<Entity.CorpInfo>(ReadData);
        }

        #region ConnectionStrings




        #endregion

        private IEnumerable<Entity.CorpInfo> ReadData(DbCache<Entity.CorpInfo>.Entry sender, Entity.CorpInfo[] oldValue)
        {
            string sql = $"select * from {TableName<Entity.CorpInfo>.Value} nolock";
            using (SqlCmd coredb = _dataService.CoreDB_R())
            {
                var values = coredb.ToList<Entity.CorpInfo>(sql);
                var root = values.Find(x => x.Id == CorpId.Root);
                if (root != null)
                    return values;
            }
            return CreateRoot(sql);

            //for (bool f = true; ; f = false)
            //{
            //    var values = coredb.ToList<Data.CorpInfo>(sql);
            //    var root = values.Find(x => x.Id == CorpId.Root);
            //    if (root != null)
            //        return values;
            //    if (f)
            //        CreateRoot();
            //    else
            //        throw new Exception("System Error! Unable to init root");
            //}
        }

        public Entity.CorpInfo[] All => _cache.GetValues();
        public Entity.CorpInfo Root => this.Get(CorpId.Root);

        public bool Get(CorpId? id, out Entity.CorpInfo result)
        {
            if (id.HasValue && id.Value.IsValid)
            {
                CorpId _id = id.Value;
                for (int i = 0; i < All.Length; i++)
                {
                    if (All[i].Id == _id)
                    {
                        result = All[i];
                        return true;
                    }
                }
            }
            return _null.noop(false, out result);
        }

        public bool Get(UserName name, out Entity.CorpInfo result)
        {
            if (name.IsValid)
            {
                for (int i = 0; i < All.Length; i++)
                {
                    if (All[i].Name == name)
                    {
                        result = All[i];
                        return true;
                    }
                }
            }
            return _null.noop(false, out result);
        }

        public Entity.CorpInfo Get(CorpId? id)
        {
            this.Get(id, out var result);
            return result;
        }

        public Entity.CorpInfo Get(UserName name)
        {
            this.Get(name, out var result);
            return result;
        }

        private IEnumerable<Entity.CorpInfo> CreateRoot(string sql_all)
        {
            var _sql = new SqlBuilder(typeof(Entity.CorpInfo))
            {
                { "w", nameof(Entity.CorpInfo.Id)             , CorpId.Root },
                { " ", nameof(Entity.CorpInfo.Name)           , UserName.root },
                { " ", nameof(Entity.CorpInfo.Active)         , ActiveState.Active },
                { "N", nameof(Entity.CorpInfo.DisplayName)    , UserName.root },
                { " ", nameof(Entity.CorpInfo.Currency)       , CurrencyCode.Default },
                { UserId.System, UserId.System }
            };

            string sql = _sql.FormatWith($@"{_sql.insert_into()}");
            using (SqlCmd coredb = _dataService.CoreDB_W())
            {
                try { coredb.ExecuteNonQuery(sql, transaction: true); }
                catch (SqlException ex) when (ex.IsDuplicateKey()) { }
                return coredb.ToList<Entity.CorpInfo>(sql_all);
            }
            throw new Exception("System Error! Unable to init root");
        }

        /// <remarks>
        /// 建立 <see cref="Entity.CorpInfo"/> 物件之後, 必須指定 UserDB 的 ConnectionString 才能夠進行用戶資料操作
        /// </remarks>
        public Status Create(Models.CorpModel model, out Entity.CorpInfo result)
        {
            CorpId corpId = model.Id.Value;

            Entity.CorpInfo data;
            if (this.Get(corpId, out data))
                return _null.noop(Status.CorpAlreadyExist, out result);

            if (this.Get(model.Name, out data))
                return _null.noop(Status.CorpAlreadyExist, out result);

            UserId op_user = _dataService.GetHttpContext().User.GetUserId();// .GetCurrentUser().Id;
            var _sql = new SqlBuilder(typeof(Entity.CorpInfo))
            {
                { "w", nameof(Entity.CorpInfo.Id)             , corpId},
                { " ", nameof(Entity.CorpInfo.Name)           , model.Name },
                { " ", nameof(Entity.CorpInfo.Active)         , model.Active ?? ActiveState.Active},
                { "N", nameof(Entity.CorpInfo.DisplayName)    , model.DisplayName ?? model.Name},
                { " ", nameof(Entity.CorpInfo.Currency)       , model.Currency ?? CurrencyCode.Default},
                { op_user, op_user }
            };

            string sql = _sql.FormatWith($@"{_sql.insert_into()}
{_sql.select_where()}");
            try
            {
                using (SqlCmd coredb = _dataService.CoreDB_W())
                    result = coredb.ToObject<Entity.CorpInfo>(sql, transaction: true);
            }
            catch (SqlException ex) when (ex.IsDuplicateKey())
            {
                return _null.noop(Status.CorpAlreadyExist, out result);
            }
            _cache.UpdateVersion();
            return Status.Success;
        }

        public Status Update(Models.CorpModel model, out Entity.CorpInfo result)
        {
            UserId op_user = _dataService.GetHttpContext().User.GetUserId();//.GetCurrentUser().Id;

            if (!this.Get(out var status, model.Id, model.Name, out result))
                return status;

            //if (!this.Get(model.Id, model.Name, out corp))
            //    return _null.noop(Status.CorpNotExist, out corp);

            var _sql = new SqlBuilder(model)
            {
                { " w", nameof(Entity.CorpInfo.Id)             , result.Id           },
                { " u", nameof(Entity.CorpInfo.Active)         },
                { "Nu", nameof(Entity.CorpInfo.DisplayName)    },
                { op_user }
            };

            if (_sql.update_set(out string sql_u))
            {
                string sql_w = _sql.where();
                string sql = _sql.FormatWith($@"{sql_u} {sql_w}
select * from {SqlBuilder.TableName} {sql_w}");

                using (SqlCmd coredb = _dataService.CoreDB_W())
                    result = coredb.ToObject<Entity.CorpInfo>(sql, transaction: true);
                _cache.UpdateVersion();
            }
            return Status.Success;
        }

        /// <summary>
        /// 取得 <see cref="Entity.CorpInfo"/>, 並且檢查 user 是否有存取權限
        /// </summary>
        public bool Get(out Status status, CorpId? id, UserName name, out Entity.CorpInfo corp, bool chechActive = true)
        {
            corp = null;
            if (this.Get(id, out corp))
                goto _step2;

            if (this.Get(name, out corp))
                goto _step2;

            status = Status.CorpNotExist;
            return false;

            _step2:

            if (chechActive)
            {
                if (corp.Active != ActiveState.Active)
                {
                    status = Status.CorpDisabled;
                    return false;
                }
            }

            UserId op_user = _dataService.GetHttpContext().User.GetUserId();//.GetCurrentUser().Id;

            if (!_dataService.GetService<AclDataProvider>().HasPermission(corp, op_user))
            {
                status = Status.AccessDenied;
                return false;
            }

            status = Status.Success;
            return true;
        }

        //public int SetDbConfig(CorpId id, DbConnectionString user_r, DbConnectionString user_w, DbConnectionString log_r, DbConnectionString log_w)
        //{
        //    if (user_r.IsEmpty &&
        //        user_w.IsEmpty &&
        //        log_r.IsEmpty &&
        //        log_w.IsEmpty)
        //        return 0;

        //    List<Entity.Config> values = new List<Entity.Config>();
        //    if (SqlCmd.TestConnectionString(user_r))
        //        values.Add(new Entity.Config() { CorpId = id, PlatformId = 0, Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.UserDB_R, Value = user_r });
        //    if (SqlCmd.TestConnectionString(user_w))
        //        values.Add(new Entity.Config() { CorpId = id, PlatformId = 0, Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.UserDB_W, Value = user_w });
        //    if (SqlCmd.TestConnectionString(log_r))
        //        values.Add(new Entity.Config() { CorpId = id, PlatformId = 0, Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.LogDB_R, Value = log_r });
        //    if (SqlCmd.TestConnectionString(log_w))
        //        values.Add(new Entity.Config() { CorpId = id, PlatformId = 0, Key1 = _Consts.db.SqlConnection, Key2 = _Consts.db.LogDB_W, Value = log_w });
        //    return _config.SetConfigData(values.ToArray()).Length;
        //}
    }
}