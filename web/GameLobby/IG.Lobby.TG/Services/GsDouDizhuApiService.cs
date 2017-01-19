using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IG.Lobby.TG.Services
{
    public class GsDouDizhuApiService
    {
        private string baseAddress = null;
        private int timeout = 5000;

        public GsDouDizhuApiService(string baseAddress)
        {
            this.baseAddress = baseAddress;
        }

        public GsDouDizhuApiService(string baseAddress, int timeout)
        {
            this.baseAddress = baseAddress;
            this.timeout = timeout;
        }

        public async Task<IEnumerable<ApiDouDizhuTable>> Tables()
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseAddress);
                httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

                var response = await httpClient.GetAsync("tables");
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<ApiDouDizhuTable>>(responseContent);
            }
        }
    }

    public class ApiDouDizhuTable
    {
        public long TableId { get; set; }

        public string TableName_EN { get; set; }

        public string TableName_CHS { get; set; }

        public string TableName_CHT { get; set; }

        public int BaseValue { get; set; }

        public int SecondsToCountdown { get; set; }
    }
}
