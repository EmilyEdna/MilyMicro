using Mily.Extension.ClientRpc;
using Mily.Setting;
using Mily.Socket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.Infrastructure.Common
{
    public class InitSocketProxy
    {
        /// <summary>
        /// 初始化Socket相关组件
        /// </summary>
        /// <param name="IsStart"></param>
        public static void InitSocketDependency(bool IsStart=true)
        {
            if (IsStart)
            {
                InitGateWay();
                InitInternal();
            }
        }
        /// <summary>
        /// 初始化网关客户端
        /// </summary>
        private static void InitGateWay()
        {
            //初始化网关客户端
            NetClientProvider.InitGateWayClinet(option =>
            {
                option.ServerPath = MilyConfig.SocketGateWay.GateWayIPV4;
                option.ServerPort = MilyConfig.SocketGateWay.GateWayIPV4Port;
                option.ClientPath = MilyConfig.SocketGateWay.ClientIPV4;
                option.ClientPort = MilyConfig.SocketGateWay.ClientIPV4Port;
            });
        }
        /// <summary>
        /// 初始化Socket通信中心
        /// </summary>
        private static void InitInternal()
        {
            //初始化Socket通信中心
            SocketBasic.InitInternalSocket(option =>
            {
                option.SockInfoIP = MilyConfig.SocketInternal.InternalIPV4;
                option.SockInfoPort = MilyConfig.SocketInternal.InternalIPV4Port;
                option.ClientPath = MilyConfig.SocketInternal.ClientInternalIPV4;
                option.ClientPort = MilyConfig.SocketInternal.ClientInternalIPV4Port;
            });
        }
    }
}
