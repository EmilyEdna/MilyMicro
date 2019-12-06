using BeetleX;
using Mily.Service.RcpSetting.CenterRpc;
using Mily.Service.ViewSetting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mily.Service.CenterRpc
{
    public class NetRpcServProvider
    {
        /// <summary>
        /// 初始化Rpc
        /// </summary>
        public static void InitRpcProvider()
        {
            IServer Serv = SocketFactory.CreateTcpServer<RpcServBase, RcpServerPacket>();
            Serv.Setting(option =>
            {
                option.DefaultListen.Host = Configuration.TCP_Host;
                option.DefaultListen.Port = Configuration.TCP_Port;
            });
            Serv.Open();
        }
    }
}
