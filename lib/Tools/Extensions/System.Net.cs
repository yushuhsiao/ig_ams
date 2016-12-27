using System.ComponentModel;

namespace System.Net
{
    //public static partial class extensions
    //{
    //    static void native_changeorder(byte[] addressBytes)
    //    {
    //        addressBytes[0] ^= addressBytes[3];
    //        addressBytes[3] ^= addressBytes[0];
    //        addressBytes[0] ^= addressBytes[3];

    //        addressBytes[1] ^= addressBytes[2];
    //        addressBytes[2] ^= addressBytes[1];
    //        addressBytes[1] ^= addressBytes[2];
    //    }
    //    public static unsafe long ToNative(this IPAddress ipAddress)
    //    {
    //        byte[] addressBytes = ipAddress.GetAddressBytes();
    //        native_changeorder(addressBytes);
    //        fixed (byte* p = addressBytes)
    //            return (long)(*(int*)p);
    //    }

    //    public static IPAddress FromNative(this IPAddress ipAddress, long nativeIpAddress)
    //    {
    //        byte[] addressBytes = BitConverter.GetBytes((uint)nativeIpAddress);

    //        native_changeorder(addressBytes);

    //        return new IPAddress(addressBytes);

    //        //new IPAddress((long)IPAddress.HostToNetworkOrder((int));
    //    }
    //}

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
