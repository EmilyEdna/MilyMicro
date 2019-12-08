using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.CenterRpc.RpcSetting.Handler
{
    public enum NetTypeEnum
    {
        /// <summary>
        /// 链接
        /// </summary>
        Connect = 1,
        /// <summary>
        /// 监听
        /// </summary>
        Listened = 2,
        /// <summary>
        /// 断链
        /// </summary>
        DisConnect = 3,
        /// <summary>
        /// 回调
        /// </summary>
        CallBack=4
    }
}
