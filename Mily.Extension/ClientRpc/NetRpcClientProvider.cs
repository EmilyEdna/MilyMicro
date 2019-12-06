using BeetleX;
using BeetleX.Clients;
using Mily.Extension.ClientRpc.RpcSetting;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.Extension.ClientRpc.RpcSetting.View;
using Mily.Extension.LoggerFactory;
using System.Linq;
using XExten.Common;
using System.Collections.Generic;

namespace Mily.Extension.ClientRpc
{
    public class NetRpcClientProvider
    {
        /// <summary>
        /// 初始化Rpc
        /// </summary>
        public static void InitRpcProvider()
        {
            AsyncTcpClient Client = SocketFactory.CreateClient<AsyncTcpClient, RcpClientPacket>("127.0.0.1", 9090);
            Client.Connect();
            Client.Socket.SendBufferSize = int.MaxValue;
            Client.Socket.ReceiveBufferSize = int.MaxValue;
            Client.PacketReceive = (Client, Data) =>
            {
                ProxyHandler.InitProxy((ResultProvider)Data);
            };
            Client.ClientError = (Client, Error) =>
            {
                LogFactoryExtension.WriteError(Error.Error.Source, Error.Error.TargetSite.Name, string.Join("|", Error.Error.TargetSite.GetParameters().ToList()), Error.Message, "");
            };
            Client.Send(ResultProvider.SetValue(NetTypeEnum.Connect, new Dictionary<object, object> { { "RegistService", "Other" } }));
        }
    }
}
