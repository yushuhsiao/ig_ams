using mshtml;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace cash.webatm.icbc
{
    //[ComVisible(true), ClassInterface(ClassInterfaceType.None)/*, Guid(icbc._GUID)*/]
    public class icbc_bho : BrowserHelperObject<icbc_site>
    {
    }

    public class icbc_site : cash.webatm.BHO_SITE
    {
        [STAThread]
        static void Main(params string[] args)
        {
            //Uri uri = new Uri("https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602");
            //var x = System.Web.HttpUtility.ParseQueryString(uri.Query);
            icbc_bho._Main(args);
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
        
        protected override void CommandStateChange(int Command, bool Enable)
        {
        }

        protected override void DocumentComplete(InternetExplorer ie, string url)
        {
            base.frmBrowser.StatusText1 = "";
            HTMLWindow2 _top_window = (HTMLWindow2)((HTMLDocument)ie.Document).parentWindow.top;
            if (0 != string.Compare(urls.host, _top_window.document.location.hostname, true))
                return;
            HTMLDocument _top = (HTMLDocument)_top_window.document;
            if (_top.is_page(urls.index_jsp, null))
            {
                HTMLDocument f_indexFrame;
                if (_top.find_page(urls.login_jsp, "indexFrame", out f_indexFrame))
                {
                    HTMLDocument indexicbc_htm, Verifyimage2;
                    if (f_indexFrame.find_page(urls.indexicbc_htm, null, out indexicbc_htm) &&
                        f_indexFrame.find_page(urls.Verifyimage2, null, out Verifyimage2))
                    {
                        this.write_log("type", base.frmBrowser.StatusText1 = "icbc:login");
                        return;
                    }
                }

                // indexFrame.downFrame.leftFrame.onSubleftForm('060298','1')

                HTMLDocument f_topFrame, f_up, f_bottom;
                if (_top.find_page(urls.ICBCINBSEstablishSessionServlet, "indexFrame", out f_indexFrame) &&
                    f_indexFrame.find_page(urls.EBANKPtop_jsp, "topFrame", out f_topFrame) &&
                    f_topFrame.find_page(urls.ebanktop_area_jsp, "up", out f_up) &&
                    f_topFrame.find_page(urls.ICBCINBSReqServlet, "bottom", out f_bottom))
                {
                    HTMLDocument f_downFrame, f_leftFrame, f_bottomserverice, f_mainFrame;
                    if (f_indexFrame.find_page(null, "downFrame", out f_downFrame) &&
                        f_downFrame.find_page(urls.leftframejs_jsp, "leftFrame", out f_leftFrame) &&
                        f_downFrame.find_page(null, "mainFrame", out f_mainFrame) &&
                        f_downFrame.find_page(urls.bottomservice_jsp, "bottomserverice", out f_bottomserverice))
                    {
                        Uri uri = new Uri(f_mainFrame.url);
                        var querystring = System.Web.HttpUtility.ParseQueryString(uri.Query);

                        #region 首頁
                        if (0 == string.Compare(f_downFrame.location.pathname, urls.open_content_jsp, true))
                        {
                            this.write_log("type", base.frmBrowser.StatusText1 = "icbc:open_content");
                            return;
                        }
                        #endregion

                        //Request	GET /servlet/ICBCINBSCenterServlet?id=060298&dse_sessionId=EPAZFTCSDAHVFIGQFQIGCXFBGJCVCFCCDIECCJDE HTTP/1.1

                        #region 3-4     境內匯款查詢

                        if (querystring["id"] == "060298")
                        {
                            HTMLSelectElement 交易類型, 交易種類, 註冊卡號, 下掛帳戶;
                            HTMLInputTextElement 起始日期, 結束日期;
                            if (f_mainFrame.querySelector("form[name=InfoForm] select[name=TranType]", out 交易類型) &&
                                f_mainFrame.querySelector("form[name=InfoForm] select[name=acctSelList]", out 交易種類) &&
                                f_mainFrame.querySelector("form[name=InfoForm] select[name=acctSelList]", out 註冊卡號) &&
                                f_mainFrame.querySelector("form[name=InfoForm] select[name=acctSelList2]", out 下掛帳戶) &&
                                f_mainFrame.querySelector("form[name=InfoForm] input[name=begDate]", out 起始日期) &&
                                f_mainFrame.querySelector("form[name=InfoForm] input[name=endDate]", out 結束日期))
                            {
                                this.write_log("type", base.frmBrowser.StatusText1 = "icbc:境內匯款查詢");
                                return;
                            }
                        }

                        #endregion


                        this.write_log("type", base.frmBrowser.StatusText1 = "icbc:content");
                        return;
                    }
                    // leftFrame        2   https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0101
                    // leftFrame        2-2 https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0101
                    // leftFrame        3-1 https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0601
                    // leftFrame        3-2 https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060299&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602
                    // leftFrame        3-3 https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602
                    // leftFrame        3-4 https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602
                    // leftFrame        4   https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602
                    // leftFrame        5a  https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602
                    // leftFrame        5b  https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602
                    // leftFrame        5c  https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602
                    // leftFrame        5d  https://mybank.icbc.com.cn/icbc/newperbank/includes/leftframejs.jsp?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI#results0602
                    // mainFrame        2   https://mybank.icbc.com.cn/servlet/ICBCINBSReqServlet
                    // mainFrame        2-2 https://mybank.icbc.com.cn/servlet/ICBCINBSReqServlet
                    // mainFrame        3-1 https://mybank.icbc.com.cn/servlet/ICBCINBSCenterServlet?dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // mainFrame        3-2 https://mybank.icbc.com.cn/servlet/ICBCINBSCenterServlet?id=0602&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // mainFrame        3-3 https://mybank.icbc.com.cn/servlet/ICBCINBSCenterServlet?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // mainFrame        3-4 https://mybank.icbc.com.cn/servlet/ICBCINBSCenterServlet?id=060298&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // mainFrame        4   https://mybank.icbc.com.cn/servlet/ICBCINBSReqServlet
                    // mainFrame        5a  https://mybank.icbc.com.cn/servlet/ICBCINBSReqServlet?Tran_flag=1&currserialNo=HQH000000000003163839649&showNum=10&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI&dse_applicationId=-1&dse_operationName=per_RemitExcQueryICBCHistoryOp&dse_pageId=7
                    // mainFrame        5b  https://mybank.icbc.com.cn/servlet/ICBCINBSReqServlet?Tran_flag=1&currserialNo=HQH000000000003163724819&showNum=10&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI&dse_applicationId=-1&dse_operationName=per_RemitExcQueryICBCHistoryOp&dse_pageId=7
                    // mainFrame        5c  https://mybank.icbc.com.cn/servlet/ICBCINBSReqServlet?Tran_flag=1&currserialNo=HQH000000000003163470109&showNum=10&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI&dse_applicationId=-1&dse_operationName=per_RemitExcQueryICBCHistoryOp&dse_pageId=7
                    // mainFrame        5d  https://mybank.icbc.com.cn/servlet/ICBCINBSReqServlet?Tran_flag=1&currserialNo=HQH000000000003163220425&showNum=10&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI&dse_applicationId=-1&dse_operationName=per_RemitExcQueryICBCHistoryOp&dse_pageId=7
                    // bottomserverice  2   https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  2-2 https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  3-1 https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  3-2 https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  3-3 https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  3-4 https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  4   https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  5a  https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  5b  https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  5c  https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                    // bottomserverice  5d  https://mybank.icbc.com.cn/icbc/newperbank/includes/bottomservice.jsp?languagebot=zh_CN&dse_sessionId=GTJLBJCFEWBFFYAPGFIUFUFUEJCKCAEZHCCOBUCI
                }
            }
        }
    }
        

    static class urls
    {
        public const string host = "mybank.icbc.com.cn";
        public const string index_jsp = "/icbc/perbank/index.jsp";
        public const string login_jsp = "/icbc/newperbank/main/login.jsp";
        public const string _1_html = "/icbc/perbank/1.html";
        public const string indexicbc_htm = "/indexicbc.htm";
        public const string Verifyimage2 = "/servlet/com.icbc.inbs.person.servlet.Verifyimage2";
        public const string ICBCINBSEstablishSessionServlet = "/servlet/com.icbc.inbs.servlet.ICBCINBSEstablishSessionServlet";
        public const string EBANKPtop_jsp = "/icbc/newperbank/includes/EBANKPtop.jsp";
        public const string open_content_jsp = "/icbc/newperbank/includes/open_content.jsp";
        public const string ebanktop_area_jsp = "/icbc/newperbank/includes/ebanktop_area.jsp";
        public const string ICBCINBSReqServlet = "/servlet/ICBCINBSReqServlet";
        public const string leftframejs_jsp = "/icbc/newperbank/includes/leftframejs.jsp";
        public const string bottomservice_jsp = "/icbc/newperbank/includes/bottomservice.jsp";
        public const string ICBCINBSChangeMenuServlet = "/servlet/ICBCINBSChangeMenuServlet";
        public const string ICBCINBSCenterServlet = "/servlet/ICBCINBSCenterServlet";
    }

    static class util
    {
        //public static IEnumerable<T> querySelectorAll<T>(this HTMLDocument doc, string v) where T:class
        //{
        //    IHTMLDOMChildrenCollection c = doc.querySelectorAll(v);
        //    for (int i = 0; i < c.length; i++)
        //    {
        //        T n = c.item(i) as T;
        //        if (n == null) continue;
        //        yield return n;
        //    }
        //}

        public static bool find_page(this HTMLDocument doc, string url, string name, out HTMLDocument result)
        {
            if (doc != null)
            {
                for (int i = 0; i < doc.frames.length; i++)
                {
                    object o = i;
                    HTMLWindow2 w = (HTMLWindow2)doc.frames.item(ref o);
                    HTMLDocument doc2 = (HTMLDocument)w.document;
                    if (doc2.is_page(url, name))
                    {
                        result = doc2;
                        return true;
                    }
                }
            }
            result = null;
            return false;
        }
        public static bool is_page(this HTMLDocument doc, string url, string name)
        {
            if (doc != null)
            {
                readyState r = doc.readyState.ToEnum<readyState>() ?? readyState._unknown;
                if (r.In(readyState.interactive, readyState.complete))
                {
                    if ((url == null) && (name == null)) return false;
                    name = name ?? doc.parentWindow.name;
                    url = url ?? doc.location.pathname;
                    if ((0 == string.Compare(name, doc.parentWindow.name, true)) &&
                        (0 == string.Compare(url, doc.location.pathname, true)))
                        return true;
                }
            }
            return false;
        }

        public static bool querySelector<T>(this HTMLDocument doc, string selector, out T result) where T : class
        {
            IHTMLElement e1 = doc.querySelector(selector);
            result = e1 as T;
            return result != null;
        }

        //public static bool match_page(this HTMLWindow2 w, ref HTMLDocument result, string url)
        //{
        //    if (result == null)
        //    {
        //        mshtml.readyState r = w.document.readyState.ToEnum<mshtml.readyState>() ?? mshtml.readyState._unknown;
        //        if (r.In(mshtml.readyState.interactive, mshtml.readyState.complete))
        //        {
        //            if (0 == string.Compare(url, w.document.location.pathname, true))
        //            {
        //                result = (HTMLDocument)w.document;
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}
    }
}

namespace mshtml
{
    public enum readyState
    {
        _unknown,
        uninitialized, loading, loaded, interactive, complete
    }
}
