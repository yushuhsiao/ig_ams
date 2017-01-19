using System;
using System.Web.Configuration;

namespace IG.Lobby.LC.Helpers
{
    public static class ConfigHelper
    {
        private static string antiForgeryCookieName = WebConfigurationManager.AppSettings["AntiForgeryCookieName"];
        public static string AntiForgeryCookieName { get { return antiForgeryCookieName; } }

        private static string cultureCookieName = WebConfigurationManager.AppSettings["CultureCookieName"];
        public static string CultureCookieName { get { return cultureCookieName; } }

        private static string version = WebConfigurationManager.AppSettings["Version"];
        public static string Version { get { return version; } }

        private static string siteName = WebConfigurationManager.AppSettings["SiteName"];
        public static string SiteName { get { return siteName; } }

        private static string apiSecretToken = WebConfigurationManager.AppSettings["ApiSecretToken"];
        public static string ApiSecretToken { get { return apiSecretToken; } }

        private static string assetServerUrl = WebConfigurationManager.AppSettings["AssetServerUrl"];
        public static string AssetServerUrl { get { return assetServerUrl; } }

        private static string rtmpServerUrl = WebConfigurationManager.AppSettings["RtmpServerUrl"];
        public static string RtmpServerUrl { get { return rtmpServerUrl; } }

        private static string recognitionApiUrl = WebConfigurationManager.AppSettings["RecognitionApiUrl"];
        public static string RecognitionApiUrl { get { return recognitionApiUrl; } }

        private static string liveCasinoGsApiUrl = WebConfigurationManager.AppSettings["LiveCasinoGsApiUrl"];
        public static string LiveCasinoGsApiUrl { get { return liveCasinoGsApiUrl; } }

        private static string noticeGroupName = "NOTICE";
        public static string NoticeGroupName { get { return noticeGroupName; } }

        private static string liveCasinoGroupName = "LIVE_CASINO_LOBBY";
        public static string LiveCasinoGroupName { get { return liveCasinoGroupName; } }
    }
}
