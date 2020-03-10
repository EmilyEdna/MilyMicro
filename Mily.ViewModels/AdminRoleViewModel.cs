using Mily.Setting.ModelEnum;
using Mily.ViewModels.Basic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.ViewModels
{

    /// <summary>
    /// 用户信息
    /// </summary>
    public class AdminRoleViewModel : BasicViewModel
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
        /// 角色类型
        /// </summary>
        public RoleTypeEnum? RoleType { get; set; }
    }
}
