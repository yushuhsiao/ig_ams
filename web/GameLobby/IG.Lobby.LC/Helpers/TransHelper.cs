using IG.Lobby.LC.Resources;
using System;

namespace IG.Lobby.LC.Helpers
{
    public class TransHelper
    {
        public static string Ui(string key)
        {
            if (String.IsNullOrEmpty(key))
            {
                return String.Empty;
            }

            var value = UiResource.ResourceManager.GetString(key);

            return (value == null) ? key : value;
        }
    }
}
