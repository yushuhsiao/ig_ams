using IG.Lobby.TG.Resources;
using System;

namespace IG.Lobby.TG.Helpers
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
