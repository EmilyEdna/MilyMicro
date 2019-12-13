using Mily.ViewModels;
using Mily.Setting;
using System;
using System.Threading.Tasks;
using XExten.Common;
using Mily.Setting.DbTypes;

namespace Mily.MainLogic.LogicInterface
{
    public interface IMainLogic : IService
    {
        #region 登录注册

        Task<AdminRoleViewModel> Login(ResultProvider Provider);

        Task<Object> RegistAdmin(ResultProvider Provider);

        #endregion

        #region 管理员

        Task<Object> SearchAdminPage(PageQuery Page);

        Task<Object> EditAdmin(ResultProvider Provider);

        Task<Object> RemoveAdmin(ResultProvider Provider);

        Task<Object> DeleteAdmin(ResultProvider Provider);

        Task<Object> RecoveryAdminData(ResultProvider Provider);

        #endregion

        #region 获取菜单
        Task<Object> SearchMenuItem(ResultProvider Provider);
        #endregion
    }
}