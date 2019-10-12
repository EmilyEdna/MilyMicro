using System.ComponentModel;

namespace Mily.Setting.ModelEnum
{
    /// <summary>
    /// 操作枚举
    /// </summary>
    public enum HandleEnum
    {
        /// <summary>
        /// 查询操作
        /// </summary>
        [Description("查询操作")]
        Query,

        /// <summary>
        /// 新增操作
        /// </summary>
        [Description("新增操作")]
        Add,

        /// <summary>
        /// 编辑操作
        /// </summary>
        [Description("编辑操作")]
        Edit,

        /// <summary>
        /// 删除操作
        /// </summary>
        [Description("删除操作")]
        Remove
    }
}