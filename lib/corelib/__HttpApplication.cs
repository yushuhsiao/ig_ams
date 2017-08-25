using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web
{
    public class _HttpApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            base.Init();
            base.AddOnBeginRequestAsync(BeginRequest1, BeginRequest2);
        }

        IAsyncResult BeginRequest1(object sender, EventArgs e, AsyncCallback cb, object extraData)
        {
            return AsyncResult.Completed;
        }
        void BeginRequest2(IAsyncResult ar)
        {
        }
    }
}
