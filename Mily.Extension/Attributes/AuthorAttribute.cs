﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Mily.Extension.Attributes.PermissionHandler;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mily.Extension.Attributes
{
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
                context.Result = new ObjectResult(ResultApiMiddleWare.Instance(false, StatusCodes.Status401Unauthorized, null, "无权访问!"));
            }
        }
    }
}
