using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Setting.ModelEnum
{
    /// <summary>
    /// 返回类型枚举
    /// </summary>
    public enum DbReturnEnum
    {
        #region EnumOfInsert

        /// <summary>
        /// 返回bool
        /// </summary>
        InsertDefault = 0,

        /// <summary>
        ///  返回当前行
        /// </summary>
        Rowspan = 1,

        /// <summary>
        ///  返回Int
        /// </summary>
        Integer = 2,

        /// <summary>
        /// 返回Long
        /// </summary>
        BigInteger = 3,

        /// <summary>
        /// 返回新增实体
        /// </summary>
        Model = 4,

        #endregion EnumOfInsert

        #region EnumOfAlter

        /// <summary>
        ///  条件更新
        /// </summary>
        AlterDefault = 5,

        /// <summary>
        ///  更新实体
        /// </summary>
        AlterEntity = 6,

        /// <summary>
        ///  更新指定列
        /// </summary>
        AlterCols = 7,

        #endregion EnumOfAlter

        #region EnumOfRemove

        /// <summary>
        ///  通过Id删除
        /// </summary>
        RemoveDefault = 8,

        /// <summary>
        ///  删除实体
        /// </summary>
        RemoveEntity = 9,

        /// <summary>
        /// 批量删除实体
        /// </summary>
        RemoveEntities = 10,

        /// <summary>
        ///  不通过Id删除
        /// </summary>
        WithNoId = 11,

        /// <summary>
        ///  条件删除
        /// </summary>
        WithWhere = 12,

        #endregion EnumOfRemove
    }
}
