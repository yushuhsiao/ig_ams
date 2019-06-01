using Dapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.Data.SqlClient;

namespace InnateGlory
{
    public class MemberDataProvider : Entity.Abstractions.UserDataProvider<Entity.Member>, IDataService
    {
        public override UserType UserType => UserType.Member;
        protected override Status Status_UserAlreadyExist => Status.MemberAlreadyExist;
        protected override Status Status_UserNotExist => Status.MemberNotExist;
        protected override Status Status_UserDisabled => Status.MemberDisabled;

        public MemberDataProvider(DataService dataService) : base(dataService) { }

        public override bool Get(out Status status, UserId? id, CorpId? corpId, UserName corpname, UserName name, out Entity.Member agent, bool chechActive = true)
        {
            return base.Get(out status, id, corpId, corpname, name, out agent, chechActive);
        }

        public Status Create(Models.MemberModel model, out Entity.Member result)
        {
            result = null;
            UserId op_user = _dataService.HttpContext().User.GetUserId();//.GetCurrentUser().Id;

            if (!_dataService.Corps.Get(out var statusCode, model.CorpId, model.CorpName, out var corp))
                return statusCode;

            using (IDbConnection userdb = _dataService.DbConnections.UserDB_W(corp.Id))
            {
                if (_dataService.Agents.Get(model.CorpId, model.Name, out var _agent) ||
                    _dataService.Members.Get(model.CorpId, model.Name, out var _member))
                    return Status.MemberAlreadyExist;

                if (!_dataService.Agents.Get(model.Id, corp.Id, model.ParentName, out var parent))
                    return Status.ParentNotExist;

                #region check depth
                int depth = parent.Depth + 1;
                foreach (var _parent in _dataService.Agents.GetParents(parent, include_self: true))
                {
                    if (_parent.Active != ActiveState.Active)
                        return Status.ParentDisabled;
                }
                #endregion
                SqlBuilder _sql = new SqlBuilder(typeof(Entity.Member))
                {
                    { "w", nameof(Entity.Member.Id) },
                    { " ", nameof(Entity.Member.CorpId)       , parent.CorpId },
                    { " ", nameof(Entity.Member.Name)         , model.Name },
                    { " ", nameof(Entity.Member.Active)       , model.Active ?? ActiveState.Active },
                    { " ", nameof(Entity.Member.ParentId)     , parent.Id },
                    { "N", nameof(Entity.Member.DisplayName)  , model.DisplayName ?? model.Name },
                    { " ", nameof(Entity.Member.Depth)        , depth },
                    { op_user, op_user }
                };

                if (CheckMaxLimit(userdb, parent.Id, parent.MaxMembers))
                    return Status.MaxMemberLimit;

                if (!AllocUserId(userdb, corp.Id, UserType.Agent | UserType.Member, model.Name, out var new_id, out statusCode, out var tran))
                    return statusCode;
                using (tran)
                {
                    _sql[nameof(Entity.Member.Id)] = new_id;

                    string sql_insert = _sql.FormatWith($@"{_sql.insert_into()}
{_sql.select_where()}");
                    result = userdb.QueryFirstOrDefault<Entity.Member>(sql_insert, null, tran);
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

        public Status Update(Models.MemberModel model, out Entity.Member result)
        {
            return _null.noop(Status.Success, out result);
        }



        public Entity.UserBalance GetBalance(Entity.Member member)
        {
            string sql = $"select * from {TableName<Entity.UserBalance>.Value} where Id={member.Id}";
            using (IDbConnection userdb = _dataService.DbConnections.UserDB_R(member.CorpId))
                return userdb.QuerySingleOrDefault<Entity.UserBalance>(sql) ?? new Entity.UserBalance() { Id = member.Id };
        }
    }
}
