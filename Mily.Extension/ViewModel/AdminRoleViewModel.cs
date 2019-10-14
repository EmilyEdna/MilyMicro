using System;

namespace Mily.Extension.ViewModel
{
    public class AdminRoleViewModel : BaseViewModel
    {
        /// <summary>
        /// 管理员
        /// </summary>
        public string AdminName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string PassWord { get; set; }

        /// <summary>
        /// 权限许可ID
        /// </summary>
        public Guid? RolePermissionId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 操作角色
        /// </summary>
        public string HandlerRole { get; set; }
    }
}