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
using Mily.DbEntity.SystemView.RoleSeries;
using Mily.DbEntity.SystemView.MenuSeries;

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
                    RoleType = Role.RoleType,
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
            Guid RoleId = Guid.Parse(Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString());
            List<MenuItems> MenuLv2 = DbContext().Queryable<MenuItems>().Where(Item => Item.Lv == MenuItemEnum.Lv2).ToList();
            List<MenuItems> MenuLv3 = DbContext().Queryable<MenuItems>().Where(Item => Item.Lv == MenuItemEnum.Lv3).ToList();
            return await DbContext().Queryable<RoleMenuItems, MenuItems>((Role, Menu) => new Object[] { JoinType.Left, Role.MenuItemsId == Menu.KeyId })
               .Where(Role => Role.RolePermissionId == RoleId).Select((Role, Menu) => new RoleMenuItemViewModel
               {
                   KeyId = Menu.KeyId,
                   Lv = Menu.Lv,
                   Icon = Menu.Icon,
                   Title = Menu.Title,
                   ParentId = Menu.ParentId,
                   Parent = Menu.Parent,
                   Created = Menu.Created,
                   Deleted = Menu.Deleted
               }).MergeTable().Where(Item => Item.Lv == MenuItemEnum.Lv1).Mapper(Item =>
               {
                   Item.ChildMenus = MenuLv2.Where(VModel => VModel.ParentId == Item.KeyId).ToMappers<MenuItems, RoleMenuItemViewModel>();
                   Item.ChildMenus.ForEach(Menus =>
                   {
                       Menus.ChildMenus = MenuLv3.Where(VModel => VModel.ParentId == Menus.KeyId).ToMappers<MenuItems, RoleMenuItemViewModel>();
                   });
               }).ToListAsync();
        }

        /// <summary>
        /// 获取菜单功能
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public async Task<Object> GetMenuFeatures(ResultProvider Provider)
        {
            Guid MenuId = Guid.Parse(Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString());
            return await DbContext().Queryable<MenuItems, MenuFeatures>((Items, Feats) => new Object[] { JoinType.Left, Items.KeyId == Feats.MenuItemId })
                .Where(Items => Items.KeyId == MenuId).Select((Items, Feats) => new RoleMenuFeatsViewModel
                {
                    KeyId = Feats.KeyId,
                    MenuItemId = Feats.MenuItemId,
                    FeatName = Feats.FeatName,
                    EnableOrDisable = Feats.EnableOrDisable,
                    Icon = Feats.Icon
                }).ToListAsync();
        }

        /// <summary>
        /// 获取菜单路由
        /// </summary>
        /// <returns></returns>
        public async Task<Object> GetMenuRouter(ResultProvider Provider)
        {
            Guid RoleId = Guid.Parse(Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString());
            return await DbContext().Queryable<RoleMenuItems, MenuItemsRouter>((Item, ItemRouter) => new Object[] { JoinType.Left, Item.MenuItemsId == ItemRouter.MenuItemId })
                 .Where((Item, ItemRouter) => Item.RolePermissionId == RoleId).Select((Item, ItemRouter) => new RoleMenuRouterViewModel
                 {
                     MenuItemId = ItemRouter.MenuItemId,
                     Title = ItemRouter.Title,
                     MenuPath = ItemRouter.PathRoad,
                     RouterPath = ItemRouter.PathRouter
                 }).MergeTable().Where(Item=> Item.MenuItemId != null).Mapper(Item =>
                 {
                     Item.ChildFeatures = DbContext().Queryable<RoleMenuFeatures, MenuFeaturesRouter>((Feat, FeatRouter) => new Object[] { JoinType.Inner, Feat.MenuFeatId == FeatRouter.MenuFeatId })
                       .Where((Feat, FeatRouter) => Feat.RolePermissionId == RoleId && Feat.MenuItemId == Item.MenuItemId).Select((Feat, FeatRouter) => new RoleMenuRouterViewModel
                       {
                           Title = FeatRouter.Title,
                           MenuPath = FeatRouter.PathRoad,
                           RouterPath = FeatRouter.PathRouter
                       }).ToList();
                 }).ToListAsync();
        }

        #region 新增
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
        /// 新增功能菜单
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public async Task<Object> InsertMenuFeatures(ResultProvider Provider)
        {
            MenuFeatures MenuFeats = Provider.DictionaryStringProvider.ToJson().ToModel<MenuFeatures>();
            return await base.InsertData<MenuFeatures>(MenuFeats);
        }

        /// <summary>
        /// 新增菜单路由
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public async Task<Object> InsertMenuRouter(ResultProvider Provider)
        {
            MenuItemsRouter Router = Provider.DictionaryStringProvider.ToJson().ToModel<MenuItemsRouter>();
            return await base.InsertData<MenuItemsRouter>(Router);
        }

        /// <summary>
        /// 新增菜单功能路由
        /// </summary>
        /// <returns></returns>
        public async Task<Object> InsertMenuFeatsRouter(ResultProvider Provider)
        {
            MenuFeaturesRouter FeatsRouter = Provider.DictionaryStringProvider.ToJson().ToModel<MenuFeaturesRouter>();
            return await base.InsertData<MenuFeaturesRouter>(FeatsRouter);
        }
        #endregion

        /// <summary>
        /// 逻辑删除菜单
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public async Task<Object> DeleteMenuItem(ResultProvider Provider)
        {
            string Key = Provider.DictionaryStringProvider.Values.FirstOrDefault().ToString();
            List<MenuItems> Items = DbContext().Queryable<MenuItems>().WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString())).ToList();
            return await base.LogicDeleteOrRecovery(Items, true, null, null, t => t.Deleted == false);
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
                     .OrderBy((Role, Menu) => Menu.Lv, OrderByType.Asc)
                     .WhereIF(!Page.KeyWord["Title"].IsNullOrEmpty(), (Role, Menu) => Menu.Title.Contains(Page.KeyWord["Title"].ToString()))
                     .WhereIF(!Page.KeyWord["MenuLv"].IsNullOrEmpty(), (Role, Menu) => Menu.Lv == (MenuItemEnum)Page.KeyWord["MenuLv"])
                     .Select((Role, Menu) => new RoleMenuItemViewModel
                     {
                         KeyId = Menu.KeyId,
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