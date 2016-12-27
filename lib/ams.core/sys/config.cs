using ams.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ams.Controllers
{
}
//namespace ams.Models
//{
//    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
//    public class ConfigModel : SqlConfig.Row
//    {
//        [JsonProperty, DbImport]
//        public override int CorpID
//        {
//            get { return base.CorpID; }
//            set { base.CorpID = value; }
//        }
//        [JsonProperty, DbImport]
//        public override int PlatformID
//        {
//            get { return base.PlatformID; }
//            set { base.PlatformID = value; }
//        }
//        [JsonProperty, DbImport]
//        public override string Key1
//        {
//            get { return base.Key1; }
//            set { base.Key1 = value; }
//        }
//        [JsonProperty, DbImport]
//        public override string Key2
//        {
//            get { return base.Key2; }
//            set { base.Key2 = value; }
//        }
//        [JsonProperty, DbImport]
//        public override string Value
//        {
//            get { return base.Value; }
//            set { base.Value = value; }
//        }
//        [JsonProperty, DbImport]
//        public override string Description
//        {
//            get; set;
//        }
//    }
//}