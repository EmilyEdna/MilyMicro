using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using Mily.Extension.LoggerFactory;
using System.Linq;

namespace Mily.Extension.Filters
{
    public class ActionFilter : IActionFilter
    {
        /// <summary>
        /// 第四执行
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                string Path = context.Exception.Source;
                string WebPath = context.HttpContext.Request.Path;
                string MethodName = context.Exception.TargetSite.Name;
                string Parameter = string.Empty;
                string Message = context.Exception.Message;
                context.Exception.TargetSite.GetParameters().ToList().ForEach(t =>
                {
                    Parameter += "[" + t.Name + "]";
                });
                LogFactoryExtension.WriteError(Path, MethodName, Parameter, Message, WebPath);
                return;
            }
            ResultApiMiddleWare Result = ResultApiMiddleWare.Instance(true, context.HttpContext.Response.StatusCode, (context.Result as ObjectResult).Value, "执行成功!");
            context.Result = new ObjectResult(Result);
        }

        /// <summary>
        /// 第三执行
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}