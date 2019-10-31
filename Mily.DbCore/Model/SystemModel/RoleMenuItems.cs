﻿using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbCore.Model.SystemModel
{
    [SugarTable("System_RoleMenuItems", "权限菜单表")]
    public class RoleMenuItems : BaseModel
    {
        /// <summary>
        /// 权限许可ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "权限许可ID")]
        public Guid? RolePermissionId { get; set; }
        /// <summary>
        /// 菜单表ID
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDescription = "菜单表ID")]
        public Guid? MenuItemsId { get; set; }
    }
}
