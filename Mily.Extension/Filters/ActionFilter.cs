using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mily.Extension.LoggerFactory;
using System.Linq;
using Mily.Setting;
using XExten.Common;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using Mily.Extension.Infrastructure.Common;
using Mily.Setting.ModelEnum;
using XExten.XCore;

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
            ResultCondition Condition = ResultCondition.Instance(Item =>
            {
                Item.IsSuccess = true;
                Item.StatusCode = Context.HttpContext.Response.StatusCode;
                Item.ResultData = (Context.Result as ObjectResult).Value;
                Item.Info = "执行成功!";
                Item.ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            });
            ResultCondition Result = Condition;
            Context.Result = new ObjectResult(Result);
        }

        /// <summary>
        /// 第三执行
        /// </summary>
        /// <param name="Context"></param>
        public void OnActionExecuting(ActionExecutingContext Context)
        {
            Object ParamType = Context.ActionArguments.Values.FirstOrDefault();
            var ContentType = Context.HttpContext.Request.ContentType;
            if (!ContentType.IsNullOrEmpty() && ContentType.Contains("application/json"))
                return;
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
                        if (!item.Key.Contains("Provider"))
                            DictionaryStringProvider.Add(item.Key, item.Value.ToString());
                    });
                    ((ResultProvider)ParamType).DictionaryStringProvider = DictionaryStringProvider;
                }
            }
            if (ParamType?.GetType() == typeof(PageQuery))
            {
                Dictionary<String, Object> Query = new Dictionary<String, Object>();
                Request.Query.ToList().ForEach(item =>
                {
                    var Key = item.Key.ToLower();
                    if (!Key.Contains("pagesize") && !Key.Contains("pageindex"))
                        Query.Add(Key, item.Value.ToString());
                });
                ((PageQuery)ParamType).KeyWord = Query;
            }
        }

        internal void InvokeHeader(HttpRequest Request)
        {
            int.TryParse(Request.Headers["ActionType"].ToString(), out int Target);
            if (Target > 5 && Target <= 0)
                MilyConfig.DbTypeAttribute = DBTypeEnum.Default;
            else
                MilyConfig.DbTypeAttribute = (DBTypeEnum)Target;
        }
    }
}