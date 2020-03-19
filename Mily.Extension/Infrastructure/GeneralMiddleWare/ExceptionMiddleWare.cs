using Microsoft.AspNetCore.Http;
using Mily.Extension.Infrastructure.Common;
using Mily.Setting.ModelEnum;
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
                        ResultCondition Result = ResultCondition.Instance(Item =>
                        {
                            Item.IsSuccess = false;
                            Item.Info = Ex.Message;
                            Item.StatusCode = Context.Response.StatusCode;
                            Item.ResultData = null;
                            Item.ServerDateStr = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff毫秒");
                            Item.ServerDateLong = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssffff"));
                        });
                        Context.Response.ContentType = "application/json";
                        await Context.Response.WriteAsync(JsonConvert.SerializeObject(Result), Encoding.UTF8);
                    }
                }
            }
        }
    }
}