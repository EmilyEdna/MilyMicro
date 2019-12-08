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
        /// 初始化Rpc
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <param name="BaseType"></param>
        public static void InitRpcProvider(string Ip,int Port,Type BaseType)
        {
            AsyncTcpClient ClientAsnyc = SocketFactory.CreateClient<AsyncTcpClient, RcpClientPacket>(Ip,Port);
            ClientAsnyc.Connect();
            ClientAsnyc.Socket.SendBufferSize = int.MaxValue;
            ClientAsnyc.Socket.ReceiveBufferSize = int.MaxValue;
            ClientAsnyc.PacketReceive = (Client, Data) =>
            {
                ResultProvider Provider = ProxyHandler.Instance.InitProxy((ResultProvider)Data, BaseType);
                if (Client.IsConnected && Provider != null)
                    ClientHandler.Instance.SendInvoke(ClientAsnyc, Provider);
            };
            ClientAsnyc.ClientError = (Client, Error) =>
            {
                LogFactoryExtension.WriteError(Error.Error.Source, Error.Error.TargetSite.Name, string.Join("|", Error.Error.TargetSite.GetParameters().ToList()), Error.Message, "");
            };
            ClientAsnyc.Send(ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.Connect, MilyConfig.Discovery), ClientValue.SetStrValue("注册服务", MilyConfig.Discovery)));
        }
        /// <summary>
        /// 初始化Rpc
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        /// <param name="BaseType"></param>
        public virtual void InitRpcProviderCustomer(string Ip, int Port, Type BaseType)
        {
            AsyncTcpClient ClientAsnyc = SocketFactory.CreateClient<AsyncTcpClient, RcpClientPacket>(Ip, Port);
            ClientAsnyc.Connect();
            ClientAsnyc.Socket.SendBufferSize = int.MaxValue;
            ClientAsnyc.Socket.ReceiveBufferSize = int.MaxValue;
            ClientAsnyc.PacketReceive = (Client, Data) =>
            {
                ResultProvider Provider = ProxyHandler.Instance.InitProxy((ResultProvider)Data, BaseType);
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
