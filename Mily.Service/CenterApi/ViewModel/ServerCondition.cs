using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.CenterApi.ViewModel
{
    public class ServerCondition:BaseCondition
    {
        public int No { get; set; }
        public string ServiceName { get; set; }
        public string Host { get; set; }
        public string TcpPort { get; set; }
        public string HttpPort { get; set; }
        /// <summary>
        /// 1 表示正常 0 表示错误
        /// </summary>
        public int Stutas { get; set; }
    }
}
