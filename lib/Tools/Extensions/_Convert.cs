using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;
using _PropertyInfo = System.Reflection.PropertyInfo;
using ____FieldInfo = System.Reflection.FieldInfo;

namespace System
{
    [_DebuggerStepThrough]
    public static class _Convert
    {
        #region ConvertFrom

        static bool ConvertFrom(MemberInfo m, Type srcType, object srcValue, Type dstType, out object result)
        {
            if ((srcType == dstType) || (dstType == typeof(object)))
            {
                result = srcValue;
                return true;
            }
            if (dstType.IsGenericType)
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
            if ((m != null) && (c = TypeDescriptor.GetConverter(m)).CanConvertFrom(srcType))
            {
                result = c.ConvertFrom(srcValue);
                return true;
            }
            if ((c = TypeDescriptor.GetConverter(dstType)).CanConvertFrom(srcType))
            {
                result = c.ConvertFrom(srcValue);
                return true;
            }
            if (dstType.IsEnum)
            {
                try { result = Enum.ToObject(dstType, srcValue); return true; }
                catch { }
            }
            try { result = Convert.ChangeType(srcValue, dstType); return true; }
            catch { }
            result = null;
            return false;
        }

        public static bool ConvertFrom(Type srcType, object srcValue, Type dstType, out object result) => ConvertFrom(null, srcType, srcValue, dstType, out result);

        public static bool ConvertFrom<TSrc, TDst>(TSrc srcValue, out TDst result)
        {
            object _result;
            if (ConvertFrom(null, typeof(TSrc), srcValue, typeof(TDst), out _result))
            {
                try { result = (TDst)_result; return true; }
                catch { }
            }
            return _null.noop(false, out result);
        }

        public static bool ConvertFrom(this _PropertyInfo p, Type srcType, object value, out object result)
        {
            if (p == null)
                return _null.noop(false, out result);
            return ConvertFrom(p, srcType, value, p.PropertyType, out result);
        }
        public static bool ConvertFrom(this ____FieldInfo f, Type srcType, object value, out object result)
        {
            if (f == null)
                return _null.noop(false, out result);
            return ConvertFrom(f, srcType, value, f.FieldType, out result);
        }

        public static bool ConvertFrom<T>(this _PropertyInfo p, T value, out object result) => ConvertFrom(p, typeof(T), value, out result);
        public static bool ConvertFrom<T>(this ____FieldInfo f, T value, out object result) => ConvertFrom(f, typeof(T), value, out result);

        #endregion

        #region ConvertTo

        static bool ConvertTo(MemberInfo m, Type srcType, object srcValue, Type dstType, out object result)
        {
            if (srcType == dstType)
            {
                result = srcValue;
                return true;
            }
            if (dstType.IsGenericType)
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
            if ((m != null) && (c = TypeDescriptor.GetConverter(m)).CanConvertTo(dstType))
            {
                result = c.ConvertTo(srcValue, dstType);
                return true;
            }
            if ((c = TypeDescriptor.GetConverter(srcType)).CanConvertTo(dstType))
            {
                result = c.ConvertTo(srcValue, dstType);
                return true;
            }
            if (dstType.IsEnum)
            {
                try { result = Enum.ToObject(dstType, srcValue); return true; }
                catch { }
            }
            try { result = Convert.ChangeType(srcValue, dstType); return true; }
            catch { }
            result = null;
            return false;
        }

        public static bool ConvertTo(Type srcType, object srcValue, Type dstType, out object result) => ConvertTo(null, srcType, srcValue, dstType, out result);

        public static bool ConvertTo<TSrc, TDst>(TSrc srcValue, out TDst result)
        {
            object _result;
            if (ConvertTo(null, typeof(TSrc), srcValue, typeof(TDst), out _result))
            {
                try { result = (TDst)_result; return true; }
                catch { }
            }
            result = default(TDst);
            return false;
        }

        public static bool ConvertTo(this _PropertyInfo p, Type dstType, object value, out object result)
        {
            if (p == null)
                return _null.noop(false, out result);
            return ConvertTo(p, p.PropertyType, value, dstType, out result);
        }
        public static bool ConvertTo(this ____FieldInfo f, Type dstType, object value, out object result)
        {
            if (f == null)
                return _null.noop(false, out result);
            return ConvertTo(f, f.FieldType, value, dstType, out result);
        }

        public static bool ConvertTo<T>(this _PropertyInfo p, object value, out T result)
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
            return _null.noop(false, out result);
        }
        public static bool ConvertTo<T>(this ____FieldInfo f, object value, out T result)
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
            return _null.noop(false, out result);
        }

        #endregion
    }

    //[_DebuggerStepThrough]
    //public class ExpandableObjectConverter_ : ExpandableObjectConverter
    //{
    //    // Methods
    //    public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    //    {
    //        if (value == null)
    //        {
    //            return null;
    //        }
    //        return value.GetType().Name;
    //    }
    //}

}
//namespace System.Reflection
//{
//    partial class _ReflectionExtension
//    {
//        #region ConvertFrom

//        #endregion
//        #region ConvertTo

//        #endregion
//    }
//}