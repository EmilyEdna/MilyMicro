using BeetleX;
using Mily.Service.MiddleConfig;
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
            Serv.Setting(option =>
            {
                option.DefaultListen.Host = Configuration.TCP_Host;
                option.DefaultListen.Port = Configuration.TCP_Port;
            });
            Serv.Open();
        }
    }
}
