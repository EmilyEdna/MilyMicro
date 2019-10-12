using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;

namespace Mily.Extension.Attributes.PermissionHandler
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