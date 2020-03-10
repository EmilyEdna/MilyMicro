using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mily.Extension.Attributes;
using Mily.Extension.Authentication.JwtAuthentication;
using Mily.Extension.Constant;
using Mily.Setting.ModelEnum;
using System;
using System.Threading.Tasks;
using XExten.Common;

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
            var RoleAdmin = await SysService.Login(Provider);
            String AuthorToken = null;
            if (RoleAdmin == null)
                return new { Data = "登录失败，请检查用户名和密码是否正确!", AuthorToken };
            else
                AuthorToken = JsonWebToken.InitToken(option =>
                 {
                     option.KeyId = RoleAdmin.KeyId.Value;
                     option.RoleId = RoleAdmin.RolePermissionId.Value;
                     option.UserName = RoleAdmin.AdminName;
                     option.RoleType = RoleAdmin.RoleType;
                 });
            return new { Data = RoleAdmin, AuthorToken };
        }

        #endregion

        #region 管理员

        /// <summary>
        /// 管理员分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> SearchAdminPage(PageQuery Page) => await SysService.SearchAdminPage(Page);

        /// <summary>
        /// 逻辑删除管理员
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> DeleteAdmin(ResultProvider Provider) => await SysService.DeleteAdmin(Provider);

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> RemoveAdmin(ResultProvider Provider) => await SysService.RemoveAdmin(Provider);

        /// <summary>
        /// 编辑管理员
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> EditAdmin(ResultProvider Provider) => await SysService.EditAdmin(Provider);

        /// <summary>
        /// 恢复管理员数据
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> RecoveryAdminData(ResultProvider Provider) => await SysService.RecoveryAdminData(Provider);

        #endregion

        #region 获取菜单
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> GetMenuItem(ResultProvider Provider) => await SysService.GetMenuItem(Provider);

        /// <summary>
        /// 获取菜单功能
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> GetMenuFeatures(ResultProvider Provider) => await SysService.GetMenuFeatures(Provider);

        /// <summary>
        /// 获取菜单路由
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> GetMenuRouter(ResultProvider Provider)=> await SysService.GetMenuRouter(Provider);

        #region 新增
        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<Object> InsertMenuItem(ResultProvider Provider) => await SysService.InsertMenuItem(Provider);


        /// <summary>
        /// 新增功能菜单
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> InsertMenuFeatures(ResultProvider Provider) => await SysService.InsertMenuFeatures(Provider);

        /// <summary>
        /// 新增菜单路由
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> InsertMenuRouter(ResultProvider Provider) => await SysService.InsertMenuRouter(Provider);


        /// <summary>
        /// 新增菜单功能路由
        /// </summary>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> InsertMenuFeatsRouter(ResultProvider Provider) => await SysService.InsertMenuFeatsRouter(Provider);
        #endregion

        /// <summary>
        /// 逻辑删除菜单
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> DeleteMenuItem(ResultProvider Provider) => await SysService.DeleteMenuItem(Provider);

        /// <summary>
        /// 菜单分页查询
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        [AcceptVerbs("GET", "POST")]
        [Author(RoleTypeEnum.Administrator)]
        [Authorize]
        public async Task<ActionResult<Object>> SearchMenuItemPage(PageQuery Page) => await SysService.SearchMenuItemPage(Page);
        #endregion
    }
}