using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace InnateGlory
{
    [_DebuggerStepThrough]
    [TypeConverter(typeof(_TypeConverter))]
    [JsonConverter(typeof(_JsonConverter))]
    //[StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct GameId //: IBaseType
    {
        public static readonly GameId Null = new GameId(0);
        public static readonly GameId Min = new GameId(0);
        public static readonly GameId Max = new GameId(0x7fffffff);
        //[FieldOffset(0)]
        public Int32 Id;
        //[FieldOffset(2)]
        //public PlatformId PlatformId;

        public GameId(Int32 id)
        {
            //this.PlatformId = 0;
            this.Id = id;
        }

        public bool IsNull => this.Id == Null.Id;
        //public bool IsValid => PlatformId.IsValid;

        public static implicit operator GameId? (Int32? id)
        {
            if (id.HasValue) return new GameId(id.Value); return null;
        }
        public static implicit operator GameId(Int32 id) => new GameId(id);
        public static implicit operator Int32(GameId id) => id.Id;

        public static bool operator ==(GameId? src, object obj)
        {
            if (src.HasValue)
                return src.Value.Equals(obj);
            return object.ReferenceEquals(obj, null);
        }
        public static bool operator !=(GameId? src, object obj) => !(src == obj);
        public static bool operator ==(GameId src, object obj) => src.Equals(obj);
        public static bool operator !=(GameId src, object obj) => !(src == obj);

        public override bool Equals(object obj)
        {
            if (obj is GameId)
                return this.Id == ((GameId)obj).Id;
            else if (obj is Int32)
                return this.Id == (Int32)obj;
            else
                return false;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => "0x" + this.Id.ToString("x8");

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
                    return (GameId)(Int32)value;
                if (value is Int32?)
                    return (GameId?)(Int32?)value;
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is GameId)
                    return ((GameId)value).Id;
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
                if (objectType == typeof(GameId))
                    return (GameId)serializer.Deserialize<Int32>(reader);
                if (objectType == typeof(GameId?))
                    return (GameId?)serializer.Deserialize<Int32?>(reader);
                else
                    return serializer.Deserialize(reader, objectType);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is GameId?)
                {
                    GameId? _value = (GameId?)value;
                    if (_value.HasValue)
                        value = _value.Value;
                    else
                        value = null;
                }
                if (value is GameId)
                    serializer.Serialize(writer, ((GameId)value).Id);
                else
                    serializer.Serialize(writer, value);
            }
        }
    }
}