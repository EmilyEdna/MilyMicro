using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

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
                    var UserClaim = context.User.Claims.ToList();
                    if (UserClaim.Count > 0)
                    {
                        String PrimaryKey = UserClaim.Where(t => t.Type == "KeyId").FirstOrDefault().Value;
                        String RolePromise = UserClaim.Where(t => t.Type == "RoleId").FirstOrDefault().Value;
                        String UserName = UserClaim.Where(t => t.Type == "UserName").FirstOrDefault().Value;
                        String UserRole = UserClaim.Where(t => t.Type == "UserRole").FirstOrDefault().Value;
                        UserRole.Split("|").ToList().ForEach(Item =>
                        {
                            if (requirement.Names.Contains(Item))
                                context.Succeed(requirement);
                        });
                        await Task.CompletedTask;
                    }
                }
            }
        }
    }
}