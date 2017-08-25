using ams.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ModelBinding;

namespace ams
{
    public static class ModelStateDictionaryValidateExtensions
    {
        //public static T ToObject<T>(this SqlCmd sqlcmd, string commandText, Func<T> create = null, SqlBuilder.err err = default(SqlBuilder.err)) where T : class
        //{
        //    foreach (SqlDataReader r in sqlcmd.ExecuteReaderEach(commandText))
        //    {
        //        if (!string.IsNullOrEmpty(err.name))
        //        {
        //            int n;
        //            if (r.GetInt32(err.name, out n))
        //                throw new _Exception((Status)n, err.msg);
        //        }
        //        return r.ToObject<T>(create ?? Activator.CreateInstance<T>);
        //    }
        //    return null;
        //}

        [DebuggerStepThrough]
        public static void IsValid(this ModelStateDictionary modelstate, bool throw_exception = true)
        {
            if (modelstate.IsValid) return;
            if (throw_exception) throw new _Exception(modelstate);
        }

        public static void AddModelError(this ModelStateDictionary modelstate, string key, Status status, string errorMessage = null, bool throw_exception = false)
        {
            modelstate.AddModelError(key, new _Exception(status, errorMessage));
            if (throw_exception == true)
                modelstate.IsValid(true);
        }



        public static T ToObject<T>(this ModelStateDictionary modelstate, string key, string json_string, bool allow_null = false)
        {
            T obj = default(T);
            if (allow_null && string.IsNullOrEmpty(json_string))
                modelstate.AddModelError(key, Status.MissingParameter);
            else
            {
                try
                {
                    obj = json.DeserializeObject<T>(json_string);
                    if (allow_null && (obj == null))
                        modelstate.AddModelError(key, Status.MissingParameter);
                }
                catch { modelstate.AddModelError(key, Status.InvalidParameter); }
            }
            return obj;
        }

        public static T? Validate<T>(this ModelStateDictionary modelstate, string key, T? value, Func<T, bool> min = null, Func<T, bool> max = null, bool allow_null = false) where T : struct
        {
            key = key.Trim(false);
            Type t = typeof(T);
            if (value.HasValue)
            {
                if ((min != null) && !min(value.Value)) modelstate.AddModelError(key, Status.InvalidParameter);
                if ((max != null) && !max(value.Value)) modelstate.AddModelError(key, Status.InvalidParameter);
            }
            else if (!allow_null)
                modelstate.AddModelError(key, Status.MissingParameter);
            return value;
        }

        public static T? ValidateEnum<T>(this ModelStateDictionary modelstate, string key, T? value, bool allow_null = false) where T : struct
        {
            Type t = typeof(T);
            if (t.IsEnum)
            {
                if (value.HasValue)
                {
                    if (!t.IsEnumDefined(value.Value))
                    {
                        modelstate.AddModelError(key, Status.InvalidParameter);
                        return value;
                    }
                }
                else if (!allow_null)
                {
                    modelstate.AddModelError(key, Status.MissingParameter);
                    return value;
                }
            }
            return value;
        }

        public static void Validate(this ModelStateDictionary modelstate, JObject obj, string key, bool allow_null = false)
        {
            JToken value;
            if (!obj.TryGetValue(key, StringComparison.OrdinalIgnoreCase, out value))
                modelstate.AddModelError(key, Status.MissingParameter);
        }

        public static void Validate(this ModelStateDictionary modelstate, string key, string value, bool allow_null = false)
        {
            if (!allow_null && string.IsNullOrEmpty(value))
                modelstate.AddModelError(key, Status.MissingParameter);
        }

        public static bool Validate(this ModelStateDictionary modelstate, string key, UserName value, bool allow_null = false)
        {
            if (value.IsNullOrEmpty)
            {
                if (allow_null) return true;
                modelstate.AddModelError(key, Status.MissingParameter);
                return false;
            }
            else if (value.IsValid) return true;
            modelstate.AddModelError(key, Status.InvalidParameter);
            return false;
        }

        //public static T ValidateUser<T>(this ModelStateDictionary modelstate, string key_corp, UserID? corpID, string key_user, UserID? userID, UserName username, bool allow_null = false) where T : UserData<T>
        //{
        //    CorpInfo corp = modelstate.ValidateCorpID(key_corp, corpID);
        //    if (corp != null)
        //    {
        //        if (userID.HasValue)
        //            return corp.GetUserData<T>(userID.Value);
        //        else if (username.IsValidEx)
        //            return corp.GetUserData<T>(username.Value);
        //        else modelstate.AddModelError(key_user, Status.InvalidParameter);
        //    }
        //    return null;
        //}

        //public static CorpInfo ValidateCorpName(this ModelStateDictionary modelstate, string key, UserName value, SqlCmd coredb = null)
        //{
        //    CorpInfo corp = null;
        //    if (_User.Current.CorpID.IsRoot)
        //    {
        //        if (!modelstate.Validate(key, value))
        //            return null;
        //        corp = CorpInfo.GetCorpInfo(value, coredb);
        //    }
        //    else
        //    {
        //        corp = CorpInfo.GetCorpInfo(_User.Current.CorpID, coredb);
        //    }
        //    if (corp == null)
        //        modelstate.AddModelError(key, Status.CorpNotExist);
        //    return corp;
        //}

        //public static CorpInfo ValidateCorpID(this ModelStateDictionary modelstate, string key, UserID? value, SqlCmd coredb = null)
        //{
        //    CorpInfo corp = null;
        //    _User _user = _User.Current;
        //    if (_user.CorpID.IsRoot)
        //    {
        //        if (value.HasValue)
        //            corp = CorpInfo.GetCorpInfo(value.Value, coredb);
        //        else
        //        {
        //            modelstate.AddModelError(key, Status.InvalidParameter);
        //            return null;
        //        }
        //    }
        //    else
        //    {
        //        corp = CorpInfo.GetCorpInfo(_user.CorpID, coredb);
        //    }
        //    if (corp == null)
        //        modelstate.AddModelError(key, Status.CorpNotExist);
        //    return corp;
        //}

        public static PlatformInfo ValidatePlatformName(this ModelStateDictionary modelstate, string key, UserName value, SqlCmd coredb = null)
        {
            if (modelstate.Validate(key, value))
            {
                PlatformInfo platform;
                if (coredb == null)
                    platform = PlatformInfo.GetPlatformInfo(value);
                else
                    platform = PlatformInfo.GetPlatformInfo(value);
                if (platform == null)
                    modelstate.AddModelError(key, Status.PlatformNotExist);
                return platform;
            }
            return null;
        }

        public static bool Validate(this ModelStateDictionary modelstate, string key, LogType? value, params LogType[] logTypes)
        {
            if (value.HasValue)
            {
                LogType n = value.Value;
                for (int i = 0; i < logTypes.Length; i++)
                    if (n == logTypes[i])
                        return true;
                modelstate.AddModelError(key, Status.InvalidParameter);
            }
            else
                modelstate.AddModelError(key, Status.MissingParameter);
            return false;
        }


        //public static SqlCmd ValidateConnectString(this ModelStateDictionary modelstate, string key, UserID? id, string connectionString, bool allow_null = false)
        //{
        //    key = key.Trim(true);
        //    SqlCmd sqlcmd = null;
        //    if (string.IsNullOrEmpty(connectionString))
        //    {
        //        if (!allow_null)
        //            modelstate.AddModelError(key, Status.MissingParameter);
        //    }
        //    else
        //    {
        //        if (id.HasValue && !id.Value.IsRoot)
        //        {
        //            if ((string.Compare(connectionString, DB.Core01R, true) == 0) ||
        //                (string.Compare(connectionString, DB.Core01W, true) == 0))
        //                modelstate.AddModelError("User01R", Status.ParameterNotAllow);
        //        }
        //        try
        //        {
        //            //sqlcmd = new SqlCmd(null, connectionString);
        //            sqlcmd = _HttpContext.Current.GetSqlCmd(connectionString);
        //            sqlcmd.ExecuteNonQuery("select getdate()");
        //            return sqlcmd;
        //        }
        //        catch { }
        //        using (sqlcmd)
        //            modelstate.AddModelError(key, Status.InvalidParameter);
        //    }
        //    return null;
        //}

        public static void SetUserID(this SqlBuilder sqlBuilder, bool createTime = true, bool createUser = true, bool modifyTime = false, bool modifyUser = false)
        {
            if (createTime) sqlBuilder["", "CreateTime", ""] = SqlBuilder.str.getdate;
            if (modifyTime) sqlBuilder["", "ModifyTime", ""] = SqlBuilder.str.getdate;
            if (createUser) sqlBuilder["", "CreateUser", ""] = _User.Current.ID;
            if (modifyUser) sqlBuilder["", "ModifyUser", ""] = _User.Current.ID;
        }
        public static void SetCreateTime(this SqlBuilder s, string flag = null) { s[flag, "CreateTime", ""] = SqlBuilder.str.getdate; }
        public static void SetModifyTime(this SqlBuilder s, string flag = null) { s[flag, "ModifyTime", ""] = SqlBuilder.str.getdate; }
        public static void SetCreateUser(this SqlBuilder s, string flag = null) { s[flag, "CreateUser", ""] = _User.Current.ID; }
        public static void SetModifyUser(this SqlBuilder s, string flag = null) { s[flag, "ModifyUser", ""] = _User.Current.ID; }

        public static void SetCurrentTime(this SqlBuilder s, string flag = null, string name = null) { s[flag, name, ""] = SqlBuilder.str.getdate; }
        public static void SetCurrentUser(this SqlBuilder s, string flag = null, string name = null) { s[flag, name, ""] = _User.Current.ID; }
    }

    //public class ModelStateSqlBuilder : SqlBuilder
    //{
    //    ModelStateDictionary modelstate;
    //    public ModelStateSqlBuilder(ModelStateDictionary modelstate, object args)
    //    {
    //        if (args == null) throw new _Exception(Status.InvalidParameter);
    //        this.modelstate = modelstate;
    //    }



    //    public void ValidateModelState(bool throw_exception = true)
    //    {
    //        modelstate.IsValid(throw_exception);
    //    }



    //    public void AddValue(string flag, string field, object value, string format = null)
    //    {
    //        var n = base.SetValue(null, flag, field, format, null, value);
    //        if (n.require && (value == null))
    //            modelstate.AddModelError(n.field, Status.MissingParameter);
    //    }

    //    public void AddNumber<T>(string flag, string field, T? value, string format = null, Func<T, bool> min = null, Func<T, bool> max = null) where T : struct
    //    {
    //        var n = base.SetValue(null, flag, field, format, null, value);
    //        modelstate.Validate(n.field, value, min, max, !n.require);
    //    }

    //    public void AddEnum<T>(string flag, string field, T? value, string format = null) where T : struct
    //    {
    //        var n = base.SetValue(null, flag, field, format, null, value);
    //        modelstate.ValidateEnum(n.field, value, !n.require);
    //    }

    //    public void AddString(string flag, string field, string value, string format = null)
    //    {
    //        var n = base.SetValue(null, flag, field, format, null, value);
    //        modelstate.Validate(n.field, value, !n.require);
    //    }

    //    public void AddUserID(string flag, string field, UserID? value, UserType? userType = null)
    //    {
    //        var n = base.SetValue(null, flag, field, null, value);
    //        modelstate.ValidateUserID(n.field, value, userType, !n.require);
    //    }

    //    public void AddUserName(string flag, string field, UserName value)
    //    {
    //        var n = base.SetValue(null, flag, field, null, null, value);
    //        modelstate.Validate(n.field, value, !n.require);
    //    }

    //    public CorpInfo AddCorpID(string flag, string field, UserName value)
    //    {
    //        if (value.IsValid)
    //        {
    //            CorpInfo corp = CorpInfo.GetCorpInfo(value);
    //            if (corp == null)
    //                modelstate.AddModelError(field, Status.CorpNotExist);
    //            else
    //                this.AddUserID(flag, field, corp.ID, corp.ID.UserType);
    //            return corp;
    //        }
    //        else if (value.IsNullOrEmpty)
    //            modelstate.AddModelError(field, Status.MissingParameter);
    //        else
    //            modelstate.AddModelError(field, Status.InvalidParameter);
    //        return null;
    //    }
    //    public AgentData AddAgentID(string flag, string field, UserName value, CorpInfo corp)
    //    {
    //        if (corp == null) return null;
    //        if (value.IsValid)
    //        {
    //            AgentData agent = corp.GetAgentData(value);
    //            if (agent == null)
    //                modelstate.AddModelError(field, Status.AgentNotExist);
    //            else
    //                this.AddUserID(flag, field, agent.ID, agent.ID.UserType);
    //            return agent;
    //        }
    //        else if (value.IsNullOrEmpty)
    //            modelstate.AddModelError(field, Status.MissingParameter);
    //        else
    //            modelstate.AddModelError(field, Status.InvalidParameter);
    //        return null;
    //    }
    //}
}