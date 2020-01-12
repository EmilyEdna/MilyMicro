using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Gateway.GatewaySetting.ViewModel
{
    public class ServerCondition : BaseCondition
    {
        /// <summary>
        /// Session Id
        /// </summary>
        public int No { get; set; }
        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }
        /// <summary>
        /// IP地址
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// TCP端口
        /// </summary>
        public string TcpPort { get; set; }
        /// <summary>
        /// HTTP端口
        /// </summary>
        public string HttpPort { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        public string Route { get; set; }
        /// <summary>
        ///状态 1 表示正常 0 表示错误
        /// </summary>
        public int Stutas { get; set; }
        /// <summary>
        /// 是否启用HTTP
        /// </summary>
        public bool UseHttp { get; set; }
        /// <summary>
        /// TCP权重
        /// </summary>
        public string TcpWeight { get; set; }
        /// <summary>
        /// HTTP权重
        /// </summary>
        public string HttpWeight { get; set; }
    }
}
