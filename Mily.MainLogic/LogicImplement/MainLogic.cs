using Mily.DbCore;
using Mily.DbCore.Model.SystemModel;
using Mily.Extension.ModelMapper;
using Mily.ViewModels;
using Mily.MainLogic.LogicInterface;
using Mily.Setting.ModelEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.Common;
using XExten.XCore;

namespace Mily.MainLogic.LogicImplement
{
    public class MainLogic : SugerDbContext, IMainLogic
    {
        #region 登录注册

        /// <summary>
        /// 注册后台管理员
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public async Task<Object> RegistAdmin(AdminRoleViewModel ViewModel)
        {
            Administrator Admin = ViewModel.ToMapper<AdminRoleViewModel, Administrator>();
            return await base.InsertData<Administrator>(Admin);
        }

        /// <summary>
        /// 登录后台管理员
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public async Task<AdminRoleViewModel> Login(AdminRoleViewModel ViewModel)
        {
            AdminRoleViewModel AdminRole = DbContext().Queryable<Administrator, RolePermission>((Admin, Role) => new Object[] { JoinType.Left, Admin.RolePermissionId == Role.KeyId })
                .Where(Admin => Admin.Account.Equals(ViewModel.Account))
                .Where(Admin => Admin.PassWord.Equals(ViewModel.PassWord))
                .Select((Admin, Role) => new AdminRoleViewModel
                {
                    KeyId = Admin.KeyId,
                    Account = Admin.Account,
                    AdminName = Admin.AdminName,
                    RolePermissionId = Admin.RolePermissionId,
                    HandlerRole = Role.HandlerRole,
                    RoleName = Role.RoleName
                }).First();
            if (AdminRole != null)
                await Caches.RedisCacheSetAsync(AdminRole.KeyId.ToString(), AdminRole, 120);
            return AdminRole;
        }

        #endregion

        #region 管理员

        /// <summary>
        /// 管理员分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public async Task<Object> SearchAdminPage(PageQuery Page)
        {
            return await DbContext().Queryable<Administrator>()
                .WhereIF(Page.KeyWord.ContainsKey("AdminName"), t => t.AdminName == Page.KeyWord["AdminName"].ToString())
                .Where(t => t.IsDelete == false).ToPageListAsync(Page.PageIndex, Page.PageSize);
        }

        /// <summary>
        /// 软删除管理员
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public async Task<Object> DeleteAdmin(string Key)
        {
            List<Administrator> administrator = DbContext().Queryable<Administrator>()
           .WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString()))
           .Where(t => t.IsDelete == false).ToList();
            return await base.AlterData<Administrator>(null, administrator, DbReturnTypes.AlterSoft);
        }

        /// <summary>
        /// 真删除管理员
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public async Task<Object> RemoveAdmin(string Key)
        {
            List<Administrator> administrator = DbContext().Queryable<Administrator>()
                .WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString())).ToList();
            return await base.RemoveData<Administrator>(null, administrator);
        }

        /// <summary>
        /// 编辑管理员
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public async Task<Object> EditAdmin(AdminRoleViewModel ViewModel)
        {
            Administrator administrator = ViewModel.AutoMapper<Administrator>();
            return await base.AlterData(administrator, null, DbReturnTypes.AlterDefault, null, t => t.KeyId == ViewModel.KeyId);
        }

        /// <summary>
        /// 恢复管理员数据
        /// </summary>
        /// <returns></returns>
        public async Task<Object> RecoveryAdminData(string Key)
        {
            List<Administrator> administrator = DbContext().Queryable<Administrator>()
                .WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString()))
                .Where(t => t.IsDelete == true).ToList();
            return await base.AlterData<Administrator>(null, administrator, DbReturnTypes.AlterSoft, false);
        }

        #endregion

        #region 菜单管理

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public async Task<Object> SearchMenuItem()
        {
            return await DbContext().Queryable<MenuItems>().Where(Item => Item.Lv == MenuItemEnum.Lv1).Mapper((Item, Cache) =>
             {
                 Item.ChildMenus = DbContext().Queryable<MenuItems>().Where(VModel => VModel.ParentId == Item.KeyId).Where(t => t.Lv == MenuItemEnum.Lv2).ToList();
                 Item.ChildMenus.ForEach(Menus =>
                 {
                     Menus.ChildMenus = DbContext().Queryable<MenuItems>().Where(VModel => VModel.ParentId == Menus.KeyId).Where(t => t.Lv == MenuItemEnum.Lv3).ToList();
                 });
             }).ToListAsync();
        }

        #endregion
    }
}