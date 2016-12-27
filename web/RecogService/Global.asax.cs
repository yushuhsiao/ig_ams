using ams;

namespace RecogService
{
    public class Global : _HttpApplication
    {
        public Global()
        {
            typeof(ams.Data.IG01PlatformInfo).ToString();
        }
    }
}