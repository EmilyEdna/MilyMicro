using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using Mily.Extension.LoggerFactory;
using System.Linq;
using XExten.XCore;
using Mily.Setting;
using XExten.Common;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Mily.Extension.Filters
{
    public class ActionFilter : IActionFilter
    {
        /// <summary>
        /// 第四执行
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext Context)
        {
            if (Context.Exception != null)
            {
                string Path = Context.Exception.Source;
                string WebPath = Context.HttpContext.Request.Path;
                string MethodName = Context.Exception.TargetSite.Name;
                string Parameter = string.Empty;
                string Message = Context.Exception.Message;
                Context.Exception.TargetSite.GetParameters().ToList().ForEach(t =>
                {
                    Parameter += "[" + t.Name + "]";
                });
                LogFactoryExtension.WriteError(Path, MethodName, Parameter, Message, WebPath);
                return;
            }
            ResultApiMiddleWare Result = ResultApiMiddleWare.Instance(true, Context.HttpContext.Response.StatusCode, (Context.Result as ObjectResult).Value, "执行成功!");
            Context.Result = new ObjectResult(Result);
        }

        /// <summary>
        /// 第三执行
        /// </summary>
        /// <param name="Context"></param>
        public void OnActionExecuting(ActionExecutingContext Context)
        {
            Object ParamType = Context.ActionArguments.Values.FirstOrDefault();
            HttpRequest Request = Context.HttpContext.Request;
            if (ParamType.GetType() == typeof(ResultProvider))
            {
                Dictionary<String, Object> DictionaryStringProvider = new Dictionary<String, Object>();
                if (Request.Method == "POST")
                {
                    Request.Form.ToList().ForEach(item =>
                    {
                        DictionaryStringProvider.Add(item.Key, item.Value.ToString());
                    });
                    ((ResultProvider)ParamType).DictionaryStringProvider = DictionaryStringProvider;
                }
                else
                {
                    Request.Query.ToList().ForEach(item =>
                    {
                        DictionaryStringProvider.Add(item.Key, item.Value.ToString());
                    });
                    ((ResultProvider)ParamType).DictionaryStringProvider = DictionaryStringProvider;
                }
            }
        }
    }
}