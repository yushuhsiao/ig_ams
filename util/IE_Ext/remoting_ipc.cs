using System;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Ipc;

// Sample Remote Object
namespace RemotingSample
{
    class RemoteObject : MarshalByRefObject
    {
        //////////////////////////////////////////////////////////////////////////////
        ///constructor
        public RemoteObject()
        {
            Console.WriteLine("Remote object activated");
        }
        //////////////////////////////////////////////////////////////////////////////
        ///return message reply
        public String ReplyMessage(String msg)
        {
            Console.WriteLine("Client : " + msg);//print given message on console 
            return "Server : I'm alive !";
        }
    }
}

// Sample Remote Server
namespace RemotingSample
{
    class Server
    {
        /////////////////////////////////////////////////////////////////////////////
        ///constructor
        public Server()
        { }
        /////////////////////////////////////////////////////////////////////////////
        ///main method
        public static int RemotingServer_Main(string[] args)
        {
            //select channel to communicate
            //TcpChannel chan = new TcpChannel(8085);
            IpcChannel chan = new IpcChannel("Server");
            //register channel
            ChannelServices.RegisterChannel(chan, false);
            //register remote object
            RemotingConfiguration.RegisterWellKnownServiceType(
                   Type.GetType("RemotingSample.RemoteObject,RemoteObject"),
                   "RemotingServer",
                   WellKnownObjectMode.SingleCall);
            //inform consol
            Console.WriteLine("Server Activated");
            Console.ReadLine();
            return 0;
        }
    }
}

// Sample Remote Client
namespace RemotingSample
{
    class Client
    {
        /////////////////////////////////////////////////////////////////////////////
        ///constructor
        public Client()
        { }
        //////////////////////////////////////////////////////////////////////////////
        ///main method
        public static int RemotingClient_Main(string[] args)
        {
            //select channel to communicate with server
            //TcpChannel chan = new TcpChannel();
            IpcChannel chan = new IpcChannel("Client");
            ChannelServices.RegisterChannel(chan);
            RemoteObject remObject = (RemoteObject)Activator.GetObject(
                        typeof(RemotingSample.RemoteObject),
                //"tcp://localhost:8085/RemotingServer");
                        "ipc://Server/RemotingServer");
            if (remObject == null)
                Console.WriteLine("cannot locate server");
            else
                remObject.ReplyMessage("You there?");
            return 0;
        }
    }
}