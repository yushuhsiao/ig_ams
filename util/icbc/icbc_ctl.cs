using mshtml;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace cash.webatm.icbc
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.None)/*, Guid(icbc._GUID)*/]
    [BandObject("webatm.icbc", BandObjectStyle.Horizontal | BandObjectStyle.ExplorerToolbar | BandObjectStyle.TaskbarToolBar, HelpText = "webatm.icbc toolbar")]
    public class icbc_bandobject : BandObjects
    {
        private System.Windows.Forms.ToolStripButton tsMsg;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ToolStripLabel tsPageTime;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsActive;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel tsPageType;
        private ToolStripLabel tsHome;
        private ToolStripLabel tsPageStep;
        private System.Windows.Forms.ToolStrip toolStrip1;

        public icbc_bandobject()
        {
            InitializeComponent();
            while (this.ClientSize.Height > toolStrip1.Height)
                this.Height--;
            while (this.ClientSize.Height < toolStrip1.Height)
                this.Height++;
            tsMsg.Checked = false;
            tsMsg_CheckStateChanged(tsMsg, EventArgs.Empty);
            frmMessage().VisibleChanged += frmMessage_VisibleChanged;
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(icbc_bandobject));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsHome = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsActive = new System.Windows.Forms.ToolStripButton();
            this.tsPageType = new System.Windows.Forms.ToolStripLabel();
            this.tsPageStep = new System.Windows.Forms.ToolStripLabel();
            this.tsPageTime = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsMsg = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsHome,
            this.toolStripSeparator1,
            this.tsActive,
            this.tsPageType,
            this.tsPageStep,
            this.tsPageTime,
            this.toolStripSeparator2,
            this.tsMsg,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(530, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsHome
            // 
            this.tsHome.AutoSize = false;
            this.tsHome.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("tsHome.BackgroundImage")));
            this.tsHome.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tsHome.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.tsHome.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsHome.IsLink = true;
            this.tsHome.LinkVisited = true;
            this.tsHome.Name = "tsHome";
            this.tsHome.Size = new System.Drawing.Size(75, 22);
            this.tsHome.VisitedLinkColor = System.Drawing.Color.Red;
            this.tsHome.Click += new System.EventHandler(this.tsHome_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsActive
            // 
            this.tsActive.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsActive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsActive.Name = "tsActive";
            this.tsActive.Size = new System.Drawing.Size(23, 22);
            this.tsActive.Text = ".";
            this.tsActive.Click += new System.EventHandler(this.tsActive_Click);
            // 
            // tsPageType
            // 
            this.tsPageType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsPageType.Image = ((System.Drawing.Image)(resources.GetObject("tsPageType.Image")));
            this.tsPageType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsPageType.Name = "tsPageType";
            this.tsPageType.Size = new System.Drawing.Size(36, 22);
            this.tsPageType.Text = "none";
            // 
            // tsPageStep
            // 
            this.tsPageStep.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsPageStep.Name = "tsPageStep";
            this.tsPageStep.Size = new System.Drawing.Size(10, 22);
            this.tsPageStep.Text = ".";
            // 
            // tsPageTime
            // 
            this.tsPageTime.Name = "tsPageTime";
            this.tsPageTime.Size = new System.Drawing.Size(43, 22);
            this.tsPageTime.Text = "--:--:--";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsMsg
            // 
            this.tsMsg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsMsg.Image = ((System.Drawing.Image)(resources.GetObject("tsMsg.Image")));
            this.tsMsg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsMsg.Name = "tsMsg";
            this.tsMsg.Size = new System.Drawing.Size(23, 22);
            this.tsMsg.Text = "Message";
            this.tsMsg.CheckStateChanged += new System.EventHandler(this.tsMsg_CheckStateChanged);
            this.tsMsg.Click += new System.EventHandler(this.tsMsg_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // icbc_bandobject
            // 
            this.Controls.Add(this.toolStrip1);
            this.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.IntegralSize = new System.Drawing.Size(100, -1);
            this.Name = "icbc_bandobject";
            this.Size = new System.Drawing.Size(530, 86);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        [STAThread]
        static void Main(params string[] args) { BandObjects._Main(args); }

        protected override void ie_CommandStateChange(int Command, bool Enable)
        {
        }
        protected override void ie_DocumentComplete(InternetExplorer ie, string url)
        {
            base.ie_DocumentComplete(ie, url);
            document_complete = true;
        }

        bool _active = !util.Active;
        bool active
        {
            get { return this._active; }
            set
            {
                if (this._active == value) return;
                this._active = value;
                tsActive.Image = value ? util.ActiveImage1 : util.ActiveImage2;
                tsActive.Text = value ? "on" : "off";
            }
        }

        PageType _page_type;
        PageType page_type
        {
            get { return this._page_type; }
            set
            {
                if (this._page_type == value) return;
                this._page_type = value;
                this.page_step = util.stat_idle;
                this.page_step = util.stat_init;
                this.tsPageType.Text = value.ToString();
            }
        }
        string _page_step;
        string page_step
        {
            get { return this._page_step; }
            set
            {
                if (this._page_step == value) return;
                this._page_step = value;
                this.time_wait = null;
                this.tsPageStep.Text = value;
            }
        }
        DateTime? time_wait;
        TimeSpan? _page_time;
        TimeSpan? page_time
        {
            set
            {
                if (this._page_time == value) return;
                this._page_time = value;
                if (value.HasValue)
                    tsPageTime.Text = value.Value.ToString("hh\\:mm\\:ss");
                else
                    tsPageTime.Text = "";
            }
        }
        bool document_complete;
        void SetState(string state = util.stat_idle, double ms = 0)
        {
            this.page_step = state;
            if (ms == 0)
                time_wait = null;
            else
                time_wait = DateTime.Now.AddMilliseconds(ms);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ie == null) return;
            if (timer1.Tag != null) return;
            try
            {
                timer1.Tag = timer1;
                if (this.active = util.Active)
                    this.page_type = parse_page(ie);
                else
                    this.page_type = PageType.none;
                if (time_wait.HasValue)
                    page_time = time_wait.Value - DateTime.Now;
                else
                    page_time = null;
            }
            catch (Exception ex) { this.writelog("error", ex.ToString()); }
            finally { timer1.Tag = null; }
        }


        PageType parse_page(InternetExplorer ie)
        {
            if (document_complete)
            {
                document_complete = false;
                return PageType.none;
            }
            if (time_wait.HasValue)
            {
                if (DateTime.Now < time_wait.Value)
                    return this.page_type;
                else
                    time_wait = null;
            }
            else if (this.page_step == util.stat_idle)
                return this.page_type;
            HTMLWindow2 _top_window = (HTMLWindow2)((HTMLDocument)ie.Document).parentWindow.top;
            if (0 == string.Compare(urls.host, _top_window.document.location.hostname, true))
            {
                HTMLDocument _top = (HTMLDocument)_top_window.document;
                if (_top.is_page(urls.index_jsp, null))
                {
                    if (parse_login(ie, _top)) return PageType.login;

                    // indexFrame.downFrame.leftFrame.onSubleftForm('060298','1')

                    // 內容頁面主要結構
                    HTMLDocument f_indexFrame, f_topFrame, f_up, f_bottom, f_downFrame, f_leftFrame, f_bottomserverice, f_mainFrame;

                    if (_top.find_page(urls.ICBCINBSEstablishSessionServlet, "indexFrame", out f_indexFrame) &&
                        f_indexFrame.find_page(urls.EBANKPtop_jsp, "topFrame", out f_topFrame) &&
                        f_topFrame.find_page(urls.ebanktop_area_jsp, "up", out f_up) &&
                        f_topFrame.find_page(urls.ICBCINBSReqServlet, "bottom", out f_bottom) &&
                        f_indexFrame.find_page(null, "downFrame", out f_downFrame) &&
                        f_downFrame.find_page(urls.leftframejs_jsp, "leftFrame", out f_leftFrame) &&
                        f_downFrame.find_page(null, "mainFrame", out f_mainFrame) &&
                        f_downFrame.find_page(urls.bottomservice_jsp, "bottomserverice", out f_bottomserverice))
                    {
                        Uri uri = new Uri(f_mainFrame.url);
                        var querystring = System.Web.HttpUtility.ParseQueryString(uri.Query);

                        // 首頁
                        if (0 == string.Compare(f_downFrame.location.pathname, urls.open_content_jsp, true))
                            return PageType.open;

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
                                return PageType.境內匯款查詢;
                            }
                        }

                        #endregion

                        #region url
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
                        #endregion
                    }
                    return PageType.other;
                }
            }
            return PageType.none;
        }

        bool parse_login(InternetExplorer ie, HTMLDocument _top)
        {
            HTMLDocument f_indexFrame;
            if (_top.find_page(urls.login_jsp, "indexFrame", out f_indexFrame))
            {
                HTMLDocument indexicbc_htm, Verifyimage2;
                if (f_indexFrame.find_page(urls.indexicbc_htm, null, out indexicbc_htm) &&
                    f_indexFrame.find_page(urls.Verifyimage2, null, out Verifyimage2))
                {
                    IHTMLElement Verifyimage2_img;
                    Verifyimage2.querySelector("img", out Verifyimage2_img);
                    //string code = base.StoreImage(Verifyimage2_img, "1.bmp"); 
                    
                    HTMLInputTextElement logonCardNum;
                    HTMLObjectElement safeEdit1;
                    HTMLObjectElement KeyPart;
                    if (f_indexFrame.querySelector("input[name=logonCardNum]", out logonCardNum) &&
                        f_indexFrame.querySelector("object[id=safeEdit1]", out safeEdit1) &&
                        f_indexFrame.querySelector("object[id=KeyPart]", out KeyPart))
                    {
                        if (this.page_type != PageType.login)
                            return true;
                        switch (this.page_step)
                        {
                            case util.stat_init:
                                this.SetState("a", 5000);
                                break;
                            case "a":
                                logonCardNum.focus();
                                logonCardNum.click();
                                logonCardNum.value = "chou28litianyi";
                                this.SetState("b", 3000);
                                safeEdit1.focus();
                                //IntPtr hwnd;
                                //var pwd = (f_indexFrame.ownerDocument as IHTMLDocument3).getElementById("safeEdit1") as IHTMLObjectElement;
                                //IOleWindow sp = (IOleWindow)pwd.@object;
                                //sp.GetWindow(out hwnd);

                                foreach (char c in "tiantian88")
                                {
                                    //SendMessageW((int)hwnd, WM_CHAR, (int)c, 0);
                                    SendKeys.SendWait(c.ToString());
                                }
                                
                                break;
                            case "b":
                                this.SetState();
                                KeyPart.focus();
                                base.StoreImage(Verifyimage2_img, "1.bmp");
                                foreach (char c in "1234")
                                {
                                    //SendMessageW((int)hwnd, WM_CHAR, (int)c, 0);
                                    SendKeys.SendWait(c.ToString());
                                }
                                break;
                        }
                        return true;
                    }
                }
            }
            return false;
        }


        enum PageType { none, other, login, open, 境內匯款查詢 }

        private void tsMsg_Click(object sender, EventArgs e)
        {
            tsMsg.Checked ^= true;
        }

        private void tsMsg_CheckStateChanged(object sender, EventArgs e)
        {
            this.frmMessage().Visible = tsMsg.Checked;
        }

        void frmMessage_VisibleChanged(object sender, EventArgs e)
        {
            tsMsg.Checked = frmMessage().Visible;
        }

        private void tsActive_Click(object sender, EventArgs e)
        {
            util.Active ^= true;
        }

        void SetText(ToolStripItem item, string text)
        {
            if (item.Text != text)
                item.Text = text;
        }

        private void tsHome_Click(object sender, EventArgs e)
        {
            if (ie != null)
                ie.Navigate(urls.icbc_home);
        }

    }

    static class urls
    {
        [AppSetting, DefaultValue("https://mybank.icbc.com.cn")]
        public static string icbc_home
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()); }
        }
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
        [AppSetting("Active"), DefaultValue(true)]
        public static bool Active
        {
            [DebuggerStepThrough]
            get { return app.config.GetValue<bool>(MethodBase.GetCurrentMethod()); }
            [DebuggerStepThrough]
            set { app.config.SetValue<bool>(MethodBase.GetCurrentMethod(), value); }
        }

        public const string stat_init = null;
        public const string stat_idle = "";

        public static Image ActiveImage1 = Properties.Resources.green.ToBitmap();
        public static Image ActiveImage2 = Properties.Resources.red.ToBitmap();
        
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
            result = doc.querySelector(selector) as T;
            return result != null;
        }
        public static T querySelector<T>(this HTMLDocument doc, string selector) where T : class
        {
            return doc.querySelector(selector) as T;
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
