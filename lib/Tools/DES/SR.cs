#if netcore
namespace System.Security.Cryptography
{
    static class SR
    {
        internal static readonly string Cryptography_InvalidKeySize = "Cryptography_InvalidKeySize";
        internal static readonly string Cryptography_InvalidKey_SemiWeak = "Cryptography_InvalidKey_SemiWeak";
        internal static readonly string Cryptography_InvalidKey_Weak = "Cryptography_InvalidKey_Weak";
    }
    static class Locale
    {
        public static string GetText(string format, params object[] args) => string.Format(format, args);
    }
}
namespace System.Diagnostics.Contracts { }
#endif