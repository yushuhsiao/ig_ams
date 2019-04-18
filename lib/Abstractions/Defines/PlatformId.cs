using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace InnateGlory
{
    [_DebuggerStepThrough]
    [TypeConverter(typeof(_TypeConverter))]
    [JsonConverter(typeof(_JsonConverter))]
    //[StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct PlatformId //: IBaseType
    {
        public static readonly PlatformId Null = new PlatformId(0);
        public static readonly PlatformId Min = new PlatformId(0);
        public static readonly PlatformId Max = new PlatformId(0x7fffffff);
        //[FieldOffset(0)]
        public readonly Int32 Id;
        public PlatformId(Int32 id) { this.Id = id; }

        public bool IsNull => this.Id == Null.Id;
        public bool IsValid => this.Id.IsBetWeens(Min.Id, Max.Id);

        public static implicit operator PlatformId? (Int32? id)
        {
            if (id.HasValue) return new PlatformId(id.Value); return null;
        }
        public static implicit operator PlatformId(Int32 id) => new PlatformId(id);
        public static implicit operator Int32(PlatformId id) => id.Id;

        public static bool operator ==(PlatformId? src, object obj)
        {
            if (src.HasValue)
                return src.Value.Equals(obj);
            return object.ReferenceEquals(obj, null);
        }
        public static bool operator !=(PlatformId? src, object obj) => !(src == obj);
        public static bool operator ==(PlatformId src, object obj) => src.Equals(obj);
        public static bool operator !=(PlatformId src, object obj) => !(src == obj);

        public override bool Equals(object obj)
        {
            if (obj is PlatformId)
                return this.Id.Equals(((PlatformId)obj).Id);
            else
                return this.Id.Equals(obj);
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => "0x" + this.Id.ToString("x4");

        [_DebuggerStepThrough]
        class _TypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(Int32))
                    return true;
                if (sourceType == typeof(Int32?))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is Int32)
                    return (PlatformId)(Int32)value;
                if (value is Int32?)
                    return (PlatformId?)(Int32?)value;
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is PlatformId)
                    return ((PlatformId)value).Id;
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
                if (objectType == typeof(PlatformId))
                    return (PlatformId)serializer.Deserialize<Int32>(reader);
                if (objectType == typeof(PlatformId?))
                    return (PlatformId?)serializer.Deserialize<Int32?>(reader);
                else
                    return serializer.Deserialize(reader, objectType);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is PlatformId?)
                {
                    PlatformId? _value = (PlatformId?)value;
                    if (_value.HasValue)
                        value = _value.Value;
                    else
                        value = null;
                }
                if (value is PlatformId)
                    serializer.Serialize(writer, ((PlatformId)value).Id);
                else
                    serializer.Serialize(writer, value);
            }
        }
    }
}