using Newtonsoft.Json;
using System.Reflection;
using System.Threading;

namespace InnateGlory
{
    public class ServerInfoData
    {
        public string Name { get; set; }
        public string Product { get; set; }
        public string Title { get; set; }

        [JsonIgnore]
        private string _json;
        [JsonIgnore]
        public string JsonString
        {
            get
            {
                string result = Interlocked.CompareExchange(ref this._json, null, null);
                if (result == null)
                {
                    result = JsonHelper.SerializeObject(this);
                    Interlocked.Exchange(ref this._json, result);
                }
                return result;
            }
        }

        public ServerInfoData(bool createInfo = false)
        {
            if (createInfo)
            {
                var asm = Assembly.GetEntryAssembly();
                Name = asm.GetName().Name;
                Product = asm.GetCustomAttribute<AssemblyProductAttribute>().Product;
                Title = asm.GetCustomAttribute<AssemblyTitleAttribute>().Title;
            }
        }
    }
}
