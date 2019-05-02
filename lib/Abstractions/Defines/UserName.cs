using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace InnateGlory
{
    [_DebuggerStepThrough, DebuggerDisplay("{Value}")]
    [TypeConverter(typeof(_TypeConverter))]
    [JsonConverter(typeof(_JsonConverter))]
    //[System.ComponentModel.DataAnnotations.CustomValidation()]
    public struct UserName : System.StringFormatWith.StringValue
    //, System.ComponentModel.DataAnnotations.IValidatableObject
    {
        public static readonly UserName root = "root";
        public static readonly UserName Empty = "";
        public const string PATTERN = "0123456789abcdefghijklmnopqrstuvwxyz_";
        public const int MAX_LENGTH = 20;
        public readonly string OrginalValue;
        public readonly string Value;

        public UserName(string value)
        {
            this.OrginalValue = value;
            if (value == null)
                this.Value = value;
            else
            {
                StringBuilder s = new StringBuilder();
                for (int i = 0; (i < value.Length) && (s.Length < UserName.MAX_LENGTH); i++)
                {
                    char c = char.ToLower(value[i]);
                    if (PATTERN.Contains(c))
                        s.Append(c);
                }
                this.Value = s.ToString();
            }
        }

        public bool HasValue => !string.IsNullOrEmpty(this.Value);
        public bool IsNullOrEmpty => string.IsNullOrEmpty(this.Value);

        //public bool IsOrginalValue => this.Value.IsEquals(this.OrginalValue, true);

        public bool IsValid => this.HasValue && this.Value.IsEquals(this.OrginalValue, true);

        //public UserName() { }
        //public UserName(string value)
        //{
        //    this.Value = value;
        //}
        public static implicit operator UserName(string n) => new UserName(n);
        public static implicit operator string(UserName n) => n.Value;

        public static bool operator ==(UserName src, object obj) => src.Equals(obj);
        public static bool operator !=(UserName src, object obj) => !src.Equals(obj);

        //public static bool operator ==(UserName a, UserName b)
        //{
        //    bool aa = a.value == null;
        //    bool bb = b.value == null;
        //    if (aa && bb)
        //        return true;
        //    else if (aa || bb)
        //        return false;
        //    return string.Compare(a.value, b.value, true) == 0;
        //}
        //public static bool operator !=(UserName a, UserName b)
        //{
        //    return !(a == b);
        //}

        public override bool Equals(object obj)
        {
            if (obj is UserName)
                return this.Value.Equals(((UserName)obj).Value);
            //    return string.Compare(this.Value, ((UserName)obj).Value, true) == 0;
            //else if (obj is string)
            //    return string.Compare(this.Value, (string)obj, true) == 0;
            else
                return this.Value.Equals(obj);
        }

        public override int GetHashCode() => base.GetHashCode();

        public override string ToString() => this.Value;

        //IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        //{
        //    throw new NotImplementedException();
        //}

        class _TypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }
            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string)
                    return new UserName((string)value);
                return base.ConvertFrom(context, culture, value);
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                return base.CanConvertTo(context, destinationType);
            }
            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (value is UserName)
                    return ((UserName)value).Value;
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        class _JsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(UserName);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                if (objectType == typeof(UserName) || objectType == typeof(UserName?))
                {
                    string s = (string)serializer.Deserialize(reader, typeof(string));
                    if (objectType == typeof(UserName))
                        return (UserName)s;
                    if (string.IsNullOrEmpty(s))
                        return null;
                    return (UserName?)s;
                }
                else
                    return serializer.Deserialize(reader, objectType);
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is UserName)
                    serializer.Serialize(writer, ((UserName)value).Value);
                else
                    serializer.Serialize(writer, value);
            }
        }
    }
    //    [FakeDebuggerStepThrough]
    //public static class _UserNameExtensions
    //{
    //    public static UserName IsNullOrEmpty(this UserName username) { return username.IsNullOrEmpty ? UserName.Null : username; }
    //}
}