using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.Authentication.CookieAuthentication
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        public List<String> Names { get; set; }

        public PermissionAuthorizationRequirement(List<String> Param)
        {
            Names = Param;
        }
    }
}
