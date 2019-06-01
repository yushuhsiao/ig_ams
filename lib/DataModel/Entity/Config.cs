using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Threading;

namespace InnateGlory.Entity
{
    [DebuggerDisplay("{json}")]
    [TableName("Config", Database = _Consts.db.CoreDB)]
    public class Config
    {
        public long Id { get; set; }
        public CorpId CorpId { get; set; }
        public PlatformId PlatformId { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        //private string json => Newtonsoft.Json.JsonConvert.SerializeObject(this);

        //private object[] _values;
        //public bool GetValueAs<TValue>(out TValue result)
        //{
        //    object[] values = Interlocked.CompareExchange(ref this._values, null, null);
        //    int len = values?.Length ?? 0;
        //    for (int i = 0; i < len; i++)
        //    {
        //        object value = values[i];
        //        if (value == null)
        //            continue;
        //        else if (value is TValue)
        //        {
        //            result = (TValue)value;
        //            return true;
        //        }
        //    }

        //    if (_Convert.ConvertTo(this.Value, out result))
        //    {
        //        Interlocked.Exchange(ref this._values, (values ?? new object[0]).Add(result));
        //        return true;
        //    }
        //    return _null.noop(false, out result);
        //}
    }
}
namespace InnateGlory.Models
{
    [TableName(typeof(Entity.Config))]
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public struct ConfigModel
    {
        [JsonProperty]
        public long? Id;
        [JsonProperty]
        public CorpId? CorpId;
        [JsonProperty]
        public PlatformId? PlatformId;
        [JsonProperty]
        public string Key1;
        [JsonProperty]
        public string Key2;
        [JsonProperty]
        public string Value;
        [JsonProperty]
        public string Description;
    }
}
