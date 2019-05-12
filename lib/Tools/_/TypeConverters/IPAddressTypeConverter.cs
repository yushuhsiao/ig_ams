using System.ComponentModel;

namespace System.Net
{
    public partial class IPAddressTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            IPAddress ip = value as IPAddress;
            if ((ip != null) && (destinationType == typeof(string)))
                return ip.ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string ips = value as string;
            IPAddress ip;
            if ((ips != null) && (IPAddress.TryParse(ips, out ip)))
                return ip;
            return base.ConvertFrom(context, culture, value);
        }
    }
}
