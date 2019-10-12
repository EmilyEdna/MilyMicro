using Mily.Setting.ModelEnum;
using System;

namespace Mily.Extension.ViewModel
{
    public class BaseViewModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid KeyId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// 删除人
        /// </summary>
        public string DeleteUser { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 修改人Id
        /// </summary>
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 删除人Id
        /// </summary>
        public Guid? DeleteUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime? DeleteTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditStatusEnum? AuditStatus { get; set; }

        /*public virtual void Load(IBinaryReader reader)
        {
            KeyId = Guid.Parse(reader.ReadUTF());
            CreateUser = reader.ReadUTF();
            UpdateUser = reader.ReadUTF();
            DeleteUser = reader.ReadUTF();
            CreateUserId = Guid.Parse(reader.ReadUTF());
            UpdateUserId = Guid.Parse(reader.ReadUTF());
            DeleteUserId = Guid.Parse(reader.ReadUTF());
            CreateTime = reader.ReadDateTime();
            UpdateTime = reader.ReadDateTime();
            DeleteTime = reader.ReadDateTime();
            IsDelete = reader.ReadBool();
            AuditStatus = (AuditStatusEnum)reader.ReadInt32();
        }

        public virtual void Save(IBinaryWriter writer)
        {
            writer.Write(KeyId.ToString());
            writer.Write(CreateUser);
            writer.Write(UpdateUser);
            writer.Write(DeleteUser);
            writer.Write(CreateUserId.ToString());
            writer.Write(UpdateUserId.ToString());
            writer.Write(DeleteUserId.ToString());
            writer.Write(CreateTime.Value);
            writer.Write(UpdateTime.Value);
            writer.Write(DeleteTime.Value);
            writer.Write(IsDelete.Value);
            writer.Write((int)AuditStatus);
        }*/
    }
}