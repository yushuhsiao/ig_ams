namespace System.ComponentModel
{
    public static class _Extensions
    {
        static _Extensions()
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
            //TypeDescriptor.AddAttributes(typeof(System.Net.IPAddress), new TypeConverterAttribute(typeof(System.Net.IPAddressTypeConverter)));
            //TypeDescriptor.AddAttributes(typeof(System.Net.IPAddress[]), new TypeConverterAttribute(typeof(ArrayConverter<System.Net.IPAddress>)));

            //TypeDescriptor.AddAttributes(typeof(Byte?), new TypeConverterAttribute(typeof(NullableByteConverter)));
            //TypeDescriptor.AddAttributes(typeof(Int16?), new TypeConverterAttribute(typeof(NullableInt16Converter)));
            //TypeDescriptor.AddAttributes(typeof(Int32?), new TypeConverterAttribute(typeof(NullableInt32Converter)));
            //TypeDescriptor.AddAttributes(typeof(Int64?), new TypeConverterAttribute(typeof(NullableInt64Converter)));
            //TypeDescriptor.AddAttributes(typeof(Decimal?), new TypeConverterAttribute(typeof(NullableDecimalConverter)));
        }

        public static T GetValue<T>(this DefaultValueAttribute a)
        {
            if (a != null)
            {
                if (a.Value != null)
                {
                    if (a.Value.GetType() == typeof(T))
                    {
                        try { return (T)a.Value; }
                        catch { }
                    }
                }
                TypeConverter c = TypeDescriptor.GetConverter(typeof(T));
                if (c.CanConvertTo(typeof(T)))
                {
                    try { return (T)c.ConvertTo(a.Value, typeof(T)); }
                    catch { }
                }
            }
            return default(T);
        }
    }
}
