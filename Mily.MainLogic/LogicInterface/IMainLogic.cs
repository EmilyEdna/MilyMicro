using Mily.Extension.ViewModel;
using Mily.Setting;
using System;
using System.Threading.Tasks;
using XExten.Common;

namespace Mily.MainLogic.LogicInterface
{
    public interface IMainLogic : IService
    {
        #region 管理员

        Task<Object> SearchAdminPage(PageQuery Page);

        Task<Object> EditAdmin(AdminRoleViewModel ViewModel);

        Task<Object> RemoveAdmin(string Key);

        Task<Object> DeleteAdmin(string Key);

        Task<Object> RecoveryAdminData(string Key);

        Task<AdminRoleViewModel> Login(AdminRoleViewModel ViewModel);

        Task<Object> RegistAdmin(AdminRoleViewModel ViewModel);

        #endregion 管理员
    }
}