using IG.Lobby.VA.Resources;
using System;

namespace IG.Lobby.VA.Helpers
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
