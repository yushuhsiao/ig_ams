using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Dynamic;
using Newtonsoft.Json.Linq;

namespace test
{
    class test1
    {
        static Socket sck = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static string text = @"POST /gameadmin/api/001/test2 HTTP/1.1
Host: google.com
Connection: keep-alive
Content-Length: 43
Origin: http://localhost
User-Agent: Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/41.0.2272.101 Safari/537.36
Content-Type: application/x-www-form-urlencoded; charset=UTF-8
Accept: application/json, text/javascript, */*; q=0.01
X-Requested-With: XMLHttpRequest
Cookie: abc=123;
aaa: bbb
ccc: ddd
eee: fff
DNT: 1
Referer: http://localhost/gameadmin/api
Accept-Encoding: gzip, deflate
Accept-Language: zh-TW,zh;q=0.8,en-US;q=0.6,en;q=0.4,zh-CN;q=0.2

0=%7Ba%3A1%2Cb%3A2%7D&1=%7Bc%3A3%2Cd%3A4%7D
";

        static void Main(string[] args)
        {
            string s;
            s = "{{}}{a:00}{b}{c}{d.d2}".FormatWith(sql: true, obj: new { a = "1", b = 2, c = 3, d = new { d1 = "aa", d2 = "bb" } });
            s = string.Format("{{}}", 1);
            
            #region
            return;
            Console.WriteLine("press any key to send...");
            Console.ReadKey();
            sck.BeginConnect("localhost", 80, cb_1, null);
            //WebClient wc = new WebClient();
            //for (int i = 0; i < 10; i++)
            //    wc.Headers.Add("test" + i.ToString(), i.ToString());
            //wc.DownloadData("http://localhost/gameadmin/api/001/test2");
            Console.WriteLine("press any key to exit...");
            Console.ReadKey();
            #endregion
        }

        static void cb_1(IAsyncResult ar)
        {
            sck.EndConnect(ar);
            sck.Send(Encoding.UTF8.GetBytes(text));
        }

        StringBuilder Append(char ch)
        {
            return null;
        }
        StringBuilder Append(string s)
        {
            return null;
        }
        StringBuilder Append(char value, int repeatCount)
        {
            return null;
        }
        private static void FormatError()
        {
            throw new FormatException("Format_InvalidString");
        }

        public StringBuilder AppendFormat(IFormatProvider provider, String format, params Object[] args)
        {
            if (format == null || args == null)
            {
                throw new ArgumentNullException((format == null) ? "format" : "args");
            }
            //Contract.Ensures(Contract.Result<StringBuilder>() != null);
            //Contract.EndContractBlock();

            int pos = 0;
            int len = format.Length;
            char ch = '\x0';

            ICustomFormatter cf = null;
            if (provider != null)
            {
                cf = (ICustomFormatter)provider.GetFormat(typeof(ICustomFormatter));
            }

            while (true)
            {
                int p = pos;
                int i = pos;
                while (pos < len)
                {
                    ch = format[pos];

                    pos++;
                    if (ch == '}')
                    {
                        if (pos < len && format[pos] == '}') // Treat as escape character for }}
                            pos++;
                        else
                            FormatError();
                    }

                    if (ch == '{')
                    {
                        if (pos < len && format[pos] == '{') // Treat as escape character for {{
                            pos++;
                        else
                        {
                            pos--;
                            break;
                        }
                    }

                    Append(ch);
                }

                if (pos == len) break;
                pos++;
                if (pos == len || (ch = format[pos]) < '0' || ch > '9') FormatError();
                int index = 0;
                do
                {
                    index = index * 10 + ch - '0';
                    pos++;
                    if (pos == len) FormatError();
                    ch = format[pos];
                } while (ch >= '0' && ch <= '9' && index < 1000000);
                if (index >= args.Length) throw new FormatException("Format_IndexOutOfRange");
                while (pos < len && (ch = format[pos]) == ' ') pos++;
                bool leftJustify = false;
                int width = 0;
                if (ch == ',')
                {
                    pos++;
                    while (pos < len && format[pos] == ' ') pos++;

                    if (pos == len) FormatError();
                    ch = format[pos];
                    if (ch == '-')
                    {
                        leftJustify = true;
                        pos++;
                        if (pos == len) FormatError();
                        ch = format[pos];
                    }
                    if (ch < '0' || ch > '9') FormatError();
                    do
                    {
                        width = width * 10 + ch - '0';
                        pos++;
                        if (pos == len) FormatError();
                        ch = format[pos];
                    } while (ch >= '0' && ch <= '9' && width < 1000000);
                }

                while (pos < len && (ch = format[pos]) == ' ') pos++;
                Object arg = args[index];
                StringBuilder fmt = null;
                if (ch == ':')
                {
                    pos++;
                    p = pos;
                    i = pos;
                    while (true)
                    {
                        if (pos == len) FormatError();
                        ch = format[pos];
                        pos++;
                        if (ch == '{')
                        {
                            if (pos < len && format[pos] == '{')  // Treat as escape character for {{
                                pos++;
                            else
                                FormatError();
                        }
                        else if (ch == '}')
                        {
                            if (pos < len && format[pos] == '}')  // Treat as escape character for }}
                                pos++;
                            else
                            {
                                pos--;
                                break;
                            }
                        }

                        if (fmt == null)
                        {
                            fmt = new StringBuilder();
                        }
                        fmt.Append(ch);
                    }
                }
                if (ch != '}') FormatError();
                pos++;
                String sFmt = null;
                String s = null;
                if (cf != null)
                {
                    if (fmt != null)
                    {
                        sFmt = fmt.ToString();
                    }
                    s = cf.Format(sFmt, arg, provider);
                }

                if (s == null)
                {
                    IFormattable formattableArg = arg as IFormattable;

#if FEATURE_LEGACYNETCF
                    if(CompatibilitySwitches.IsAppEarlierThanWindowsPhone8) {
                        // TimeSpan does not implement IFormattable in Mango
                        if(arg is TimeSpan) {
                            formattableArg = null;
                        }
                    }
#endif
                    if (formattableArg != null)
                    {
                        if (sFmt == null && fmt != null)
                        {
                            sFmt = fmt.ToString();
                        }

                        s = formattableArg.ToString(sFmt, provider);
                    }
                    else if (arg != null)
                    {
                        s = arg.ToString();
                    }
                }

                if (s == null) s = String.Empty;
                int pad = width - s.Length;
                if (!leftJustify && pad > 0) Append(' ', pad);
                Append(s);
                if (leftJustify && pad > 0) Append(' ', pad);
            }
            return null;
        }

    }
}
