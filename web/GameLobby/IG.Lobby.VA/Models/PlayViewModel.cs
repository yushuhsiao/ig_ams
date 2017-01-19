
namespace IG.Lobby.VA.Models
{
    public class PlayLegacyViewModel
    {
        public string LoaderToken { get; set; }

        public string GameName { get; set; }

        public string GameToken { get; set; }

        public string Culture { get; set; }

        public int GameWidth { get; set; }

        public int GameHeight { get; set; }

        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public string AccessToken { get; set; }

        public string Account { get; set; }
    }

    public class PlayHtml5ViewModel
    {
        public string GameName { get; set; }

        public string GameToken { get; set; }

        public string Culture { get; set; }

        public string ServerUrl { get; set; }

        public int ServerPort { get; set; }

        public string AccessToken { get; set; }
    }
}
