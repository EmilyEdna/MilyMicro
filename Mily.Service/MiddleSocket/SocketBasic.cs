using BeetleX;
using Mily.Service.MiddleConfig;
using Mily.Service.MiddleEvent;
using Mily.Service.MiddleSocket.Setting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.MiddleSocket
{
    public class SocketBasic
    {
        /// <summary>
        /// 启动Socket服务
        /// </summary>
        public static void Bootstrap()
        {
            IServer Serv = SocketFactory.CreateTcpServer<SocketHandlerBase, SocketPacket>();
            CallEvent.Instance().Changed += new CallEvent.ResultEventHandler(EventAction.Instance().OnResponse);
            Serv.Setting(option =>
            {
                option.DefaultListen.Host = Configuration.TCP_Host;
                option.DefaultListen.Port = Configuration.TCP_Port;
            });
            Serv.Open();
        }
    }
}
