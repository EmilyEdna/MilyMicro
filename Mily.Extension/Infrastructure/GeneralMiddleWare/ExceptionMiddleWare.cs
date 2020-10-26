using Microsoft.AspNetCore.Http;
using Mily.Extension.Infrastructure.Common;
using Mily.Setting;
using Mily.Setting.ModelEnum;
using Mily.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XExten.XCore;

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
            ExceptionMap = new Dictionary<int, String>();
            Array Stutas = Enum.GetValues(typeof(ResponseEnum));
            for (int Item = 0; Item < Stutas.Length; Item++)
            {
                var CodeResponse = (ResponseEnum)Stutas.GetValue(Item);
                ExceptionMap.Add((int)CodeResponse, CodeResponse.ToDescription());
            }
        }

        public async Task Invoke(HttpContext Context)
        {
            Exception Ex = null;
            try
            {
                if (!Context.Request.Path.Value.Contains("Login"))
                    MilyConfig.CacheKey = Context.Request.Headers["Global"].ToString().IsNullOrEmpty() ? String.Empty : Context.Request.Headers["Global"].ToString().ToLzStringDec().ToMD5();
                Context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
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
                        ErrorMsg = Context.Response.StatusCode == 200 ? string.Empty : ExceptionMap[Context.Response.StatusCode];
                    if (!Context.Request.Path.Value.Contains("swagger"))
                    {
                        if (Ex != null|| !ErrorMsg.IsNullOrEmpty())
                        {
                            ResultCondition Result = ResultCondition.Instance(Item =>
                            {
                                Item.IsSuccess = false;
                                Item.Info = ErrorMsg;
                                Item.StatusCode = Context.Response.StatusCode;
                                Item.ResultData = null;
                                Item.ServerDate = DateTime.Now.ToFmtDate(1);
                            });
                            Context.Response.ContentType = "application/json";
                            await Context.Response.WriteAsync(JsonConvert.SerializeObject(Result), Encoding.UTF8);
                        }
                    }
                }
            }
        }
    }
}