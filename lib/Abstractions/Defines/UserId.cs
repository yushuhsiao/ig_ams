using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace InnateGlory
{
    public interface IUser
    {
        UserId Id { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 編碼規則(二進位):
    ///     0ccccccc cccccccc ccccuuuu uuuuuuuu uuuuuuuu uuuuuuuu uuuuuuuu uuuuuuuu
    ///     0 : 最高 bit 固定為 0, 確保數值為正數
    ///     c : 19 bits, CorpId
    ///     u : 44 bit, 實際 UserId (不同 UserDB 可能會重複, 搭配 CorpId 避免發生重複)
    ///     
    /// 特殊情況:
    ///     CorpRoot 的 UserId=CorpId
    ///     Root 的 UserId 和 CorpId 皆為 1
    /// </remarks>
    /// <see cref="InnateGlory.CorpId"/>
    [_DebuggerStepThrough]
    [TypeConverter(typeof(_TypeConverter))]
    [JsonConverter(typeof(_JsonConverter))]
    public struct UserId //: IBaseType //: IValidatableObject
    {
        private const int bits = 44;
        public static readonly UserId Null = new UserId(CorpId.Null.Id);
        public static readonly UserId Guest = new UserId(0);
        public static readonly UserId System = new UserId(0);
        public static readonly UserId Root = new UserId(CorpId.Root.Id);
        public readonly Int64 Id;

        public UserId(Int64 id) { this.Id = id; }

        /// <summary>
        /// bit63 ~ bit44 的值 (unsigned int 19)
        /// </summary>
        public CorpId CorpId
        {
            get
            {
                if (this.Id.IsBetWeens(CorpId.Root.Id, CorpId.Max.Id))
                    return (CorpId)this.Id;
                return (CorpId)global::System.Bitmask.GetBits(this.Id, bits, CorpId.bits);
            }
            //set => this.Id = global::System.Bitmask.SetBits(this.Id, bits, CorpId.bits, value);
        }

        public UserId SetCorpId(CorpId value) => new UserId(global::System.Bitmask.SetBits(this.Id, bits, CorpId.bits, value));

        /// <summary>
        /// bit47 ~ bit0 的值 (unsigned int 44)
        /// </summary>
        public long PartId => Id.GetBits(0, bits);

        /// <summary>
        /// 是否為 Root (Id == 1)
        /// </summary>
        public bool IsRoot => this.Id == Root.Id;

        /// <summary>
        /// 是否為 CorpRoot (Corp 之下的頂級帳號)
        /// </summary>
        public bool IsCorpRoot => this.Id <= CorpId.Max.Id;
        public bool IsGuest => this.Id == Guest.Id;
        public bool IsNull => this.Id == Null.Id;
        public bool IsValid => this.Id > 0;

        public static implicit operator UserId? (Int64? id)
        {
            if (id.HasValue) return new UserId(id.Value); return null;
        }
        public static implicit operator UserId(Int64 id) => new UserId(id);
        public static implicit operator Int64(UserId id)
        {
            return id.Id;
        }
        public static explicit operator UserId(CorpId corpId) => new UserId((Int64)corpId.Id);
        public static explicit operator UserId? (CorpId? corpId)
        {
            if (corpId.HasValue)
                return new UserId((Int64)corpId.Value.Id);
            else
                return null;
        }

        public static bool operator ==(UserId? src, object obj)
        {
            if (src.HasValue)
                return src.Value.Equals(obj);
            return object.ReferenceEquals(obj, null);
        }
        public static bool operator !=(UserId? src, object obj) => !(src == obj);
        public static bool operator ==(UserId src, object obj) => src.Equals(obj);
        public static bool operator !=(UserId src, object obj) => !(src == obj);

        public override bool Equals(object obj)
        {
            if (obj is UserId)
                return this.Id == ((UserId)obj).Id;
            else if (obj is Int64)
                return this.Id == (Int64)obj;
            else
                return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => "0x" + this.Id.ToString("x16");

        //IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{
        //    yield return ValidationResult.Success;
        //}

        [_DebuggerStepThrough]
        class _TypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                var r = 
                    sourceType == typeof(Int64) ||
                    sourceType == typeof(Int64?) ||
                    sourceType == typeof(string);
                return r;
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                Int64 _value;
                if (value is Int64)
                    return (UserId)(Int64)value;
                if (value is Int64?)
                    return (UserId?)(Int64?)value;
                if (value is string)
                {
                    string value_s = (string)value;
                    if (value_s.StartsWith("0x"))
                    {
                        try
                        {
                            value = Convert.ToInt64(value_s, 16);
                            return (UserId)(Int64)value;
                        }
                        catch { }
                    }
                    if (((string)value).ToInt64(out _value))
                        return (UserId)_value;
                    else
                        return default(Int64?);
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                var r = 
                    destinationType == typeof(Int64) ||
                    destinationType == typeof(Int64?) ||
                    destinationType == typeof(string);
                return r;
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is UserId)
                {
                    UserId id = (UserId)value;
                    if (destinationType == typeof(Int64))
                        return id.Id;
                    if (destinationType == typeof(Int64?))
                        return (Int64?)id.Id;
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

            static _TypeConverter converter = new _TypeConverter();

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                object tmp = serializer.Deserialize(reader);
                if (tmp is string)
                {
                    return converter.ConvertFrom(null, null, tmp);
                }
                else if (tmp is Int64 || tmp is Int32)
                {
                    if (objectType == typeof(UserId))
                        return (UserId)tmp;
                }
                else if (tmp is Int64? || tmp is Int32?)
                {
                    if (objectType == typeof(UserId?))
                        return (UserId?)tmp;
                }
                return tmp;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is UserId?)
                {
                    UserId? _value = (UserId?)value;
                    if (_value.HasValue)
                        value = _value.Value;
                    else
                        value = null;
                }
                if (value is UserId)
                {
                    string s = ((UserId)value).ToString();
                    serializer.Serialize(writer, s);
                    //serializer.Serialize(writer, ((UserId)value).Id);
                }
                else
                    serializer.Serialize(writer, value);
            }
        }
    }
}