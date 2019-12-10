using BeetleX;
using BeetleX.Clients;
using Mily.Extension.ClientRpc.RpcSetting;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.Extension.ClientRpc.RpcSetting.View;
using Mily.Extension.LoggerFactory;
using Mily.Setting;
using System;
using System.Linq;
using XExten.Common;

namespace Mily.Extension.ClientRpc
{
    public class NetRpcClientProvider
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string IPAddress { get; set; }
        /// <summary>
        /// 端口
        /// </summary>
        public int IPPort { get; set; }
        /// <summary>
        /// 初始化客服端
        /// </summary>
        /// <param name="Action"></param>
        public static void InitClinet(Action<NetRpcClientProvider> Action)
        {
            NetRpcClientProvider Client = new NetRpcClientProvider();
            Action(Client);
            Client.InitRpcProviderCustomer(Client.IPAddress, Client.IPPort);
        }
        /// <summary>
        /// 初始化Rpc
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        public virtual void InitRpcProviderCustomer(string Ip, int Port)
        {
            AsyncTcpClient ClientAsnyc = SocketFactory.CreateClient<AsyncTcpClient, RcpClientPacket>(Ip, Port);
            ClientAsnyc.Connect();
            ClientAsnyc.Socket.SendBufferSize = int.MaxValue;
            ClientAsnyc.Socket.ReceiveBufferSize = int.MaxValue;
            ClientAsnyc.PacketReceive = (Client, Data) =>
            {
                ResultProvider Provider = ProxyHandler.Instance.InitProxy((ResultProvider)Data);
                if (Client.IsConnected && Provider != null)
                    ClientHandler.Instance.SendInvoke(ClientAsnyc, Provider);
            };
            ClientAsnyc.ClientError = (Client, Error) =>
            {
                LogFactoryExtension.WriteError(Error.Error.Source, Error.Error.TargetSite.Name, string.Join("|", Error.Error.TargetSite.GetParameters().ToList()), Error.Message, "");
            };
            ClientAsnyc.Send(ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.Connect, MilyConfig.Discovery), ClientValue.SetStrValue("注册服务", MilyConfig.Discovery)));
        }
    }
}
