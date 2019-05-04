using System.Globalization;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.ComponentModel
{
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
}
