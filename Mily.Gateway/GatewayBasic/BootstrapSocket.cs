using BeetleX;
using Mily.Gateway.GatewayBasic.SocketBasic;
using Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting;
using Mily.Gateway.GatewayEvent.SocketEvent;
using Mily.Gateway.GatewaySetting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Gateway.GatewayBasic
{
    /// <summary>
    /// Socket服务
    /// </summary>
    public class BootstrapSocket
    {
        /// <summary>
        /// 启动Socket服务
        /// </summary>
        public static void Bootstrap()
        {
            IServer Serv = SocketFactory.CreateTcpServer<MicroSocket, SocketPacket>();
            ListenEvent.Instance().Changed += new ListenEvent.ResultEventHandler(EventAction.Instance().OnResponse);
            Serv.Setting(option =>
            {
                option.DefaultListen.Host = Configuration.TCP_Host;
                option.DefaultListen.Port = Configuration.TCP_Port;
            });
            Serv.Open();
        }
    }
}
