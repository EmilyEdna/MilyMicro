using Mily.DbCore;
using Mily.DbCore.Model.SystemModel;
using Mily.Extension.ModelMapper;
using Mily.Extension.ViewModel;
using Mily.MainLogic.LogicInterface;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.Common;
using XExten.XCore;

namespace Mily.MainLogic.LogicImplement
{
    public class MainLogic: SugerDbContext ,IMainLogic
    {
        #region 管理员
        /// <summary>
        /// 管理员分页
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public async Task<Object> SearchAdminPage(PageQuery Page)
        {
            return await Emily.Queryable<Administrator>()
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
            List<Administrator> administrator = Emily.Queryable<Administrator>()
           .WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString()))
           .Where(t => t.IsDelete == false).ToList();
            return await base.AlterData<Administrator>(null, administrator, DBType.MSSQL, DbReturnTypes.AlterSoft);
        }
        /// <summary>
        /// 真删除管理员
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public async Task<Object> RemoveAdmin(string Key)
        {
            List<Administrator> administrator = Emily.Queryable<Administrator>()
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
            return await base.AlterData(administrator, null, DBType.MSSQL, DbReturnTypes.AlterDefault, null, t => t.KeyId == ViewModel.KeyId);
        }
        /// <summary>
        /// 恢复管理员数据
        /// </summary>
        /// <returns></returns>
        public async Task<Object> RecoveryAdminData(string Key)
        {
            List<Administrator> administrator = Emily.Queryable<Administrator>()
                .WhereIF(!Key.IsNullOrEmpty(), t => Key.Contains(t.KeyId.ToString()))
                .Where(t => t.IsDelete == true).ToList();
            return await base.AlterData<Administrator>(null, administrator, DBType.MSSQL, DbReturnTypes.AlterSoft, false);

        }

        #region 注册登录后台管理员
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
            AdminRoleViewModel AdminRole = Emily.Queryable<Administrator, RolePermission>((t, x) => new Object[] { JoinType.Left, t.RolePermissionId == x.KeyId })
                .Where(t => t.Account.Equals(ViewModel.Account))
                .Where(t => t.PassWord.Equals(ViewModel.PassWord))
                .Select<AdminRoleViewModel>().First();
            if (AdminRole != null)
                await Caches.RedisCacheSetAsync(AdminRole.GetType().FullName, AdminRole,120);
            return AdminRole;
        }
        #endregion
        #endregion
    }
}
