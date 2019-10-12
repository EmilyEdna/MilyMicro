namespace Mily.DbCore
{
    public enum DbReturnTypes
    {
        #region EnumOfInsert

        /// <summary>
        /// Return Boolean
        /// </summary>
        InsertDefault = 0,

        /// <summary>
        ///  Return Rows
        /// </summary>
        Rowspan = 1,

        /// <summary>
        ///  Return Int
        /// </summary>
        Integer = 2,

        /// <summary>
        /// Return Long
        /// </summary>
        BigInteger = 3,

        /// <summary>
        /// Return Entity
        /// </summary>
        Model = 4,

        #endregion EnumOfInsert

        #region EnumOfAlter

        /// <summary>
        ///  Return Int
        /// </summary>
        AlterDefault = 5,

        /// <summary>
        ///  Return Int
        /// </summary>
        AlterEntity = 6,

        /// <summary>
        ///  Return Int
        /// </summary>
        AlterCols = 7,

        /// <summary>
        ///  Return Int
        /// </summary>
        AlterSoft = 8,

        #endregion EnumOfAlter

        #region EnumOfRemove

        /// <summary>
        ///  Return Int
        /// </summary>
        RemoveDefault = 9,

        /// <summary>
        ///  Return Int
        /// </summary>
        RemoveEntity = 10,

        /// <summary>
        ///  Return Int
        /// </summary>
        WithNoId = 11,

        /// <summary>
        ///  Return Int
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