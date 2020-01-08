using Mily.Setting.ModelEnum;
using System;

namespace Mily.ViewModels
{
    public class BaseViewModel
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid KeyId { get; set; }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        public bool? Deleted { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DBTypeEnum? DbTypeAttribute { get; set; }
    }
}
