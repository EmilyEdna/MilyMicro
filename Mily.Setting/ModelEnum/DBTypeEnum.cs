using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Setting.ModelEnum
{
    /// <summary>
    /// 数据库类型枚举
    /// </summary>
    public enum DBTypeEnum
    {
        #region DBTypeEnum

        /// <summary>
        /// 默认值SQLSERVER
        /// </summary>
        Default = 0,

        /// <summary>
        /// SQLSERVER
        /// </summary>
        MSSQL = 1,

        /// <summary>
        /// MYSQL
        /// </summary>
        MYSQL = 2,

        /// <summary>
        /// ORACLE
        /// </summary>
        ORACLE = 3,

        /// <summary>
        /// SQLITE
        /// </summary>
        SQLITE = 4,

        /// <summary>
        /// POSTGRESQL
        /// </summary>
        POSTGRESQL = 5

        #endregion DBTypeEnum
    }
}
