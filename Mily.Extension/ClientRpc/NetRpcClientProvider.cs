using BeetleX;
using BeetleX.Clients;
using Mily.Extension.ClientRpc.RpcSetting;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.Extension.ClientRpc.RpcSetting.View;
using Mily.Extension.LoggerFactory;
using System.Linq;
using XExten.Common;

namespace Mily.Extension.ClientRpc
{
    public class NetRpcClientProvider
    {
        /// <summary>
        /// 初始化Rpc
        /// </summary>
        public static void InitRpcProvider()
        {
            AsyncTcpClient ClientAsnyc = SocketFactory.CreateClient<AsyncTcpClient, RcpClientPacket>("127.0.0.1", 9090);
            ClientAsnyc.Connect();
            ClientAsnyc.Socket.SendBufferSize = int.MaxValue;
            ClientAsnyc.Socket.ReceiveBufferSize = int.MaxValue;
            ClientAsnyc.PacketReceive = (Client, Data) =>
            {
                ResultProvider  Provider = ProxyHandler.InitProxy((ResultProvider)Data);
                if (Client.IsConnected)
                    ClientHandler.SendInvoke(ClientAsnyc, Provider);
            };
            ClientAsnyc.ClientError = (Client, Error) =>
            {
                LogFactoryExtension.WriteError(Error.Error.Source, Error.Error.TargetSite.Name, string.Join("|", Error.Error.TargetSite.GetParameters().ToList()), Error.Message, "");
            };
            ClientAsnyc.Send(ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.Connect, "Other"), ClientValue.SetStrValue("注册服务", "Others")));
        }
    }
}
