using System;
using System.Net;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public partial class RequestSending : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://kioskpublicapi.redhorse88.com/getPlayerInfo/PPLAYMARTIN");
        request.Method = "GET";
        request.KeepAlive = true;
        string path = Server.MapPath("~/PTCertificate/");

        request.ClientCertificates.Add(new X509Certificate2(path + "play.p12", "0lfJl6Yj"));

        String message = null;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        using (Stream stream = response.GetResponseStream())
        {
            StreamReader sr = new StreamReader(stream, Encoding.UTF8);
            message = sr.ReadToEnd().Trim();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                message += "err: " + response.StatusCode + Environment.NewLine;
            }
        }
        Response.Write(message);
    }
}