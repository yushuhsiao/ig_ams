#if NET40
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using _DebuggerStepThrough = System.Diagnostics.DebuggerStepThroughAttribute;

namespace System.Runtime.Remoting.Channels
{
    [_DebuggerStepThrough]
    public class ClientIPInjectorSink : BaseChannelObjectWithProperties, IServerChannelSink
    {
        private IServerChannelSink _nextSink;

        public ClientIPInjectorSink(IServerChannelSink nextSink)
        {
            _nextSink = nextSink;
        }

        public void AsyncProcessResponse(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers, Stream stream)
        {
            //get the client's ip address, and put it in the call context.  
            //This value will be extracted later so we can determine the actual 
            //address of the client. 
            try
            {
                CallContext.SetData(CommonTransportKeys.IPAddress, headers[CommonTransportKeys.IPAddress]);
                CallContext.SetData(CommonTransportKeys.ConnectionId, headers[CommonTransportKeys.ConnectionId]);
                CallContext.SetData(CommonTransportKeys.RequestUri, headers[CommonTransportKeys.RequestUri]);
            }
            catch { }

            //forward to stack for further processing 
            sinkStack.AsyncProcessResponse(msg, headers, stream);
        }

        public Stream GetResponseStream(IServerResponseChannelSinkStack sinkStack, object state, IMessage msg, ITransportHeaders headers)
        {
            return null;
        }

        public IServerChannelSink NextChannelSink
        {
            get { return _nextSink; }
        }

        public ServerProcessing ProcessMessage(IServerChannelSinkStack sinkStack, IMessage requestMsg, ITransportHeaders requestHeaders, Stream requestStream, out IMessage responseMsg, out ITransportHeaders responseHeaders, out Stream responseStream)
        {
            //get the client's ip address, and put it in the call context.  
            //This value will be extracted later so we can determine the actual 
            //address of the client. 
            try
            {
                CallContext.SetData(CommonTransportKeys.IPAddress, requestHeaders[CommonTransportKeys.IPAddress]);
                CallContext.SetData(CommonTransportKeys.ConnectionId, requestHeaders[CommonTransportKeys.ConnectionId]);
                CallContext.SetData(CommonTransportKeys.RequestUri, requestHeaders[CommonTransportKeys.RequestUri]);
            }
            catch { }

            //pushing onto stack and forwarding the call 
            sinkStack.Push(this, null);

            ServerProcessing srvProc = _nextSink.ProcessMessage(sinkStack, requestMsg, requestHeaders, requestStream, out responseMsg, out responseHeaders, out responseStream);
            if (srvProc == ServerProcessing.Complete)
            {
                //TODO - implement post processing 
            }
            return srvProc;
        }
    }

    //this class is used as the sink provider that creates the  
    //CClientiPInjectorSink. It is used by the channel 
    //so that we can get the caller's ip address 
    [_DebuggerStepThrough]
    public class ClientIPInjectorSinkProvider : IServerChannelSinkProvider
    {
        private IServerChannelSinkProvider _nextProvider;

        public ClientIPInjectorSinkProvider() { }

        public ClientIPInjectorSinkProvider(IDictionary properties, ICollection providerdata) { }

        public IServerChannelSink CreateSink(IChannelReceiver channel)
        {
            //create other sinks in the chain 
            IServerChannelSink nextSink = _nextProvider.CreateSink(channel);
            return new ClientIPInjectorSink(nextSink);
        }


        public void GetChannelData(IChannelDataStore channelData)
        {
            //not needed 
        }

        public IServerChannelSinkProvider Next
        {
            get { return _nextProvider; }
            set { _nextProvider = value; }
        }
    }
}
#endif