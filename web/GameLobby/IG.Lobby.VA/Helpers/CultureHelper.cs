using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace IG.Lobby.VA.Helpers
{
    public class CultureHelper
    {
        /// <summary>
        /// 已經實作的語系清單，新增 Resource 後要記得來添加，第一個語系為預設語系
        /// </summary>
        private static readonly string[] cultures = new string[]
        {
            "en-US",
            "zh-CN",
            "zh-TW"
        };

        private static readonly Dictionary<string, string> languageMap = new Dictionary<string, string>
        {
            { "en-US", "EN" },
            { "zh-CN", "CHS" },
            { "zh-TW", "CHT" }
        };

        private static readonly Dictionary<string, string> gameCultureMap = new Dictionary<string, string>
        {
            { "en-US", "en-US" },
            { "zh-CN", "zh-CN" },
            { "zh-TW", "zh-CN" }
        };

        /// <summary>
        /// 檢查並取得已實作的語系
        /// </summary>
        public static string GetImplementedCulture(string name)
        {
            // 如果為空字串時回傳預設語系
            if (String.IsNullOrEmpty(name))
            {
                return GetDefaultCulture();
            }

            // 該語系在清單中，回傳此語系
            var culture = cultures.FirstOrDefault(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (culture != null)
            {
                return culture;
            }

            // 嘗試取得最接近的語系
            var neutralCulture = GetNeutralCulture(name);

            culture = cultures.FirstOrDefault(x => x.StartsWith(neutralCulture, StringComparison.OrdinalIgnoreCase));
            if (culture != null)
            {
                return culture;
            }

            // 沒有符合的，回傳預設語系
            return GetDefaultCulture();
        }

        public static string GetDefaultCulture()
        {
            return cultures[0];
        }

        public static string GetCurrentCulture()
        {
            return Thread.CurrentThread.CurrentCulture.Name;
        }

        public static string GetCurrentLanguages()
        {
            string language;
            if (languageMap.TryGetValue(GetCurrentCulture(), out language))
            {
                return language;
            }

            return languageMap.First().Value;
        }

        public static string GetCurrentGameCulture()
        {
            string gameCulture;
            if (gameCultureMap.TryGetValue(GetCurrentCulture(), out gameCulture))
            {
                return gameCulture;
            }

            return gameCultureMap.First().Value;
        }

        /// <summary>
        /// 取得最接近的語系，回傳前兩個字元，例："en", "zh"
        /// </summary>
        private static string GetNeutralCulture(string name)
        {
            return (name.Length < 2) ? name : name.Substring(0, 2);
        }
    }
}
