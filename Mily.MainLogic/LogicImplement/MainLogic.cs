using Mily.DbCore;
using Mily.MainLogic.LogicInterface;
using Mily.Setting.ModelEnum;
using Mily.ViewModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.Common;
using XExten.XCore;
using System.Linq;
using Mily.DbEntity.SystemView;

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
        public async Task<Object> RegistAdmin(ResultProvider Provider)
        {
            Administrator Admin = Provider.DictionaryStringProvider.ToJson().ToModel<AdminRoleViewModel>().ToAutoMapper<Administrator>();
            return await base.InsertData<Administrator>(Admin);
        }

        /// <summary>
        /// 登录后台管理员
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public async Task<AdminRoleViewModel> Login(ResultProvider Provider)
        {
            AdminRoleViewModel ViewModel = Provider.DictionaryStringProvider.ToJson().ToModel<AdminRoleViewModel>();
            AdminRoleViewModel AdminRole = await DbContext().Queryable<Administrator, RolePermission>((Admin, Role) => new Object[] { JoinType.Left, Admin.RolePermissionId == Role.KeyId })
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
                }).FirstAsync();
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
                .WhereIF(!Page.KeyWord["AdminName"].IsNullOrEmpty(), t => t.AdminName == Page.KeyWord["AdminName"].ToString())
                .Where(t => t.Deleted == false).ToPageListAsync(Page.PageIndex, Page.PageSize);
        }

        /// <summary>
        /// 逻辑删除管理员
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public async Task<Object> DeleteAdmin(ResultProvider Provider)
        {
            string Key = Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString();
            List<Administrator> administrator = DbContext().Queryable<Administrator>()
                .WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString())).Where(t => t.Deleted == false).ToList();
            return await base.LogicDeleteOrRecovery<Administrator>(administrator, true);
        }

        /// <summary>
        /// 真删除管理员
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public async Task<Object> RemoveAdmin(ResultProvider Provider)
        {
            string Key = Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString();
            List<Administrator> administrator = DbContext().Queryable<Administrator>()
                .WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString())).ToList();
            return await base.RemoveData<Administrator>(administrator);
        }

        /// <summary>
        /// 编辑管理员
        /// </summary>
        /// <param name="ViewModel"></param>
        /// <returns></returns>
        public async Task<Object> EditAdmin(ResultProvider Provider)
        {
            Administrator Admin = Provider.DictionaryStringProvider.ToJson().ToModel<AdminRoleViewModel>().ToAutoMapper<Administrator>();
            return await base.AlterData(Admin, null, DbReturnEnum.AlterDefault, null, t => t.KeyId == Admin.KeyId);
        }

        /// <summary>
        /// 恢复管理员数据
        /// </summary>
        /// <returns></returns>
        public async Task<Object> RecoveryAdminData(ResultProvider Provider)
        {
            string Key = Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString();
            List<Administrator> administrator = DbContext().Queryable<Administrator>()
                .WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString()))
                .Where(t => t.Deleted == true).ToList();
            return await base.LogicDeleteOrRecovery<Administrator>(administrator, false);
        }

        #endregion

        #region 菜单管理

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<Object> GetMenuItem(ResultProvider Provider)
        {
            Guid Key = Guid.Parse(Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString());
            return await DbContext().Queryable<RoleMenuItems, MenuItems>((Role, Menu) => new Object[] { JoinType.Left, Role.MenuItemsId == Menu.KeyId })
               .Where(Role => Role.RolePermissionId == Key).Select((Role, Menu) => new MenuItems
               {
                   KeyId = Menu.KeyId,
                   RouterPath = Menu.RouterPath,
                   Path = Menu.Path,
                   Lv = Menu.Lv,
                   Icon = Menu.Icon,
                   Title = Menu.Title,
                   ParentId = Menu.ParentId,
                   Parent = Menu.Parent,
                   Created = Menu.Created,
                   Deleted = Menu.Deleted
               }).MergeTable().Where(Item => Item.Lv == MenuItemEnum.Lv1).Mapper(Item =>
               {
                   Item.ChildMenus = DbContext().Queryable<MenuItems>().Where(VModel => VModel.ParentId == Item.KeyId).Where(t => t.Lv == MenuItemEnum.Lv2).ToList();
                   Item.ChildMenus.ForEach(Menus =>
                   {
                       Menus.ChildMenus = DbContext().Queryable<MenuItems>().Where(VModel => VModel.ParentId == Menus.KeyId).Where(t => t.Lv == MenuItemEnum.Lv3).ToList();
                   });
               }).ToListAsync();
        }

        /// <summary>
        /// 新增菜单
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public async Task<Object> InsertMenuItem(ResultProvider Provider)
        {
            MenuItems Menu = Provider.DictionaryStringProvider.ToJson().ToModel<MenuItems>();
            return await base.InsertData<MenuItems>(Menu);
        }

        /// <summary>
        /// 逻辑删除菜单
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public async Task<Object> DeleteMenuItem(ResultProvider Provider)
        {
            string Key = Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString();
            List<MenuItems> Items = DbContext().Queryable<MenuItems>().WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString()))
                  .Where(t => t.Deleted == false).ToList();
            return await base.LogicDeleteOrRecovery(Items, true);
        }

        /// <summary>
        /// 菜单分页
        /// </summary>
        /// <param name="Page"></param>
        /// <returns></returns>
        public async Task<Object> SearchMenuItemPage(PageQuery Page)
        {
            return await DbContext().Queryable<RoleMenuItems, MenuItems>((Role, Menu) => new Object[] { JoinType.Left, Role.MenuItemsId == Menu.KeyId })
                     .Where((Role, Menu) => Role.Deleted == false && Menu.Deleted == false)
                     .OrderBy((Role, Menu)=>Menu.Lv,OrderByType.Asc)
                     .WhereIF(!Page.KeyWord["Title"].IsNullOrEmpty(), (Role, Menu) => Menu.Title.Contains(Page.KeyWord["Title"].ToString()))
                     .WhereIF(!Page.KeyWord["MenuLv"].IsNullOrEmpty(), (Role, Menu) => Menu.Lv == (MenuItemEnum)Page.KeyWord["MenuLv"])
                     .Select((Role, Menu) => new MenuItems
                     {
                         KeyId = Menu.KeyId,
                         RouterPath = Menu.RouterPath,
                         Path = Menu.Path,
                         Lv = Menu.Lv,
                         Icon = Menu.Icon,
                         Title = Menu.Title,
                         ParentId = Menu.ParentId,
                         Parent = Menu.Parent,
                         Created = Menu.Created,
                         Deleted = Menu.Deleted
                     }).ToPageListAsync(Page.PageIndex, Page.PageSize);
        }
        #endregion
    }
}