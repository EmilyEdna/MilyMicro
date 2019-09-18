using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.SocketClient.SocketCommon
{
    public enum NetType
    {
        /// <summary>
        /// 链接
        /// </summary>
        Connect = 1,
        /// <summary>
        /// 监听
        /// </summary>
        Listen = 2,
        /// <summary>
        /// 断开链接
        /// </summary>
        DisConnect = 3,
        /// <summary>
        /// 其他
        /// </summary>
        Ohter = 4
    }
}
