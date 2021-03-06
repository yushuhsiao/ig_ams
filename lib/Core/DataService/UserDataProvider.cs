using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory
{
    public class UserDataProvider : IDataService
    {
        private readonly DataService _dataService;
        private IConfiguration _config;

        public UserDataProvider(DataService dataService)
        {
            this._dataService = dataService;
            this._config = dataService.GetService<IConfiguration<UserDataProvider>>();
        }


        public bool GetUser(UserName corpname, UserName username, out Entity.UserData result)
        {
            if (corpname.IsValid && username.IsValid)
            {
                if (_dataService.Corps.Get(corpname, out var corp))
                {
                    return GetUser(corp.Id, username, out result);
                }
            }
            return _null.noop(false, out result);
        }
        public bool GetUser(CorpId corpId, UserName name, out Entity.UserData result)
        {
            result = null;
            if (corpId.IsValid && name.IsValid)
            {
                if (_dataService.Agents.Get(corpId, name, out var agent))
                    result = agent;
                else if (_dataService.Admins.Get(corpId, name, out var admin))
                    result = admin;
                else if (_dataService.Members.Get(corpId, name, out var member))
                    result = member;
            }
            return result != null;
        }

        public bool GetUser(UserId id, out Entity.UserData result/*, bool withCorpId = true*/)
        {
            result = null;
            if (id.IsValid)
            {
                if (_dataService.Agents.Get(id, out var agent/*, withCorpId*/))
                    result = agent;
                else if (_dataService.Admins.Get(id, out var admin/*, withCorpId*/))
                    result = admin;
                else if (_dataService.Members.Get(id, out var member/*, withCorpId*/))
                    result = member;
            }
            return result != null;
        }
        public Entity.UserData GetUser(UserId id/*, bool withCorpId = true*/)
        {
            this.GetUser(id, out var result/*, withCorpId*/);
            return result;
        }



        public Status UserLogin(Models.LoginModel model, out Entity.UserData userData, bool loginLog = false)
        {
            userData = null;

            if (_dataService.Corps.Get(model.CorpName, out var corp))
                model.CorpId = corp.Id;
            else
                return Status.CorpNotExist;
            if (corp.Active != ActiveState.Active)
                return Status.CorpDisabled;

            #region Get UserData

            if (model.LoginType == UserType.Agent)
            {
                if (!this.AllowAgentLogin)
                    return Status.UserTypeNotAllow;

                if (_dataService.Agents.Get(corp.Id, model.UserName, out var agent))
                    model.UserId = agent.Id;
                else
                    return Status.AgentNotExist;

                if (agent.Active != ActiveState.Active)
                    return Status.AgentDisabled;

                userData = agent;
            }
            else if (model.LoginType == UserType.Admin)
            {
                if (!this.AllowAdminLogin)
                    return Status.UserTypeNotAllow;

                if (_dataService.Admins.Get(corp.Id, model.UserName, out var admin))
                    model.UserId = admin.Id;
                else
                    return Status.AdminNotExist;

                if (admin.Active != ActiveState.Active)
                    return Status.AdminDisabled;

                userData = admin;
            }
            else if (model.LoginType == UserType.Member)
            {
                if (!this.AllowMemberLogin)
                    return Status.UserTypeNotAllow;

                if (_dataService.Members.Get(corp.Id, model.UserName, out var member))
                    model.UserId = member.Id;
                else
                    return Status.MemberNotExist;

                if (member.Active != ActiveState.Active)
                    return Status.MemberDisabled;

                userData = member;
            }
            else
            {
                return Status.InvalidParameter;
            }

            #endregion

            var pws = _dataService.GetService<PasswordProvider>();
            var pwd = pws.Get(userData.Id);
            if (pwd == null)
                return Status.PasswordNotFound;
            if (pwd.IsExpire)
                return Status.PasswordExpired;
            if (!pws.IsMatch(pwd, model.Password))
                return Status.PasswordNotMatch;

            return Status.Success;
        }

        public void WriteLoginLog(Status statusCode, Models.LoginModel model, CorpId? corpId, HttpContext httpContext)
        {
            try
            {
                var _sql = new SqlBuilder(typeof(Entity.LoginLog))
                {
                    { " ", nameof(Entity.LoginLog.LoginType)      , model.LoginType?.ToString() },
                    { " ", nameof(Entity.LoginLog.CorpName)       , model.CorpName },
                    { " ", nameof(Entity.LoginLog.UserName)       , model.UserName },
                    { " ", nameof(Entity.LoginLog.IP)             , httpContext.Connection.RemoteIpAddress.ToString() },
                    { " ", nameof(Entity.LoginLog.Result)         , statusCode.ToString() },
                    { " ", nameof(Entity.LoginLog.LoginTime)      , model.Time },
                };

                if (model.CorpId.HasValue)
                    _sql.Add(" ", nameof(Entity.LoginLog.CorpId), model.CorpId);
                if (model.UserId.HasValue)
                    _sql.Add(" ", nameof(Entity.LoginLog.UserId), model.UserId);

                if (statusCode != Status.Success)
                {
                    _sql.Add(" ", nameof(Entity.LoginLog.Password), model.Password);
                }

                string sql = _sql.FormatWith(_sql.insert_into());

                var dataService = httpContext.RequestServices.GetService<DataService>();
                using (IDbConnection logdb = dataService.DbConnections.LogDB_W(corpId ?? CorpId.Root))
                using (IDbTransaction tran = logdb.BeginTransaction())
                {
                    logdb.Execute(sql, null, tran);
                    tran.Commit();
                }
            }
            catch
            {
            }
        }



        //[AppSetting(SectionName = _Consts.UserManager.ConfigSection, Key = _Consts.UserManager.InternalApiServer), DefaultValue(false)]
        //public bool InternalApiServer => _config.GetValue<bool>();

        [AppSetting(SectionName = _Consts.UserManager.ConfigSection), DefaultValue(true)]
        public bool AllowAgentLogin => _config.GetValue<bool>();

        [AppSetting(SectionName = _Consts.UserManager.ConfigSection), DefaultValue(true)]
        public bool AllowAdminLogin => _config.GetValue<bool>();

        [AppSetting(SectionName = _Consts.UserManager.ConfigSection), DefaultValue(false)]
        public bool AllowMemberLogin => _config.GetValue<bool>();
    }
}
namespace InnateGlory.Entity.Abstractions
{
    public abstract class UserDataProvider<TUserData> where TUserData : UserData
    {
        public abstract UserType UserType { get; }
        protected abstract Status Status_UserAlreadyExist { get; }
        protected abstract Status Status_UserNotExist { get; }
        protected abstract Status Status_UserDisabled { get; }

        protected readonly DataService _dataService;

        public UserDataProvider(DataService dataService)
        {
            this._dataService = dataService;
            //this._cache_by_name = dataService.GetDbCache<Dictionary<UserName, TUserData>>(_ReadData, name: $"UserCacheByName_{typeof(TUserData).Name}");
            //this._cache_by_id = dataService.GetDbCache<Dictionary<UserId, TUserData>>(_ReadData, name: $"UserCacheById_{typeof(TUserData).Name}");
        }

        protected virtual string sql_get(UserId id)
            => $@"select * from {TableName<UserData>.Value} where Id=@Id";
        protected virtual string sql_get(CorpId corpId, UserName name)
            => $@"select * from {TableName<UserData>.Value} where CorpId=@CorpId and Name=@Name";

        public virtual bool Get(UserId? id, out TUserData result)
        {
            if (id.HasValue && id.Value.IsValid)
            {
                string sql = sql_get(id.Value);
                using (var userdb = _dataService.DbConnections.UserDB_W(id.Value.CorpId))
                {
                    result = userdb.QueryFirstOrDefault<TUserData>(sql, new
                    {
                        Id = id.Value.Id
                    });
                }
            }
            else result = null;
            return result != null;
        }

        public virtual bool Get(CorpId? corpId, UserName name, out TUserData result)
        {
            if (corpId.HasValue && corpId.Value.IsValid && name.IsValid)
            {
                string sql = sql_get(corpId.Value, name);
                using (var userdb = _dataService.DbConnections.UserDB_W(corpId.Value))
                {
                    result = userdb.QueryFirstOrDefault<TUserData>(sql, new
                    {
                        CorpId = corpId.Value.Id,
                        Name = name.Value
                    });
                }
                //Dictionary<UserName, TUserData> cache = null;
                ////if (fromCache)
                ////{
                ////    cache = _cache_by_name[corpId].GetFirstValue();
                ////    if (cache.TryGetValue(name, out result, syncLock: true))
                ////        return result != null;
                ////}
                //string sql = $"select * from {TableName<TUserData>.Value} where CorpId={corpId.Value} and Name='{name}'";
                //using (SqlCmd userdb = _dataService.SqlCmds.UserDB_R(corpId.Value))
                //    result = userdb.ToObject<TUserData>(sql);
                //cache?.SetValue(name, result, syncLock: true);
            }
            else result = null;
            return result != null;
        }

        public bool Get(UserId? id, CorpId? corpId, UserName name, out TUserData result)
        {
            if (this.Get(id, out result))
                return true;
            return this.Get(corpId, name, out result);
        }



        public TUserData Get(UserId? id)
        {
            this.Get(id, out var result);
            return result;
        }

        public TUserData Get(CorpId? corpId, UserName name)
        {
            this.Get(corpId, name, out var result);
            return result;
        }

        public TUserData Get(UserId? id, CorpId? corpId, UserName name)
        {
            this.Get(id, corpId, name, out var result);
            return result;
        }



        public virtual bool Get(out Status status, UserId? id, CorpId? corpId, UserName corpname, UserName name, out TUserData agent, bool chechActive = true)
        {
            if (this.Get(id, out agent))
                goto _step2;

            if (this.Get(corpId, name, out agent))
                goto _step2;

            if (_dataService.Corps.Get(corpname, out var _corp) &&
                this.Get(_corp.Id, name, out agent))
                goto _step2;

            status = Status_UserNotExist;
            return false;

            _step2:

            if (chechActive)
            {
                if (agent.Active == ActiveState.Active)
                {
                    status = Status_UserDisabled;
                    return false;
                }
            }

            status = Status.Success;
            return true;
        }


        //public /**/ bool Get(UserId? id, CorpId? corpId, UserName name, out TUserData result)
        //{
        //    if (id.HasValue && this.Get(id.Value, out result))
        //        return true;
        //    if (corpId.HasValue && this.Get(corpId.Value, name, out result))
        //        return true;
        //    return _null.noop(false, out result);
        //}
        //public TUserData Get(UserId? id, CorpId? corpId, UserName name)
        //{
        //    this.Get(id, corpId, name, out var result);
        //    return result;
        //}

        //protected bool AllocUserId(SqlCmd userdb, CorpId corpId, UserType userType, UserName userName, out UserId result, out Status statusCode)
        //{
        //    result = default(UserId);
        //    statusCode = Status.Unknown;
        //    try
        //    {
        //        for (int retry = 5; retry >= 0; retry--)
        //        {
        //            foreach (Action commit in userdb.BeginTran())
        //            {
        //                string sql1 = new SqlBuilder()
        //                {
        //                    { "", "CorpId"  , corpId   },
        //                    { "", "UserType", userType },
        //                    { "", "UserName", userName }
        //                }.exec("alloc_UserId", formatWith: true);
        //                //string sql1 = @"exec alloc_UserId @CorpId={CorpId}, @UserType={UserType}, @UserName={UserName}".
        //                //    FormatWith(new { CorpId = corpId, UserType = userType, UserName = userName }, sql: true);
        //                UserId? n = null;
        //                foreach (SqlDataReader r in userdb.ExecuteReaderEach(sql1))
        //                    n = r.GetInt64N("Id");

        //                if (n.HasValue)
        //                {
        //                    result = n.Value.SetCorpId(corpId);
        //                    string sql2 = $"select Id from {TableName<TUserData>.Value} where Id={result}";
        //                    foreach (SqlDataReader r in userdb.ExecuteReaderEach(sql2))
        //                        n = null;

        //                    if (n.HasValue)
        //                    {
        //                        commit();
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //        statusCode = Status.UnableAllocateUserID;
        //        return false;
        //    }
        //    catch (SqlException ex) when (ex.IsDuplicateKey())
        //    {
        //        statusCode = Status_UserAlreadyExist;
        //    }
        //    catch
        //    {
        //    }
        //    return false;
        //}
        protected bool AllocUserId(IDbConnection userdb, CorpId corpId, UserType userType, UserName userName, 
            out UserId result, out Status statusCode, out IDbTransaction transaction)
        {
            result = default(UserId);
            statusCode = Status.Unknown;
            transaction = null;
            try
            {
                for (int retry = 5; retry >= 0; retry--)
                {
                    var tran = userdb.BeginTransaction();
                    string sql1 = new SqlBuilder()
                        {
                            { "", "CorpId"  , corpId   },
                            { "", "UserType", userType },
                            { "", "UserName", userName }
                        }.exec("alloc_UserId", formatWith: true);
                    UserId? n = userdb.ExecuteScalar<long>(sql1, null, tran);

                    if (n.HasValue)
                    {
                        result = n.Value.SetCorpId(corpId);
                        string sql2 = $"select Id from {TableName<UserData>.Value} where Id={result}";
                        n = userdb.ExecuteScalar<long>(sql2, null, tran);

                        if (n.HasValue)
                        {
                            transaction = tran;
                            return true;
                        }
                    }
                    using (tran)
                        tran.Rollback();
                }
                statusCode = Status.UnableAllocateUserID;
                return false;
            }
            catch (SqlException ex) when (ex.IsDuplicateKey())
            {
                statusCode = Status_UserAlreadyExist;
            }
            catch
            {
            }
            return false;
        }

        //protected bool CheckMaxLimit(SqlCmd userdb, UserId parentId, int? maxValue)
        //{
        //    if (maxValue.HasValue && maxValue.Value > 0)
        //    {
        //        string sql = $"select count(Id) as cnt from {TableName<TUserData>.Value} where ParentId={parentId} and UserType={(int)UserType}";
        //        int count = (int)userdb.ExecuteScalar(sql);
        //        if (count >= maxValue.Value)
        //            return true;
        //    }
        //    return false;
        //}
        protected bool CheckMaxLimit(IDbConnection userdb, UserId parentId, int? maxValue)
        {
            if (maxValue.HasValue && maxValue.Value > 0)
            {
                string sql = $@"select count(Id) as cnt from {TableName<UserData>.Value}
where ParentId=@ParentId and UserType=@UserType";
                int count = userdb.ExecuteScalar<int>(sql, new
                {
                    ParentId = (int)parentId,
                    UserType = (int)this.UserType
                });
                if (count >= maxValue.Value)
                    return true;
            }
            return false;
        }
    }
}
