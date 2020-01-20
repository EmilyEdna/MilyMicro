using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Setting.SocketConfig
{
    /// <summary>
    /// 网关配置
    /// </summary>
   public class SocketGateWayConfig
    {
        /// <summary>
        /// 网关IP
        /// </summary>
        public string GateWayIPV4 { get; set; }
        /// <summary>
        /// 网关端口
        /// </summary>
        public int GateWayIPV4Port { get; set; }
        /// <summary>
        /// 本机IP
        /// </summary>
        public string ClientIPV4 { get; set; }
        /// <summary>
        /// 链接网关后本机固定的端口
        /// </summary>
        public int ClientIPV4Port { get; set; }
    }
}
