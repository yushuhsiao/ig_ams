using ams;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Threading;

namespace api_test_client
{
    static class Program
    {
        static void Main(string[] args)
        {
            api_client api = new api_client()
            {
                AUTH_SITE = "ig02",
                AUTH_USER = "_website",
                API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQAZLz/8Gl9LLTnS8KzceC+Y4bHdplgcyCzLsE17L1du8/P8g20Y9w3hCoiy63ziIyshig2eOjpQZfm1b7F+5YUUURuOTlAU552a0+U4Js9BVEh5PLUHmkqUULv+paXpIjC98HweAuOX4EBZI6w9riwgErz3Q9Dv1ddgMJUbka7QwA==",
                //AUTH_SITE = "ig07",
                //AUTH_USER = "_api_user",
                //API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQArS1TqSr1Te3J5iaSDzERfjyhFfpNrTYkNAmyyQkK7k0spsJ9CWuOKlJM4j9kFWZrqJK9rOsY0GQVOitGgIa5uVeZAGacsL3G8T7jXHN2Xv5tbkUCULwErJImJC7GcYXSSt9KxjLW9Elpe4lOazrnJfJ0X+OoX52tegbjGhN89qQ==",
                BASE_URL = "http://ams.betis73168.com:7001",
            };
            
            object n = null;
            //n = api.GetWaitUserCount();
            //n = api.CreateMember("test38");
            //n = api.PlatformDepositEx("appeal", "test37", 100);
            //n = ams.api_client.ImageUrl.GetValue(api, "yushu");
            //Debugger.Break();
            //string recog_id;
            //recog_id = api.RecogImage("aa0002");
            //n = ams.api_client.ImageUrl.GetValue(api, "yushu111");
            //recog_id = "45d7d9ac-8d9a-48ec-978d-43b900881458";//"3B34EA49-EDC3-4A39-9B20-98E0DD8322FB";
            //Debugger.Break();
            //n = api.GetRecogResult(recog_id, MatchUserDetails: true);
            //n = api.GetRecogResult(recog_id);
            //n = api.GetBlackList("andy168", "geniusbull");
            n = api_client.TakePictureUrls.GetValue(api, "yushu", "sample", true);
            Debugger.Break();
            //n = api.PhotoUnregister("yushu");
            //n = api.GetBalance("aaa568", "agltd");
            //Debugger.Break();
            //n = api.GetBlackList("aaa568", "geniusbull");
            //Debugger.Break();
            //n = api.CreateBlackMember("aaa568", "geniusbull", "nn1313");
            //n = api.GetBlackList("aaa568", "geniusbull");
            //Debugger.Break();
            //n = api.RemoveBlackList("aaa568", "geniusbull", "nn1313");
            //n = api.GetBlackList("aaa568", "geniusbull");
            //Debugger.Break();
            //n = api.GetBlackList("yushu", "geniusbull");
            //n = api.RemoveBlackList("yushu", "geniusbull", "yushu1");
            //n = api.GetBalance("yushu", "agltd");
            //n = api.GetGameLog("aaa123456", GameClass: "EGame", GameName: "MONSTER", BeginTime: new DateTime(2016, 9, 1), EndTime: new DateTime(2016, 10, 1));
            //n = api.PlatformDeposit("geniusbull", "test", 100);
            //n = api.PlatformWithdrawal("geniusbull", "test", 100);
            //api.test1();
            //n = api.GetTranLog("yushu", BeginTime: DateTime.Now, EndTime: DateTime.Now.AddMonths(-1));
            //n = api.Appeal("a123123", "geniusbull", "MahjongTW", 13765, "abcdefg");
            //n = api.MemberLogin("yushu", "12345");
            //n = api.GetGameLog("test2", BeginTime: DateTime.Now.AddDays(-2), EndTime: DateTime.Now);
            //n = api.GetPaymentList();
            //n = api.GetBalance("m001", "innateglory");
            //n = api.MemberBalanceIn("yushu", 100);
            //n = api.MemberBalanceOut("yushu", 100);
            //n = api.PlatformDeposit("geniusbull", "m001", 100);
            //Debugger.Break();
            //n = api.PlatformWithdrawal("geniusbull", "m001", 100);
            //Debugger.Break();
            //n = api.GetPaymentList();
            //n = api.GetAnnounce();
            //n = api.GetGameLog("a12166667", GameClass: "Poker", GameName: new[] { "TWMAHJONGVIDEO", "SICBOBASIC" }, BeginTime: DateTime.Now.AddDays(-10), EndTime: DateTime.Now);
            //n = api.GetBalance("yushu");
            //Console.WriteLine(JsonConvert.SerializeObject(n));
            //var d = api.GetMemberDetail("test2");
            //d = api.SetMemberDetail(new MemberDetail() { UserName = "test2", Address1 = "a", Address2 = "b", District = "xxx" });
            //d=api.SetMember
            //Console.ReadKey();
            //dynamic response;
            //response = api.GetBalance("yushu");
            //var nn = api.CreateMember("test28");
            //var nn = api.GetBalance("yushu");
            //n = api.MemberLogin("yushu", "12345");
            //var nn = api.ForwardGame("geniusbull", "yushu");
            //n = api.PlatformDeposit("geniusbull", "yushu", 2, onError: (msg) => { Debugger.Break(); });
            //n = api.PlatformWithdrawal("geniusbull", "yushu", 1, onError: (msg) => { Debugger.Break(); });
            //var n1 = api.GetTranLog("yushu");
            //var n2 = api.GetGameLog("yushu", "geniusbull");
            //var n5 = api.GetMemberDetail("yushu");
            for (DateTime t1 = DateTime.Now; ;)
            {
                TimeSpan t2 = DateTime.Now - t1;
                if (t2.TotalSeconds > 10)
                    break;
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.KeyChar == ' ')
                    {
                        Console.ReadKey();
                        break;
                    }
                }
            }
        }
    }
}