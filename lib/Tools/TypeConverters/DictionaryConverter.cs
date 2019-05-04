using System.Collections;
using System.Globalization;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.ComponentModel
{
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
                for (Type t1 = t; t1 != null; t1 = t1.BaseType)
                {
                    if (t1.BaseType == typeof(object))
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
}
