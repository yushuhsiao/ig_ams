
namespace IG.Lobby.LC.Helpers
{
    public static class NcryptHelper
    {
        public static string Encrypt(string password)
        {
            return password;
        }

        public static bool Verify(string password, string encryptPassword)
        {
            return password == encryptPassword;
        }
    }
}
