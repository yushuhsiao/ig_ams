using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    [TypeConverter(typeof(CorpID._TypeConverter))]
    [JsonConverter(typeof(CorpID._JsonConverter))]
    public struct CorpID
    {
        public static readonly CorpID guest = new CorpID(0);
        public static readonly CorpID root = new CorpID(1);

        public readonly Int16 ID;

        public CorpID(Int16 id) { this.ID = id; }

        public bool IsRoot
        {
            get { return this.ID == root.ID; }
        }

        public bool IsGuest
        {
            get { return this.ID == guest.ID; }
        }

        public static implicit operator CorpID? (Int16? id)
        {
            if (id.HasValue) return new CorpID(id.Value); return null;
        }
        public static implicit operator CorpID(Int16 id)
        {
            return new CorpID(id);
        }
        public static implicit operator Int16(CorpID id)
        {
            return id.ID;
        }

        public static bool operator ==(CorpID? src, object obj)
        {
            if (src.HasValue)
                return src.Value.Equals(obj);
            return object.ReferenceEquals(obj, null);
        }
        public static bool operator !=(CorpID? src, object obj)
        {
            return !(src == obj);
        }
        public static bool operator ==(CorpID src, object obj)
        {
            return src.Equals(obj);
        }
        public static bool operator !=(CorpID src, object obj)
        {
            return !(src == obj);
        }

        public override bool Equals(object obj)
        {
            if (obj is CorpID)
                return this.ID == ((CorpID)obj).ID;
            else if (obj is Int16)
                return this.ID == (Int16)obj;
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

        class _TypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(Int16))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is Int16)
                    return (CorpID)((Int16)value);
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is CorpID)
                    return ((CorpID)value).ID;
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        class _JsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (objectType == typeof(CorpID))
                    return (CorpID)serializer.Deserialize<Int16>(reader);
                else
                    return serializer.Deserialize(reader, objectType);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is CorpID)
                    serializer.Serialize(writer, ((CorpID)value).ID);
                else
                    serializer.Serialize(writer, value);
            }
        }
    }
}
