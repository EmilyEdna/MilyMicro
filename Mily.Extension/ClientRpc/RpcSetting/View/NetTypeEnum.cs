using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mily.Extension.ClientRpc.RpcSetting.View
{
    public enum NetTypeEnum
    {
        /// <summary>
        /// 链接
        /// </summary>
        [Description("链接")]
        Connect = 1,
        /// <summary>
        /// 监听
        /// </summary>
        [Description("监听")]
        Listened = 2,
        /// <summary>
        /// 断链
        /// </summary>
        [Description("断链")]
        DisConnect = 3,
        /// <summary>
        /// 回调
        /// </summary>
        [Description("回调")]
        CallBack =4,
    }
}
