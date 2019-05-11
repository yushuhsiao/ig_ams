using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;


namespace InnateGlory.Api
{
    public static class ModelStateExtension
    {
        public static void IsValid(this ModelStateDictionary modelState)
        {
            if (false == modelState.IsValid)
            {
                throw new ApiException(Status.ModelStateError)
                {
                    HttpStatusCode = System.Net.HttpStatusCode.BadRequest,
                    ModelError = new SerializableError(modelState)
                };
            }
            //new Microsoft.AspNetCore.Mvc.SerializableError()
        }

        private static bool GetValue<T>(object model, string key, out T value)
        {
            value = default(T);
            if (model == null)
                return false;
            if (model.GetType().GetFieldOrProperty(key, out var m))
            {
                object _value = m.GetValue(model);
                return _value.TryCast(out value);
            }
            return false;
        }

        public static ModelStateDictionary Valid(this ModelStateDictionary modelState, object model, string key, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
        {
            Type modelType = model?.GetType();
            if (modelType == null) return modelState;
            if (modelType.GetFieldOrProperty(key, out var m))
            {
                object value = m.GetValue(model);
                Type valueType = m.GetValueType();

                if (valueType.Is<UserId>(true))
                    return modelState.Valid(model, key, value as UserId?, required, message);

                if (valueType.Is<CorpId>(true))
                    return modelState.Valid(model, key, value as CorpId?, required, message);

                if (valueType.Is<UserName>())
                    return modelState.Valid(model, key, (UserName)value, required, statusCode, message);

                if (valueType.Is<string>())
                    return modelState.Valid(model, key, (string)value, required, statusCode, message);

                if (valueType.Is<Guid>(true))
                    return modelState.Valid(model, key, (Guid?)value, required, statusCode, message);

                if (valueType.Is<bool>(true))
                    return modelState.Valid(model, key, (bool?)value, required, statusCode, message);
            }
            else if (required)
            {
                modelState.TryAddModelError(key, message ?? Status.RequiredParameter.ToString());
            }

            return modelState;
        }


        public static ModelStateDictionary Valid(this ModelStateDictionary modelState, object model, string key, string value, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
        {
            if (string.IsNullOrEmpty(value) && required)
                modelState.TryAddModelError(key, message);
            return modelState;
        }

        public static ModelStateDictionary Valid(this ModelStateDictionary modelState, object model, string key, bool? value, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
        {
            if (false == value.HasValue && required)
                modelState.TryAddModelError(key, message);
            return modelState;
        }

        public static ModelStateDictionary Valid(this ModelStateDictionary modelState, object model, string key, Guid? value, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
        {
            if (false == value.HasValue && required)
                modelState.TryAddModelError(key, message);
            return modelState;
        }

        public static ModelStateDictionary Valid(this ModelStateDictionary modelState, object model, string key, UserName value, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
        {
            if (value.IsNullOrEmpty && required)
                modelState.TryAddModelError(key, message ?? "");
            else if (!value.IsValid)
                modelState.TryAddModelError(key, message ?? "");
            return modelState;
        }

        public static ModelStateDictionary Valid(this ModelStateDictionary modelState, object model, string key, CorpId? value, bool required = true, string message = null)
        {
            if (value.HasValue)
            {
                if (!value.Value.IsValid)
                    modelState.TryAddModelError(key, message);
            }
            else if (required)
            {
                modelState.TryAddModelError(key, message);
            }
            return modelState;
        }

        public static ModelStateDictionary Valid(this ModelStateDictionary modelState, object model, string key, UserId? value, bool required = true, string message = null)
        {
            if (value.HasValue)
            {
                if (!value.Value.IsValid)
                    modelState.TryAddModelError(key, message);
            }
            else if (required)
            {
                modelState.TryAddModelError(key, message);
            }
            return modelState;
        }

        public static ModelStateDictionary Valid(this ModelStateDictionary modelState, object model, string key, UserType? value, bool required = true, string message = null)
        {
            if (value.HasValue)
            {
                if (!Enum<UserType>.IsDefined(value))
                    modelState.TryAddModelError(key, message);
            }
            else if (required)
            {
                modelState.TryAddModelError(key, message);
            }
            return modelState;
        }

        public static ModelStateDictionary ValidCorp(this ModelStateDictionary modelState, object model, string key_CorpId, CorpId? id, string key_CorpName, UserName name, bool required = true)
        {
            if (id.HasValue || name.IsValid)
                return modelState;
            modelState.Valid(model, key_CorpId, id);
            modelState.Valid(model, key_CorpName, name);
            return modelState;
        }

        public static ModelStateDictionary ValidCorp(this ModelStateDictionary modelState, object model, string key_CorpId, string key_CorpName, bool required = true)
        {
            GetValue(model, key_CorpId, out CorpId? id);
            GetValue(model, key_CorpName, out UserName name);
            return modelState.ValidCorp(model, key_CorpId, id, key_CorpName, name, required);
        }

        public static ModelStateDictionary ValidIdOrName(this ModelStateDictionary modelState, object model, string key_UserId, string key_CorpId, string key_CorpName, string key_UserName)
        {
            var v1 = GetValue(model, key_UserId, out UserId? userId) && userId.HasValue && userId.Value.IsValid;
            var v2 = GetValue(model, key_CorpId, out CorpId? corpId) && corpId.HasValue && corpId.Value.IsValid;
            var v3 = GetValue(model, key_CorpName, out UserName corpName) && corpName.IsValid;
            var v4 = GetValue(model, key_UserName, out UserName userName) && userName.IsValid;

            if (v1)
                return modelState;
            if (v2 && v4)
                return modelState;
            if (v3 && v4)
                return modelState;

            if (!v1) modelState.TryAddModelError(key_UserId, Status.InvalidParameter.ToString());
            if (!v2) modelState.TryAddModelError(key_CorpId, Status.InvalidParameter.ToString());
            if (!v3) modelState.TryAddModelError(key_CorpName, Status.InvalidParameter.ToString());
            if (!v4) modelState.TryAddModelError(key_UserName, Status.InvalidParameter.ToString());
            //Valid(key_UserId, userId, required: false);
            //int n1 = errors.Count;
            //if (n0 == n1)
            //{
            //    ValidCorp(key_CorpId, key_CorpName);
            //    GetValue(key_UserName, out UserName userName);
            //    Valid(key_UserName, userName);
            //}
            return modelState;
        }

        public static ModelStateDictionary ValidParent(this ModelStateDictionary modelState, object model, string key_AgentId, string key_AgentName, bool required = false)
        {
            GetValue(model, key_AgentId, out UserId? id);
            GetValue(model, key_AgentName, out UserName name);
            if (id.HasValue || name.IsValid)
                return modelState;
            if (required)
            {
                modelState.Valid(model, key_AgentId, id);
                modelState.Valid(model, key_AgentName, name);
            }
            return modelState;
        }
    }

    //[_DebuggerStepThrough]
    //[JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    //public class ApiModelValidator : ApiException, IEnumerable
    //{
    //    IEnumerator IEnumerable.GetEnumerator() => null;

    //    private object _model;

    //    private Type _modelType;

    //    public ApiModelValidator(object model = null) : base(Status.Unknown, null)
    //    {
    //        _model = model;
    //        _modelType = _model?.GetType();
    //    }

    //    public override IDictionary<string, ApiErrorEntry> Errors => errors;

    //    private Dictionary<string, ApiErrorEntry> errors = new Dictionary<string, ApiErrorEntry>();

    //    public ApiModelValidator SetStatus(Status statusCode = Status.InvalidParameter, string message = null)
    //    {
    //        this.StatusCode = statusCode;
    //        this.ResultMessage = message;
    //        return this;
    //    }

    //    public bool IsValid(bool throwException = true, Status statusCode = Status.InvalidParameter, string message = null)
    //    {
    //        if (throwException)
    //        {
    //            this.Validate();
    //            return true;
    //        }
    //        return this.errors.Count > 0; ;
    //    }

    //    public ApiModelValidator Validate(Status statusCode = Status.InvalidParameter, string message = null)
    //    {
    //        bool hasError = this.errors.Count > 0;
    //        if (hasError)
    //            throw this.SetStatus(statusCode, message);
    //        return this;
    //    }

    //    public ApiModelValidator AddError(string key, Status statusCode = Status.InvalidParameter, string message = null)
    //    {
    //        errors[key] = new ApiErrorEntry()
    //        {
    //            StatusCode = statusCode,
    //            Message = message ?? statusCode.ToString()
    //        };
    //        return this;
    //    }

    //    private bool GetValue<T>(string key, out T value)
    //    {
    //        value = default(T);
    //        if (_modelType == null)
    //            return false;
    //        if (_modelType.GetFieldOrProperty(key, out var m))
    //        {
    //            object _value = m.GetValue(_model);
    //            return _value.TryCast(out value);
    //        }
    //        return false;
    //    }

    //    /// <remarks> <see cref="_model"/> != null </remarks>
    //    public ApiModelValidator Valid(string key, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
    //    {
    //        if (_modelType == null) return this;
    //        if (_modelType.GetFieldOrProperty(key, out var m))
    //        {
    //            object value = m.GetValue(_model);
    //            Type valueType = m.GetValueType();

    //            if (valueType.Is<UserId>(true))
    //                return this.Valid(key, value as UserId?, required, message);

    //            if (valueType.Is<CorpId>(true))
    //                return this.Valid(key, value as CorpId?, required, message);

    //            if (valueType.Is<UserName>())
    //                return this.Valid(key, (UserName)value, required, message);

    //            if (valueType.Is<string>())
    //                return this.Valid(key, (string)value, required, statusCode, message);

    //            if (valueType.Is<Guid>(true))
    //                return this.Valid(key, (Guid?)value, required, statusCode, message);

    //            if (valueType.Is<bool>(true))
    //                return this.Valid(key, (bool?)value, required, statusCode, message);

    //            //if (valueType == typeof(CorpId))
    //            //if (value == null)
    //            //{
    //            //    if (required)
    //            //        AddError(key, Status.RequiredParameter);
    //            //}
    //            //else
    //            //{
    //            //}
    //        }
    //        else if (required)
    //        {
    //            AddError(key, Status.RequiredParameter);
    //        }
    //        return this;
    //    }

    //    public ApiModelValidator Valid(string key, string value, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
    //    {
    //        if (string.IsNullOrEmpty(value) && required)
    //            AddError(key, statusCode, message);
    //        return this;
    //    }

    //    public ApiModelValidator Valid(string key, bool? value, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
    //    {
    //        if (false == value.HasValue && required)
    //            AddError(key, statusCode, message);
    //        return this;
    //    }

    //    public ApiModelValidator Valid(string key, Guid? value, bool required = true, Status statusCode = Status.InvalidParameter, string message = null)
    //    {
    //        if (false == value.HasValue && required)
    //            AddError(key, statusCode, message);
    //        return this;
    //    }

    //    public ApiModelValidator Valid(string key, UserName value, bool required = true, string message = null)
    //    {
    //        if (value.IsNullOrEmpty && required)
    //            AddError(key, Status.RequiredParameter, message);
    //        else if (!value.IsValid)
    //            AddError(key, Status.InvalidParameter, message);
    //        return this;
    //    }

    //    public ApiModelValidator Valid(string key, CorpId? value, bool required = true, string message = null)
    //    {
    //        if (value.HasValue)
    //        {
    //            if (!value.Value.IsValid)
    //                AddError(key, Status.InvalidParameter, message);
    //        }
    //        else if (required)
    //        {
    //            AddError(key, Status.RequiredParameter, message);
    //        }
    //        return this;
    //    }

    //    public ApiModelValidator Valid(string key, UserId? value, bool required = true, string message = null)
    //    {
    //        if (value.HasValue)
    //        {
    //            if (!value.Value.IsValid)
    //                AddError(key, Status.InvalidParameter, message);
    //        }
    //        else if (required)
    //        {
    //            AddError(key, Status.RequiredParameter, message);
    //        }
    //        return this;
    //    }

    //    public ApiModelValidator Valid(string key, UserType? value, bool required = true, string message = null)
    //    {
    //        if (value.HasValue)
    //        {
    //            if (!Enum<UserType>.IsDefined(value))
    //                AddError(key, Status.InvalidParameter);
    //        }
    //        else if (required)
    //        {
    //            AddError(key, Status.RequiredParameter, message);
    //        }
    //        return this;
    //    }

    //    public ApiModelValidator ValidCorp(string key_CorpId, CorpId? id, string key_CorpName, UserName name, bool required = true)
    //    {
    //        if (id.HasValue || name.IsValid)
    //            return this;
    //        this.Valid(key_CorpId, id);
    //        this.Valid(key_CorpName, name);
    //        return this;
    //    }

    //    public ApiModelValidator ValidCorp(string key_CorpId, string key_CorpName, bool required = true)
    //    {
    //        // todo : 如果目前使用者不具有跨站台存取的權限, 這個值將會被取代
    //        GetValue(key_CorpId, out CorpId? id);
    //        GetValue(key_CorpName, out UserName name);
    //        return ValidCorp(key_CorpId, id, key_CorpName, name, required);
    //    }

    //    /// <summary>
    //    /// Validate UserId or CorpId+UserName or CorpName+UserName
    //    /// </summary>
    //    public ApiModelValidator ValidIdOrName(string key_UserId, string key_CorpId, string key_CorpName, string key_UserName)
    //    {
    //        var v1 = GetValue(key_UserId, out UserId? userId) && userId.HasValue && userId.Value.IsValid;
    //        var v2 = GetValue(key_CorpId, out CorpId? corpId) && corpId.HasValue && corpId.Value.IsValid;
    //        var v3 = GetValue(key_CorpName, out UserName corpName) && corpName.IsValid;
    //        var v4 = GetValue(key_UserName, out UserName userName) && userName.IsValid;

    //        if (v1)
    //            return this;
    //        if (v2 && v4)
    //            return this;
    //        if (v3 && v4)
    //            return this;

    //        if (!v1) AddError(key_UserId, Status.InvalidParameter);
    //        if (!v2) AddError(key_CorpId, Status.InvalidParameter);
    //        if (!v3) AddError(key_CorpName, Status.InvalidParameter);
    //        if (!v4) AddError(key_UserName, Status.InvalidParameter);
    //        //Valid(key_UserId, userId, required: false);
    //        //int n1 = errors.Count;
    //        //if (n0 == n1)
    //        //{
    //        //    ValidCorp(key_CorpId, key_CorpName);
    //        //    GetValue(key_UserName, out UserName userName);
    //        //    Valid(key_UserName, userName);
    //        //}
    //        return this;
    //    }

    //    public ApiModelValidator ValidParent(string key_AgentId, string key_AgentName, bool required = false)
    //    {
    //        GetValue(key_AgentId, out UserId? id);
    //        GetValue(key_AgentName, out UserName name);
    //        if (id.HasValue || name.IsValid)
    //            return this;
    //        if (required)
    //        {
    //            this.Valid(key_AgentId, id);
    //            this.Valid(key_AgentName, name);
    //        }
    //        return this;
    //    }



    //    //public bool Valid(string key, System.Data.DbConnectionString c, string message = null)
    //    //{
    //    //    try
    //    //    {
    //    //        if (c.IsEmpty)
    //    //        {
    //    //            AddError(key, Status.MissingParameter);
    //    //        }
    //    //        else
    //    //        {
    //    //            using (SqlCmd sqlcmd = new SqlCmd(c))
    //    //            {
    //    //                DateTime? ct = sqlcmd.ExecuteScalar("select getdate()") as DateTime?;
    //    //                if (ct.HasValue)
    //    //                    return true;
    //    //                AddError(key, Status.InvalidParameter);
    //    //            }
    //    //        }
    //    //    }
    //    //    catch
    //    //    {
    //    //        AddError(key, Status.InvalidParameter);
    //    //    }
    //    //    return false;
    //    //}
    //}
}