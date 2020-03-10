using Microsoft.AspNetCore.Authorization;
using Mily.Setting.ModelEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.Authentication.CookieAuthentication
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public List<RoleTypeEnum> Roles { get; set; }

        public PermissionAuthorizationRequirement(List<RoleTypeEnum> Param)
        {
            Roles = Param;
        }
    }
}
