using Luxand;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace RecogService
{
    public class FaceData : IDisposable
    {
        public const float FaceDetectionThreshold = 3;
        public const float FARValue = 100;
        public const string log_Msg = "FaceSDK";
        public const string log_Err = "FaceSDK_Error";

        [AppSetting, DefaultValue("MvxY1iIQ2Y8kfLoTCCu80f9jY2h1HEGEdmnHEwlR9uEcMKL/l4FE3EHW0ZMpEKxLOoHx8cZui8qQrMzzlu3d1E0iDuPF/nY50HpUGDBlHte1MnCmgu0k8dxIOULinSdfGHThY3wwkjsJlXhMUEzG1VgpsbTUFY+2fuFuV/keJxk=")]
        static string LicenseKey
        {
            get { return app.config.GetValue<string>(MethodBase.GetCurrentMethod()) ?? ""; }
        }

        public static void Init()
        {
            lock (typeof(FSDK))
            {
                log.message(log_Msg, "FSDK.ActivateLibrary()...");
                if (FSDK.FSDKE_OK != FSDK.ActivateLibrary(LicenseKey))
                {
                    //log.message(FDSK_Error, "Error activating FaceSDK: Please run the License Key Wizard (Start - Luxand - FaceSDK - License Key Wizard)");
                    log.message(log_Err, "FSDK.ActivateLibrary()");
                    return;
                }

                log.message(log_Msg, "FSDK.InitializeLibrary()...");
                if (FSDK.FSDKE_OK != FSDK.InitializeLibrary())
                {
                    log.message(log_Err, "FSDK.InitializeLibrary()");
                    return;
                }
                FSDK.SetFaceDetectionParameters(false, true, 384);
                FSDK.SetFaceDetectionThreshold((int)FaceDetectionThreshold);

                log.message(log_Msg, "Luxand FaceSDK Initialized.");
            }
        }



        FSDK.CImage image;
        FSDK.TFacePosition FacePosition;
        FSDK.CImage faceImage;
        FSDK.TPoint[] FacialFeatures;
        byte[] Template;

        public bool HasImage { get { return this.image != null; } }
        public bool HasDetectFace { get { return this.FacePosition?.w != 0; } }
        public bool HasFaceImage { get { return this.faceImage != null; } }
        public bool HasDetectEyes { get { return this.FacialFeatures != null; } }
        public bool HasTemplate { get { return this.Template != null; } }

        public bool GetTemplate(out byte[] Template)
        {
            lock (typeof(FSDK))
            {
                if ((this.Template == null) && this.HasImage && (this.FacePosition != null))
                {
                    FSDK.TFacePosition FacePosition = this.FacePosition;
                    this.Template = image.GetFaceTemplateInRegion(ref FacePosition);
                }
                Template = this.Template;
                return Template != null;
            }
        }

        public bool GetTemplate(ImageData image)
        {
            if (image == null)
                return false;
            byte[] template;
            if (!this.GetTemplate(out template))
                return false;
            image.template = template;
            return true;
        }

        void IDisposable.Dispose()
        {
            lock (typeof(FSDK))
            {
                using (this.image)
                using (this.faceImage)
                    return;
            }
        }

        public static FaceData Create(ImageData image)
        {
            if (image == null) return null;
            if (image.data != null)
            {
                using (MemoryStream ms = new MemoryStream(image.data))
                    return new FaceData(ms, image.ID) { Template = image.template };
            }
            else if (image.template != null)
                return new FaceData(image.ID) { Template = image.template };
            return null;
        }

        Guid? image_id;
        FaceData(Guid? image_id) { this.image_id = image_id; }
        FaceData(Stream stream, Guid? image_id) : this(image_id)
        {
            lock (typeof(FSDK))
            {
                try
                {
                    using (Image img = Image.FromStream(stream))
                    {
                        this.image = new FSDK.CImage(img);
                        if (!this.HasImage)
                        { return; }

                        this.FacePosition = image.DetectFace();
                        if (!this.HasDetectFace)
                        { log.message(FaceData.log_Msg, $"No faces found. {image_id}"); return; }

                        this.faceImage = image.CopyRect(
                            (int)(FacePosition.xc - Math.Round(FacePosition.w * 0.5)),
                            (int)(FacePosition.yc - Math.Round(FacePosition.w * 0.5)),
                            (int)(FacePosition.xc + Math.Round(FacePosition.w * 0.5)),
                            (int)(FacePosition.yc + Math.Round(FacePosition.w * 0.5)));
                        if (!this.HasFaceImage)
                        { return; }

                        this.FacialFeatures = image.DetectEyesInRegion(ref FacePosition);
                        if (!this.HasDetectEyes)
                        { return; }

                        //this.Template = image.GetFaceTemplateInRegion(ref FacePosition);
                        //if (!this.HasTemplate)
                        //    return;
                    }
                }
                catch (Exception ex)
                {
                    log.message(FaceData.log_Err, $"{ex.Message}");
                }
            }
        }

        public float CompareTo(FaceData dest)
        {
            lock (typeof(FSDK))
            {
                byte[] Template1, Template2;
                if (this.GetTemplate(out Template1) && dest.GetTemplate(out Template2))
                {
                    float Threshold = 0.0f;
                    FSDK.GetMatchingThresholdAtFAR(FaceData.FARValue / 100, ref Threshold);
                    float Similarity = 0.0f;
                    FSDK.MatchFaces(ref Template1, ref Template2, ref Similarity);
                    log.message(log_Msg, $"FSDK.MatchFaces : {Similarity * 100}%, {this.image_id}<=>{dest.image_id}");
                    return Similarity;
                }
            }
            return -1;
        }
    }
}
