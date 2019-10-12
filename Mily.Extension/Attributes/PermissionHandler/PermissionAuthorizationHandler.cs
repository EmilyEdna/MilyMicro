using Microsoft.AspNetCore.Authorization;
using Mily.Extension.ViewModel;
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
                    var UserIdClaim = context.User.FindFirst(t => t.Type == ClaimTypes.NameIdentifier);
                    if (UserIdClaim != null)
                    {
                        AdminRoleViewModel AdminRole = await RedisCaches.StringGetAsync<AdminRoleViewModel>(typeof(AdminRoleViewModel).FullName);
                        if (AdminRole.RolePermissionId == Guid.Parse(UserIdClaim.Value))
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