namespace RecogService
{
    public class TakePictureUrls
    {
        public string swfUrl;
        public string recognitionUrl;
        public string accessToken;

        public static string GetSwfUrl(string AssetServerUrl, ImageType? ImageKey)
        {
            switch (ImageKey)
            {
                case ImageType.action: return $"{AssetServerUrl}/webcam/PhotoVerify.swf?v=201611011136";
                case ImageType.recog: return $"{AssetServerUrl}/webcam/PhotoCheck.swf?v=201611011136";
                case ImageType.sample: return $"{AssetServerUrl}/webcam/PhotoCapture.swf?v=201611011136";
                default: return null;
            }
        }
    }
}
