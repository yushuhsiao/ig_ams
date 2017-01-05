using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    [TypeConverter(typeof(PlatformID._TypeConverter))]
    [JsonConverter(typeof(PlatformID._JsonConverter))]
    public struct PlatformID
    {
        public static readonly PlatformID Null = new PlatformID(0);
        public readonly Int32 ID;
        public PlatformID(Int32 id) { this.ID = id; }
        
        public bool IsNull
        {
            get { return this.ID == Null.ID; }
        }

        public static implicit operator PlatformID? (Int32? id)
        {
            if (id.HasValue) return new PlatformID(id.Value); return null;
        }
        public static implicit operator PlatformID(Int32 id)
        {
            return new PlatformID(id);
        }
        public static implicit operator Int32(PlatformID id)
        {
            return id.ID;
        }

        public static bool operator ==(PlatformID? src, object obj)
        {
            if (src.HasValue)
                return src.Value.Equals(obj);
            return object.ReferenceEquals(obj, null);
        }
        public static bool operator !=(PlatformID? src, object obj)
        {
            return !(src == obj);
        }
        public static bool operator ==(PlatformID src, object obj)
        {
            return src.Equals(obj);
        }
        public static bool operator !=(PlatformID src, object obj)
        {
            return !(src == obj);
        }

        public override bool Equals(object obj)
        {
            if (obj is PlatformID)
                return this.ID == ((PlatformID)obj).ID;
            else if (obj is Int32)
                return this.ID == (Int32)obj;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return this.ID.ToString();
        }

        [_DebuggerStepThrough]
        class _TypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(Int32))
                    return true;
                if (sourceType == typeof(Int32?))
                    return true;
                if (sourceType == typeof(string))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                    value = ((string)value).ToInt32();
                if (value is Int32)
                    return (PlatformID)(Int32)value;
                if (value is Int32?)
                    return (PlatformID?)(Int32?)value;
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is PlatformID)
                    return ((PlatformID)value).ID;
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
                if (objectType == typeof(PlatformID))
                    return (PlatformID)serializer.Deserialize<Int32>(reader);
                if (objectType == typeof(PlatformID?))
                    return (PlatformID?)serializer.Deserialize<Int32?>(reader);
                else
                    return serializer.Deserialize(reader, objectType);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is PlatformID?)
                {
                    PlatformID? _value = (PlatformID?)value;
                    if (_value.HasValue)
                        value = _value.Value;
                    else
                        value = null;
                }
                if (value is PlatformID)
                    serializer.Serialize(writer, ((PlatformID)value).ID);
                else
                    serializer.Serialize(writer, value);
            }
        }
    }
}