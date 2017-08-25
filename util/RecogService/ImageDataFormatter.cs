using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace RecogService
{
    class ImageDataFormatter : MediaTypeFormatter
    {
        public ImageDataFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpeg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/jpg"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("image/png"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(ImageData);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }

        public async override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            if (!content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            MultipartMemoryStreamProvider parts = await content.ReadAsMultipartAsync();

            string token = null;
            byte[] picture = null;
            foreach (var nn in parts.Contents)
            {
                if (nn.Headers.ContentDisposition.Name == "\"token\"")
                    token = await nn.ReadAsStringAsync();
                else if (nn.Headers.ContentDisposition.Name == "\"picture\"")
                    picture = await nn.ReadAsByteArrayAsync();
            }
            if (string.IsNullOrEmpty(token) || (picture == null)) return null;
            return new ImageData() { token = token, data = picture };
        }
    }
}
