using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Tools;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace System.Net
{

    [_DebuggerStepThrough]
    public sealed class SocketStream : Stream
    {
        public delegate void ConnectCallbackHandler(SocketStream s, Exception err);
        public delegate void CloseCallbackHandler(SocketStream s);

        internal readonly Socket socket;
        public Socket Socket { get { return this.socket; } }
        public readonly IPEndPoint LocalEndPoint;
        public readonly IPEndPoint RemoteEndPoint;

        public static SocketStream Connect(EndPoint remoteEP)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(remoteEP);
            return new SocketStream(s);
        }
        public static SocketStream Connect(IPAddress address, int port)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(address, port);
            return new SocketStream(s);
        }
        public static SocketStream Connect(IPAddress[] addresses, int port)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(addresses, port);
            return new SocketStream(s);
        }
        public static SocketStream Connect(string host, int port)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            s.Connect(host, port);
            return new SocketStream(s);
        }

        struct _state
        {
            public Socket socket;
            public ConnectCallbackHandler cb;
        }

        public static IAsyncResult Connect(EndPoint remoteEP, ConnectCallbackHandler cb, int timeout = 0)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return connect(s.BeginConnect(remoteEP, ConnectCallback, new _state { socket = s, cb = cb }), timeout);
        }
        public static IAsyncResult Connect(IPAddress address, int port, ConnectCallbackHandler cb, int timeout = 0)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return connect(s.BeginConnect(address, port, ConnectCallback, new _state { socket = s, cb = cb }), timeout);
        }
        public static IAsyncResult Connect(IPAddress[] addresses, int port, ConnectCallbackHandler cb, int timeout = 0)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return connect(s.BeginConnect(addresses, port, ConnectCallback, new _state { socket = s, cb = cb }), timeout);
        }
        public static IAsyncResult Connect(string host, int port, ConnectCallbackHandler cb, int timeout = 0)
        {
            Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            return connect(s.BeginConnect(host, port, ConnectCallback, new _state { socket = s, cb = cb }), timeout);
        }
        static IAsyncResult connect(IAsyncResult ar, int timeout)
        {
            _state _state = (_state)ar.AsyncState;
            if (timeout > 0)
                ThreadPool.RegisterWaitForSingleObject(ar.AsyncWaitHandle, ConnectTimeoutCallback, _state, timeout, true);
            return ar;
        }

        static void ConnectTimeoutCallback(object state, bool timedOut)
        {
            _state _state = (_state)state;
            if (timedOut)
            {
                _state.cb(null, new SocketException((int)SocketError.TimedOut));
            }
        }

        static void ConnectCallback(IAsyncResult ar)
        {
            _state _state = (_state)ar.AsyncState;
            try
            {
                _state.socket.EndConnect(ar);
                _state.cb(new SocketStream(_state.socket), null);
            }
            catch (Exception ex)
            {
                _state.cb(null, ex);
            }
        }

        internal SocketStream(Socket socket)
        {
            this.socket = socket;
            this.socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            this.socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            this.LocalEndPoint = (IPEndPoint)socket.LocalEndPoint;
            this.RemoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
        }

        CloseCallbackHandler close_callback;
        AutoResetEvent close_event;
        static Queue<AutoResetEvent> close_events = new Queue<AutoResetEvent>();
        RegisteredWaitHandle close_wait;

        public override void Close()
        {
            CloseCallbackHandler close_callback = Interlocked.Exchange(ref this.close_callback, null) ?? _null.noop;
            lock (this)
            {
                using (close_event)
                using (socket)
                {
                    try { socket.Shutdown(SocketShutdown.Both); }
                    catch { }
                    try { socket.Close(); close_callback(this); }
                    catch { }
                }
                base.Close();
            }
        }

        public void Close(int timeout = 0, CloseCallbackHandler callback = null)
        {
            lock (this)
            {
                if (timeout > 0)
                {
                    this.Close(0, null);
                    this.close_callback = callback;
                    this.close_event = this.close_event ?? new AutoResetEvent(false);
                    this.close_wait = ThreadPool.RegisterWaitForSingleObject(this.close_event, close_proc, null, timeout, true);
                }
                else
                {
                    this.close_callback = null;
                    if (this.close_wait != null)
                    {
                        RegisteredWaitHandle close_wait = this.close_wait;
                        this.close_wait = null;
                        close_wait.Unregister(this.close_event);
                    }
                }
            }
        }

        void close_proc(object state, bool timedOut)
        {
            lock (this)
            {
                if (this.close_wait == null) return;
                using (this)
                    return;
            }
        }

        //bool close_tick()
        //{
        //    if (Interlocked.CompareExchange(ref close_callback, null, null) == null)
        //        return false;
        //    if (!Monitor.TryEnter(this))
        //        return false;
        //    try
        //    {
        //        if (close_time >= DateTime.Now)
        //            return true;
        //        using (this)
        //            Tick.OnTick -= close_tick;
        //    }
        //    finally { Monitor.Exit(this); }
        //    return true;
        //}

        //internal byte[] handshake_buf;
        //internal int handshake_index;

        public int Available
        {
            get { return socket.Available; }
        }

        [DebuggerStepThrough]
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return socket.BeginReceive(buffer, offset, count, SocketFlags.None, callback, state);
        }
        [DebuggerStepThrough]
        public override int EndRead(IAsyncResult asyncResult)
        {
            return socket.EndReceive(asyncResult);
        }
        [DebuggerStepThrough]
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return socket.BeginSend(buffer, offset, count, SocketFlags.None, callback, state);
        }
        [DebuggerStepThrough]
        public override void EndWrite(IAsyncResult asyncResult)
        {
            socket.EndSend(asyncResult);
        }
        public override bool CanRead
        {
            [DebuggerStepThrough]
            get { return true; }
        }
        public override bool CanSeek
        {
            [DebuggerStepThrough]
            get { return false; }
        }
        public override bool CanWrite
        {
            [DebuggerStepThrough]
            get { return true; }
        }
        [DebuggerStepThrough]
        public override void Flush()
        {
        }
        public override long Length
        {
            [DebuggerStepThrough]
            get { return 0; }
        }
        public override long Position
        {
            [DebuggerStepThrough]
            get { return 0; }
            [DebuggerStepThrough]
            set { }
        }
        [DebuggerStepThrough]
        public override int Read(byte[] buffer, int offset, int count)
        {
            return socket.Receive(buffer, offset, count, SocketFlags.None);
        }
        [DebuggerStepThrough]
        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }
        [DebuggerStepThrough]
        public override void SetLength(long value)
        {
        }
        [DebuggerStepThrough]
        public override void Write(byte[] buffer, int offset, int count)
        {
            socket.Send(buffer, offset, count, SocketFlags.None);
        }

        public int Write(string str)
        {
            return Write(str, Encoding.UTF8);
        }
        public int Write(string str, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(str);
            socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
            return buffer.Length;
        }

        public int WriteLine(string str)
        {
            return this.WriteLine(str, Encoding.UTF8);
        }
        public int WriteLine(string str, Encoding encoding)
        {
            return this.Write(str, encoding) + this.Write("\r\n", encoding);
        }

        //static X509Certificate serverCertificate = null;

        //static SocketStream()
        //{
        //    serverCertificate = X509Certificate.CreateFromSignedFile(@"ws.pfx");


        //    //string certificate = null;
        //    //if (args == null || args.Length < 1)
        //    //{
        //    //    DisplayUsage();
        //    //}
        //    //certificate = args[0];
        //    //try
        //    //{
        //    //    X509Store store = new X509Store(StoreName.My);
        //    //    store.Open(OpenFlags.ReadWrite);

        //    //    // 检索证书 
        //    //    X509Certificate2Collection certs = store.Certificates.Find(X509FindType.FindBySubjectName, "MyServer", false); // vaildOnly = true时搜索无结果。
        //    //    if (certs.Count == 0) return;

        //    //    serverCertificate = certs[0];
        //    //    RunServer();
        //    //    store.Close(); // 关闭存储区。
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.Message);
        //    //}
        //    //Console.ReadLine();
        //    //return 0;



        //    //try
        //    //{
        //    //    Console.WriteLine("服务端输出：" + ServiceSecurityContext.Current.PrimaryIdentity.AuthenticationType);
        //    //    Console.WriteLine(ServiceSecurityContext.Current.PrimaryIdentity.Name);
        //    //    Console.WriteLine("服务端时间：" + DateTime.Now.ToString());
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.Message);
        //    //}
        //    //Console.ReadLine();
        //}

        //public void ssl()
        //{
        //    using (SslStream sslStream = new SslStream(this, false))
        //    {
        //        try
        //        {
        //            sslStream.AuthenticateAsServer(serverCertificate, false, SslProtocols.Tls, true);
        //            DisplaySecurityLevel(sslStream);
        //            DisplaySecurityServices(sslStream);
        //            DisplayCertificateInformation(sslStream);
        //            DisplayStreamProperties(sslStream);

        //            sslStream.ReadTimeout = 5000;
        //            sslStream.WriteTimeout = 5000;
        //            Console.WriteLine("Waiting for client message...");
        //            string messageData = ReadMessage(sslStream);
        //            Console.WriteLine("Received: {0}", messageData);
        //            byte[] message = Encoding.UTF8.GetBytes("Hello from the server.");
        //            Console.WriteLine("Sending hello message.");
        //            sslStream.Write(message);
        //        }
        //        catch (AuthenticationException e)
        //        {
        //            Console.WriteLine("Exception: {0}", e.Message);
        //            if (e.InnerException != null)
        //            {
        //                Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
        //            }
        //            Console.WriteLine("Authentication failed - closing the connection.");
        //            return;
        //        }
        //    }
        //}

        //static string ReadMessage(SslStream sslStream)
        //{
        //    byte[] buffer = new byte[2048];
        //    StringBuilder messageData = new StringBuilder();
        //    int bytes = -1;
        //    do
        //    {
        //        bytes = sslStream.Read(buffer, 0, buffer.Length);
        //        Decoder decoder = Encoding.UTF8.GetDecoder();
        //        char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
        //        decoder.GetChars(buffer, 0, bytes, chars, 0);
        //        messageData.Append(chars);
        //        if (messageData.ToString().IndexOf("") != -1)
        //        {
        //            break;
        //        }
        //    }
        //    while (bytes != 0);

        //    return messageData.ToString();
        //}

        //static void DisplaySecurityLevel(SslStream stream)
        //{
        //    Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
        //    Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
        //    Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
        //    Console.WriteLine("Protocol: {0}", stream.SslProtocol);
        //}

        //static void DisplaySecurityServices(SslStream stream)
        //{
        //    Console.WriteLine("Is authenticated: {0} as server? {1}", stream.IsAuthenticated, stream.IsServer);
        //    Console.WriteLine("IsSigned: {0}", stream.IsSigned);
        //    Console.WriteLine("Is Encrypted: {0}", stream.IsEncrypted);
        //}

        //static void DisplayStreamProperties(SslStream stream)
        //{
        //    Console.WriteLine("Can read: {0}, write {1}", stream.CanRead, stream.CanWrite);
        //    Console.WriteLine("Can timeout: {0}", stream.CanTimeout);
        //}

        //static void DisplayCertificateInformation(SslStream stream)
        //{
        //    Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

        //    X509Certificate localCertificate = stream.LocalCertificate;
        //    if (stream.LocalCertificate != null)
        //    {
        //        Console.WriteLine("Local cert was issued to {0} and is valid from {1} until {2}.",
        //        localCertificate.Subject,
        //            localCertificate.GetEffectiveDateString(),
        //            localCertificate.GetExpirationDateString());
        //    }
        //    else
        //    {
        //        Console.WriteLine("Local certificate is null.");
        //    }
        //    X509Certificate remoteCertificate = stream.RemoteCertificate;
        //    if (stream.RemoteCertificate != null)
        //    {
        //        Console.WriteLine("Remote cert was issued to {0} and is valid from {1} until {2}.",
        //            remoteCertificate.Subject,
        //            remoteCertificate.GetEffectiveDateString(),
        //            remoteCertificate.GetExpirationDateString());
        //    }
        //    else
        //    {
        //        Console.WriteLine("Remote certificate is null.");
        //    }
        //}

        //private static void DisplayUsage()
        //{
        //    Console.WriteLine("To start the server specify:");
        //    Console.WriteLine("serverSync certificateFile.cer");
        //    //Environment.Exit(1);
        //}



    }
}
