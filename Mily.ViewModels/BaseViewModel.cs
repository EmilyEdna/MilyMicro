using Mily.Setting.DbTypes;
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
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DBType? DbTypeAttribute { get; set; }
    }
}
