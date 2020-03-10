using Microsoft.AspNetCore.Authorization;
using Mily.Setting.ModelEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XExten.XCore;

namespace Mily.Extension.Authentication.CookieAuthentication
{
    /// <summary>
    /// 验证操作权限
    /// </summary>
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                var UserClaim = context.User.Claims.ToList();
                if (UserClaim.Count > 0)
                {
                    String PrimaryKey = UserClaim.Where(t => t.Type == "KeyId").FirstOrDefault().Value;
                    String RolePromise = UserClaim.Where(t => t.Type == "RoleId").FirstOrDefault().Value;
                    String UserName = UserClaim.Where(t => t.Type == "UserName").FirstOrDefault().Value;
                    string UserRole = UserClaim.Where(t => t.Type == "UserRole").FirstOrDefault().Value;
                    if (requirement.Roles.Contains(UserRole))
                        context.Succeed(requirement);
                    await Task.CompletedTask;
                }
            }
        }
    }
}
