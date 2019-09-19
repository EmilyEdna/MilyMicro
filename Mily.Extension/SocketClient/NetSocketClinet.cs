using System;
using System.Threading.Tasks;
using BeetleX;
using BeetleX.Clients;
using Mily.Extension.SocketClient.SocketCommon;

namespace Mily.Extension.SocketClient
{
    public class NetSocketClinet
    {
        public static Type BaseType { get; set; }
        private static object Locker = new object();
        /// <summary>
        /// 同步注册Socket服务
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="type"></param>
        public static void Socket(int Port, Type type)
        {
            BaseType = type;
            TcpClient Client = SocketFactory.CreateClient<TcpClient>("127.0.0.1", Port);
            Client.Connect();
            Client.Socket.ReceiveTimeout = int.MaxValue;
            Client.Socket.SendBufferSize = int.MaxValue;
            Client.Socket.ReceiveBufferSize = int.MaxValue;
            SocketAop.InitClient(Client, NetType.Connect);
            Task.Run(() =>
            {
                while (true)
                {
                    lock (Locker)
                    {
                        var Data = Client.Receive().ToPipeStream().ReadLine();
                        Commanding Reader = ParseCmd.Parse(Data);
                        SocketAop.RecordHandler(Reader, BaseType, null, Client);
                    }
                }
            });
        }
    }
}
