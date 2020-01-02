﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mily.Extension.Attributes;
using Mily.Extension.Attributes.RoleHandler;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using XExten.Common;
using XExten.XCore;

namespace Miliy.MainApi.Controllers
{
    /// <summary>
    /// 系统
    /// </summary>
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class SystemController : Controller
    {
        #region 注册登录

        /// <summary>
        /// 注册后台管理员API
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public async Task<ActionResult<Object>> RegistAdmin(ResultProvider Provider) => await SysService.RegistAdmin(Provider);

        /// <summary>
        /// 登录后台API
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public async Task<ActionResult<Object>> Login(ResultProvider Provider)
        {
            var claimIdentity = new ClaimsIdentity("Cookie");
            var RoleAdmin = await SysService.Login(Provider);
            if (RoleAdmin == null)
                return new { Data = "登录失败，请检查用户名和密码是否正确!", Target = false };
            if (HttpContext != null)
            {
                claimIdentity.AddClaim(new Claim(ClaimTypes.Authentication, RoleAdmin.KeyId.ToString()));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Role, RoleAdmin.RolePermissionId.ToString()));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, RoleAdmin.AdminName));
                await HttpContext.SignInAsync(new ClaimsPrincipal(claimIdentity), new AuthenticationProperties { IsPersistent = true });
            }
            return new { Data = RoleAdmin, Target = true };
        }

        #endregion

        #region 管理员

        /// <summary>
        /// 管理员分页API
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Read)]
        public async Task<ActionResult<Object>> SearchAdminPage(PageQuery Page) => await SysService.SearchAdminPage(Page);

        /// <summary>
        /// 软删除管理员API
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Delete)]
        public async Task<ActionResult<Object>> DeleteAdmin(ResultProvider Provider) => await SysService.DeleteAdmin(Provider);

        /// <summary>
        /// 真删除管理员API
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Delete)]
        public async Task<ActionResult<Object>> RemoveAdmin(ResultProvider Provider) => await SysService.RemoveAdmin(Provider);

        /// <summary>
        /// 编辑管理员API
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Update)]
        public async Task<ActionResult<Object>> EditAdmin(ResultProvider Provider) => await SysService.EditAdmin(Provider);

        /// <summary>
        /// 恢复管理员数据API
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Update)]
        public async Task<ActionResult<Object>> RecoveryAdminData(ResultProvider Provider) => await SysService.RecoveryAdminData(Provider);

        #endregion

        #region 获取菜单
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Read)]
        public async Task<ActionResult<Object>> SearchMenuItem(ResultProvider Provider) => await SysService.SearchMenuItem(Provider);
        #endregion
    }
}