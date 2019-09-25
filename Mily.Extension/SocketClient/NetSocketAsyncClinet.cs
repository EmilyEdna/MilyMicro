using System;
using System.Linq;
using BeetleX;
using BeetleX.Clients;
using Mily.Extension.LoggerFactory;
using Mily.Extension.SocketClient.SocketCommon;
using Mily.Setting;

namespace Mily.Extension.SocketClient
{
    public class NetSocketAsyncClinet
    {
        public static bool Boots { get; set; } = true;
        public static Type BaseType { get; set; }
        /// <summary>
        /// 异步注册Socket服务
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="type"></param>
        public static void Socket(int Port, Type type)
        {
            BaseType = type;
            AsyncTcpClient Client = SocketFactory.CreateClient<AsyncTcpClient>("127.0.0.1", Port);
            Client.Connect();
            Client.Socket.SendBufferSize = int.MaxValue;
            Client.Socket.ReceiveBufferSize = int.MaxValue;
            Client.ClientError = (client, Err) =>
            {
                LogFactoryExtension.WriteError(Err.Error.Source, Err.Error.TargetSite.Name, string.Join("|", Err.Error.TargetSite.GetParameters().ToList()), Err.Message, "");
            };
            Client.DataReceive = (client, reader) =>
            {
                string result = reader.Stream.ToPipeStream().ReadLine();
                Commanding Command = ParseCmd.Parse(result);
                SocketAop.RecordHandler(Command,BaseType, Client);
            };
            if (Boots)
                SocketAop.InitClientAsync(Client, NetType.Connect, MilyConfig.Discovery);
        }
    }
}
