using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Setting.SocketConfig
{
    /// <summary>
    /// 内部通信配置
    /// </summary>
    public class SocketInternalConfig
    {
        /// <summary>
        /// 内部通信中间件IP
        /// </summary>
        public string InternalIPV4 { get; set; }
        /// <summary>
        /// 内部通信中间件端口
        /// </summary>
        public int InternalIPV4Port { get; set; }
        /// <summary>
        /// 本机IP
        /// </summary>
        public string ClientInternalIPV4 { get; set; }
        /// <summary>
        /// 连接通信中间件后的本机固定端口
        /// </summary>
        public int ClientInternalIPV4Port { get; set; }
    }
}
