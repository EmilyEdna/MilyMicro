using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Mily.Setting.ModelEnum
{
    /// <summary>
    /// 响应枚举
    /// </summary>
    public enum ResponseEnum
    {
        [Description("请求成功")]
        请求成功 =200,
        [Description("错误请求")]
        错误请求 = 400,
        [Description("未授权请求")]
        未授权请求 = 401,
        [Description("拒绝授权请求")]
        拒绝授权请求 = 403,
        [Description("页面丢失")]
        页面丢失 = 404,
        [Description("内部服务器错误")]
        内部服务器错误 = 500,
        [Description("不支持的请求")]
        不支持的请求 = 501,
        [Description("错误的网关")]
        错误的网关 = 502,
        [Description("服务不可用")]
        服务不可用 = 503,
        [Description("网关超时")]
        网关超时 = 504
    }
}
