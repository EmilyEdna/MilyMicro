﻿namespace Mily.DbCore
{
    public enum DbReturnTypes
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

        /// <summary>
        ///  软删除
        /// </summary>
        AlterSoft = 8,

        #endregion EnumOfAlter

        #region EnumOfRemove

        /// <summary>
        ///  通过Id删除
        /// </summary>
        RemoveDefault = 9,

        /// <summary>
        ///  删除实体
        /// </summary>
        RemoveEntity = 10,

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

    public enum DBType
    {
        #region DBType

        /// <summary>
        /// SQLSERVER
        /// </summary>
        MSSQL = 1,

        /// <summary>
        /// MYSQL
        /// </summary>
        MYSQL = 2

        #endregion DBType
    }
}