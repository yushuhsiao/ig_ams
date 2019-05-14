using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace InnateGlory
{
    public class UserDataProvider : IDataService
    {
        private readonly DataService _dataService;

        //public AgentDataProvider Agents => _dataService.Agents;
        //public AdminDataProvider Admins => _dataService.Admins;
        //public MemberDataProvider Members => _dataService.Members;

        public UserDataProvider(DataService dataService)
        {
            this._dataService = dataService;
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



        public Status UserLogin(Models.LoginModel model, out Entity.CorpInfo corp, out Entity.UserData userData, bool loginLog = false)
        {
            IUserManager userManager = _dataService.GetService<IUserManager>();
            userData = null;

            if (_dataService.Corps.Get(model.CorpName, out corp))
                model.CorpId = corp.Id;
            else
                return Status.CorpNotExist;
            if (corp.Active != ActiveState.Active)
                return Status.CorpDisabled;

            #region Get UserData

            if (model.LoginType == UserType.Agent)
            {
                if (!userManager.AllowAgentLogin)
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
                if (!userManager.AllowAdminLogin)
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
                if (!userManager.AllowMemberLogin)
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

            if (statusCode == Status.Success)
            {
            }
            else
            {
                _sql.Add(" ", nameof(Entity.LoginLog.Password), model.Password);
            }

            string sql = _sql.FormatWith(_sql.insert_into());
            ;

            try
            {
                var dataService = httpContext.RequestServices.GetService<DataService>();
                using (SqlCmd logdb = dataService.LogDB_W(corpId ?? CorpId.Root))
                    logdb.ExecuteNonQuery(sql, transaction: true);
            }
            catch
            {
            }
        }
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
        //protected UserDataProvider UserService => _dataService.Users;
        //protected DbCache<Dictionary<UserName, TUserData>> _cache_by_name;
        //protected DbCache<Dictionary<UserId, TUserData>> _cache_by_id;

        public UserDataProvider(DataService dataService)
        {
            this._dataService = dataService;
            //this._cache_by_name = dataService.GetDbCache<Dictionary<UserName, TUserData>>(_ReadData, name: $"UserCacheByName_{typeof(TUserData).Name}");
            //this._cache_by_id = dataService.GetDbCache<Dictionary<UserId, TUserData>>(_ReadData, name: $"UserCacheById_{typeof(TUserData).Name}");
        }

        //private IEnumerable<Dictionary<UserName, TUserData>> _ReadData(DbCache<Dictionary<UserName, TUserData>>.Entry sender, Dictionary<UserName, TUserData>[] oldValue)
        //{
        //    yield return new Dictionary<UserName, TUserData>();
        //}
        //private IEnumerable<Dictionary<UserId, TUserData>> _ReadData(DbCache<Dictionary<UserId, TUserData>>.Entry sender, Dictionary<UserId, TUserData>[] oldValue)
        //{
        //    yield return new Dictionary<UserId, TUserData>();
        //}

        public virtual bool Get(UserId? id, out TUserData result)
        {
            if (id.HasValue && id.Value.IsValid)
            {
                //Dictionary<UserId, TUserData> cache = null;
                //if (fromCache)
                //{
                //    cache = _cache_by_id[id.CorpId].GetFirstValue();
                //    if (cache.TryGetValue(id, out result, syncLock: true))
                //        return result != null;
                //}
                string sql;
                //if (withCorpId)
                //    sql = $"{SqlBuilder.select_all_from<TUserData>()} where CorpId={id.CorpId} and Id={id}";
                //else
                sql = $"select * from {TableName<TUserData>.Value} nolock where Id={id.Value}";

                using (SqlCmd userdb = _dataService.UserDB_R(id.Value.CorpId))
                    result = userdb.ToObject<TUserData>(sql);

                //cache?.SetValue(_id, result, syncLock: true);
            }
            else result = null;
            return result != null;
        }

        public virtual bool Get(CorpId? corpId, UserName name, out TUserData result)
        {
            if (corpId.HasValue && corpId.Value.IsValid && name.IsValid)
            {
                Dictionary<UserName, TUserData> cache = null;
                //if (fromCache)
                //{
                //    cache = _cache_by_name[corpId].GetFirstValue();
                //    if (cache.TryGetValue(name, out result, syncLock: true))
                //        return result != null;
                //}
                string sql = $"select * from {TableName<TUserData>.Value} nolock where CorpId={corpId.Value} and Name='{name}'";
                using (SqlCmd userdb = _dataService.UserDB_R(corpId.Value))
                    result = userdb.ToObject<TUserData>(sql);
                cache?.SetValue(name, result, syncLock: true);
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

        protected bool AllocUserId(SqlCmd userdb, CorpId corpId, UserType userType, UserName userName, out UserId result, out Status statusCode)
        {
            result = default(UserId);
            statusCode = Status.Unknown;
            try
            {
                for (int retry = 5; retry >= 0; retry--)
                {
                    foreach (Action commit in userdb.BeginTran())
                    {
                        string sql1 = new SqlBuilder()
                        {
                            { "", "CorpId"  , corpId   },
                            { "", "UserType", userType },
                            { "", "UserName", userName }
                        }.exec("alloc_UserId", formatWith: true);
                        //string sql1 = @"exec alloc_UserId @CorpId={CorpId}, @UserType={UserType}, @UserName={UserName}".
                        //    FormatWith(new { CorpId = corpId, UserType = userType, UserName = userName }, sql: true);
                        UserId? n = null;
                        foreach (SqlDataReader r in userdb.ExecuteReaderEach(sql1))
                            n = r.GetInt64N("Id");

                        if (n.HasValue)
                        {
                            result = n.Value.SetCorpId(corpId);
                            string sql2 = $"select Id from {TableName<TUserData>.Value} nolock where Id={result}";
                            foreach (SqlDataReader r in userdb.ExecuteReaderEach(sql2))
                                n = null;

                            if (n.HasValue)
                            {
                                commit();
                                return true;
                            }
                        }
                    }
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

        protected bool CheckMaxLimit(SqlCmd userdb, UserId parentId, int? maxValue)
        {
            if (maxValue.HasValue && maxValue.Value > 0)
            {
                string sql = $"select count(Id) as cnt from {TableName<TUserData>.Value} nolock where ParentId={parentId}";
                int count = (int)userdb.ExecuteScalar(sql);
                if (count >= maxValue.Value)
                    return true;
            }
            return false;
        }
    }
}
