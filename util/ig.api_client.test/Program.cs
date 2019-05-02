#define ig02
using InnateGlory;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

namespace api_test_client
{
    static class Program
    {
        static void Main(string[] args)
        {
            api_client api = new api_client()
            {
#if ig02
                AUTH_SITE = "ig02",
                AUTH_USER = "_website",
                API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQAZLz/8Gl9LLTnS8KzceC+Y4bHdplgcyCzLsE17L1du8/P8g20Y9w3hCoiy63ziIyshig2eOjpQZfm1b7F+5YUUURuOTlAU552a0+U4Js9BVEh5PLUHmkqUULv+paXpIjC98HweAuOX4EBZI6w9riwgErz3Q9Dv1ddgMJUbka7QwA==",
#elif ig07
                AUTH_SITE = "ig07",
                AUTH_USER = "_api_user",
                API_KEY = "BgIAAACkAABSU0ExAAQAAAEAAQArS1TqSr1Te3J5iaSDzERfjyhFfpNrTYkNAmyyQkK7k0spsJ9CWuOKlJM4j9kFWZrqJK9rOsY0GQVOitGgIa5uVeZAGacsL3G8T7jXHN2Xv5tbkUCULwErJImJC7GcYXSSt9KxjLW9Elpe4lOazrnJfJ0X+OoX52tegbjGhN89qQ==",
#endif
                BASE_URL = "http://127.0.0.1:7011",
            };

            object n = null;
            //n = api.SubmitPayment("aaa568", amount: 1000, PaymentType: "SunTech_WebATM");
            //n = api.AcceptPayment("549F0C90-359C-4587-81B0-53B4391DD886");
            //n = api.GetWaitUserCount();
            //n = api.CreateMember("test38");
            //n = api.PlatformDepositEx("appeal", "test37", 100);
            //n = api_client.ImageUrl.GetValue(api, "ro123123");
            //string recog_id;
            //recog_id = api.RecogImage("aa0002");
            n = api_client.ImageUrl.GetValue(api, "yushu111");
            //recog_id = "c9683e10-9b08-48a0-bcf6-72d87d4dd6a1"; //"45d7d9ac-8d9a-48ec-978d-43b900881458";//"3B34EA49-EDC3-4A39-9B20-98E0DD8322FB";
            //n = api.GetRecogResult(recog_id, MatchUserDetails: true);
            //n = api.GetRecogResult(recog_id);
            //n = api.GetBlackList("andy168", "geniusbull");
            n = api_client.TakePictureUrls.GetValue(api, "yushu", "sample", true);
            //n = api.PhotoUnregister("yushu");
            //n = api.GetBalance("yuko999", "appeal");
            //n = api.GetBlackList("aaa568", "geniusbull");
            //n = api.CreateBlackMember("aaa568", "geniusbull", "nn1313");
            //n = api.GetBlackList("aaa568", "geniusbull");
            //n = api.RemoveBlackList("aaa568", "geniusbull", "nn1313");
            //n = api.GetBlackList("aaa568", "geniusbull");
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
            //n = api.PlatformWithdrawal("geniusbull", "m001", 100);
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
            //var nn = api.ForwardGame("geniusbull", "dev07", PhotoRegistered: false);
            //n = api.PlatformDeposit("geniusbull", "yushu", 2, onError: (msg) => { Debugger.Break(); });
            //n = api.PlatformWithdrawal("geniusbull", "yushu", 1, onError: (msg) => { Debugger.Break(); });
            //var n1 = api.GetTranLog("yushu");
            //var n2 = api.GetGameLog("yushu", "geniusbull");
            //var n5 = api.GetMemberDetail("yushu");
            delay();
        }

        [DebuggerStepThrough]
        static void delay(double time = 10)
        {
            for (DateTime t1 = DateTime.Now; ;)
            {
                TimeSpan t2 = DateTime.Now - t1;
                if (t2.TotalSeconds > time)
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

        static void Main1(string[] args)
        {
            string n = @"[ {  Id: 1220,  PlayerName: ""c469092f5f224ac08c9cc16261a28fa0"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:3:1"" } {  Id: 1337,  PlayerName: ""db1cb4d58bde42b89845695fb2229878"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:4:14"" } {  Id: 1202,  PlayerName: ""04774cc9b5dc4daa8c5d70ac15d23990"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:26:43"" } {  Id: 14,  PlayerName: ""2ffdd9c5efe64566b91ec7c599d15659"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:26:45"" } {  Id: 1316,  PlayerName: ""148f2bf778fc49459a98bc7a8544ef85"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:22:36"" } {  Id: 16,  PlayerName: ""9ebc73af83eb4eb08af693ddd70c14e0"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:26:46"" } {  Id: 5,  PlayerName: ""844bf5eb9d984e0b83e0bbd73d7b8486"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:21:23"" } {  Id: 1314,  PlayerName: ""95baae6143824c129fe4e8e7a38c0d38"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:23:14"" } {  Id: 1339,  PlayerName: ""63432b49b00841a59c6d43a1d89b61a0"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:26:46"" } {  Id: 26,  PlayerName: ""07b2936f2a63431081adc3c8c8fcc24b"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:23:47"" } {  Id: 1288,  PlayerName: ""458fb26b122943b6ae48a765016a291d"",  GameId: 1093,  GameName: ""TWMAHJONGVIDEO"",  LoginIp: ""10.10.10.254"",  LoginTime: ""2017 / 2 / 9 2:34:8"" }]";
            StringBuilder s1 = new StringBuilder(n);
            for (int i = 0; i < s1.Length; i++)
            {
                char c = s1[i];
                if (c == '}')
                {
                    Debugger.Break();
                    bool n1 = false;
                    i++;
                    for (; i < s1.Length; i++)
                    {
                        c = s1[i];
                        n1 |= c == ',';
                        if (c == '{')
                        {
                            if (n1 == false)
                                s1.Insert(i, ',');
                            Debugger.Break();
                            break;
                        }
                    }
                }
            }
            n = s1.ToString();
            var nn = JsonConvert.DeserializeObject(n);

            Debugger.Break();
        }
    }
}