using System;
using System.Collections.Generic;
using System.Text;
using BeetleX.Buffers;

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

        /*public override void Load(IBinaryReader reader)
        {
            base.Load(reader);
            AdminName = reader.ReadUTF();
            Account = reader.ReadUTF();
            PassWord = reader.ReadUTF();
            RolePermissionId = Guid.Parse(reader.ReadUTF());
            RoleName = reader.ReadUTF();
            HandlerRole = reader.ReadUTF();
        }
        public override void Save(IBinaryWriter writer)
        {
            base.Save(writer);
            writer.Write(AdminName);
            writer.Write(Account);
            writer.Write(PassWord);
            writer.Write(RolePermissionId.ToString());
            writer.Write(RoleName);
            writer.Write(HandlerRole);
        }*/
    }
}
