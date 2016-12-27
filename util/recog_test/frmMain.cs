using Luxand;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace recog_test
{
    public partial class frmMain : Form
    {
        public static float FaceDetectionThreshold = 3;
        public static float FARValue = 100;

        public frmMain()
        {
            InitializeComponent();
        }

        static string path = @"\\dg05\c$\download";
        static void Main(string[] args)
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            frmMain frmMessage = new frmMain();
            ThreadPool.QueueUserWorkItem((state) =>
            {
                log.message(null, "Initializing Luxand FaceSDK...");
                if (FSDK.FSDKE_OK != FSDK.ActivateLibrary("MvxY1iIQ2Y8kfLoTCCu80f9jY2h1HEGEdmnHEwlR9uEcMKL/l4FE3EHW0ZMpEKxLOoHx8cZui8qQrMzzlu3d1E0iDuPF/nY50HpUGDBlHte1MnCmgu0k8dxIOULinSdfGHThY3wwkjsJlXhMUEzG1VgpsbTUFY+2fuFuV/keJxk="))
                {
                    log.message(null, "Error activating FaceSDK: Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)");
                    return;
                }

                if (FSDK.InitializeLibrary() != FSDK.FSDKE_OK)
                    log.message(null, "Error : Error initializing FaceSDK!");

                //DirectoryInfo dir = new DirectoryInfo(path);
                //DirectoryInfo[] subdir = dir.GetDirectories();
                log.message(null, "Initialized");
                TFaceRecord src = TFaceRecord.enrollFaces(@"\\dg05\c$\download\sample\17\17-1479789697730-.png");
                if (src != null)
                {
                    foreach (var n in new DirectoryInfo(@"\\dg05\c$\download\recog\17").GetFiles())
                    {
                        TFaceRecord dst = TFaceRecord.matchFace(n.FullName);
                        if (dst == null)
                            continue;
                        float nn = src.CompareTo(dst);
                        log.message(null,$@"{nn}
{src.ImageFileName}
{dst.ImageFileName}");
                    }
                }
            });
            System.Windows.Forms.Application.Run(frmMessage);
        }

    }
}