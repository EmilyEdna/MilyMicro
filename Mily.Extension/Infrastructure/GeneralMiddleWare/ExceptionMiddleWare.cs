using Microsoft.AspNetCore.Http;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mily.Extension.Infrastructure.GeneralMiddleWare
{
    /// <summary>
    /// 异常中间件
    /// </summary>
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate RequestDelegate;
        private readonly IDictionary<int, String> ExceptionMap;

        public ExceptionMiddleWare(RequestDelegate RequestDelegates)
        {
            RequestDelegate = RequestDelegates;
            ExceptionMap = new Dictionary<int, String>
            {
                { 400, "错误请求" },
                { 401, "未授权请求" },
                { 403, "拒绝授权请求" },
                { 404, "页面丢失" },
                { 500, "内部服务器错误" },
                { 501, "不支持的请求"},
                { 502, "错误的网关"},
                { 503, "服务不可用"},
                { 504, "网关超时" },
            };
        }

        public async Task Invoke(HttpContext Context)
        {
            Exception Ex = null;
            try
            {
                await RequestDelegate(Context);
            }
            catch (Exception ex)
            {
                Context.Response.Clear();
                Context.Response.StatusCode = 500;
                Ex = ex;
            }
            finally
            {
                if (ExceptionMap.ContainsKey(Context.Response.StatusCode) && !Context.Items.ContainsKey("ExceptionHandled"))
                {
                    string ErrorMsg;
                    if (Context.Response.StatusCode == 500 && Ex != null)
                        ErrorMsg = $"状态信息:{ExceptionMap[Context.Response.StatusCode]}";
                    else
                        ErrorMsg = ExceptionMap[Context.Response.StatusCode];
                    Ex = new Exception(ErrorMsg);
                    if (Ex != null)
                    {
                        ResultApiMiddleWare Result = new ResultApiMiddleWare() { IsSuccess = false, Info = Ex.Message, StatusCode = Context.Response.StatusCode };
                        Context.Response.ContentType = "application/json";
                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Result), Encoding.UTF8);
                    }
                }
            }
        }
    }
}