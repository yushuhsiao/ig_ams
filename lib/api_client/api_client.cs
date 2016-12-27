using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Linq;
using System.Web;

namespace ams
{
    public partial class api_client
    {
        public string AUTH_SITE { get; set; }
        public string AUTH_USER { get; set; }
        public string API_KEY { get; set; }
        public string BASE_URL { get; set; }



        T invoke<T>(string url, dynamic request, Action<ErrorMessage> onError = null)
        {
            if (url.StartsWith("~")) url = url.Substring(1);
            return api_client.invoke<T>(AUTH_SITE, AUTH_USER, API_KEY, BASE_URL + url, request, onError);
        }
        #region 會員註冊
        /// <param name="UserName">會員帳號</param>
        /// <param name="Password">會員密碼</param>
        /// <param name="AgentName">代理名稱</param>
        /// <param name="NickName">會員暱稱</param>
        /// <param name="Active1">會員激活</param>
        /// <param name="Active2">遊戲激活</param> 
        /// <param name="RegisterIP">註冊IP位置</param> 
        public bool CreateMember(string UserName, string Password = null, string AgentName = null, string NickName = null, bool Active1 = true, bool Active2 = false, string RegisterIP = null, Action<ErrorMessage> onError = null)
        {
            MemberData data = this.invoke<MemberData>("~/Users/Member/add", new
            {
                Password = Password,
                AgentName = AgentName ?? this.AUTH_SITE,
                UserName = UserName,
                NickName = NickName,
                Active1 = Active1,
                Active2 = Active2,
                RegisterIP = RegisterIP,
            }, onError);
            return data != null;
        }
        #endregion
        #region 會員登入
        /// <param name="UserName">會員帳號</param>
        /// <param name="Password">會員密碼</param>
        public bool MemberLogin(string UserName, string Password, Action<ErrorMessage> onError = null)
        {
            ErrorMessage _msg = null;
            this.invoke<dynamic>("~/Users/Member/Login", new { UserName = UserName, Password = Password, }, (msg) =>
            {
                _msg = msg;
                (onError ?? _null_OnError)(msg);
            });
            return _msg == null;
        }
        #endregion
        #region 取得會員個人資料
        /// <param name="UserName">會員帳號</param>
        public MemberDetail GetMemberDetail(string UserName, Action<ErrorMessage> onError = null)
        {
            return this.invoke<MemberDetail>("~/Users/Member/GetDetails", new { UserName = UserName, }, onError);
        }
        #endregion
        #region 設定會員個人資料
        /// <param name="MemberDetail">會員Class</param>

        public MemberDetail SetMemberDetail(MemberDetail values, Action<ErrorMessage> onError = null)
        {
            return this.invoke<MemberDetail>("~/Users/Member/SetDetails", values, onError);
        }
        /// <summary>設定會員個人資料-bool</summary>
        /// <param name="MemberDetail">會員Class</param>
        public bool SetMemberDetail(out MemberDetail result, MemberDetail values, Action<ErrorMessage> onError = null)
        {
            return (result = this.SetMemberDetail(values, onError)) != null;
        }
        #endregion
        #region 修改會員密碼與暱稱
        /// <param name="userId">會員辨識碼</param>
        /// <param name="password">新密碼</param>
        /// <param name="nNickName">新暱稱</param>
        public bool SetMember(int userId, string password = null, string NickName = null, Action<ErrorMessage> onError = null)
        {
            var set = this.invoke<dynamic>("~/Users/Member/set", new
            {
                Id = userId,
                Password = password,
                NickName = NickName
            }, onError);
            return set != null;
        }
        #endregion
        #region 遊戲跳轉
        // 2016.10.31 增加 NickName
        /// <param name="PlatformName">geniusbull, innateglory, agltd</param>
        /// <param name="MemberName"></param>
        /// <param name="Lang"></param>
        /// <param name="Lobby">LiveCasino, TabletopGames, VideoArcade</param>
        /// <param name="RequestIP">發送方IP位置</param>
        public ForwardGameResult ForwardGame(string PlatformName, string MemberName, string NickName = null, string Lang = null, string Lobby = null, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            return this.invoke<ForwardGameResult>("~/Users/Member/game/forward", new
            {
                PlatformName = PlatformName,
                MemberName = MemberName,
                Lang = Lang,
                Lobby = Lobby,
                NickName = NickName,
                RequestIP = RequestIP,
            }, onError);
        }
        #endregion
        #region 帳戶轉點:主帳戶→個別平台帳戶
        /// <param name="PlatformName">平台帳號</param>
        /// <param name="UserName">會員帳號</param>
        /// <param name="amount">點數</param>
        /// <param name="RequestIP">發送方IP位置</param>
        public TranResult PlatformDeposit(string PlatformName, string UserName, decimal amount, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            return this.invoke<TranResult>("~/Users/Member/PlatformDeposit/addx", new
            //return this.invoke<TranResult>("~/Users/Member-PlatformDeposit", new
            {
                PlatformName = PlatformName,
                UserName = UserName,
                Amount1 = amount,
                RequestIP = RequestIP,
            }, onError);
        }
        /// <summary>帳戶轉點:主帳戶→個別平台帳戶(bool)</summary>
        public bool PlatformDeposit(out TranResult result, string PlatformName, string UserName, decimal amount, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            result = this.PlatformDeposit(PlatformName, UserName, amount, RequestIP, onError);
            if (result != null) return result.State == TranState.Finished;
            return false;
        }
        /// <summary>帳戶轉點:主帳戶→個別平台帳戶(管理後臺專用)</summary>
        public object PlatformDepositEx(string PlatformName, string UserName, decimal amount, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            return this.invoke<TranResult>("~/Users/Member/PlatformDeposit/addxx", new
            {
                PlatformName = PlatformName,
                UserName = UserName,
                Amount1 = amount,
                RequestIP = RequestIP,
            }, onError);
        }
        #endregion
        #region 帳戶轉點:個別平台帳戶→主帳戶
        /// <param name="PlatformName">平台帳號</param>
        /// <param name="UserName">會員帳號</param>
        /// <param name="amount">點數</param>
        /// <param name="RequestIP">發送方IP位置</param>
        public TranResult PlatformWithdrawal(string PlatformName, string UserName, decimal amount, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            return this.invoke<TranResult>("~/Users/Member/PlatformWithdrawal/addx", new
            //return this.invoke<TranResult>("~/Users/Member-PlatformWithdrawal", new
            {
                PlatformName = PlatformName,
                UserName = UserName,
                Amount1 = amount,
                RequestIP = RequestIP,
            }, onError);
        }
        /// <summary>帳戶轉點:個別平台帳戶→主帳戶(bool)</summary>
        public bool PlatformWithdrawal(out TranResult result, string PlatformName, string UserName, decimal amount, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            result = this.PlatformWithdrawal(PlatformName, UserName, amount, RequestIP, onError);
            if (result != null) return result.State == TranState.Finished;
            return false;
        }
        /// <summary>帳戶轉點:個別平台帳戶→主帳戶(管理後臺專用)</summary>
        public object PlatformWithdrawalEx(string PlatformName, string UserName, decimal amount, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            return this.invoke<TranResult>("~/Users/Member/PlatformWithdrawal/addxx", new
            {
                PlatformName = PlatformName,
                UserName = UserName,
                Amount1 = amount,
                RequestIP = RequestIP,
            }, onError);
        }
        #endregion
        #region 代理用戶:增加金額限度
        /// <param name="UserName">會員帳號</param>
        /// <param name="amount">點數</param>
        /// <param name="RequestIP">發送方IP位置</param>
        public object MemberBalanceIn(string UserName, decimal amount, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            return this.invoke<TranResult>("~/Users/Member/BalanceIn/addx", new
            {
                UserName = UserName,
                Amount1 = amount,
                RequestIP = RequestIP,
            }, onError);
        }
        #endregion
        #region 代理用戶:減少金額限度
        /// <param name="UserName">會員帳號</param>
        /// <param name="amount">點數</param>
        /// <param name="RequestIP">發送方IP位置</param>
        public object MemberBalanceOut(string UserName, decimal amount, string RequestIP = null, Action<ErrorMessage> onError = null)
        {
            return this.invoke<TranResult>("~/Users/Member/BalanceOut/addx", new
            {
                UserName = UserName,
                Amount1 = amount,
                RequestIP = RequestIP,
            }, onError);
        }
        #endregion
        #region 取得帳戶點數
        /// <param name="username">會員帳號</param>
        /// <param name="PlatformName">平台帳號</param>
        public decimal GetBalance(string username, string platformName = null, Action<ErrorMessage> onError = null)
        {
            dynamic n = this.invoke<dynamic>("~/Users/Member/Balance", new { UserName = username, PlatformName = platformName }, onError);
            if (n != null) return n.Balance;
            return 0;
        }
        #endregion
        #region 取得交易紀錄
        /// <param name="username">會員帳號</param>
        /// /// <param name="LogType">交易類型</param>
        /// <param name="BeginTime">開始時間</param>
        /// <param name="EndTime">結束時間</param>
        /// <param name="page_size">每頁筆數(50~1000)</param>
        /// <param name="page_number">頁碼</param>
        public ListResult<TranLog> GetTranLog(string username, dynamic LogType = null, DateTime? BeginTime = null, DateTime? EndTime = null, int page_size = 50, int page_number = 0, Action<ErrorMessage> onError = null)
        {
            return this.invoke<ListResult<TranLog>>("~/reports/tranlog/list", new
            {
                UserName = username,
                LogType = LogType,
                BeginTime = BeginTime,
                EndTime = EndTime,
                PageSize = page_size,
                PageNumber = page_number,
            }, onError);
        }
        #endregion
        #region 取得遊戲紀錄
        /// <param name="PlatformName"></param>
        /// <param name="UserName">會員帳號</param>
        /// <param name="GameClass">遊戲類別, string/string[]</param>
        /// <param name="GameName">遊戲名稱, string/string[]</param>
        /// <param name="BeginTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="page_size">每頁筆數(50~1000)</param>
        /// <param name="page_number">頁碼</param>
        public ListResult<GameLog> GetGameLog(string UserName, dynamic GameClass = null, dynamic GameName = null, DateTime? BeginTime = null, DateTime? EndTime = null, int page_size = 50, int page_number = 0, Action<ErrorMessage> onError = null)
        {
            if (BeginTime.HasValue)
                BeginTime = BeginTime.Value.ToUniversalTime();
            if (EndTime.HasValue)
                EndTime = EndTime.Value.ToUniversalTime();
            return this.invoke<ListResult<GameLog>>("~/reports/gamelog/list", new
            {
                //PlatformID = new[] { 1,2 },
                UserName = UserName,
                GameClass = GameClass,
                GameName = GameName,
                BeginTime = BeginTime,
                EndTime = EndTime,
                PageSize = page_size,
                PageNumber = page_number,
            }, onError);
        }
        #endregion
        #region 取得會員帳務資訊(非個人詳細資料)
        /// <param name="username">會員帳號</param>
        public MemberData GetMember(string username, Action<ErrorMessage> onError = null)
        {
            var get = this.invoke<MemberData>("~/Users/Member/get", new
            {
                UserName = username
            }, onError);
            return get;
        }
        #endregion
        #region 取得平台狀態
        public ListResult<Platform> PlatformList(Action<ErrorMessage> onError = null)
        {
            return this.invoke<ListResult<Platform>>("~/sys/platforms/list", new { }, onError);
        }
        #endregion
        #region 取得遊戲清單
        public ListResult<GameList> GameList(Action<ErrorMessage> onError = null)
        {
            return this.invoke<ListResult<GameList>>("~/Sys/Games/list", new { }, onError);
        }
        #endregion
        #region 取得平台遊戲清單
        public ListResult<PlatformGames> PlatformGameList(Action<ErrorMessage> onError = null)
        {
            return this.invoke<ListResult<PlatformGames>>("~/Sys/PlatformGames/list", new { }, onError);
        }
        #endregion
        #region 取得公告列表
        public ListResult<Announce> GetAnnounce(Action<ErrorMessage> onError = null)
        {
            return this.invoke<ListResult<Announce>>("~/events/announces/list_current", new { }, onError);
        }
        #endregion
        #region 忘記密碼(未實作)
        public bool ForgetPassword(string UserName, string Tel, Action<ErrorMessage> onError = null)
        {
            ErrorMessage _msg = null;
            /*this.invoke<dynamic>("????", new { UserName = UserName, Tel = Tel, }, (msg) =>
            {
                _msg = msg;
                (onError ?? _null_OnError)(msg);
            });*/
            return _msg == null;
        }
        #endregion
        #region 取得可用的支付項目
        public ListResult<PaymentItem> GetPaymentList(Action<ErrorMessage> onError = null)
        {
            return this.invoke<ListResult<PaymentItem>>("~/Users/PaymentList/list_current", new { }, onError);
        }
        #endregion
        #region 送出第三方支付訊息
        /// <param name="PaymentName">支付帳號</param>
        /// <param name="PaymentType">支付類型 (SunTech_BuySafe, SunTech_24Payment, SunTech_WebATM, SunTech_PayCode)</param>
        /// <param name="UserName">會員帳號</param>
        /// <param name="amount">存款金額</param>
        /// <param name="NotifyUrl">處理網址</param>
        /// <param name="ResultUrl">回傳網址</param>
        // * 參數順序依照作用排列, 請勿隨意更改 PaymentType, PaymentName, ResultUrl, NotifyUrl 改用具名引數
        public ForwardGameResult SubmitPayment(string UserName, decimal amount, string PaymentType = null, string PaymentName = null, string ResultUrl = null, string NotifyUrl = null, string requestIP = null, Action<ErrorMessage> onError = null)
        {
            HttpContext context = HttpContext.Current;
            try { requestIP = requestIP ?? context?.Request.UserHostAddress; }
            catch { }
            var n = this.invoke<PaymentSubmitResult>("~/Users/Member/Payment/add", new
            {
                UserName = UserName,
                Amount1 = amount,
                PaymentType = PaymentType,
                PaymentName = PaymentName,
                ResultUrl = ResultUrl,
                NotifyUrl = NotifyUrl,
                RequestIP = requestIP,
            }, onError);
            if (n != null)
                return n.ForwardData;
            return null;
        }
        #endregion
        #region 送出申訴
        /// <param name="UserName">會員帳號</param>
        /// <param name="platformName">平台名稱</param>
        /// <param name="GameName">會員帳號</param> 
        /// <param name="GroupID">遊戲局號</param>
        /// <param name="text">申訴內容</param>
        /// <param name="requestIP">發送要求IP</param> 
        public AppealLog Appeal(string UserName, string platformName, string GameName, long? GroupID, string text, string requestIP = null, Action<ErrorMessage> onError = null)
        {
            try { requestIP = requestIP ?? HttpContext.Current?.Request.UserHostAddress; }
            catch { }
            var n = this.invoke<AppealLog>("~/Users/Member/Appeal/appeal", new
            {
                UserName = UserName,
                PlatformName = platformName,
                GameName = GameName,
                GroupID = GroupID,
                Text = text,
                RequestIP = requestIP,
            }, onError);
            return n;
        }
        #endregion
        #region 查詢申訴
        /// <param name="UserName">會員帳號</param>
        /// <param name="page_size">每頁筆數(50~1000)</param>
        /// <param name="page_number">頁碼</param> 
        public ListResult<AppealLog> GetAppeal(string UserName, int page_size = 50, int page_number = 0, Action<ErrorMessage> onError = null)
        {
            var n = this.invoke<ListResult<AppealLog>>("~/Users/Member/Appeal/list", new
            {
                UserName = UserName,
                PageSize = page_size,
                PageNumber = page_number
            });
            return n;
        }
        #endregion
        #region 驗證會員代號(未實作)
        /// <param name="nickName">新暱稱</param>
        public bool IsMemberLive(string nickName, Action<ErrorMessage> onError = null)
        {
            ErrorMessage _msg = null;
            /*this.invoke<dynamic>("????", new { UserName = UserName, Tel = Tel, }, (msg) =>
            {
                _msg = msg;
                (onError ?? _null_OnError)(msg);
            });*/
            return _msg == null;
        }
        #endregion
        #region 黑名單查詢
        public List<MemberBlacklist> GetBlackList(string UserName, string PlatformName, Action<ErrorMessage> onError = null)
        {
            var n = this.invoke<List<MemberBlacklist>>("~/Users/Member/BlackList/List", new
            {
                UserName = UserName,
                PlatformName = PlatformName,
            });
            return n;
        }
        #endregion
        #region 新增黑名單帳號
        public bool CreateBlackMember(string UserName, string PlatformName, string BlacklistName, Action<ErrorMessage> onError = null)
        {
            return this.invoke<bool>("~/Users/Member/BlackList/Add", new
            {
                UserName = UserName,
                PlatformName = PlatformName,
                BlacklistName = BlacklistName,
            });
        }
        #endregion
        #region 刪除黑名單帳號
        public bool RemoveBlackList(string UserName, string PlatformName, string BlacklistName, Action<ErrorMessage> onError = null)
        {
            return this.invoke<bool>("~/Users/Member/BlackList/Remove", new
            {
                UserName = UserName,
                PlatformName = PlatformName,
                BlacklistName = BlacklistName,
            });
        }
        #endregion
        #region 會員轉點(未實作)
        public bool ExChange(string UserName, Action<ErrorMessage> onError = null)
        {
            // ~/Users/Member/Exchange/addx
            // ~/Users/Member/Exchange/add
            // ~/Users/Member/Exchange/accept1
            // ~/Users/Member/Exchange/accept2
            // ~/Users/Member/Exchange/delete
            ErrorMessage _msg = null;
            /*this.invoke<dynamic>("????", new { UserName = UserName, Tel = Tel, }, (msg) =>
            {
                _msg = msg;
                (onError ?? _null_OnError)(msg);
            });*/
            return _msg == null;
        }
        #endregion
        #region 修改密碼(未實作)
        /// <param name="UserName">會員帳號</param>
        /// <param name="PasswordType">密碼類別</param>
        /// <param name="Password">新密碼</param> 
        public bool SetPassword(string UserName, string PasswordType, string Password, Action<ErrorMessage> onError = null)
        {
            return false;
        }
        #endregion
        #region 驗證密碼(未實作)
        /// <param name="UserName">會員帳號</param>
        /// <param name="PasswordType">密碼類別</param>
        /// <param name="Password">驗證密碼</param> 
        public bool ValidatePassword(string UserName, string PasswordType, string Password, Action<ErrorMessage> onError = null)
        {
            return false;
        }
        #endregion
        public void test1(Action<ErrorMessage> onError = null)
        {
            ErrorMessage _msg = null;
            this.invoke<dynamic>("~/Sys/GeniusBull/EprobTable/list", new
            {
                PlatformName = "geniusbull",
                GameName = "ASTRALLUCK",
            }, (msg) =>
            {
                _msg = msg;
                (onError ?? _null_OnError)(msg);
            });
            Debugger.Break();
        }

        #region 取消註冊照片(刪除照片)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="ImageKey">'recog', 'sample' or 'action', null for delete all </param>
        /// <returns></returns>
        public bool PhotoUnregister(string UserName, string ImageKey = null) => this.invoke<bool>("~/Users/Member/Photo/Unregister", new { UserName = UserName, ImageKey = ImageKey });
        #endregion
        #region 取得拍照網址
        public class TakePictureUrls
        {
            public string swfUrl;
            public string recognitionUrl;
            public string accessToken;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="client"></param>
            /// <param name="UserName"></param>
            /// <param name="ImageKey">'recog':進大廳前拍照，確認本人與註冊照片是否相符, 'sample':註冊時拍照，提供未來比對的樣本, 'action':註冊時拍照，需要比出正確手勢並由人工驗證</param>
            /// <returns></returns>
            public static TakePictureUrls GetValue(api_client client, string UserName, string ImageKey, bool DeleteExists = false, string TakePictureKey = null) => client.invoke<TakePictureUrls>("~/Users/Member/Photo/GetArguments", new
            {
                UserName = UserName,
                ImageKey = ImageKey,
                DeleteExists = DeleteExists,
                TakePictureKey = TakePictureKey,
            });
        }
        #endregion
        #region 取得圖片網址
        public class ImageUrl
        {
            public string action1;
            public string action2;
            public string recog;
            public string sample;
            public ImageUrl defaultUrl;
            public PictureInformation[] TakePictures;
            public static ImageUrl GetValue(api_client client, string UserName) => client.invoke<ImageUrl>("~/Users/Member/Photo/GetImageUrl", new
            {
                UserName = UserName,
            });

            public static ImageUrl GetGetImageUrls(api_client client, string TakePictureKey) => client.invoke<ImageUrl>("~/Users/Member/Photo/GetImageUrl", new
            {
                TakePictureKey = TakePictureKey,
            });
        }

        #endregion

        #region 比對照片
        /// <summary>
        /// 比對照片
        /// </summary>
        /// <returns>比對工作階段識別碼</returns>
        public string RecogImage(string UserName, Action<ErrorMessage> onError = null)
        {
            return this.invoke<string>("~/Users/Member/Photo/RecogImage", new
            {
                UserName = UserName,
            });
        }
        #endregion
        #region 取得比對結果
        /// <summary>
        /// 取得比對結果
        /// </summary>
        /// <param name="recog_session">比對工作階段識別碼</param>
        /// <returns> -1: busy, n: number of matched</returns>
        public RecogResult GetRecogResult(string recog_session, double? Similarity = null, bool MatchUserDetails = false, Action<ErrorMessage> onError = null)
        {
            return this.invoke<RecogResult>("~/Users/Member/Photo/RecogResult", new
            {
                Session = recog_session,
                Similarity = Similarity,
                MatchUserDetails = MatchUserDetails,
            });
        }
        public class RecogResult
        {
            public Guid Session;
            public bool Finish;
            public int NumberOfMatch;
            public int NumberOfItems;
            public string[] MatchUsers;
            public PictureInformation[] MatchUserDetails;
        }
        #endregion

    }
    public class PictureInformation
    {
        public Guid? ImageID;
        public string ImageType;
        public bool? Success;
        public DateTime? CreateTime;
        public string Url;
        public float? Similarity;
        public double? TTL;
    }

    #region Error Process
    partial class api_client
    {
        static void _null_OnError(ErrorMessage msg) { }

        static T invoke<T>(string corp, string user, string apikey, string url, dynamic request, Action<ErrorMessage> onError = null)
        {
            onError = onError ?? _null_OnError;
            try
            {
                string response_text;
                string request_text = JsonConvert.SerializeObject(request);
                HttpStatusCode ret = invoke(corp, user, apikey, url, request_text, out response_text);
                if (ret == HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<T>(response_text);
                ErrorMessage _errmsg = JsonConvert.DeserializeObject<ErrorMessage>(response_text);
                _errmsg.HttpStatusCode = ret;
                onError(_errmsg);
            }
            catch (Exception ex)
            {
                onError.Invoke(new ErrorMessage() { HttpStatusCode = HttpStatusCode.InternalServerError, Exception = ex });
            }
            return default(T);
        }

        static HttpStatusCode invoke(string corp, string user, string apikey, string url, string request_text, out string response_text)
        {
            DateTime t1 = DateTime.Now;
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("IG-AUTH-SITE", corp);
            request.Headers.Add("IG-AUTH-USER", user);

            using (ToBase64Transform t = new ToBase64Transform())
            using (CryptoStream cs = new CryptoStream(request.GetRequestStream(), t, CryptoStreamMode.Write))
            using (RSAEncryptStream rsa = new RSAEncryptStream(cs, true) { Base64CspBlob = apikey })
            using (StreamWriter sw = new StreamWriter(rsa, Encoding.UTF8))
                sw.Write(request_text);
            Console.WriteLine($"Request : {request_text}");
            HttpWebResponse response = null;
            try { response = (HttpWebResponse)request.GetResponse(); }
            catch (WebException ex) { response = (HttpWebResponse)ex.Response; }
            using (response)
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    response_text = sr.ReadToEnd();
                TimeSpan t2 = DateTime.Now - t1;
                Console.WriteLine($@"Response : {response.StatusCode}, {t2.TotalMilliseconds}ms
{response_text}");
                return response.StatusCode;
            }
        }
    }
    #endregion
    #region Class
    public class MemberBlacklist
    {
        public long Id;
        public int MemberId;
        public int BlacklistId;
        public string BlacklistName;
        public DateTime BlacklistTime;
    }
    #region 錯誤訊息
    public class ErrorMessage
    {
        public HttpStatusCode HttpStatusCode;
        public Exception Exception;
        public int Status;
        public string Message;
        public dynamic Data;
    }
    #endregion
    #region 平台激活
    public class Platform
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        public int ID;
        /// <summary>
        /// 平台類型
        /// </summary>
        public string PlatformType;
        /// <summary>
        /// 平台名稱
        /// </summary>
        public string PlatformName;
        /// <summary>
        /// 是否激活
        /// </summary>
        public string Active;
    }
    #endregion
    #region 會員帳務資料
    public class MemberData
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        public int ID;
        /// <summary>
        /// 所屬公司識別碼
        /// </summary>
        public int CorpID;
        /// <summary>
        /// 所屬上級識別碼
        /// </summary>
        public int ParentID;
        /// <summary>
        /// 帳號
        /// </summary>
        public string UserName;
        /// <summary>
        /// 顯示名稱
        /// </summary>
        public string NickName;
        /// <summary>
        /// 帳號深度
        /// </summary>
        public int Depth;
        /// <summary>
        /// 啟用/停用
        /// </summary>
        public bool Active1;
        /// <summary>
        /// 鎖單
        /// </summary>
        public bool Active2;
        /// <summary>
        /// 總額度
        /// </summary>
        public decimal TotalBalance;
        /// <summary>
        /// 額度1
        /// </summary>
        public decimal Balance1;
        /// <summary>
        /// 額度2
        /// </summary>
        public decimal Balance2;
    }
    #endregion
    #region 會員個人資料
    public class MemberDetail
    {
        public long? ver;
        /// <summary>
        /// 帳號
        /// </summary>
        public string UserName;
        /// <summary>
        /// 真實姓名
        /// </summary>
        public string RealName;
        /// <summary>
        /// 電話
        /// </summary>
        public string Tel;
        /// <summary>
        /// 電子信箱
        /// </summary>
        public string E_Mail;
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday;
        /// <summary>
        /// 地址:國家
        /// </summary>
        public string Country;
        /// <summary>
        /// 地址:洲
        /// </summary>
        public string State;
        /// <summary>
        /// 地址:城市
        /// </summary>
        public string City;
        /// <summary>
        /// 地址:區域
        /// </summary>
        public string District;
        /// <summary>
        /// 地址1
        /// </summary>
        public string Address1;
        /// <summary>
        /// 地址2
        /// </summary>
        public string Address2;
        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string PostalCode;
        /// <summary>
        /// 初始化
        /// </summary>
        public bool Init
        {
            get { return this.ver.HasValue; }
        }
    }
    #endregion
    #region 遊戲列表
    public class GameList
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        public int GameID;
        /// <summary>
        /// 遊戲名稱
        /// </summary>
        public string Name;
        /// <summary>
        /// 遊戲類別
        /// </summary>
        public string GameClass;
        /// <summary>
        /// 平台名稱
        /// </summary>
        public string PlatformName;
    }
    #endregion
    #region 平台遊戲列表
    public class PlatformGames
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        public int OriginalID;
        /// <summary>
        /// 平台名稱
        /// </summary>
        public string PlatformName;
        /// <summary>
        /// 遊戲名稱
        /// </summary>
        public string GameName;
        /// <summary>
        /// 遊戲類別
        /// </summary>
        public string GameClass;
    }
    #endregion
    #region 遊戲跳轉
    public class ForwardGameResult
    {
        /// <summary>
        /// Url, FormPost
        /// </summary>
        public string ForwardType;
        /// <summary>
        /// ForwardType=Url
        /// </summary>
        public string Url;
        /// <summary>
        /// ForwardType=FormPost
        /// </summary>
        public Dictionary<string, string> Body;
    }
    #endregion
    #region 轉點資訊
    public class TranResult
    {
        /// <summary>
        /// 交易識別碼
        /// </summary>
        public Guid TranID;
        /// <summary>
        /// 交易類型
        /// </summary>
        public string LogType;
        /// <summary>
        /// 交易序號
        /// </summary>
        public string SerialNumber;
        /// <summary>
        /// 交易狀態
        /// </summary>
        public TranState State;
        /// <summary>
        /// 交易內容
        /// </summary>
        public TranLog TranLog;
        /// <summary>
        /// 主帳戶金額
        /// </summary>
        public decimal Balance
        {
            get { return TranLog?.Balance ?? 0; }
        }
        /// <summary>
        /// 平台帳戶金額
        /// </summary>
        public decimal PlatformBalance;
    }
    #endregion
    #region 回傳結果清單
    public class ListResult<T>
    {
        /// <summary>
        /// 清單
        /// </summary>
        public List<T> records;
        /// <summary>
        /// 總筆數
        /// </summary>
        public int totalrecords;
    }
    #endregion
    #region 交易紀錄
    public class TranLog
    {
        /// <summary>
        /// 系統序號
        /// </summary>
        public long sn;
        /// <summary>
        /// 帳務資料類型
        /// </summary>
        public string LogType;
        /// <summary>
        /// 所屬公司識別碼
        /// </summary>
        public int CorpID;
        /// <summary>
        /// 所屬公司名稱
        /// </summary>
        public string CorpName;
        /// <summary>
        /// 所屬上級識別碼
        /// </summary>
        public int ParentID;
        /// <summary>
        /// 所屬公司名稱
        /// </summary>
        public string ParentName;
        /// <summary>
        /// 會員識別碼
        /// </summary>
        public int UserID;
        /// <summary>
        /// 會員帳號
        /// </summary>
        public string UserName;
        /// <summary>
        /// 遊戲平台識別碼
        /// </summary>
        public int PlatformID;
        /// <summary>
        /// 遊戲平台名稱
        /// </summary>
        public string PlatformName;
        /// <summary>
        /// 儲值轉點前額度1
        /// </summary>
        public decimal PrevBalance1;
        /// <summary>
        /// 儲值轉點前額度2
        /// </summary>
        public decimal PrevBalance2;
        /// <summary>
        /// 儲值轉點前額度3
        /// </summary>
        public decimal PrevBalance3;
        /// <summary>
        /// 儲值轉點額度1
        /// </summary>
        public decimal Amount1;
        /// <summary>
        /// 儲值轉點額度2
        /// </summary>
        public decimal Amount2;
        /// <summary>
        /// 儲值轉點額度3
        /// </summary>
        public decimal Amount3;
        /// <summary>
        /// 儲值轉點後額度1
        /// </summary>
        public decimal Balance1;
        /// <summary>
        /// 儲值轉點後額度2
        /// </summary>
        public decimal Balance2;
        /// <summary>
        /// 儲值轉點後額度3
        /// </summary>
        public decimal Balance3;
        /// <summary>
        /// 使用貨幣A(郁書更正)
        /// </summary>
        public string CurrencyA;
        /// <summary>
        /// 使用貨幣B(郁書更正)
        /// </summary>
        public string CurrencyB;
        /// <summary>
        /// 使用貨幣X(郁書更正)
        /// </summary>
        public decimal CurrencyX;
        /// <summary>
        /// 交易識別碼
        /// </summary>
        public Guid TranID;
        /// <summary>
        /// 交易流水號
        /// </summary>
        public string SerialNumber;
        /// <summary>
        /// 發送請求方IP
        /// </summary>
        public string RequestIP;
        /// <summary>
        /// 發送請求時間
        /// </summary>
        public DateTime RequestTime;
        /// <summary>
        /// 交易成立時間
        /// </summary>
        public DateTime CreateTime;
        /// <summary>
        /// 交易前總額:PB1+PB2+PB3
        /// </summary>
        public decimal PrevBalance;
        /// <summary>
        /// 交易總額
        /// </summary>
        public decimal Amount;
        /// <summary>
        /// 交易後總額:PB1+PB2+PB3
        /// </summary>
        public decimal Balance;
    }
    #endregion
    #region 遊戲紀錄
    public class GameLog
    {
        /// <summary>
        /// 系統序號
        /// </summary>
        public long sn;
        /// <summary>
        /// 遊戲局號
        /// </summary>
        public long GroupID;
        /// <summary>
        /// 所屬公司識別碼
        /// </summary>
        public int CorpID;
        /// <summary>
        /// 所屬公司名稱
        /// </summary>
        public string CorpName;
        /// <summary>
        /// 所屬上級識別碼
        /// </summary>
        public int ParentID;
        /// <summary>
        /// 所屬公司名稱
        /// </summary>
        public string ParentName;
        /// <summary>
        /// 會員識別碼
        /// </summary>
        public int UserID;
        /// <summary>
        /// 會員帳號
        /// </summary>
        public string UserName;
        /// <summary>
        /// 帳號深度
        /// </summary>
        public int Depth;
        /// <summary>
        /// 遊戲平台識別碼
        /// </summary>
        public int PlatformID;
        /// <summary>
        /// 遊戲平台名稱
        /// </summary>
        public string PlatformName;
        /// <summary>
        /// 遊戲類型
        /// </summary>
        public string GameClass;
        /// <summary>
        /// 遊戲識別碼
        /// </summary>
        public int GameID;
        /// <summary>
        /// 遊戲名稱
        /// </summary>
        public string GameName;
        /// <summary>
        /// 遊戲開始時間
        /// </summary>
        public DateTime PlayStartTime;
        /// <summary>
        /// 遊戲結束時間
        /// </summary>
        public DateTime PlayEndTime;
        /// <summary>
        /// 有效投注
        /// </summary>
        public decimal BetAmount;
        /// <summary>
        /// 總投注額
        /// </summary>
        public decimal TotalBetAmount;
        /// <summary>
        /// 獲利額度
        /// </summary>
        public decimal WinAmount;
        /// <summary>
        /// 實際獲利:TotalBetAmount-WinAmount=Amount;
        /// </summary>
        public decimal Amount;
        /// <summary>
        /// 使用貨幣A(郁書更正)
        /// </summary>
        public string CurrencyA;
        /// <summary>
        /// 使用貨幣B(郁書更正)
        /// </summary>
        public string CurrencyB;
        /// <summary>
        /// 使用貨幣X(郁書更正)
        /// </summary>
        public decimal CurrencyX;
        /// <summary>
        /// 紀錄成立時間
        /// </summary>
        public DateTime CreateTime;
    }
    #endregion
    #region 交易狀態
    public enum TranState
    {
        Rejected = 0,//拒絕
        Finished = 1//成功
    }
    #endregion
    #region 交易類型
    public enum LogType
    {
        //BalanceIn,//存款(暫時代替)
        //BalanceOut,//提款
        PaymentAPI,
        PlatformWithdrawal,//平台帳戶→主帳戶
        PlatformDeposit//主帳戶→平台帳戶
    }
    #endregion
    #region 公告
    public class Announce
    {
        /// <summary>
        /// 內文
        /// </summary>
        public string Text;
        /// <summary>
        /// 排列優先層級
        /// </summary>
        public int? Order;
        /// <summary>
        /// 新增時間
        /// </summary>
        public DateTime CreateTime;
    }
    #endregion
    #region 第三方支付
    public class PaymentItem
    {
        /// <summary>
        /// 支付帳號
        /// </summary>
        public string Name;
        /// <summary>
        /// 廠商名稱
        /// </summary>
        public string PaymentProvider;
        /// <summary>
        /// 付款方式
        /// </summary>
        public string PaymentType;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active;
        
    }
    #endregion
    #region 第三方支付跳轉
    public class PaymentSubmitResult
    {
        public ForwardGameResult ForwardData;
    }
    #endregion
    #region 申訴Log
    public class AppealLog
    {
        /// <summary>
        /// 申訴識別碼
        /// </summary>
        public Guid ID;
        /// <summary>
        /// 申訴案件序號
        /// </summary>
        public string SerialNumber;
        /// <summary>
        /// 所屬公司識別碼
        /// </summary>
        public int CorpID;
        /// <summary>
        /// 所屬公司名稱
        /// </summary>
        public string CorpName;
        /// <summary>
        /// 會員識別碼
        /// </summary>
        public int UserID;
        /// <summary>
        /// 會員帳號
        /// </summary>
        public string UserName;
        /// <summary>
        /// 遊戲平台識別碼
        /// </summary>
        public int PlatformID;
        /// <summary>
        /// 遊戲平台名稱
        /// </summary>
        public string PlatformName;
        /// <summary>
        /// 遊戲識別碼
        /// </summary>
        public int GameID;
        /// <summary>
        /// 遊戲名稱
        /// </summary>
        public string GameName;
        /// <summary>
        /// 遊戲局號
        /// </summary>
        public long GroupID;
        /// <summary>
        /// 申訴處理狀態
        /// </summary>
        public AppealState State;
        /// <summary>
        /// 發送請求時間
        /// </summary>
        public DateTime RequestTime;
    }
    #endregion
    #region 申訴狀態
    public enum AppealState : byte
    {
        /// <summary>
        /// 新申請
        /// </summary>
        New,
        /// <summary>
        /// 已受理
        /// </summary>
        Accepted,
        /// <summary>
        /// 申訴駁回
        /// </summary>
        Rejected,
        /// <summary>
        /// 完成
        /// </summary>
        Finished,
    }
    #endregion

    #endregion
}
#region System.Security.Cryptography
namespace System.Security.Cryptography
{
    abstract class RSAStream : Stream
    {
        protected Stream _s1;
        protected Stream s1;
        protected MemoryStream s2;
        protected RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

        public string XmlString { get; set; }
        public byte[] CspBlob { get; set; }
        public string Base64CspBlob
        {
            get { if (this.CspBlob == null) return null; return Convert.ToBase64String(this.CspBlob); }
            set { if (value == null) this.CspBlob = null; else this.CspBlob = Convert.FromBase64String(value); }
        }
        public RSAParameters? Parameter { get; set; }

        protected RSAStream(Stream stream, bool leaveOpen = false)
        {
            this.s1 = stream;
            this.s2 = new MemoryStream();
            if (!leaveOpen)
                _s1 = s1;
        }

        public override bool CanRead
        {
            get { return s1.CanRead; }
        }

        public override bool CanSeek
        {
            get { return s1.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return s1.CanWrite; }
        }

        public override long Length
        {
            get { return s1.Length; }
        }

        public override long Position
        {
            get { return s1.Position; }
            set { s1.Position = value; }
        }

        public override void Close()
        {
            using (_s1)
            using (s2)
                base.Close();
        }

        public override void Flush()
        {
            s1.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return s1.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            s1.SetLength(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return s1.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            s1.Write(buffer, offset, count);
        }
    }

    class RSAEncryptStream : RSAStream
    {
        public RSAEncryptStream(Stream stream, bool leaveOpen = false) : base(stream, leaveOpen) { }

        public override void Write(byte[] buffer, int offset, int count)
        {
            s2.Write(buffer, offset, count);
        }

        public override void Flush()
        {
            using (RSACryptoServiceProvider rsa = Interlocked.Exchange(ref base.rsa, null))
            {
                if (this.XmlString != null)
                    rsa.FromXmlString(this.XmlString);
                else if (this.CspBlob != null)
                    rsa.ImportCspBlob(this.CspBlob);
                else if (this.Parameter.HasValue)
                    rsa.ImportParameters(this.Parameter.Value);
                s2.Flush();
                s2.Position = 0;
                rsa.Encrypt(s2, s1);
            }
        }
    }

    static class Crypto
    {
        public static string MD5(this string input)
        {
            return MD5(input, null);
        }
        public static string MD5(this string input, Encoding encoding)
        {
            return Convert.ToBase64String(MD5((encoding ?? Encoding.UTF8).GetBytes(input)));
        }
        public static byte[] MD5(this byte[] input)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                return md5.ComputeHash(input);
        }
        public static string MD5Hex(this string input)
        {
            return MD5Hex(input, null);
        }
        public static string MD5Hex(this string input, Encoding encoding)
        {
            byte[] data = MD5((encoding ?? Encoding.UTF8).GetBytes(input));
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                s.AppendFormat("{0:x2}", data[i]);
            return s.ToString();
        }

        public static void Encrypt(this RSACryptoServiceProvider rsa, byte[] input, Stream output)
        {
            int blockSize = rsa.KeySize / 8 - 11;
            int offset = 0;
            while (offset < input.Length)
            {
                int tmp_size = input.Length - offset;
                if (tmp_size > blockSize)
                    tmp_size = blockSize;
                byte[] tmp = new byte[tmp_size];
                Array.Copy(input, offset, tmp, 0, tmp_size);
                byte[] tmp_enc = rsa.Encrypt(tmp, false);
                output.Write(tmp_enc, 0, tmp_enc.Length);
                offset += tmp_size;
            }
            output.Flush();
        }
        public static void Decrypt(this RSACryptoServiceProvider rsa, byte[] input, Stream stream)
        {
            int keySize = rsa.KeySize / 8;
            int offset = 0;
            while (offset < input.Length)
            {
                int tmp_size = input.Length - offset;
                if (tmp_size > keySize)
                    tmp_size = keySize;
                byte[] tmp = new byte[tmp_size];
                Array.Copy(input, offset, tmp, 0, tmp_size);
                byte[] tmp_dec = rsa.Decrypt(tmp, false);
                stream.Write(tmp_dec, 0, tmp_dec.Length);
                offset += tmp_size;
            }
            stream.Flush();
        }

        public static void Encrypt(this RSACryptoServiceProvider rsa, Stream input, Stream output)
        {
            int blockSize = rsa.KeySize / 8 - 11;
            while (input.Position < input.Length)
            {
                int tmp_size = (int)(input.Length - input.Position);
                if (tmp_size > blockSize)
                    tmp_size = blockSize;
                byte[] tmp = new byte[tmp_size];
                input.Read(tmp, 0, tmp_size);
                byte[] tmp_enc = rsa.Encrypt(tmp, false);
                output.Write(tmp_enc, 0, tmp_enc.Length);
            }
            output.Flush();
        }
        public static void Decrypt(this RSACryptoServiceProvider rsa, Stream input, Stream stream)
        {
            int keySize = rsa.KeySize / 8;
            for (;;)
            {
                byte[] tmp = new byte[keySize];
                int n = input.Read(tmp, 0, keySize);
                if (n == 0) break;
                if (n != keySize) Array.Resize(ref tmp, n);
                byte[] tmp_dec = rsa.Decrypt(tmp, false);
                stream.Write(tmp_dec, 0, tmp_dec.Length);
            }
            stream.Flush();
            //while (input.Position < input.Length)
            //{
            //    int tmp_size = (int)(input.Length - input.Position);
            //    if (tmp_size > keySize)
            //        tmp_size = keySize;
            //    byte[] tmp = new byte[tmp_size];
            //    input.Read(tmp, 0, tmp.Length);
            //    byte[] tmp_dec = rsa.Decrypt(tmp, false);
            //    stream.Write(tmp_dec, 0, tmp_dec.Length);
            //}
            //stream.Flush();
        }

#if !NET20
        public static AesCryptoServiceProvider AES = new AesCryptoServiceProvider();
#endif
        public static DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
        public static TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();

        public static byte[] Encrypt<T>(this T provider, string input, string password, string salt, Encoding encoding) where T : SymmetricAlgorithm
        {
            encoding = encoding ?? Encoding.UTF8;
            return provider.Encrypt<T>(encoding.GetBytes(input), password, encoding.GetBytes(salt));
        }
        public static byte[] Encrypt<T>(this T provider, byte[] input, string password, byte[] salt) where T : SymmetricAlgorithm
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
            using (MemoryStream ms = new MemoryStream())
            using (ICryptoTransform transform = provider.CreateEncryptor(rfc.GetBytes(provider.KeySize / 8), rfc.GetBytes(provider.BlockSize / 8)))
            using (CryptoStream encryptor = new CryptoStream(ms, transform, CryptoStreamMode.Write))
            {
                encryptor.Write(input, 0, input.Length);
                encryptor.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        public static string Decrypt<T>(this T provider, byte[] input, string password, string salt, Encoding encoding) where T : SymmetricAlgorithm
        {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetString(provider.Decrypt<T>(input, password, encoding.GetBytes(salt)));
        }
        public static byte[] Decrypt<T>(this T provider, byte[] input, string password, byte[] salt) where T : SymmetricAlgorithm
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt);
            using (MemoryStream ms = new MemoryStream())
            using (ICryptoTransform transform = provider.CreateDecryptor(rfc.GetBytes(provider.KeySize / 8), rfc.GetBytes(provider.BlockSize / 8)))
            using (CryptoStream decryptor = new CryptoStream(ms, transform, CryptoStreamMode.Write))
            {
                decryptor.Write(input, 0, input.Length);
                decryptor.FlushFinalBlock();
                return ms.ToArray();
            }
        }
    }
}
#endregion