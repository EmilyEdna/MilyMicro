using Microsoft.AspNetCore.Authorization;
using Mily.ViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using XExten.CacheFactory.RedisCache;

namespace Mily.Extension.Attributes.PermissionHandler
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
    {
        protected async override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement)
        {
            if (context.User != null)
            {
                if (context.User.IsInRole("Admin"))
                    context.Succeed(requirement);
                else
                {
                    var UserIdClaim = context.User.Claims.ToList();
                    if (UserIdClaim.Count > 0)
                    {
                        String PrimaryKey = UserIdClaim.Where(t => t.Type == ClaimTypes.Authentication).FirstOrDefault().Value;
                        Guid RolePromise = Guid.Parse(UserIdClaim.Where(t => t.Type == ClaimTypes.Role).FirstOrDefault().Value);
                        String UserName = UserIdClaim.Where(t => t.Type == ClaimTypes.Name).FirstOrDefault().Value;
                        AdminRoleViewModel AdminRole = await RedisCaches.StringGetAsync<AdminRoleViewModel>(PrimaryKey);
                        if (AdminRole.RolePermissionId == RolePromise && AdminRole.AdminName == UserName) 
                        {
                            AdminRole.HandlerRole.Split('|').ToList().ForEach(t =>
                            {
                                if (requirement.Names.Contains(t))
                                    context.Succeed(requirement);
                            });
                        }
                    }
                }
            }
        }
    }
}