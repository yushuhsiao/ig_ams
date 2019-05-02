using System.Collections;
using System.Globalization;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.ComponentModel
{
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
                for (Type t1 = t; t1 != null; t1 = t1.BaseType)
                {
                    if (t1.BaseType == typeof(object))
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
