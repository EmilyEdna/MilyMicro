using Mily.Setting.ModelEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.DbCore.Model.SystemModel
{
    /// <summary>
    /// 菜单表
    /// </summary>
    [SugarTable("System_MenuItems", "系统菜单表")]
    public class MenuItems : BaseModel
    {
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "图标", Length = 50)]
        public string Icon { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "菜单名称", Length = 50)]
        public string Title { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "NVARCHAR", ColumnDescription = "菜单地址", Length = 50)]
        public string Path { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "INT", ColumnDescription = "菜单级别")]
        public MenuItemEnum? Lv { get; set; }
        [SugarColumn(IsNullable = true, ColumnDataType = "BIT", ColumnDescription = "是否父菜单")]
        public bool? IsParent { get; set; }
        [SugarColumn(IsNullable = true, ColumnDescription = "父菜单")]
        public Guid? ParentId { get; set; }
    }
}
