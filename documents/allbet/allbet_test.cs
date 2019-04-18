using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace QueryHandicap
{
    class allbet
    {
        public static string UserName = "yzww1a";
        public static string Password = "csab150801y";
        public const string ALLBET_DES_KEY = "GyHG5H3FeSAB0jrf/vgutISpJEyDjZOn";
        public const string ALLBET_MD5_KEY = "x7hzUlxRQMM/vO/axCsuYdzb2P/5UF1eUDjOmFOxWt4=";
        public const string ALLBET_PROPERTY_ID = "0450701";
        public const string ALLBET_API_URL = "http://api3.allbetdemo.net:8088";

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*** Get");
            Console.ResetColor();
            Get.Test(args);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*** Post");
            Console.ResetColor();
            Post.Test(args);
            Console.Read();
        }

        public static string random()
        {
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
            byte[] byteCsp = new byte[5];
            csp.GetBytes(byteCsp);
            return BitConverter.ToString(byteCsp);
        }

        public static void printResponse(HttpWebResponse response)
        {
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);

            Console.WriteLine("status: [{0}]", response.StatusCode);
            Console.WriteLine("json: {0}", reader.ReadToEnd());

            reader.Close();
            responseStream.Close();
        }
    }


    //DES加解密工具类
    class TripleDES
    {
        private static byte[] NULL_IV = Convert.FromBase64String("AAAAAAAAAAA=");

        private TripleDES() { }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <param name="keyBytes"></param>
        /// <param name="ivBytes"></param>
        /// <returns></returns>
        public static byte[] decrypt(byte[] dataBytes, byte[] keyBytes, byte[] ivBytes)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.BlockSize = 64;
            des.Padding = PaddingMode.PKCS7;
            des.Key = keyBytes;
            des.IV = ivBytes;

            MemoryStream stream = new MemoryStream();

            CryptoStream encStream = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Read);
            encStream.Write(dataBytes, 0, dataBytes.Length);
            encStream.FlushFinalBlock();
            return stream.ToArray();

        }
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="data">被解密的字符串</param>
        /// <param name="base64edKey">base64编码过的Key</param>
        /// <param name="base64edIv">base64编码过的向量, 如果传递null, 则使用默认的向量"AAAAAAAAAAA="</param>
        /// <returns></returns>
        public static string decrypt(string data, string base64edKey, string base64edIv)
        {

            return Encoding.UTF8.GetString(decrypt(Encoding.UTF8.GetBytes(data), Convert.FromBase64String(base64edKey), base64edIv == null ? NULL_IV : Convert.FromBase64String(base64edIv)));
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="dataBytes"></param>
        /// <param name="keyBytes"></param>
        /// <param name="ivBytes"></param>
        /// <returns></returns>
        public static byte[] encrypt(byte[] dataBytes, byte[] keyBytes, byte[] ivBytes)
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Mode = CipherMode.CBC;
            des.BlockSize = 64;
            des.Padding = PaddingMode.PKCS7;
            des.Key = keyBytes;
            des.IV = ivBytes;

            MemoryStream stream = new MemoryStream();

            CryptoStream encStream = new CryptoStream(stream, des.CreateEncryptor(), CryptoStreamMode.Write);
            encStream.Write(dataBytes, 0, dataBytes.Length);
            encStream.FlushFinalBlock();
            return stream.ToArray();
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="data">被加密的字符串</param>
        /// <param name="base64Key">base64编码过的Key</param>
        /// <param name="base64edIv">base64编码过的向量, 如果传递null, 则使用默认的向量"AAAAAAAAAAA="</param>
        /// <returns></returns>
        public static string encrypt(string data, string base64Key, string base64edIv)
        {
            return Convert.ToBase64String(encrypt(Encoding.UTF8.GetBytes(data), Convert.FromBase64String(base64Key), base64edIv == null ? NULL_IV : Convert.FromBase64String(base64edIv)));
        }
    }

    /// <summary>
    /// MD5签名工具类
    /// </summary>
    class MD5
    {
        /// <summary>
        /// 以Base64格式返回签名
        /// </summary>
        /// <param name="data">要被签名的字符串</param>
        /// <returns></returns>
        public static string base64edMd5(string data)
        {
            return Convert.ToBase64String(md5(Encoding.UTF8.GetBytes(data)));
        }

        /// <summary>
        /// 以byte数组格式返回签名
        /// </summary>
        /// <param name="data">被签名的数据</param>
        /// <returns></returns>
        public static byte[] md5(byte[] data)
        {
            MD5CryptoServiceProvider md5Crp = new MD5CryptoServiceProvider();
            return md5Crp.ComputeHash(data);
        }
    }

    class Get
    {
        public static void Test(string[] args)
        {
            Console.WriteLine("desKey: [{0}]", allbet.ALLBET_DES_KEY);
            Console.WriteLine("md5Key: [{0}]", allbet.ALLBET_MD5_KEY);
            Console.WriteLine("propertyId: [{0}]", allbet.ALLBET_PROPERTY_ID);
            Console.WriteLine("apiUrl: [{0}]", allbet.ALLBET_API_URL);

            try
            {
                string agent = allbet.UserName;

                //接口文档中描述的参数按照: 参数名1=参数值1&参数名2=参数值2 的格式拼接
                string realParam = "random=" + allbet.random() + "&agent=" + agent;
                Console.WriteLine("realParam: [{0}]", realParam);

                //将以上拼接结果加密
                string data = TripleDES.encrypt(realParam, allbet.ALLBET_DES_KEY, null);
                Console.WriteLine("data: [{0}]", data);

                //将加密结果与md5Key拼接
                string stingToSign = data + allbet.ALLBET_MD5_KEY;
                Console.WriteLine("stringToSign: [{0}]", stingToSign);

                //进行md5签名
                string sign = MD5.base64edMd5(stingToSign);
                Console.WriteLine("sign: [{0}]", sign);

                //将propertId, data, sign作为请求参数
                string queryString = "propertyId=" + allbet.ALLBET_PROPERTY_ID + "&data=" + System.Web.HttpUtility.UrlEncode(data) + "&sign=" + System.Web.HttpUtility.UrlEncode(sign);
                Console.WriteLine("urlEncoded queryString: [{0}]", queryString);

                //发送请求
                HttpWebRequest request = WebRequest.Create(allbet.ALLBET_API_URL + "/query_handicap?" + queryString) as HttpWebRequest;
                request.Method = "GET";

                //获取结果
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                allbet.printResponse(response);
            }
            catch (WebException ex)
            {
                allbet.printResponse(ex.Response as HttpWebResponse);
            }
        }
    }

    class Post
    {
        public static void Test(string[] args)
        {
            Console.WriteLine("desKey: [{0}]", allbet.ALLBET_DES_KEY);
            Console.WriteLine("md5Key: [{0}]", allbet.ALLBET_MD5_KEY);
            Console.WriteLine("propertyId: [{0}]", allbet.ALLBET_PROPERTY_ID);
            Console.WriteLine("apiUrl: [{0}]", allbet.ALLBET_API_URL);

            try
            {
                string agent = allbet.UserName;

                //接口文档中描述的参数按照: 参数名1=参数值1&参数名2=参数值2 的格式拼接
                string realParam = "random=" + allbet.random() + "&agent=" + agent;
                Console.WriteLine("realParam: [{0}]", realParam);

                //将以上拼接结果加密
                string data = TripleDES.encrypt(realParam, allbet.ALLBET_DES_KEY, null);
                Console.WriteLine("data: [{0}]", data);

                //将加密结果与md5Key拼接
                string stingToSign = data + allbet.ALLBET_MD5_KEY;
                Console.WriteLine("stringToSign: [{0}]", stingToSign);

                //进行md5签名
                string sign = MD5.base64edMd5(stingToSign);
                Console.WriteLine("sign: [{0}]", sign);

                //将propertId, data, sign作为请求参数
                string queryString = "propertyId=" + allbet.ALLBET_PROPERTY_ID + "&data=" + System.Web.HttpUtility.UrlEncode(data) + "&sign=" + System.Web.HttpUtility.UrlEncode(sign);
                Console.WriteLine("urlEncoded queryString: [{0}]", queryString);

                //发送请求
                HttpWebRequest request = WebRequest.Create(allbet.ALLBET_API_URL + "/query_handicap?") as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                byte[] payload = System.Text.Encoding.UTF8.GetBytes(queryString);
                request.ContentLength = payload.Length;

                Stream outStream = request.GetRequestStream();
                outStream.Write(payload, 0, payload.Length);
                outStream.Close();

                //获取结果
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                allbet.printResponse(response);
            }
            catch (WebException ex)
            {
                allbet.printResponse(ex.Response as HttpWebResponse);
            }

        }
    }
}
