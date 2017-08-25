using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ams.Models;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Configuration;
using System.Reflection;
using System.ComponentModel;

namespace ams.Data
{
    [PlatformInfo(PlatformType = PlatformType.InnateGloryC)]
    class IG03PlatformInfo : PlatformInfo<IG03PlatformInfo, IG03MemberPlatformData>
    {
        [SqlSetting(CorpID = 0, Key1 = "", Key2 = "ApiUrl"), DefaultValue("http://192.168.5.82:8888/api")]
        public string ApiUrl
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod(), this.ID); }// GetConfig(0, this.ID, "", "ApiUrl") ?? "http://192.168.5.82:8888/api"; }
        }

        public override bool Deposit(MemberData member, decimal amount, out decimal balance, bool force)
        {
            return base.Deposit(member, amount, out balance, force);
        }

        public override bool Withdrawal(MemberData member, decimal amount, out decimal balance, bool force)
        {
            return base.Withdrawal(member, amount, out balance, force);
        }

        protected override IG03MemberPlatformData CreateMember(MemberData member)
        {
            return base.CreateMember(member);
        }

        public override bool GetBalance(MemberData member, out decimal balance)
        {
            balance = 0;
            return true;
            var r = this.invoke<ResponseData>($"{ApiUrl}/GetBalance", new
            {
                userID = member.UserName
            });
            if (r == null) return _null.noop(false, out balance);
            balance = r.Result;
            return r.StatusCode == Status.Success;
            //return base.GetBalance(member, out balance);
        }

        public override ForwardGameArguments ForwardGame(_ApiController c, ForwardGameArguments args)
        {
            return base.ForwardGame(c, args);
        }



        T invoke<T>(string url, dynamic request_obj)
        {
            string request_text = JsonConvert.SerializeObject(request_obj);
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            using (StreamWriter sw = new StreamWriter(request.GetRequestStream()))
                sw.Write(request_text);
            HttpWebResponse response = null;
            string response_text;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex) { response = (HttpWebResponse)ex.Response; }
            using (response)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    response_text = sr.ReadToEnd();
                log.message("api.agltd", $@"{url}
{request_text}
{response_text}");
                if (response.StatusCode == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<T>(response_text);
            }
            return default(T);
        }

        public class ResponseData
        {
            public Status StatusCode { get; set; }
            public string Message { get; set; }
            public dynamic Result { get; set; }
        }
    }
    [TableName("MemberPlatform_IG03")]
    class IG03MemberPlatformData : MemberPlatformData
    {
    }
}