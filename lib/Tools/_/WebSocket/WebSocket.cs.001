using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
// Rfc6455 WebSocket
namespace TopGame.WebSockets
{
    public delegate void AcceptSocketHandler(WebSocketServer sender, Socket socket);
    public delegate void AcceptWebSocketHandler(WebSocketServer sender, WebSocket ws);

    public class WebSocketServer
    {
        public Uri Url { get; private set; }
        public WebSocketServer(string url)
        {
            this.HandshakeTimeout = 3000;
            this.Url = new Uri(url);
            this.Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            this.Listener.Bind(new IPEndPoint(IPAddress.Any, this.Url.Port));
            this.Listener.Listen(100);
            this.Listener.BeginAccept(accept, null);
            log.message(null, "WebSocket url : {0}", this.Url);
        }

        void accept(IAsyncResult ar)
        {
            Socket socket = this.Listener.EndAccept(ar);
            this.Listener.BeginAccept(accept, null);
            this.OnAcceptSocket(socket);
            new WebSocket(this, socket);
        }

        internal Socket Listener { get; private set; }

        public double HandshakeTimeout { get; set; }

        private AcceptSocketHandler e_AcceptSocket;
        public AcceptSocketHandler AcceptSocketEvent
        {
            get { return Interlocked.CompareExchange(ref e_AcceptSocket, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_AcceptSocket, value); }
        }
        public event AcceptSocketHandler AcceptSocket
        {
            add { this.AcceptSocketEvent = value; }
            remove { this.AcceptSocketEvent = null; }
        }
        internal void OnAcceptSocket(Socket socket)
        {
            log.message("ws", "Accept socket from {0}", socket.RemoteEndPoint);
            try { this.AcceptSocketEvent(this, socket); }
            catch { }
        }

        private AcceptWebSocketHandler e_AcceptWebSocket;
        public AcceptWebSocketHandler AcceptWebSockeEvent
        {
            get { return Interlocked.CompareExchange(ref this.e_AcceptWebSocket, null, null) ?? _null; }
            set { Interlocked.Exchange(ref this.e_AcceptWebSocket, value); }
        }
        public event AcceptWebSocketHandler AcceptWebSocket
        {
            add { this.AcceptWebSockeEvent = value; }
            remove { this.AcceptWebSockeEvent = null; }
        }
        internal void OnAcceptWebSocket(WebSocket ws)
        {
            try { this.AcceptWebSockeEvent(this, ws); }
            catch { }
        }

        static void _null<T>(WebSocketServer sender, T socket) { }
    }

    public delegate void WebSocketHandler(WebSocket ws);
    public delegate void WebSocketHandler<T>(WebSocket ws, T obj);
    public delegate void WebSocketHandler<T1, T2>(WebSocket ws, T1 obj1, T2 obj2);
    public sealed class WebSocket : IDisposable
    {
        static void _null(WebSocket ws) { }
        static void _null<T>(WebSocket ws, T obj) { }
        static void _null<T1, T2>(WebSocket ws, T1 obj1, T2 obj2) { }

        const string magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
        static byte[] null_byte = new byte[0];
        readonly WebSocketServer m_Server;
        readonly Socket m_Socket;
        public WebSocketRequest Request { get; private set; }
        public WebSocketResponse Response { get; private set; }
        public IPEndPoint LocalEndPoint { get; private set; }
        public IPEndPoint RemoteEndPoint { get; private set; }

        public WebSocket(WebSocketServer server, Socket socket)
        {
            this.Request = new WebSocketRequest() { AcceptTime = DateTime.Now, Scheme = server.Url.Scheme, };
            this.m_Server = server;
            this.m_Socket = socket;
            this.LocalEndPoint = (IPEndPoint)socket.LocalEndPoint;
            this.RemoteEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            this.FrameMasking = false;
            this.m_Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
            this.m_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            this.m_IsDisposed = server;
            this.m_Server.OnAcceptWebSocket(this);
            this.handshake_recv();
            Tick.OnTick += this.tick;
        }

        object m_IsDisposed;
        bool IsDisposed { get { return Interlocked.CompareExchange(ref this.m_IsDisposed, null, null) == null; } }

        void IDisposable.Dispose()
        {
            using (this.m_Socket)
            {
                if (Interlocked.Exchange(ref this.m_IsDisposed, null) == null)
                    return;
                Tick.OnTick -= this.tick;
                try { m_Socket.Shutdown(SocketShutdown.Both); }
                catch (ObjectDisposedException) { }
                catch { }
                try { m_Socket.Disconnect(false); this.OnDisconnected(); }
                catch (ObjectDisposedException) { }
                catch { }
            }
        }

        DateTime? m_Disconnect;
        public void Disconnect()
        {
            using (this)
                return;
        }
        public void Disconnect(int? milliseconds)
        {
            lock (this)
                if (milliseconds.HasValue)
                    this.m_Disconnect = DateTime.Now.AddMilliseconds(milliseconds.Value);
                else
                    this.m_Disconnect = null;
        }

        #region Events

        private WebSocketHandler e_Disconnected;
        public WebSocketHandler DisconnectedEvent
        {
            get { return Interlocked.CompareExchange(ref e_Disconnected, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_Disconnected, value); }
        }
        public event WebSocketHandler Disconnected
        {
            add { DisconnectedEvent = value; }
            remove { DisconnectedEvent = null; }
        }
        void OnDisconnected()
        {
            log.message("ws", "{0}, Disconnected.", this.RemoteEndPoint.Address);
            try { DisconnectedEvent(this); }
            catch { }
        }

        private WebSocketHandler e_HandshakeSuccess;
        public WebSocketHandler HandshakeSuccessEvent
        {
            get { return Interlocked.CompareExchange(ref e_HandshakeSuccess, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_HandshakeSuccess, value); }
        }
        public event WebSocketHandler HandshakeSuccess
        {
            add { HandshakeSuccessEvent = value; }
            remove { HandshakeSuccessEvent = null; }
        }
        void OnHandshakeSuccess()
        {
            try { HandshakeSuccessEvent(this); }
            catch { }
        }

        private WebSocketHandler e_HandshakeFailed;
        public WebSocketHandler HandshakeFailedEvent
        {
            get { return Interlocked.CompareExchange(ref e_HandshakeFailed, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_HandshakeFailed, value); }
        }
        public event WebSocketHandler HandshakeFailed
        {
            add { HandshakeFailedEvent = value; }
            remove { HandshakeFailedEvent = null; }
        }
        void OnHandshakeFailed()
        {
            try { HandshakeFailedEvent(this); }
            catch { }
        }

        private WebSocketHandler e_HandshakeTimeout;
        public WebSocketHandler HandshakeTimeoutEvent
        {
            get { return Interlocked.CompareExchange(ref e_HandshakeTimeout, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_HandshakeTimeout, value); }
        }
        public event WebSocketHandler HandshakeTimeout
        {
            add { HandshakeTimeoutEvent = value; }
            remove { HandshakeTimeoutEvent = null; }
        }
        void OnHandshakeTimeout()
        {
            log.message("ws", "{0}, Handshake Timeout.", this.RemoteEndPoint.Address);
            try { HandshakeTimeoutEvent(this); }
            catch { }
        }

        private WebSocketHandler<Exception> e_Error;
        public WebSocketHandler<Exception> ErrorEvent
        {
            get { return Interlocked.CompareExchange(ref e_Error, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_Error, value); }
        }
        public event WebSocketHandler<Exception> Error
        {
            add { ErrorEvent = value; }
            remove { ErrorEvent = null; }
        }
        void OnError(Exception ex, string message)
        {
            log.message("ws", "{0}, Handshake Error {2}, {1}", this.RemoteEndPoint.Address, ex, message);
            try { ErrorEvent(this, ex); }
            catch { }
        }

        #endregion

        #region Message Events

        private WebSocketHandler<Frame> e_ReceiveFrame;
        public WebSocketHandler<Frame> ReceiveFrameEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceiveFrame, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_ReceiveFrame, value); }
        }
        public event WebSocketHandler<Frame> ReceiveFrame
        {
            add { ReceiveFrameEvent = value; }
            remove { ReceiveFrameEvent = null; }
        }

        private WebSocketHandler<Frame, string> e_ReceiveText;
        public WebSocketHandler<Frame, string> ReceiveTextEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceiveText, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_ReceiveText, value); }
        }
        public event WebSocketHandler<Frame, string> ReceiveText
        {
            add { ReceiveTextEvent = value; }
            remove { ReceiveTextEvent = null; }
        }

        private WebSocketHandler<Frame, byte[]> e_ReceiveBinary;
        public WebSocketHandler<Frame, byte[]> ReceiveBinaryEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceiveBinary, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_ReceiveBinary, value); }
        }
        public event WebSocketHandler<Frame, byte[]> ReceiveBinary
        {
            add { ReceiveBinaryEvent = value; }
            remove { ReceiveBinaryEvent = null; }
        }

        private WebSocketHandler<Frame, byte[]> e_ReceivePing;
        public WebSocketHandler<Frame, byte[]> ReceivePingEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceivePing, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_ReceivePing, value); }
        }
        public event WebSocketHandler<Frame, byte[]> ReceivePing
        {
            add { ReceivePingEvent = value; }
            remove { ReceivePingEvent = null; }
        }

        private WebSocketHandler<Frame, byte[]> e_ReceivePong;
        public WebSocketHandler<Frame, byte[]> ReceivePongEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceivePong, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_ReceivePong, value); }
        }
        public event WebSocketHandler<Frame, byte[]> ReceivePong
        {
            add { ReceivePongEvent = value; }
            remove { ReceivePongEvent = null; }
        }

        private WebSocketHandler<Frame, byte[]> e_ReceiveClose;
        public WebSocketHandler<Frame, byte[]> ReceiveCloseEvent
        {
            get { return Interlocked.CompareExchange(ref e_ReceiveClose, null, null) ?? _null; }
            set { Interlocked.Exchange(ref e_ReceiveClose, value); }
        }
        public event WebSocketHandler<Frame, byte[]> ReceiveClose
        {
            add { ReceiveCloseEvent = value; }
            remove { ReceiveCloseEvent = null; }
        }

        #endregion

        void tick()
        {
            if (this.IsDisposed)
                return;

            if (!Monitor.TryEnter(this.m_Socket))
                return;
            try
            {
                if (this.m_Disconnect.HasValue)
                {
                    if (DateTime.Now > this.m_Disconnect.Value)
                    {
                        this.Disconnect();
                        return;
                    }
                }
                if (this.Response == null)
                {
                    TimeSpan t = DateTime.Now - this.Request.AcceptTime;
                    if (t.TotalMilliseconds > this.m_Server.HandshakeTimeout)
                    {
                        using (this)
                        {
                            this.OnHandshakeTimeout();
                            return;
                        }
                    }
                }
            }
            catch { }
            finally { Monitor.Exit(this.m_Socket); }
        }

        void handshake_recv()
        {
            if (this.IsDisposed) return;
            try
            {
                Monitor.Enter(this.m_Socket);
                int size = this.Request.BinaryLength;
                byte[] data = this.Request.Bytes;
                if (data == null)
                    data = new byte[512];
                if (data.Length <= size)
                    Array.Resize(ref data, data.Length * 2);
                this.Request.Bytes = data;
                this.m_Socket.BeginReceive(data, size, data.Length - size, SocketFlags.None, this.handshake_recv, null);
            }
            catch (Exception ex)
            {
                using (this) this.OnError(ex, "handshake_recv()");
            }
            finally
            {
                Monitor.Exit(this.m_Socket);
            }
        }
        void handshake_recv(IAsyncResult ar)
        {
            try
            {
                DateTime time = DateTime.Now;
                if (this.IsDisposed) return;
                Monitor.Enter(this.m_Socket);
                int count = this.m_Socket.EndReceive(ar);
                if (count == 0)
                    using (this)
                        return;
                this.Request.BinaryLength += count;
                if (this.Request.Parse())
                {
                    this.Response = new WebSocketResponse() { Time = time, };
                    handshake_send();
                }
                else
                    handshake_recv();
            }
            catch (Exception ex)
            {
                using (this) this.OnError(ex, "handshake_recv(ar)");
            }
            finally
            {
                Monitor.Exit(this.m_Socket);
            }
        }
        void handshake_send()
        {
            if (this.IsDisposed) return;
            try
            {
                if (!this.Request.WebSocketVersion.strcmp("13"))
                    this.Response.StatusCode = HttpStatusCode.NotImplemented;
                else if ((this.Request.Method == null) || (this.Request.Path == null) || (this.Request.Version == null))
                    this.Response.StatusCode = HttpStatusCode.BadRequest;
                else if (!this.Request.Method.strcmp("GET"))
                    this.Response.StatusCode = HttpStatusCode.MethodNotAllowed;
                else if (!this.Request.Version.strcmp(consts.HTTP11))
                    this.Response.StatusCode = HttpStatusCode.HttpVersionNotSupported;
                else if (!this.Request.Connection.strcmp("Upgrade"))
                    this.Response.StatusCode = HttpStatusCode.BadRequest;
                else if (!this.Request.Upgrade.strcmp("WebSocket"))
                    this.Response.StatusCode = HttpStatusCode.BadRequest;
                else
                {
                    if (this.Request.SecWebSocketKey == null)
                        this.Response.StatusCode = HttpStatusCode.BadRequest;
                    else
                    {
                        this.Response.StatusCode = HttpStatusCode.SwitchingProtocols;
                        this.Response.StatusMessage = "Switching Protocols";
                        this.Response.Upgrade = "WebSocket";// ws.Request.Upgrade
                        this.Response.Connection = "Upgrade";// ws.Response.Connection
                        try
                        {
                            byte[] key1 = Encoding.ASCII.GetBytes(this.Request.SecWebSocketKey + magic);
                            byte[] key2 = SHA1.Create().ComputeHash(key1);
                            this.Response.SecWebSocketAccept = Convert.ToBase64String(key2);
                        }
                        catch
                        {
                            this.Response.Clear();
                            this.Response.StatusCode = HttpStatusCode.InternalServerError;
                        }
                        //if (this.Request.WebSocketProtocol != null)
                        //    this.Response.SecWebSocketProtocol = null; //""
                    }
                }
                if (this.Response.StatusMessage == null)
                    this.Response.StatusMessage = this.Response.StatusCode.ToString();
                byte[] handshake = this.Response.GetBytes();
                this.m_Socket.BeginSend(handshake, 0, handshake.Length, SocketFlags.None, handshake_send, null);
            }
            catch (Exception ex)
            {
                using (this) this.OnError(ex, "handshake_send()");
            }
        }
        void handshake_send(IAsyncResult ar)
        {
            try
            {
                this.m_Socket.EndSend(ar);
                if (this.Response.StatusCode == HttpStatusCode.SwitchingProtocols)
                {
                    Interlocked.Exchange(ref this.recv_queue, new Queue<Frame>());
                    Interlocked.Exchange(ref this.send_queue, new Queue<Frame>());
                    this.OnHandshakeSuccess();
                    this.recv();
                }
                else
                {
                    this.OnHandshakeFailed();
                    this.Disconnect(1500);
                }
            }
            catch (Exception ex)
            {
                using (this) this.OnError(ex, "handshake_send(ar)");
            }
        }

        Queue<Frame> recv_queue;
        Queue<Frame> send_queue;
        Frame recv_frame;
        Frame recv_part;
        Frame send_frame;

        void recv()
        {
            if (this.IsDisposed) return;
            try
            {
                if (recv_frame == null)
                    recv_frame = new Frame();

                m_Socket.BeginReceive(recv_frame.data[recv_frame.mode], recv_frame.offset, recv_frame.data[recv_frame.mode].Length - recv_frame.offset, SocketFlags.None, this.recv, null);
            }
            catch (Exception ex)
            {
                using (this) this.OnError(ex, "recv()");
            }
            process_recv_queue();
            process_send_queue(null);
        }
        void recv(IAsyncResult ar)
        {
            if (this.IsDisposed) return;
            DateTime time = DateTime.Now;
            try
            {
                int count = m_Socket.EndReceive(ar);
                if (count == 0) using (this) return;
                if (recv_frame.IsReady(count))
                {
                    #region FrameReady

                    recv_frame.PacketReady = time;
                    if (Enum.IsDefined(typeof(FrameTypes), recv_frame.opcode))
                    {
                        if (recv_frame.FIN)
                        {
                            if (recv_part == null)
                                lock (recv_queue) recv_queue.Enqueue(recv_frame);
                            else
                            {
                                recv_part.AddNextPart(recv_frame);
                                lock (recv_queue) recv_queue.Enqueue(recv_part);
                                recv_part = null;
                            }
                        }
                        else if ((recv_frame.FrameType == FrameTypes.Ping) || (recv_frame.FrameType == FrameTypes.Pong) || (recv_frame.FrameType == FrameTypes.Close))
                            using (this) this.OnError(null, "控制訊息不能分段");
                        else if (recv_frame.FrameType == FrameTypes.Continuation)
                        {
                            if (recv_part != null)
                                recv_part.AddNextPart(recv_frame);
                            else
                                using (this) this.OnError(null, "第一個分段訊息不存在");
                        }
                        else
                        {
                            if (recv_part != null)
                                using (this) this.OnError(null, "已經有一個分段訊息了");
                        }
                    }
                    else
                        using (this) this.OnError(null, "未知的訊息類型");
                    recv_frame = null;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                using (this) this.OnError(ex, "recv(ar)");
            }
            this.recv();
        }

        void process_recv_queue()
        {
            for (; ; )
            {
                if (this.IsDisposed) return;
                if (!Monitor.TryEnter(recv_queue)) return;
                Frame frame;
                try
                {
                    if (recv_queue.Count == 0) return;
                    frame = recv_queue.Dequeue();
                }
                finally { Monitor.Exit(recv_queue); }

                try { this.ReceiveFrameEvent(this, frame); }
                catch { }

                try
                {
                    switch (frame.FrameType)
                    {
                        case FrameTypes.Text:
                            this.ReceiveTextEvent(this, frame, frame.ReadAsText());
                            break;
                        case FrameTypes.Binary:
                            this.ReceiveBinaryEvent(this, frame, frame.ReadAsBinary());
                            break;
                        case FrameTypes.Ping:
                            this.ReceivePingEvent(this, frame, frame.ReadAsBinary());
                            break;
                        case FrameTypes.Pong:
                            this.ReceivePongEvent(this, frame, frame.ReadAsBinary());
                            break;
                        case FrameTypes.Close:
                            this.ReceiveCloseEvent(this, frame, frame.ReadAsBinary());
                            this.Disconnect(1000);
                            break;
                    }
                }
                catch { }
            }
        }
        void process_send_queue(IAsyncResult ar2)
        {
            if (this.IsDisposed) return;
            Frame frame;
            if (ar2 != null)
            {
                try
                {
                    int count = m_Socket.EndSend(ar2);
                    frame = (Frame)ar2.AsyncState;
                    Interlocked.CompareExchange(ref send_frame, null, frame);
                }
                catch { }
            }
            try
            {
                if (Interlocked.CompareExchange(ref send_frame, null, null) != null)
                    return;
                if (!Monitor.TryEnter(send_queue))
                    return;
                try
                {
                    if (send_queue.Count == 0) return;
                    if (Interlocked.CompareExchange(ref send_frame, send_queue.Peek(), null) != null)
                        return;
                    frame = send_queue.Dequeue();
                }
                finally { Monitor.Exit(send_queue); }
                byte[] header = frame.GetHeader();
                m_Socket.BeginSend(header, 0, header.Length, SocketFlags.None, (ar1) =>
                {
                    if (this.IsDisposed) return;
                    try
                    {
                        int count = m_Socket.EndSend(ar1);
                        frame = (Frame)ar1.AsyncState;
                        byte[] data = frame.GetData();
                        m_Socket.BeginSend(data, 0, data.Length, SocketFlags.None, process_send_queue, frame);
                    }
                    catch { }
                }, frame);
            }
            catch { }
        }

        public bool FrameMasking { get; set; }
        public void SendFrame(Frame frame)
        {
            Queue<Frame> send_queue = Interlocked.CompareExchange(ref this.send_queue, null, null);
            if (send_queue == null) return;
            lock (send_queue)
                send_queue.Enqueue(frame);
            process_send_queue(null);
        }
        public void SendText(string text)
        {
            this.SendFrame(new Frame(text, FrameMasking));
        }
        public void SendBinary(byte[] data)
        {
            this.SendFrame(new Frame(FrameTypes.Binary, data, FrameMasking));
        }
        public void SendPing()
        {
            SendPing(Frame._null);
        }
        public void SendPing(params byte[] data)
        {
            SendFrame(new Frame(FrameTypes.Ping, data, FrameMasking));
        }
        public void SendPong()
        {
            SendPong(Frame._null);
        }
        public void SendPong(byte[] data)
        {
            SendFrame(new Frame(FrameTypes.Pong, data, FrameMasking));
        }
        public void SendClose()
        {
            SendClose(Frame._null);
        }
        public void SendClose(byte[] data)
        {
            SendFrame(new Frame(FrameTypes.Close, data, FrameMasking));
        }
    }

    public enum FrameTypes : byte
    {
        Continuation = 0x00,
        Text = 0x01,
        Binary = 0x02,
        Close = 0x08,
        Ping = 0x09,
        Pong = 0x0A,
    }

    public class Frame
    {
        public DateTime PacketStart { get; set; }
        public DateTime PacketReady { get; set; }
        public bool FIN
        {
            get { return (data[0][0] & 0x80) != 0; }
            private set { if (value) data[0][0] |= 0x80; else data[0][0] &= 0x7f; }
        }
        public bool RSV1
        {
            get { return (data[0][0] & 0x40) != 0; }
            private set { if (value) data[0][0] |= 0x40; else data[0][0] &= 0xbf; }
        }
        public bool RSV2
        {
            get { return (data[0][0] & 0x20) != 0; }
            private set { if (value) data[0][0] |= 0x20; else data[0][0] &= 0xdf; }
        }
        public bool RSV3
        {
            get { return (data[0][0] & 0x10) != 0; }
            private set { if (value) data[0][0] |= 0x10; else data[0][0] &= 0xef; }
        }
        public byte opcode
        {
            get { byte n = data[0][0]; n &= 0x0f; return n; }
            private set { data[0][0] &= 0xf0; data[0][0] |= value; }
        }
        public FrameTypes FrameType
        {
            get { return (FrameTypes)this.opcode; }
            set { this.opcode = (byte)value; }
        }
        public bool MASK
        {
            get { return (data[0][1] & 0x80) != 0; }
            set { if (value) data[0][1] |= 0x80; else data[0][1] &= 0x7f; }
        }
        public int PAYLOAD_LEN
        {
            get { return (int)(data[0][1] & 0x7f); }
            set
            {
                byte mask = data[0][1];
                mask &= 0x80;
                data[0][1] = (byte)value;
                data[0][1] &= 0x7f;
                data[0][1] |= mask;
            }
        }
        public int EXT_PAYLOAD_LEN
        {
            get
            {
                uint len = 0;
                for (int i = 0; i < data[1].Length; i++)
                {
                    len <<= 8;
                    len |= data[1][i];
                }
                return (int)len;
            }
            set
            {
                int size;
                if (value >= 32768)
                {
                    size = 8;
                    this.PAYLOAD_LEN = 127;
                }
                else if (value >= 126)
                {
                    size = 2;
                    this.PAYLOAD_LEN = 126;
                }
                else
                {
                    size = 0;
                    this.PAYLOAD_LEN = value;
                }
                if (data[1].Length != size)
                    data[1] = new byte[size];
                for (int i = size - 1, n = value; i >= 0; i--)
                {
                    data[1][i] = (byte)n;
                    n <<= 8;
                }
            }
        }

        internal static byte[] _null = new byte[0];

        internal byte[][] data = new byte[][] { new byte[2], _null, _null, _null };
        internal int mode;
        internal int offset;
        bool is_masked;

        byte[] mask_data(bool masked)
        {
            if (this.is_masked != masked)
            {
                int len1 = data[2].Length;
                int len2 = data[3].Length;
                for (int i = 0; i < len1; i++)
                {
                    byte mask = data[2][i];
                    for (int j = i; j < len2; j += len1)
                        data[3][j] ^= mask;
                }
                this.is_masked = masked;
            }
            return this.data[3];
        }

        #region for receive

        public Frame()
        {
        }

        internal bool IsReady(int count)
        {
            offset += count;
            while (offset >= data[mode].Length)
            {
                offset = 0;
                if (mode == 0)
                {
                    int PAYLOAD_LEN = this.PAYLOAD_LEN;
                    if (PAYLOAD_LEN == 127)
                        data[1] = new byte[8];
                    else if (PAYLOAD_LEN == 126)
                        data[1] = new byte[2];
                }
                else if (mode == 1)
                {
                    if (is_masked = MASK)
                        data[2] = new byte[4];
                }
                else if (mode == 2)
                {
                    int PAYLOAD_LEN = this.PAYLOAD_LEN;
                    if (PAYLOAD_LEN >= 126)
                        data[3] = new byte[EXT_PAYLOAD_LEN];
                    else
                        data[3] = new byte[PAYLOAD_LEN];
                }
                else
                    return true;
                mode++;
            }
            return false;
        }

        internal Frame NextPart;
        internal void AddNextPart(Frame frame)
        {
            if (this.NextPart == null)
                this.NextPart = frame;
            else
                this.NextPart.AddNextPart(frame);
        }

        public string ReadAsText()
        {
            return Encoding.UTF8.GetString(this.ReadAsBinary());
        }
        public byte[] ReadAsBinary()
        {
            return mask_data(false);
        }
        
        #endregion

        #region for send

        public Frame(string text, bool mask) : this(FrameTypes.Text, Encoding.UTF8.GetBytes(text), mask) { }
        public Frame(FrameTypes type, byte[] data, bool mask)
        {
            FIN = true;
            FrameType = type;
            MASK = mask;
            SetData(data);
        }

        void SetData(byte[] data)
        {
            is_masked = false;
            EXT_PAYLOAD_LEN = (this.data[3] = data).Length;
        }

        internal byte[] GetHeader()
        {
            byte[] ret = new byte[data[0].Length + data[1].Length + (MASK ? data[2].Length : 0)];
            int index = 0;
            for (int i = 0; i < data[0].Length; i++)
                ret[index++] = data[0][i];
            for (int i = 0; i < data[1].Length; i++)
                ret[index++] = data[1][i];
            if (MASK)
                for (int i = 0; i < data[2].Length; i++)
                    ret[index++] = data[2][i];
            return ret;
        }
        internal byte[] GetData()
        {
            return this.mask_data(MASK);
        }

        #endregion
    }

    public class WebSocketRequest : NameValueCollection
    {
        public string this[HttpRequestHeader name]
        {
            get { return base[name.ToString()]; }
            set { base[name.ToString()] = value; }
        }

        public DateTime AcceptTime { get; set; }
        public byte[] Bytes { get; set; }
        public int BinaryLength { get; set; }
        public string Scheme { get; set; }
        public string Method { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Upgrade
        {
            get { return this[HttpRequestHeader.Upgrade]; }
            set { this[HttpRequestHeader.Upgrade] = value; }
        }
        public string Connection
        {
            get { return this[HttpRequestHeader.Connection]; }
            set { this[HttpRequestHeader.Connection] = value; }
        }
        public string Host
        {
            get { return this[HttpRequestHeader.Host]; }
            set { this[HttpRequestHeader.Host] = value; }
        }
        public string Origin
        {
            get { return this["Origin"]; }
            set { this["Origin"] = value; }
        }
        public string SecWebSocketKey
        {
            get { return this[consts.SecWebSocketKey]; }
            set { this[consts.SecWebSocketKey] = value; }
        }
        public string SecWebSocketVersion
        {
            get { return this[consts.SecWebSocketVersion]; }
            set { this[consts.SecWebSocketVersion] = value; }
        }
        public string SecWebSocketExtensions
        {
            get { return this[consts.SecWebSocketExtensions]; }
            set { this[consts.SecWebSocketExtensions] = value; }
        }
        public string SecWebSocketDraft
        {
            get { return this["Sec-WebSocket-Draft"]; }
            set { this["Sec-WebSocket-Draft"] = value; }
        }
        public string SecWebSocketKey1
        {
            get { return this[consts.SecWebSocketKey1]; }
            set { this[consts.SecWebSocketKey1] = value; }
        }
        public string SecWebSocketKey2
        {
            get { return this[consts.SecWebSocketKey2]; }
            set { this[consts.SecWebSocketKey2] = value; }
        }
        public string SecWebSocketKey3
        {
            get { return this[consts.SecWebSocketKey3]; }
            set { this[consts.SecWebSocketKey3] = value; }
        }
        public string WebSocketProtocol
        {
            get { return this[consts.WebSocketProtocol]; }
            set { this[consts.WebSocketProtocol] = value; }
        }
        public string WebSocketVersion
        {
            get { return this.SecWebSocketVersion ?? this.SecWebSocketDraft ?? this.SecWebSocketKey1; }
        }

        public string RequestText { get; set; }

        public override void Clear()
        {
            this.Method = this.Path = this.Version = null;
            base.Clear();
        }

        internal bool Parse()
        {
            string http = this.RequestText = Encoding.UTF8.GetString(this.Bytes, 0, this.BinaryLength);
            int len = this.RequestText.Length;
            int line = 0;
            string key = null;
            this.Clear();
            for (int i = 0, start = 0; i < len; i++)
            {
                char c = http[i];
                bool newLine = http.IsNewLine(i);
                if (line == 0)
                {
                    if ((c == ' ') || newLine)
                    {
                        string tmp = http.Substring(ref start, i);
                        if (this.Method == null)
                            this.Method = tmp;
                        else if (this.Path == null)
                            this.Path = tmp;
                        else
                            this.Version = tmp;
                    }
                }
                else
                {
                    if (c == ':')
                    {
                        if (key == null)
                            key = http.Substring(ref start, i);
                    }
                    else if (newLine)
                    {
                        if (key != null)
                        {
                            string value = http.Substring(ref start, i);
                            this[key] = value;
                        }
                        key = null;
                    }
                }
                if (newLine)
                {
                    line++;
                    i++;
                    start = i + 1;
                    if (http.IsNewLine(i + 1))
                        return true;
                }
            }
            return false;
        }
    }

    public class WebSocketResponse : NameValueCollection
    {
        public string this[HttpResponseHeader name]
        {
            get { return base[name.ToString()]; }
            set { base[name.ToString()] = value; }
        }

        public DateTime Time { get; set; }
        public string Version { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public string Upgrade
        {
            get { return this[HttpResponseHeader.Upgrade]; }
            set { this[HttpResponseHeader.Upgrade] = value; }
        }
        public string Connection
        {
            get { return this[HttpResponseHeader.Connection]; }
            set { this[HttpResponseHeader.Connection] = value; }
        }
        public string SecWebSocketAccept
        {
            get { return this[consts.SecWebSocketAccept]; }
            set { this[consts.SecWebSocketAccept] = value; }
        }
        public string SecWebSocketProtocol
        {
            get { return this[consts.SecWebSocketProtocol]; }
            set { this[consts.SecWebSocketProtocol] = value; }
        }

        public override void Clear()
        {
            this.Version = this.StatusMessage = null;
            base.Clear();
        }

        public override string ToString()
        {
            this.Version = this.Version ?? consts.HTTP11;
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Version);
            sb.Append(' ');
            sb.Append((int)this.StatusCode);
            sb.Append(' ');
            if (this.StatusMessage == null)
                sb.Append(this.StatusCode.ToString());
            else
                sb.Append(this.StatusMessage);
            sb.AppendLine();

            foreach (string key in this.AllKeys)
            {
                string value = this[key];
                if (string.IsNullOrEmpty(value)) continue;
                sb.AppendFormat("{0}: {1}", key, value); sb.AppendLine();
            }
            sb.AppendLine();
            return sb.ToString();
        }

        internal byte[] GetBytes()
        {
            string text = this.ToString();
            if (!string.IsNullOrEmpty(text))
                try { return Encoding.UTF8.GetBytes(text); }
                catch { }
            return null;
        }
    }

    static class consts
    {
        static readonly byte[] byte_mask1 = new byte[] { 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80 };
        static readonly byte[] byte_mask0 = new byte[] { 0xf7, 0xfd, 0xfb, 0xf7, 0xef, 0xdf, 0xbf, 0x7f };

        public static bool GetBit(this byte b, int bit)
        {
            return (b & byte_mask1[bit]) != 0;
        }
        public static void SetBit(this byte b, int bit, bool value)
        {
            if (value) b |= byte_mask1[bit]; else b &= byte_mask0[bit];
        }

        [DebuggerStepThrough]
        public static string Substring(this string s, ref int start, int end)
        {
            string ret = s.Substring(start, end - start);
            start = end + 1;
            return ret.Trim();
        }

        [DebuggerStepThrough]
        public static bool IsNewLine(this string s, int index)
        {
            int n = index + 1;
            if (s.Length < n)
                return false;
            return (s[index] == '\r') && (s[n] == '\n');
        }

        [DebuggerStepThrough]
        public static Socket CloseSocket(this Socket socket, Action onDisconnected)
        {
            if (socket == null)
                return null;
            using (socket)
            {
                try { socket.Shutdown(SocketShutdown.Both); }
                catch (ObjectDisposedException) { }
                catch { }
                try
                {
                    socket.Close();
                    if (onDisconnected != null)
                        onDisconnected();
                }
                catch (ObjectDisposedException) { }
                catch { }
            }
            return null;
        }

        [DebuggerStepThrough]
        public static bool strcmp(this string a, string b)
        {
            return string.Compare(a, b, true) == 0;
        }

        public const string Host = "Host";
        public const string Connection = "Connection";
        public const string SecWebSocketAccept = "Sec-WebSocket-Accept";
        public const string SecWebSocketKey1 = "Sec-WebSocket-Key1";
        public const string SecWebSocketKey2 = "Sec-WebSocket-Key2";
        public const string SecWebSocketKey3 = "Sec-WebSocket-Key3";
        public const string SecWebSocketKey = "Sec-WebSocket-Key";
        public const string SecWebSocketVersion = "Sec-WebSocket-Version";
        public const string SecWebSocketProtocol = "Sec-WebSocket-Protocol";
        public const string SecWebSocketExtensions = "Sec-WebSocket-Extensions";
        public const string WebSocketProtocol = "WebSocket-Protocol";
        public const string Cookie = "Cookie";
        public const string Upgrade = "Upgrade";
        public const string Origin = "Origin";
        public const string ResponseHeadLine00 = "HTTP/1.1 101 WebSocket Protocol Handshake";
        public const string ResponseHeadLine10 = "HTTP/1.1 101 Switching Protocols";
        public const string ResponseUpgradeLine = Upgrade + ": WebSocket";
        public const string ResponseConnectionLine = Connection + ": Upgrade";
        public const string ResponseOriginLine = "Sec-WebSocket-Origin: {0}";
        public const string ResponseLocationLine = "Sec-WebSocket-Location: {0}://{1}{2}";
        public const string ResponseProtocolLine = SecWebSocketProtocol + ": {0}";
        public const string ResponseAcceptLine = "Sec-WebSocket-Accept: {0}";
        public const string HTTP11 = "HTTP/1.1";
        public const byte StartByte = 0x00;
        public const byte EndByte = 0xFF;
        public static byte[] ClosingHandshake = new byte[] { 0xFF, 0x00 };
    }
}