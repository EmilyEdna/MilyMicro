﻿using BeetleX;
using BeetleX.Clients;
using Mily.Extension.ClientRpc.RpcSetting;
using XExten.Profile.AspNetCore.InvokeTracing;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.Extension.ClientRpc.RpcSetting.Send;
using Mily.Extension.ClientRpc.RpcSetting.View;
using Mily.Extension.LoggerFactory;
using Mily.Setting;
using System;
using System.Linq;
using System.Net;
using XExten.Common;
using XExten.XCore;

namespace Mily.Extension.ClientRpc
{
    public class NetClientProvider
    {
        #region Basic Config
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string ServerPath { get; set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int ServerPort { get; set; }
        /// <summary>
        /// 客服端地址
        /// </summary>
        public string ClientPath { get; set; }
        /// <summary>
        /// 客服端端口
        /// </summary>
        public int? ClientPort { get; set; }
        #endregion

        /// <summary>
        /// 初始化网关客户端
        /// </summary>
        /// <param name="Action"></param>
        public static void InitGateWayClinet(Action<NetClientProvider> Action)
        {
            NetClientProvider Client = new NetClientProvider();
            Action(Client);
            Client.InitProviderCustomer(Client.ServerPath, Client.ServerPort);
        }
        /// <summary>
        /// 初始化通信
        /// </summary>
        /// <param name="Ip"></param>
        /// <param name="Port"></param>
        protected virtual void InitProviderCustomer(string Ip, int Port)
        {
            AsyncTcpClient ClientAsnyc = SocketFactory.CreateClient<AsyncTcpClient, SocketPacket>(Ip, Port);
            if (!ClientPath.IsNullOrEmpty() && ClientPort.HasValue)
                ClientAsnyc.LocalEndPoint = new IPEndPoint(IPAddress.Parse(ClientPath), ClientPort.Value);
            ClientAsnyc.Connect(out bool  Connect);
            ClientAsnyc.PacketReceive = (Client, Data) =>
            {
                ClientAsnyc.Socket.ByTraceSocket(Data);
                ResultProvider Provider = ProxyHandler.Instance.InitProxy((ResultProvider)Data);
                if (Client.IsConnected && Provider != null)
                    ClientSend.Instance.SendInvoke(ClientAsnyc, Provider);
            };
            ClientAsnyc.ClientError = (Client, Error) =>
            {
                LogFactoryExtension.WriteError(Error.Error);
                //LogFactoryExtension.WriteError(Error.Error.Source, Error.Error.TargetSite.Name, string.Join("|", Error.Error.TargetSite.GetParameters().ToList()), Error.Message, "");
            };
            ClientAsnyc.Send(ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.Connect, MilyConfig.Discovery), ClientValue.SetStrValue("注册服务", MilyConfig.Discovery)));
        }
    }
}
