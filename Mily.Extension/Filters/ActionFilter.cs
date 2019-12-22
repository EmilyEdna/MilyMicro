using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mily.Extension.LoggerFactory;
using System.Linq;
using Mily.Setting;
using XExten.Common;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Mily.Setting.DbTypes;
using Mily.Extension.Infrastructure.Common;

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
            ResultCondition Result = ResultCondition.Instance(true, Context.HttpContext.Response.StatusCode, (Context.Result as ObjectResult).Value, "执行成功!");
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
            InvokeHeader(Request);
            if (ParamType?.GetType() == typeof(ResultProvider))
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
            if (ParamType?.GetType() == typeof(PageQuery))
            {

                Dictionary<String, Object> Query = new Dictionary<String, Object>();
                if (Request.Method == "POST")
                {
                    Request.Form.ToList().ForEach(item =>
                    {
                        if (item.Key.Contains("KeyWord"))
                        {
                            string Key = item.Key.Split("[")[1].Split("]")[0];
                            Query.Add(Key, item.Value.ToString());
                           
                        }
                    });
                    ((PageQuery)ParamType).KeyWord = Query;
                }
                else
                {
                    Request.Query.ToList().ForEach(item =>
                    {
                        if (item.Key.Contains("KeyWord"))
                        {
                            string Key = item.Key.Split("[")[1].Split("]")[0];
                            Query.Add(Key, item.Value.ToString());

                        }
                    });
                    ((PageQuery)ParamType).KeyWord = Query;
                }
            }
        }

        internal void InvokeHeader(HttpRequest Request)
        {
            int.TryParse(Request.Headers["ActionType"].ToString(), out int Target);
            if (Target > 5 && Target <= 0)
                MilyConfig.DbTypeAttribute = DBType.Default;
            else
                MilyConfig.DbTypeAttribute = (DBType)Target;
        }
    }
}