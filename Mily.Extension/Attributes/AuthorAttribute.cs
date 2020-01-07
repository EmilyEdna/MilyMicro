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

namespace Mily.Extension.Attributes
{
    /// <summary>
    /// 权限切入点
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class AuthorAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public List<String> Names { get; set; }

        public AuthorAttribute(params String[] Param)
        {
            Names = Param.ToList();
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorizationService = context.HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var authorizationResult = await authorizationService.AuthorizeAsync(context.HttpContext.User, null, new PermissionAuthorizationRequirement(Names));
            if (!authorizationResult.Succeeded)
            {
                context.Result = new ObjectResult(ResultCondition.Instance(false, StatusCodes.Status401Unauthorized, null, ResponseEnum.Unauthorized.GetDescriptionValue()));
            }
        }
    }
}