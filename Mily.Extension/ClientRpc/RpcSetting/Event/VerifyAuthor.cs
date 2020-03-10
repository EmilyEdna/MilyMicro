using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Mily.Extension.Attributes;
using XExten.XCore;
using Mily.Extension.Authentication.JwtAuthentication;

namespace Mily.Extension.ClientRpc.RpcSetting.Event
{
    public class VerifyAuthor
    {
        /// <summary>
        /// 校验权限
        /// </summary>
        /// <param name="CtrlMethod"></param>
        /// <param name="Security"></param>
        /// <returns></returns>
        internal static bool Verify(MethodInfo CtrlMethod, String Security)
        {
            Attribute AllowAnonymous = CtrlMethod.GetCustomAttributes().Where(Item => Item.GetType() == typeof(AllowAnonymousAttribute)).FirstOrDefault();
            if (AllowAnonymous != null) return true;
            if (Security.IsNullOrEmpty()) return false;
            Attribute Authorize = CtrlMethod.GetCustomAttributes().Where(Item => Item.GetType() == typeof(AuthorizeAttribute)).FirstOrDefault();
            Attribute Author = CtrlMethod.GetCustomAttributes().Where(Item => Item.GetType() == typeof(AuthorAttribute)).FirstOrDefault();
            if (Authorize == null) return false;
            Dictionary<string, string> SecurityData = SecurityTokenDec.SecurityToken(Security);
            if (!VerifyExp(SecurityData)) return false;
            if (Author == null) return false;
            foreach (var Item in (Author as AuthorAttribute).Roles)
            {
                if (SecurityData["UserRole"] == Item)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 判断是否过期
        /// </summary>
        /// <param name="SecurityData"></param>
        /// <returns></returns>
        internal static bool VerifyExp(Dictionary<string, string> SecurityData)
        {
            DateTime ExpTime = Convert.ToDateTime(SecurityData["ExpTime"]);
            if (DateTime.Now > ExpTime) return false;
            else return true;
        }
    }
}
