#if netcore
namespace System.Security.Cryptography
{
    using System;
    using System.Diagnostics.Contracts;

    [System.Runtime.InteropServices.ComVisible(true)]
    public abstract class DES : SymmetricAlgorithm
    {
        private static KeySizes[] s_legalBlockSizes = { new KeySizes(64, 64, 0) };
        private static KeySizes[] s_legalKeySizes = { new KeySizes(64, 64, 0) };

        //protected int FeedbackSizeValue;


        protected DES()
        {
            KeySizeValue = 64;
            BlockSizeValue = 64;
            //FeedbackSizeValue = BlockSizeValue;
            LegalBlockSizesValue = s_legalBlockSizes;
            LegalKeySizesValue = s_legalKeySizes;
        }

        public bool ValidKeySize(int bitLength)
        {
            KeySizes[] validSizes = this.LegalKeySizes;
            int i, j;

            if (validSizes == null) return false;
            for (i = 0; i < validSizes.Length; i++)
            {
                if (validSizes[i].SkipSize == 0)
                {
                    if (validSizes[i].MinSize == bitLength)
                    { // assume MinSize = MaxSize
                        return true;
                    }
                }
                else
                {
                    for (j = validSizes[i].MinSize; j <= validSizes[i].MaxSize;
                         j += validSizes[i].SkipSize)
                    {
                        if (j == bitLength)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public override byte[] Key
        {
            get
            {
                if (KeyValue == null)
                {
                    // Never hand back a weak or semi-weak key
                    do
                    {
                        GenerateKey();
                    } while (IsWeakKey(KeyValue) || IsSemiWeakKey(KeyValue));
                }
                return (byte[])KeyValue.Clone();
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (!ValidKeySize(value.Length * 8))
                { // must convert bytes to bits
                    throw new ArgumentException(SR.Cryptography_InvalidKeySize);
                }
                if (IsWeakKey(value))
                {
                    throw new CryptographicException(SR.Cryptography_InvalidKey_Weak, "DES");
                }
                if (IsSemiWeakKey(value))
                {
                    throw new CryptographicException(SR.Cryptography_InvalidKey_SemiWeak, "DES");
                }
                KeyValue = (byte[])value.Clone();
                KeySizeValue = value.Length * 8;
            }
        }

        public static DES Create() => new System.Security.Cryptography.DESCryptoServiceProvider();

        public static DES Create(String algName) => new System.Security.Cryptography.DESCryptoServiceProvider();

        public static bool IsWeakKey(byte[] rgbKey)
        {
            if (!IsLegalKeySize(rgbKey))
            {
                throw new CryptographicException(SR.Cryptography_InvalidKeySize);
            }
            byte[] rgbOddParityKey = Utils.FixupKeyParity(rgbKey);
            UInt64 key = QuadWordFromBigEndian(rgbOddParityKey);
            if ((key == 0x0101010101010101) ||
                (key == 0xfefefefefefefefe) ||
                (key == 0x1f1f1f1f0e0e0e0e) ||
                (key == 0xe0e0e0e0f1f1f1f1))
            {
                return (true);
            }
            return (false);
        }

        public static bool IsSemiWeakKey(byte[] rgbKey)
        {
            if (!IsLegalKeySize(rgbKey))
            {
                throw new CryptographicException(SR.Cryptography_InvalidKeySize);
            }
            byte[] rgbOddParityKey = Utils.FixupKeyParity(rgbKey);
            UInt64 key = QuadWordFromBigEndian(rgbOddParityKey);
            if ((key == 0x01fe01fe01fe01fe) ||
                (key == 0xfe01fe01fe01fe01) ||
                (key == 0x1fe01fe00ef10ef1) ||
                (key == 0xe01fe01ff10ef10e) ||
                (key == 0x01e001e001f101f1) ||
                (key == 0xe001e001f101f101) ||
                (key == 0x1ffe1ffe0efe0efe) ||
                (key == 0xfe1ffe1ffe0efe0e) ||
                (key == 0x011f011f010e010e) ||
                (key == 0x1f011f010e010e01) ||
                (key == 0xe0fee0fef1fef1fe) ||
                (key == 0xfee0fee0fef1fef1))
            {
                return (true);
            }
            return (false);
        }

        private static bool IsLegalKeySize(byte[] rgbKey)
        {
            if (rgbKey != null && rgbKey.Length == 8) return (true);
            return (false);
        }

        private static UInt64 QuadWordFromBigEndian(byte[] block)
        {
            UInt64 x;
            x = (
                  (((UInt64)block[0]) << 56) | (((UInt64)block[1]) << 48) |
                  (((UInt64)block[2]) << 40) | (((UInt64)block[3]) << 32) |
                  (((UInt64)block[4]) << 24) | (((UInt64)block[5]) << 16) |
                  (((UInt64)block[6]) << 8) | ((UInt64)block[7])
                  );
            return (x);
        }
    }
}
#endif