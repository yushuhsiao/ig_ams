#if !NET40
namespace System.Security.Cryptography
{
    [System.Runtime.InteropServices.ComVisible(true)]
    public sealed class DESCryptoServiceProvider : DES
    {
        [System.Security.SecuritySafeCritical]  // auto-generated
        public DESCryptoServiceProvider()
        {
            //FeedbackSizeValue = 8;
        }

        [System.Security.SecuritySafeCritical]  // auto-generated
        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (IsWeakKey(rgbKey))
                throw new CryptographicException(SR.Cryptography_InvalidKey_Weak, "DES");
            if (IsSemiWeakKey(rgbKey))
                throw new CryptographicException(SR.Cryptography_InvalidKey_SemiWeak, "DES");
            return new DESTransform(this, true, rgbKey, rgbIV);
        }

        [System.Security.SecuritySafeCritical]  // auto-generated
        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] rgbIV)
        {
            if (IsWeakKey(rgbKey))
                throw new CryptographicException(SR.Cryptography_InvalidKey_Weak, "DES");
            if (IsSemiWeakKey(rgbKey))
                throw new CryptographicException(SR.Cryptography_InvalidKey_SemiWeak, "DES");
            return new DESTransform(this, false, rgbKey, rgbIV);
        }

        public override void GenerateKey()
        {
            KeyValue = new byte[8];
            Utils.StaticRandomNumberGenerator.GetBytes(KeyValue);
            // Never hand back a weak or semi-weak key
            while (DES.IsWeakKey(KeyValue) || DES.IsSemiWeakKey(KeyValue))
            {
                Utils.StaticRandomNumberGenerator.GetBytes(KeyValue);
            }
        }

        public override void GenerateIV()
        {
            IVValue = new byte[8];
            Utils.StaticRandomNumberGenerator.GetBytes(IVValue);
        }
    }
}
#endif