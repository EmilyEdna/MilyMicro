using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Mily.Extension.Authentication.JwtAuthentication
{
    /// <summary>
    /// 自定义验证Token
    /// </summary>
    public class VerifyToken : ISecurityTokenValidator
    {
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }

        public bool CanReadToken(string securityToken) => true;

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            JwtSecurityToken Token = new JwtSecurityToken(securityToken);
            String KeyId = Token.Claims.FirstOrDefault(t => t.Type == "KeyId").Value;
            String RoleId = Token.Claims.FirstOrDefault(t => t.Type == "RoleId").Value;
            String UserName = Token.Claims.FirstOrDefault(t => t.Type == "UserName").Value;
            String UserRole = Token.Claims.FirstOrDefault(t => t.Type == "UserRole").Value;
            ClaimsIdentity claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            claimIdentity.AddClaim(new Claim("KeyId", KeyId));
            claimIdentity.AddClaim(new Claim("RoleId", RoleId));
            claimIdentity.AddClaim(new Claim("UserName", UserName));
            claimIdentity.AddClaim(new Claim("UserRole", UserRole));
            validatedToken = Token;
            return  new ClaimsPrincipal(claimIdentity);
        }
    }
}
