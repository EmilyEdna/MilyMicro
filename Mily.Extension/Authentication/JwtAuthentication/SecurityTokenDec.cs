using Mily.Extension.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using XExten.XPlus;

namespace Mily.Extension.Authentication.JwtAuthentication
{
    public class SecurityTokenDec
    {
        /// <summary>
        /// 解析Token
        /// </summary>
        /// <param name="Security"></param>
        /// <returns></returns>
        public static Dictionary<string, string> SecurityToken(string Security)
        {
            Dictionary<string, string> HashData = new Dictionary<string, string>();
            Security = Security.Equals("undefined") ? string.Empty : Security;
            JwtSecurityToken Token = new JwtSecurityToken(Security);
            String KeyId = Token.Claims.FirstOrDefault(t => t.Type == "KeyId").Value;
            String RoleId = Token.Claims.FirstOrDefault(t => t.Type == "RoleId").Value;
            String UserName = Token.Claims.FirstOrDefault(t => t.Type == "UserName").Value;
            String UserRole = Token.Claims.FirstOrDefault(t => t.Type == "UserRole").Value;
            String ExpTime =XPlusEx.XConvertStamptime(Token.Claims.FirstOrDefault(t => t.Type == "exp").Value).ToString();
            HashData.Add("KeyId", KeyId);
            HashData.Add("RoleId", RoleId);
            HashData.Add("UserName", UserName);
            HashData.Add("UserRole", UserRole);
            HashData.Add("ExpTime", ExpTime);
            return HashData;
        }
    }
}
