using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IG.Lobby.TG.Services
{
    public class GsTexasHoldemApiService
    {
        private string baseAddress = null;
        private int timeout = 5000;

        public GsTexasHoldemApiService(string baseAddress)
        {
            this.baseAddress = baseAddress;
        }

        public GsTexasHoldemApiService(string baseAddress, int timeout)
        {
            this.baseAddress = baseAddress;
            this.timeout = timeout;
        }

        public async Task<IEnumerable<ApiTexasHoldemTable>> Tables()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);
                httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

                var response = await httpClient.GetAsync("texasholdemLobbyList");
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<ApiTexasHoldemTable>>(responseContent);
            }
        }
    }

    public class ApiTexasHoldemTable
    {
        public long TableId { get; set; }

        public string TableName_EN { get; set; }

        public string TableName_CHS { get; set; }

        public string TableName_CHT { get; set; }

        public string RandomID { get; set; }

        public string TableNameEx_EN
        {
            get { return $"{TableName_EN} {RandomID}"; }
        }

        public string TableNameEx_CHS
        {
            get { return $"{TableName_CHS} {RandomID}"; }
        }

        public string TableNameEx_CHT
        {
            get { return $"{TableName_CHT} {RandomID}"; }
        }

        public int SmallBlind { get; set; }

        public int BigBlind { get; set; }

        public int SecondsToCountdown { get; set; }

        public int PlayerAmount { get; set; }
    }
}
