using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Globalization;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace ams
{
    [_DebuggerStepThrough]
    [TypeConverter(typeof(UserID._TypeConverter))]
    [JsonConverter(typeof(UserID._JsonConverter))]
    public struct UserID
    {
        public static readonly UserID Null = new UserID(0);
        public static readonly UserID guest = new UserID(0);
        public static readonly UserID root = new UserID(1);
        public static readonly UserID corp_min = new UserID(1);
        public static readonly UserID corp_max = new UserID(10000);
        //public static readonly UserID agent_min = new UserID(corp_max.ID + 1);
        //public static readonly UserID agent_max = new UserID(100000);
        //public static readonly UserID admin_min = new UserID(agent_max.ID + 1);
        //public static readonly UserID admin_max = new UserID(1000000);
        //public static readonly UserID member_min = new UserID(admin_max.ID + 1);
        //public static readonly UserID member_max = new UserID(int.MaxValue);
        //             0 : guest
        //             1 : corps
        //         1,001 : agents
        //       100,001 : admins
        //     1,000,001 : member
        // 2,147,483,647 : member max

        public readonly Int32 ID;

        //public UserType UserType
        //{
        //    get
        //    {
        //        if (this.ID == guest.ID) return UserType.Guest;
        //        if (this.ID < corp_max.ID) return UserType.Corp;
        //        if (this.ID < agent_max.ID) return UserType.Agent;
        //        if (this.ID < admin_max.ID) return UserType.Admin;
        //        return UserType.Member;
        //    }
        //}

        public UserID(Int32 id) { this.ID = id; }

        public bool IsRoot
        {
            get { return this.ID == root.ID; }
        }

        public bool IsGuest
        {
            get { return this.ID == guest.ID; }
        }

        public bool IsNull
        {
            get { return this.ID == Null.ID; }
        }

        public static implicit operator UserID? (Int32? id)
        {
            if (id.HasValue) return new UserID(id.Value); return null;
        }
        public static implicit operator UserID(Int32 id)
        {
            return new UserID(id);
        }
        public static implicit operator Int32(UserID id)
        {
            return id.ID;
        }

        public static bool operator ==(UserID? src, object obj)
        {
            if (src.HasValue)
                return src.Value.Equals(obj);
            return object.ReferenceEquals(obj, null);
        }
        public static bool operator !=(UserID? src, object obj)
        {
            return !(src == obj);
        }
        public static bool operator ==(UserID src, object obj)
        {
            return src.Equals(obj);
        }
        public static bool operator !=(UserID src, object obj)
        {
            return !(src == obj);
        }

        public override bool Equals(object obj)
        {
            if (obj is UserID)
                return this.ID == ((UserID)obj).ID;
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
                return base.CanConvertFrom(context, sourceType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is Int32)
                    return (UserID)(Int32)value;
                if (value is Int32?)
                    return (UserID?)(Int32?)value;
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is UserID)
                    return ((UserID)value).ID;
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
                if (objectType == typeof(UserID))
                    return (UserID)serializer.Deserialize<Int32>(reader);
                if (objectType == typeof(UserID?))
                    return (UserID?)serializer.Deserialize<Int32?>(reader);
                else
                    return serializer.Deserialize(reader, objectType);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is UserID?)
                {
                    UserID? _value = (UserID?)value;
                    if (_value.HasValue)
                        value = _value.Value;
                    else
                        value = null;
                }
                if (value is UserID)
                    serializer.Serialize(writer, ((UserID)value).ID);
                else
                    serializer.Serialize(writer, value);
            }
        }
    }
}