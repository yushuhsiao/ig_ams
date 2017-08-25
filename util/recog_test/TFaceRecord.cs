using Luxand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace recog_test
{
    public class TFaceRecord
    {
        public byte[] Template; //Face Template;
        public FSDK.TFacePosition FacePosition;
        public FSDK.TPoint[] FacialFeatures; //Facial Features;

        public string ImageFileName;

        public FSDK.CImage image;
        public FSDK.CImage faceImage;



        public static float FaceDetectionThreshold = 3;
        public static float FARValue = 100;

        public static TFaceRecord enrollFaces(string fn)
        {
            log.message(null, $"Enrolling {fn}");
            try
            {
                //Assuming that faces are vertical (HandleArbitraryRotations = false) to speed up face detection
                FSDK.SetFaceDetectionParameters(false, true, 384);
                FSDK.SetFaceDetectionThreshold((int)FaceDetectionThreshold);

                TFaceRecord fr = new TFaceRecord();
                fr.ImageFileName = fn;
                fr.FacePosition = new FSDK.TFacePosition();
                fr.FacialFeatures = new FSDK.TPoint[2];
                fr.Template = new byte[FSDK.TemplateSize];
                fr.image = new FSDK.CImage(fn);

                //textBox1.Text += "Enrolling '" + fn + "'\r\n";
                //textBox1.Refresh();
                fr.FacePosition = fr.image.DetectFace();

                if (0 == fr.FacePosition.w)
                {
                    log.message(null, $"{fn}: No faces found. Try to lower the Minimal Face Quality parameter in the Options dialog box.\r\n");
                    return null;
                }
                fr.faceImage = fr.image.CopyRect((int)(fr.FacePosition.xc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.xc + Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc + Math.Round(fr.FacePosition.w * 0.5)));

                try
                {
                    fr.FacialFeatures = fr.image.DetectEyesInRegion(ref fr.FacePosition);
                }
                catch (Exception ex2)
                {
                    log.message(null, $"{ex2.Message}, Error detecting eyes.");
                    return null;
                }

                try
                {
                    fr.Template = fr.image.GetFaceTemplateInRegion(ref fr.FacePosition); // get template with higher precision
                    log.message(null, $"Enrolling {fn} successed!");
                    return fr;
                }
                catch (Exception ex2)
                {
                    log.message(null, $"{ex2.Message}, Error retrieving face template.");
                }

                //FaceList.Add(fr);

                //imageList1.Images.Add(fr.faceImage.ToCLRImage());
                //listView1.Items.Add((imageList1.Images.Count - 1).ToString(), fn.Split('\\')[fn.Split('\\').Length - 1], imageList1.Images.Count - 1);

                //textBox1.Text += "File '" + fn + "' enrolled\r\n";
                //textBox1.Refresh();

                //listView1.SelectedIndices.Clear();
                //listView1.SelectedIndices.Add(listView1.Items.Count - 1);
            }
            catch (Exception ex)
            {
                log.message("Error", ex.Message);
            }
            return null;
        }
        public static TFaceRecord matchFace(string fn)
        {
            try
            {
                TFaceRecord fr = new TFaceRecord();
                fr.ImageFileName = fn;
                fr.FacePosition = new FSDK.TFacePosition();
                fr.FacialFeatures = new FSDK.TPoint[FSDK.FSDK_FACIAL_FEATURE_COUNT];
                fr.Template = new byte[FSDK.TemplateSize];

                try
                {
                    fr.image = new FSDK.CImage(fn);
                }
                catch (Exception ex)
                {
                    log.message(null, $"Error loading file {ex.Message}");
                }

                fr.FacePosition = fr.image.DetectFace();
                if (0 == fr.FacePosition.w)
                {
                    log.message(null, "Enrollment error \t No faces found. Try to lower the Minimal Face Quality parameter in the Options dialog box.");
                }
                else
                {
                    fr.faceImage = fr.image.CopyRect((int)(fr.FacePosition.xc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc - Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.xc + Math.Round(fr.FacePosition.w * 0.5)), (int)(fr.FacePosition.yc + Math.Round(fr.FacePosition.w * 0.5)));

                    bool eyesDetected = false;
                    try
                    {
                        fr.FacialFeatures = fr.image.DetectEyesInRegion(ref fr.FacePosition);
                        eyesDetected = true;
                    }
                    catch (Exception ex)
                    {
                        log.message(null, $"Error detecting eyes. {ex.Message}");
                    }

                    if (eyesDetected)
                    {
                        fr.Template = fr.image.GetFaceTemplateInRegion(ref fr.FacePosition); // get template with higher precision
                    }
                }
                return fr;

                //Results frmResults = new Results();
                //frmResults.Go(fr);
            }
            catch (Exception ex)
            {
                log.message("Error", ex.Message);
            }
            return null;
        }

        public float CompareTo(TFaceRecord CurrentFace)
        {
            TFaceRecord SearchFace = this;
            float Threshold = 0.0f;
            FSDK.GetMatchingThresholdAtFAR(FARValue / 100, ref Threshold);

            //int MatchedCount = 0;
            //int FaceCount = Form1.FaceList.Count;
            //float[] Similarities = new float[FaceCount];
            //int[] Numbers = new int[FaceCount];

            //for (int i = 0; i < Form1.FaceList.Count; i++)
            {
                float Similarity = 0.0f;
                FSDK.MatchFaces(ref SearchFace.Template, ref CurrentFace.Template, ref Similarity);
                if (Similarity >= Threshold)
                {
                    return Similarity;
                    //Similarities[MatchedCount] = Similarity;
                    //Numbers[MatchedCount] = i;
                    //++MatchedCount;
                }
            }
            return -1;

            //if (MatchedCount == 0)
            //    MessageBox.Show("No matches found. You can try to increase the FAR parameter in the Options dialog box.", "No matches");
            //else
            //{
            //    floatReverseComparer cmp = new floatReverseComparer();
            //    Array.Sort(Similarities, Numbers, 0, MatchedCount, (IComparer<float>)cmp);

            //    label1.Text = "Faces Matched: " + MatchedCount.ToString();
            //    for (int i = 0; i < MatchedCount; i++)
            //    {
            //        imageList1.Images.Add(Form1.FaceList[Numbers[i]].faceImage.ToCLRImage());
            //        listView1.Items.Add((Similarities[i] * 100.0f).ToString(System.Globalization.CultureInfo.InvariantCulture.NumberFormat),
            //            Form1.FaceList[Numbers[i]].ImageFileName.Split('\\')[Form1.FaceList[Numbers[i]].ImageFileName.Split('\\').Length - 1] +
            //            "\r\nSimilarity = " + (Similarities[i] * 100).ToString(),
            //            imageList1.Images.Count - 1);
            //    }
            //}


            //this.Show();
        }
    }
}