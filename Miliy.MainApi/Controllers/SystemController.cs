using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mily.Extension.Attributes;
using Mily.Extension.Attributes.RoleHandler;
using Mily.Setting.DbTypes;
using Mily.ViewModels;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using XExten.Common;

namespace Miliy.MainApi.Controllers
{
    /// <summary>
    /// 系统
    /// </summary>
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class SystemController : BaseApiController
    {
        #region 注册登录

        /// <summary>
        /// 注册后台管理员API
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public async Task<ActionResult<Object>> RegistAdmin(AdminRoleViewModel ViewModel) => await SysService.RegistAdmin(ViewModel);

        /// <summary>
        /// 登录后台API
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [AllowAnonymous]
        public async Task<ActionResult<Object>> Login(AdminRoleViewModel ViewModel)
        {
            var claimIdentity = new ClaimsIdentity("Cookie");
            var RoleAdmin = await SysService.Login(ViewModel);
            if (RoleAdmin == null)
                return new { Data = "登录失败，请检查用户名和密码是否正确!" ,Target=false};
            if (HttpContext != null)
            {
                claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, RoleAdmin.RolePermissionId.ToString()));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Name, RoleAdmin.AdminName));
                claimIdentity.AddClaim(new Claim(ClaimTypes.Role, RoleAdmin.HandlerRole));
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
        public async Task<ActionResult<Object>> DeleteAdmin(string Key) => await SysService.DeleteAdmin(Key);

        /// <summary>
        /// 真删除管理员API
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Delete)]
        public async Task<ActionResult<Object>> RemoveAdmin(string Key) => await SysService.RemoveAdmin(Key);

        /// <summary>
        /// 编辑管理员API
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Update)]
        public async Task<ActionResult<Object>> EditAdmin(AdminRoleViewModel ViewModel) => await SysService.EditAdmin(ViewModel);

        /// <summary>
        /// 恢复管理员数据API
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Update)]
        public async Task<ActionResult<Object>> RecoveryAdminData(string Key) => await SysService.RecoveryAdminData(Key);

        #endregion

        #region 获取菜单
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(Roles.Admin, Roles.Read)]
        public async Task<ActionResult<Object>> SearchMenuItem(string Key) => await SysService.SearchMenuItem(Guid.Parse(Key));
        #endregion
    }
}