using System.ComponentModel;

namespace System.Net
{
    public static partial class extensions
    {
        static void native_changeorder(byte[] addressBytes)
        {
            addressBytes[0] ^= addressBytes[3];
            addressBytes[3] ^= addressBytes[0];
            addressBytes[0] ^= addressBytes[3];

            addressBytes[1] ^= addressBytes[2];
            addressBytes[2] ^= addressBytes[1];
            addressBytes[1] ^= addressBytes[2];
        }
        public static unsafe long ToNative(this IPAddress ipAddress)
        {
            byte[] addressBytes = ipAddress.GetAddressBytes();
            native_changeorder(addressBytes);
            fixed (byte* p = addressBytes)
                return (long)(*(int*)p);
        }

        public static IPAddress FromNative(this IPAddress ipAddress, long nativeIpAddress)
        {
            byte[] addressBytes = BitConverter.GetBytes((uint)nativeIpAddress);

            native_changeorder(addressBytes);

            return new IPAddress(addressBytes);

            //new IPAddress((long)IPAddress.HostToNetworkOrder((int));
        }
    }

}
