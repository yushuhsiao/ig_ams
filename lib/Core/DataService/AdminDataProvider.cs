using System;
using System.Data.SqlClient;

namespace InnateGlory
{
    public class AdminDataProvider : Entity.Abstractions.UserDataProvider<Entity.Admin>
    {
        public override UserType UserType => UserType.Admin;
        protected override Status Status_UserAlreadyExist => Status.AdminAlreadyExist;
        protected override Status Status_UserNotExist => Status.AdminNotExist;
        protected override Status Status_UserDisabled => Status.AgentDisabled;

        public AdminDataProvider(DataService dataService) : base(dataService) { }

        public Status Create(Models.AdminModel model, out Entity.Admin result)
        {
            result = null;
            UserId op_user = _dataService.GetCurrentUser().Id;

            if (!_dataService.Corps.Get(out var statusCode, model.CorpId, model.CorpName, out var corp))
                return statusCode;

            using (SqlCmd userdb = _dataService.UserDB_W(corp.Id))
            {
                if (_dataService.Admins.Get(model.CorpId, model.Name, out var _admin))
                    return Status.AdminAlreadyExist;

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
                SqlBuilder _sql = new SqlBuilder(typeof(Entity.Admin))
                {
                    { "w", nameof(Entity.Admin.Id) },
                    { " ", nameof(Entity.Admin.CorpId)        , parent.CorpId },
                    { " ", nameof(Entity.Admin.Name)          , model.Name },
                    { " ", nameof(Entity.Admin.Active)        , model.Active ?? ActiveState.Active },
                    { " ", nameof(Entity.Admin.ParentId)      , parent.Id },
                    { "N", nameof(Entity.Admin.DisplayName)   , model.DisplayName ?? model.Name },
                    { " ", nameof(Entity.Admin.Depth)         , depth },
                    { op_user, op_user }
                };

                if (CheckMaxLimit(userdb, parent.Id, parent.MaxAdmins))
                    return Status.MaxAdminLimit;

                if (!AllocUserId(userdb, corp.Id, UserType.Admin, model.Name, out UserId new_id, out statusCode))
                    return statusCode;
                _sql[nameof(Entity.Admin.Id)] = new_id;

                string sql_insert = _sql.FormatWith($@"{_sql.insert_into()}
{_sql.select_where()}");
                result = userdb.ToObject<Entity.Admin>(sql_insert, transaction: true);
                if (result != null)
                {
                    //_cache_by_name.UpdateVersion(result.CorpId);
                    //_cache_by_id.UpdateVersion(result.CorpId);
                    return Status.Success;
                }
            }
            return Status.Unknown;
        }

        public Status Update(Models.AdminModel model, out Entity.Admin result)
        {
            return _null.noop(Status.Success, out result);
        }

        public override bool Get(out Status status, UserId? id, CorpId? corpId, UserName corpname, UserName name, out Entity.Admin agent, bool chechActive = true)
        {
            return base.Get(out status, id, corpId, corpname, name, out agent, chechActive);
        }
    }
}
