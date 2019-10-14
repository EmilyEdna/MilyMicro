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
        /// 是否删除
        /// </summary>
        public bool? IsDelete { get; set; }
    }
}