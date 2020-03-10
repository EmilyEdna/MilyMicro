using Mily.Setting.ModelEnum;
using System;

namespace Mily.ViewModels.Basic
{
    public class BasicViewModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid? KeyId { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        public bool? Deleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? Created { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DBTypeEnum? DbTypeAttribute { get; set; }
    }
}
