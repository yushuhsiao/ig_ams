using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IG.Lobby.LC.Services
{
    public class GsLiveCasinoApiService
    {
        private string baseAddress = null;
        private int timeout = 5000;

        public GsLiveCasinoApiService(string baseAddress)
        {
            this.baseAddress = baseAddress;
        }

        public GsLiveCasinoApiService(string baseAddress, int timeout)
        {
            this.baseAddress = baseAddress;
            this.timeout = timeout;
        }

        public async Task<IEnumerable<ApiLiveCasinoTable>> Tables()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);
                httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

                var response = await httpClient.GetAsync("lobbyInfo");
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<ApiLiveCasinoTable>>(responseContent);
            }
        }
    }

    public class ApiLiveCasinoTable
    {
        public int TableId { get; set; }

        public string GameType { get; set; }

        public string Dealer { get { return "jane"; } set { } }

        public bool GamePaused { get; set; }

        public string TableState { get; set; }

        public int CountDownSeconds { get; set; }

        public string RoadMap { get; set; }

        public IEnumerable<SicboDice> LastDice { get; set; }

        public IEnumerable<int> LastNumbers { get; set; }
    }

    public class SicboDice
    {
        public int Dice1 { get; set; }

        public int Dice2 { get; set; }

        public int Dice3 { get; set; }
    }
}
