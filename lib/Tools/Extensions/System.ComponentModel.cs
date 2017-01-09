using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.ComponentModel
{
    [_DebuggerStepThrough]
    public static class Extensions
    {
        static Extensions()
        {
            TypeDescriptor.AddAttributes(typeof(Byte[]), new TypeConverterAttribute(typeof(ArrayConverter<Byte>)));
            TypeDescriptor.AddAttributes(typeof(SByte[]), new TypeConverterAttribute(typeof(ArrayConverter<SByte>)));
            TypeDescriptor.AddAttributes(typeof(Int16[]), new TypeConverterAttribute(typeof(ArrayConverter<Int16>)));
            TypeDescriptor.AddAttributes(typeof(Int32[]), new TypeConverterAttribute(typeof(ArrayConverter<Int32>)));
            TypeDescriptor.AddAttributes(typeof(Int64[]), new TypeConverterAttribute(typeof(ArrayConverter<Int64>)));
            TypeDescriptor.AddAttributes(typeof(UInt16[]), new TypeConverterAttribute(typeof(ArrayConverter<UInt16>)));
            TypeDescriptor.AddAttributes(typeof(UInt32[]), new TypeConverterAttribute(typeof(ArrayConverter<UInt32>)));
            TypeDescriptor.AddAttributes(typeof(UInt64[]), new TypeConverterAttribute(typeof(ArrayConverter<UInt64>)));
            TypeDescriptor.AddAttributes(typeof(Single[]), new TypeConverterAttribute(typeof(ArrayConverter<Single>)));
            TypeDescriptor.AddAttributes(typeof(Double[]), new TypeConverterAttribute(typeof(ArrayConverter<Double>)));
            TypeDescriptor.AddAttributes(typeof(Decimal[]), new TypeConverterAttribute(typeof(ArrayConverter<Decimal>)));
            TypeDescriptor.AddAttributes(typeof(Boolean[]), new TypeConverterAttribute(typeof(ArrayConverter<Boolean>)));
            TypeDescriptor.AddAttributes(typeof(DateTime[]), new TypeConverterAttribute(typeof(ArrayConverter<DateTime>)));
            TypeDescriptor.AddAttributes(typeof(String[]), new TypeConverterAttribute(typeof(ArrayConverter<String>)));
            TypeDescriptor.AddAttributes(typeof(TimeSpan[]), new TypeConverterAttribute(typeof(ArrayConverter<TimeSpan>)));
            TypeDescriptor.AddAttributes(typeof(System.Net.IPAddress), new TypeConverterAttribute(typeof(System.Net.IPAddressTypeConverter)));
            TypeDescriptor.AddAttributes(typeof(System.Net.IPAddress[]), new TypeConverterAttribute(typeof(ArrayConverter<System.Net.IPAddress>)));

            //TypeDescriptor.AddAttributes(typeof(Byte?), new TypeConverterAttribute(typeof(NullableByteConverter)));
            //TypeDescriptor.AddAttributes(typeof(Int16?), new TypeConverterAttribute(typeof(NullableInt16Converter)));
            //TypeDescriptor.AddAttributes(typeof(Int32?), new TypeConverterAttribute(typeof(NullableInt32Converter)));
            //TypeDescriptor.AddAttributes(typeof(Int64?), new TypeConverterAttribute(typeof(NullableInt64Converter)));
            //TypeDescriptor.AddAttributes(typeof(Decimal?), new TypeConverterAttribute(typeof(NullableDecimalConverter)));
        }

        static bool _Convert_Enum(Type dstType, object srcValue, out object result)
        {
            bool ret = true;
            result = srcValue;
            try
            {
                if (dstType.GetTypeInfo().IsEnum)
                    result = Enum.ToObject(dstType, srcValue);
                else if (dstType == typeof(Decimal))
                    result = Convert.ToDecimal(srcValue);
                else if (dstType == typeof(Double))
                    result = Convert.ToDouble(srcValue);
                else if (dstType == typeof(Single))
                    result = Convert.ToSingle(srcValue);
                else if (dstType == typeof(Int64))
                    result = Convert.ToInt64(srcValue);
                else if (dstType == typeof(Int32))
                    result = Convert.ToInt32(srcValue);
                else if (dstType == typeof(Int16))
                    result = Convert.ToInt16(srcValue);
                else if (dstType == typeof(SByte))
                    result = Convert.ToSByte(srcValue);
                else if (dstType == typeof(UInt64))
                    result = Convert.ToUInt64(srcValue);
                else if (dstType == typeof(UInt32))
                    result = Convert.ToUInt32(srcValue);
                else if (dstType == typeof(UInt16))
                    result = Convert.ToUInt16(srcValue);
                else if (dstType == typeof(Byte))
                    result = Convert.ToByte(srcValue);
                else if (dstType == typeof(Char))
                    result = Convert.ToChar(srcValue);
                else
                    ret = false;
            }
            catch { }
            return ret;
        }

        #region ConvertFrom

        static bool ConvertFrom(MemberInfo m, Type srcType, object srcValue, Type dstType, out object result)
        {
            if ((srcType == dstType) || (dstType == typeof(object)))
            {
                result = srcValue;
                return true;
            }
            if (dstType.GetTypeInfo().IsGenericType)
            {
                if (dstType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (ConvertFrom(m, srcType, srcValue, Nullable.GetUnderlyingType(dstType), out result))
                    {
                        result = Activator.CreateInstance(dstType, result);
                        return true;
                    }
                }
            }
            TypeConverter c;
            if (!(c = TypeDescriptor.GetConverter(m)).CanConvertFrom(srcType))
                if (!(c = TypeDescriptor.GetConverter(dstType)).CanConvertFrom(srcType))
                    return _Convert_Enum(dstType, srcValue, out result);
            result = c.ConvertFrom(srcValue);
            return true;
        }

        public static bool ConvertFrom(this PropertyInfo p, Type srcType, object value, out object result)
        {
            if (p == null) { result = null; return false; }
            return ConvertFrom(p, srcType, value, p.PropertyType, out result);
        }
        public static bool ConvertFrom(this FieldInfo f, Type srcType, object value, out object result)
        {
            if (f == null) { result = null; return false; }
            return ConvertFrom(f, srcType, value, f.FieldType, out result);
        }

        public static bool ConvertFrom<T>(this PropertyInfo p, T value, out object result)
        {
            return ConvertFrom(p, typeof(T), value, out result);
        }
        public static bool ConvertFrom<T>(this FieldInfo f, T value, out object result)
        {
            return ConvertFrom(f, typeof(T), value, out result);
        }

        #endregion

        #region ConvertTo

        static bool ConvertTo(MemberInfo m, Type srcType, object srcValue, Type dstType, out object result)
        {
            if (srcType == dstType)
            {
                result = srcValue;
                return true;
            }
            if (dstType.GetTypeInfo().IsGenericType)
            {
                if (dstType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (ConvertTo(m, srcType, srcValue, Nullable.GetUnderlyingType(dstType), out result))
                    {
                        result = Activator.CreateInstance(dstType, result);
                        return true;
                    }
                }
            }

            TypeConverter c;
            if (!(c = TypeDescriptor.GetConverter(srcType)).CanConvertTo(dstType))
                if (!(c = TypeDescriptor.GetConverter(m)).CanConvertTo(dstType))
                    return _Convert_Enum(dstType, srcValue, out result);
            result = c.ConvertTo(srcValue, dstType);
            return true;
        }

        public static bool ConvertTo(this PropertyInfo p, Type dstType, object value, out object result)
        {
            if (p == null) { result = null; return false; }
            return ConvertTo(p, p.PropertyType, value, dstType, out result);
        }
        public static bool ConvertTo(this FieldInfo f, Type dstType, object value, out object result)
        {
            if (f == null) { result = null; return false; }
            return ConvertTo(f, f.FieldType, value, dstType, out result);
        }

        public static bool ConvertTo<T>(this PropertyInfo p, object value, out T result)
        {
            try
            {
                object tmp;
                if (p.ConvertTo(typeof(T), value, out tmp))
                {
                    result = (T)tmp;
                    return true;
                }
            }
            catch { }
            result = default(T);
            return false;
        }
        public static bool ConvertTo<T>(this FieldInfo f, object value, out T result)
        {
            try
            {
                object tmp;
                if (f.ConvertTo(typeof(T), value, out tmp))
                {
                    result = (T)tmp;
                    return true;
                }
            }
            catch { }
            result = default(T);
            return false;
        }

        #endregion

        #region SetValue / GetValue

        public static bool SetValueFrom(this PropertyInfo p, object obj, object value, object[] index)
        {
            try
            {
                if ((p != null) && (value != null))
                {
                    Type valueType = value.GetType();
                    object n;
                    if (p.ConvertFrom(valueType, value, out n))
                        p.SetValue(obj, n, index);
                    else
                        p.SetValue(obj, value, index);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static bool GetValueTo<T>(this PropertyInfo p, object obj, object[] index, out T result)
        {
            try { return p.ConvertTo<T>(p.GetValue(obj, index), out result); }
            catch { }
            result = default(T);
            return false;
        }

        public static bool SetValueFrom(this FieldInfo f, object obj, object value)
        {
            try
            {
                if ((f != null) && (value != null))
                {
                    Type valueType = value.GetType();
                    object n;
                    if (f.ConvertFrom(valueType, value, out n))
                        f.SetValue(obj, n);
                    else
                        f.SetValue(obj, value);
                    return true;
                }
            }
            catch { }
            return false;
        }

        public static bool GetValueTo<T>(this FieldInfo f, object obj, out T result)
        {
            try { return f.ConvertTo<T>(f.GetValue(obj), out result); }
            catch { }
            result = default(T);
            return false;
        }

        #endregion
    }

    [_DebuggerStepThrough]
    public class ArrayConverter<T> : ArrayConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
            if (conv == null) return base.CanConvertFrom(context, sourceType);
            else /**********/ return conv.CanConvertFrom(sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                string[] s = ((string)value).Split(typeof(T) == typeof(string) ? ';' : ',');
                T[] o = new T[s.Length];
                TypeConverter c = TypeDescriptor.GetConverter(typeof(T));
                for (int i = 0; i < s.Length; i++)
                    try { o[i] = (T)c.ConvertFrom(s[i]); }
                    catch { }
                return o;
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
            if (conv == null) return base.CanConvertTo(context, destinationType);
            else /**********/ return conv.CanConvertTo(destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is T[])
            {
                T[] o = (T[])value;
                string[] s = new string[o.Length];
                TypeConverter c = TypeDescriptor.GetConverter(typeof(T));
                for (int i = 0; i < o.Length; i++)
                    try { s[i] = (string)c.ConvertTo(o[i], destinationType); }
                    catch { }
                return string.Join(";", s);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

    [_DebuggerStepThrough]
    public class DictionaryConverter : TypeConverter
    {
        // Methods
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is IDictionary))
            {
                Type t = value.GetType();
                for (Type t1 = t; t1 != null; t1 = t1.GetTypeInfo().BaseType)
                {
                    if (t1.GetTypeInfo().BaseType == typeof(object))
                    {
                        Type[] t2 = t1.GetGenericArguments();
                        if (t2.Length >= 2)
                        {
                            return t2[1].Name;
                        }
                    }
                }
                return t.Name;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptor[] properties = null;
            if (value is IDictionary)
            {
                IDictionary dict = (IDictionary)value;
                properties = new PropertyDescriptor[dict.Count];
                int i = 0;
                foreach (DictionaryEntry p in dict)
                {
                    Type componentType;
                    Type propertyType;
                    string name;
                    if (p.Key == null)
                    {
                        componentType = typeof(object);
                        name = "";
                    }
                    else
                    {
                        componentType = p.Key.GetType();
                        name = p.Key.ToString();
                    }
                    if (p.Value == null)
                    {
                        propertyType = typeof(object);
                    }
                    else
                    {
                        propertyType = p.Value.GetType();
                    }
                    properties[i++] = new KeyValuePairDescriptor(componentType, p.Key, name, propertyType);
                }
            }
            return new PropertyDescriptorCollection(properties);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        // Nested Types
        private class KeyValuePairDescriptor : TypeConverter.SimplePropertyDescriptor
        {
            // Fields
            private object key;

            // Methods
            public KeyValuePairDescriptor(Type componentType, object key, string name, Type propertyType)
                : base(componentType, name, propertyType)
            {
                this.key = key;
            }

            public override object GetValue(object instance)
            {
                if (instance is IDictionary)
                {
                    IDictionary dict = (IDictionary)instance;
                    if (dict.Contains(this.key))
                    {
                        return dict[this.key];
                    }
                }
                return null;
            }

            public override void SetValue(object instance, object value)
            {
                if (instance is IDictionary)
                {
                    IDictionary dict = (IDictionary)instance;
                    if (dict.Contains(this.key))
                    {
                        dict[this.key] = value;
                    }
                    this.OnValueChanged(instance, EventArgs.Empty);
                }
            }
        }
    }

#if !NET40
    public class ToolboxItemAttribute : Attribute
    {
        public ToolboxItemAttribute(bool n) { }
    }
#endif

#if !NET40
    public class ExpandableObjectConverter : TypeConverter
    {
        public ExpandableObjectConverter() { }
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return TypeDescriptor.GetProperties(value, attributes);
        }
        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
    }
#endif

    [_DebuggerStepThrough]
    public class ExpandableObjectConverter_ : ExpandableObjectConverter
    {
        // Methods
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value == null)
            {
                return null;
            }
            return value.GetType().Name;
        }
    }

    [_DebuggerStepThrough]
    public class ListConverter : ArrayConverter
    {
        // Methods
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            if ((destinationType == typeof(string)) && (value is IList))
            {
                IList list = (IList)value;
                Type t = value.GetType();
                for (Type t1 = t; t1 != null; t1 = t1.GetTypeInfo().BaseType)
                {
                    if (t1.GetTypeInfo().BaseType == typeof(object))
                    {
                        Type[] t2 = t1.GetGenericArguments();
                        if (t2.Length >= 1)
                        {
                            return t2[0].Name;
                        }
                    }
                }
                return t.Name;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            PropertyDescriptor[] properties = null;
            if (value is IList)
            {
                IList list = (IList)value;
                int length = list.Count;
                properties = new PropertyDescriptor[length];
                Type t = value.GetType();
                for (int i = 0; i < length; i++)
                {
                    Type elementType;
                    object n = list[i];
                    if (n == null)
                    {
                        elementType = typeof(object);
                    }
                    else
                    {
                        elementType = n.GetType();
                    }
                    properties[i] = new ListPropertyDescriptor(t, elementType, i, GetText(n, i));
                }
            }
            return new PropertyDescriptorCollection(properties);
        }

        protected virtual string GetText(object obj, int index)
        {
            return string.Format("[{0}]", index);
        }

        public override bool GetPropertiesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        // Nested Types
        protected class ListPropertyDescriptor : TypeConverter.SimplePropertyDescriptor
        {
            // Fields
            private int index;

            // Methods
            public ListPropertyDescriptor(Type arrayType, Type elementType, int index, string text)
                : base(arrayType, text, elementType, null)
            {
                this.index = index;
            }

            public override object GetValue(object instance)
            {
                if (instance is IList)
                {
                    IList list = (IList)instance;
                    if (list.Count > this.index)
                    {
                        return list[this.index];
                    }
                }
                return null;
            }

            public override void SetValue(object instance, object value)
            {
                if (instance is IList)
                {
                    IList list = (IList)instance;
                    if (list.Count > this.index)
                    {
                        list[this.index] = value;
                    }
                    this.OnValueChanged(instance, EventArgs.Empty);
                }
            }
        }
    }
}