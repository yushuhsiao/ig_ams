using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace InnateGlory
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 有效範圍: 0x00001~0x7ffff
    /// 總公司 CorpId=1
    /// </remarks>
    /// <see cref="InnateGlory.UserId"/>
    [_DebuggerStepThrough]
    [TypeConverter(typeof(_TypeConverter))]
    [JsonConverter(typeof(_JsonConverter))]
    public struct CorpId //: IBaseType
    {
        internal const int bits = 19;
        public static readonly CorpId Null = new CorpId(0);
        public static readonly CorpId Root = new CorpId(1);
        public static readonly CorpId Max = new CorpId((2 << (bits - 1)) - 1);

        public readonly Int32 Id;

        public CorpId(Int32 id) { this.Id = id; }

        public bool IsRoot => this.Id == Root.Id;
        public bool IsNull => this.Id == Null.Id;
        public bool IsValid => this.Id.IsBetWeens(Root.Id, Max.Id);

        public static implicit operator CorpId? (Int32? id)
        {
            if (id.HasValue) return new CorpId(id.Value); return null;
        }
        public static implicit operator CorpId(Int32 id) => new CorpId(id);
        public static implicit operator Int32(CorpId id) => id.Id;

        public static bool operator ==(CorpId? src, object obj)
        {
            if (src.HasValue)
                return src.Value.Equals(obj);
            return object.ReferenceEquals(obj, null);
        }
        public static bool operator !=(CorpId? src, object obj) => !(src == obj);
        public static bool operator ==(CorpId src, object obj) => src.Equals(obj);
        public static bool operator !=(CorpId src, object obj) => !(src == obj);

        public override bool Equals(object obj)
        {
            if (obj is CorpId)
                return this.Id == ((CorpId)obj).Id;
            else if (obj is Int32)
                return this.Id == (Int32)obj;
            else
                return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => "0x" + this.Id.ToString("x5");
        
        public static bool TryParse(string s, out CorpId result)
        {
            if (s.StartsWith("0x"))
            {
                try
                {
                    int value = Convert.ToInt32(s, 16);
                    result = (CorpId)value;
                    return true;
                }
                catch { }
            }
            result = default(CorpId);
            return false;
        }

        private static _TypeConverter _converter = new _TypeConverter();

        [_DebuggerStepThrough]
        class _TypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return
                    sourceType == typeof(Int32) ||
                    sourceType == typeof(Int32?) ||
                    sourceType == typeof(string);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                Int32 _value;
                if (value is Int32)
                    return (CorpId)(Int32)value;
                if (value is Int32?)
                    return (CorpId?)(Int32?)value;
                if (value is string)
                {
                    string value_s = (string)value;
                    if (CorpId.TryParse(value_s, out CorpId corpId))
                        return corpId;
                    if (Int32.TryParse(value_s, out _value))
                        return (CorpId)_value;
                    else
                        return default(Int32?);
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return
                    destinationType == typeof(Int32) ||
                    destinationType == typeof(Int32?) ||
                    destinationType == typeof(string);
                //return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is CorpId)
                {
                    CorpId id = (CorpId)value;
                    if (destinationType == typeof(Int32))
                        return id.Id;
                    if (destinationType == typeof(Int32?))
                        return (Int32?)id.Id;
                    if (destinationType == typeof(string))
                        return id.Id.ToString();
                }
                else
                {
                    if (destinationType == typeof(Int64?))
                        return default(Int64?);
                    if (destinationType == typeof(string))
                        return default(string);
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        [_DebuggerStepThrough]
        class _JsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                object tmp = serializer.Deserialize(reader);
                if (tmp is string)
                {
                    return _converter.ConvertFrom(null, null, tmp);
                }
                else if (tmp is Int32 || tmp is Int16)
                {
                    if (objectType == typeof(CorpId))
                        return (CorpId)tmp;
                    if (objectType == typeof(CorpId?))
                        return (CorpId?)tmp;
                }
                else if (tmp is Int64)
                {
                    Int64 n = (Int64)tmp;
                    if (objectType == typeof(CorpId))
                        return (CorpId)(Int32)n;
                    if (objectType == typeof(CorpId?))
                        return (CorpId?)(Int32)n;
                }
                return tmp;

                //if (objectType == typeof(CorpId))
                //    return (CorpId)serializer.Deserialize<Int32>(reader);
                //if (objectType == typeof(CorpId?))
                //    return (CorpId?)serializer.Deserialize<Int32?>(reader);
                //else
                //    return serializer.Deserialize(reader, objectType);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is CorpId?)
                {
                    CorpId? _value = (CorpId?)value;
                    if (_value.HasValue)
                        value = _value.Value;
                    else
                        value = null;
                }
                if (value is CorpId)
                {
                    string s = ((CorpId)value).ToString();
                    serializer.Serialize(writer, s);
                    //serializer.Serialize(writer, ((CorpId)value).Id);
                }
                else
                    serializer.Serialize(writer, value);
            }
        }
    }
}