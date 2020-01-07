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
        /// <summary>
        /// 请求成功
        /// </summary>
        [Description("请求成功")]
        OK =200,
        /// <summary>
        /// 错误请求
        /// </summary>
        [Description("错误请求")]
        BadRequest = 400,
        /// <summary>
        /// 未授权请求
        /// </summary>
        [Description("未授权请求")]
        Unauthorized = 401,
        /// <summary>
        /// 拒绝授权请求
        /// </summary>
        [Description("拒绝授权请求")]
        Forbidden = 403,
        /// <summary>
        /// 页面丢失
        /// </summary>
        [Description("页面丢失")]
        NotFound = 404,
        /// <summary>
        /// 方法不被允许
        /// </summary>
        [Description("方法不被允许")]
        MethodNotAllowed=405,
        /// <summary>
        /// 请求超时
        /// </summary>
        [Description("请求超时")]
        RequestTimeout = 408,
        /// <summary>
        /// 内部服务器错误
        /// </summary>
        [Description("内部服务器错误")]
        InternalServerError = 500,
        /// <summary>
        /// 不支持的请求
        /// </summary>
        [Description("不支持的请求")]
        NotImplemented = 501,
        /// <summary>
        /// 错误的网关
        /// </summary>
        [Description("错误的网关")]
        BadGateway = 502,
        /// <summary>
        /// 服务不可用
        /// </summary>
        [Description("服务不可用")]
        ServiceUnavailable = 503,
        /// <summary>
        /// 网关超时
        /// </summary>
        [Description("网关超时")]
        GatewayTimeout = 504
    }
}
