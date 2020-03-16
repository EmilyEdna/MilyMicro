using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Mily.Extension.Authentication.CookieAuthentication;
using Mily.Extension.Infrastructure.Common;
using Mily.Setting.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XExten.XCore;

namespace Mily.Extension.Attributes
{
    /// <summary>
    /// 权限切入点
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public List<String> Roles { get; set; }

        public AuthorAttribute(params RoleTypeEnum[] Param)
        {
            Roles = Param.Select(Item => Item.ToString()).ToList();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new PermissionAuthorizationRequirement(Roles));
            if (!authorizationResult.Succeeded)
            {
                ResultCondition Result = ResultCondition.Instance(Item =>
                {
                    Item.IsSuccess = false;
                    Item.StatusCode = StatusCodes.Status401Unauthorized;
                    Item.ResultData = null;
                    Item.Info = ResponseEnum.Unauthorized.ToDescription();
                    Item.ServerDate = DateTime.Now.ToString("yyyy-MM-dd HH：mm：ss：ffff");
                });
                context.Result = new ObjectResult(Result);
            }
        }
    }
}