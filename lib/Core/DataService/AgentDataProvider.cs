using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory
{
    public class AgentDataProvider : Entity.Abstractions.UserDataProvider<Entity.Agent>, IDataService
    {
        public override UserType UserType => UserType.Agent;
        protected override Status Status_UserAlreadyExist => Status.AgentAlreadyExist;
        protected override Status Status_UserNotExist => Status.AgentNotExist;
        protected override Status Status_UserDisabled => Status.AgentDisabled;

        public AgentDataProvider(DataService dataService) : base(dataService) { }

        protected override string sql_get(UserId id)
            => $@"select * from {TableName<Entity.UserData>.Value} a
left join {TableName<Entity.Users_Agent>.Value} b
on a.Id=b.Id
where a.Id=@Id";

        protected override string sql_get(CorpId corpId, UserName name)
            => $@"select * from {TableName<Entity.UserData>.Value} a
left join {TableName<Entity.Users_Agent>.Value} b
on a.Id=b.Id
where a.CorpId=@CorpId and a.Name=@Name";

        public override bool Get(UserId? id, out Entity.Agent result/*, bool withCorpId = true*/)
        {
            if (base.Get(id, out result/*, withCorpId*/))
                return true;
            if (id.HasValue && id.Value.IsCorpRoot && _dataService.Corps.Get(id.Value.CorpId, out var corp))
                return CreateRootAgent(corp, out result);
            return false;
        }

        public override bool Get(CorpId? corpId, UserName name, out Entity.Agent result)
        {
            if (base.Get(corpId, name, out result))
                return true;
            if (_dataService.Corps.Get(corpId, out var corp) && corp.Name == name)
                return CreateRootAgent(corp, out result);
            return false;
        }

        #region Root Agent

        //public Entity.Agent GetRootAgent(CorpId corpId) => this.Get((UserId)corpId.Id);

        public Entity.Agent GetRootAgent(Entity.CorpInfo corp) => this.Get((UserId)corp.Id.Id);

        //public bool GetRootAgent(CorpId corpId, out Entity.Agent result) => this.Get((UserId)corpId.Id, out result);

        public bool GetRootAgent(Entity.CorpInfo corp, out Entity.Agent result) => this.Get((UserId)corp.Id, out result);

        /// <summary>
        /// 建立頂級代理
        /// </summary>
        private bool CreateRootAgent(Entity.CorpInfo corp, out Entity.Agent result)
        {
            var sql_select = sql_get(default(UserId));
            var sql = $@"
if not exists (select Id from [{TableName<Entity.UserData>.Value}] where [Id] = @Id)
begin
    insert into [{TableName<Entity.UserData>.Value}] ([Id], [UserType], [CorpId], [Name], [Active], [ParentId], [DisplayName], [Depth], [CreateUser], [ModifyUser])
    values (@Id, @UserType, @CorpId, @Name, @Active, @ParentId, @DisplayName, @Depth, @CreateUser, @ModifyUser)
    if not exists (select Id from [{TableName<Entity.Users_Agent>.Value}] where Id=@Id)
        insert into [{TableName<Entity.Users_Agent>.Value}] ([Id], [MaxDepth]) values (@Id, @MaxDepth)
end
{sql_select}";
            var param = new
            {
                Id = (int)corp.Id,
                UserType = (int)UserType.Agent,
                CorpId = (long)corp.Id,
                Name = (string)corp.Name,
                Active = (int)ActiveState.Active,
                ParentId = (int)UserId.Null,
                DisplayName = corp.DisplayName ?? corp.Name,
                MaxDepth = corp.Id.IsRoot ? 0 : 1,
                Depth = 1,
                CreateUser = (int)corp.CreateUser,
                ModifyUser = (int)corp.CreateUser
            };
            try
            {
                using (IDbConnection userdb = _dataService.DbConnections.UserDB_W(corp.Id))
                using (IDbTransaction tran = userdb.BeginTransaction())
                {
                    //result = userdb.ToObject<Entity.Agent>(sql, transaction: true);
                    result = userdb.QuerySingleOrDefault<Entity.Agent>(sql, param, tran);
                    tran.Commit();
                }
            }
            catch (SqlException ex) when (ex.IsDuplicateKey())
            {
                using (IDbConnection userdb = _dataService.DbConnections.UserDB_R(corp.Id))
                {
                    result = userdb.QuerySingleOrDefault<Entity.Agent>(sql_select, param);
                }
            }
            return result != null;

            //        var _sql = new SqlBuilder(typeof(Entity.Agent))
            //        {
            //            { "w", nameof(Entity.Agent.Id)            , corp.Id },
            //            { " ", nameof(Entity.Agent.CorpId)        , corp.Id },
            //            { " ", nameof(Entity.Agent.Name)          , corp.Name },
            //            { " ", nameof(Entity.Agent.Active)        , ActiveState.Active },
            //            { " ", nameof(Entity.Agent.ParentId)      , UserId.Null },
            //            { "N", nameof(Entity.Agent.DisplayName)   , corp.DisplayName ?? corp.Name},
            //            { " ", nameof(Entity.Agent.MaxDepth)      , corp.Id.IsRoot ? 0 : 1},
            //            { " ", nameof(Entity.Agent.Depth)         , 1 },
            //            { corp.CreateUser, corp.CreateUser }
            //        };
            //        string sql = _sql.FormatWith($@"if not exists (select Id from {SqlBuilder.TableName} {_sql.where()})
            //{_sql.insert_into()}
            //{_sql.select_where()}");

            //        try
            //        {
            //            using (SqlCmd userdb = _dataService.SqlCmds.UserDB_W(corp.Id, state: _sql))
            //                result = userdb.ToObject<Entity.Agent>(sql, transaction: true);
            //        }
            //        catch (SqlException ex) when (ex.IsDuplicateKey())
            //        {
            //            using (SqlCmd userdb = _dataService.SqlCmds.UserDB_R(corp.Id))
            //                result = userdb.ToObject<Entity.Agent>(_sql.select_where());
            //        }
            //        return result != null;
        }

        #endregion

        public IEnumerable<Entity.Agent> GetParents(Entity.Agent agent, bool include_self)
        {
            if (agent != null)
            {
                if (include_self) yield return agent;
                var a = agent;
                while (this.Get(a.ParentId, out a))
                    yield return a;
            }
            //if (include_self)
            //    a = agent;
            //else
            //    a = this.Get(agent.ParentId);
            //for (; a != null; a = this.Get(a.ParentId))
            //{
            //    yield return a;
            //}
        }

        public IEnumerable<Entity.Agent> GetParents(UserId agentId, bool include_self) => GetParents(Get(agentId), include_self);

        //public List<Entity.Agent> GetChilds(UserId agentId)
        //{
        //    if (this.Get(agentId, out var agent))
        //    {
        //        string sql = $"select * from {TableName<Entity.Agent>.Value} where ParentId={agentId}";
        //        using (SqlCmd userdb = _dataService.SqlCmds.UserDB_R(agent.CorpId))
        //            return userdb.ToList<Entity.Agent>(sql);
        //    }
        //    return null;
        //}
        public IEnumerable<Entity.Agent> GetChilds(UserId agentId)
        {
            if (this.Get(agentId, out var agent))
            {
                string sql = $"select * from {TableName<Entity.UserData>.Value} where ParentId={agentId} and UserType={(int)UserType.Agent}";
                using (IDbConnection userdb = _dataService.DbConnections.UserDB_R(agent.CorpId))
                    return userdb.Query<Entity.Agent>(sql);
            }
            return null;
        }


        public Status Create(Models.AgentModel model, out Entity.Agent result)
        {
            result = null;
            UserId op_user = _dataService.HttpContext().User.GetUserId();// .GetCurrentUser().Id;

            if (!_dataService.Corps.Get(out var statusCode, model.CorpId, model.CorpName, out var corp))
                return statusCode;

            //if (!_dataService.Corps.GetWithAcl(null, model.CorpId, model.CorpName, op_user, out Status statusCode, out Data.CorpInfo corp))
            //    return statusCode;
            using (IDbConnection userdb = _dataService.DbConnections.UserDB_W(corp.Id))
            {
                if (_dataService.Agents.Get(model.CorpId, model.Name, out var _agent) ||
                    _dataService.Members.Get(model.CorpId, model.Name, out var _member))
                    return Status.AgentAlreadyExist;

                if (!_dataService.Agents.Get(model.Id, corp.Id, model.ParentName, out var parent))
                    return Status.ParentNotExist;

                #region check depth
                int depth = parent.Depth + 1;
                foreach (var _parent in _dataService.Agents.GetParents(parent, include_self: true))
                {
                    if (_parent.Active != ActiveState.Active)
                        return Status.ParentDisabled;
                    int d = depth - _parent.Depth;
                    if (d > _parent.MaxDepth)
                        return Status.MaxDepthLimit;
                }
                #endregion
                SqlBuilder _sql = new SqlBuilder(typeof(Entity.Agent))
                {
                    { "w", nameof(Entity.Agent.Id) },
                    { " ", nameof(Entity.Agent.CorpId)        , parent.CorpId },
                    { " ", nameof(Entity.Agent.Name)          , model.Name},
                    { " ", nameof(Entity.Agent.Active)        , model.Active ?? ActiveState.Active },
                    { " ", nameof(Entity.Agent.ParentId)      , parent.Id },
                    { "N", nameof(Entity.Agent.DisplayName)   , model.DisplayName ?? model.Name },
                    { " ", nameof(Entity.Agent.MaxDepth)      , 0 },
                    { " ", nameof(Entity.Agent.MaxAgents)     , model.MaxAgents ?? 0 },
                    { " ", nameof(Entity.Agent.MaxAdmins)     , model.MaxAdmins ?? 0 },
                    { " ", nameof(Entity.Agent.MaxMembers)    , SqlBuilder.IsNull(model.MaxMembers) },
                    { " ", nameof(Entity.Agent.Depth)         , depth },
                    { op_user, op_user }
                };

                if (CheckMaxLimit(userdb, parent.Id, parent.MaxAgents))
                    return Status.MaxAgentLimit;

                if (!AllocUserId(userdb, corp.Id, UserType.Agent | UserType.Member, model.Name, out var new_id, out statusCode, out var tran))
                    return statusCode;
                using (tran)
                {
                    _sql[nameof(Entity.Agent.Id)] = new_id;

                    string sql_insert = _sql.FormatWith($@"{_sql.insert_into()}
{_sql.select_where()}");
                    result = userdb.QuerySingleOrDefault<Entity.Agent>(sql_insert, null, tran);
                    if (result != null)
                    {
                        //_cache_by_name.UpdateVersion(result.CorpId);
                        //_cache_by_id.UpdateVersion(result.CorpId);
                        tran.Commit();
                        return Status.Success;
                    }
                }
            }
            return Status.Unknown;
        }

        public Status Update(Models.AgentModel model, out Entity.Agent result)
        {
            //UserId op_user = createUser ?? _dataService.GetCurrentUserId();
            return _null.noop(Status.Success, out result);
        }

        public override bool Get(out Status status, UserId? id, CorpId? corpId, UserName corpname, UserName name, out Entity.Agent result, bool chechActive = true)
        {
            if (base.Get(out status, id, corpId, corpname, name, out result, chechActive))
            {
                var op_userId = _dataService.HttpContext().User.GetUserId();//.GetCurrentUser().Id;
                if (op_userId.IsRoot)
                    return true;
                if (result.CorpId == op_userId.CorpId && op_userId.IsCorpRoot)
                    return true;
                //if (op_userId == result.Id) return true;
                //var op_user = _dataService.Users.GetUser(op_userId);
                //if (op_user.TryCast(out Data.Agent op_agent))
                //{
                //    foreach (var n in _dataService.Agents.GetParents(result, false))
                //        if (n.Id == result.Id)
                //            return true;
                //}
                //else if (op_user.TryCast(out Data.Admin op_admin))
                //{
                //}
                //else if (op_user.TryCast(out Data.Member op_member))
                //{
                //    if (op_member.ParentId == result.Id)
                //        return true;
                //    return false;
                //}
                return true;
            }
            return false;
        }



        public Entity.UserBalance GetBalance(Entity.Agent agent)
        {
            string sql = $"select * from {TableName<Entity.UserBalance>.Value} where Id={agent.Id}";
            using (IDbConnection userdb = _dataService.DbConnections.UserDB_R(agent.CorpId))
                return userdb.QuerySingleOrDefault<Entity.UserBalance>(sql) ?? new Entity.UserBalance() { Id = agent.Id };
        }
    }
}
