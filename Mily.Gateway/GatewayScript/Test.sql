-- ----------------------------
-- Records of [System_Administrator]
-- ----------------------------
INSERT INTO [dbo].[System_Administrator] ([KeyId], [AdminName], [Account], [PassWord], [RolePermissionId], [Deleted], [Created]) VALUES (N'9490009D-FE18-45C6-A113-D059167EA608', N'lzh', N'admin', N'123', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'0', NULL)
GO


-- ----------------------------
-- Records of [System_MenuItems]
-- ----------------------------
INSERT INTO [dbo].[System_MenuItems] ([KeyId], [Icon], [Title], [Path], [Lv], [ParentId], [Parent], [Deleted], [RouterPath], [Created]) VALUES (N'C078B6E0-F762-4783-8143-1F921ECD3ED9', NULL, N'后台菜单', N'Views/System/SysMenu', N'3', N'747CFFD2-6F45-4BF0-8D50-7D7469AE9F6C', N'0', N'0', N'System/SysMenu.vue', NULL)
GO

INSERT INTO [dbo].[System_MenuItems] ([KeyId], [Icon], [Title], [Path], [Lv], [ParentId], [Parent], [Deleted], [RouterPath], [Created]) VALUES (N'747CFFD2-6F45-4BF0-8D50-7D7469AE9F6C', N'el-icon-lx-settings', N'菜单管理', NULL, N'2', N'95332CD5-8159-45B8-83C8-A540F3279B0E', N'1', N'0', NULL, NULL)
GO

INSERT INTO [dbo].[System_MenuItems] ([KeyId], [Icon], [Title], [Path], [Lv], [ParentId], [Parent], [Deleted], [RouterPath], [Created]) VALUES (N'95332CD5-8159-45B8-83C8-A540F3279B0E', N'el-icon-lx-home', N'系统管理', NULL, N'1', NULL, N'1', N'0', NULL, NULL)
GO


-- ----------------------------
-- Records of [System_RoleMenuItems]
-- ----------------------------
INSERT INTO [dbo].[System_RoleMenuItems] ([KeyId], [RolePermissionId], [MenuItemsId], [Deleted], [Created]) VALUES (N'CBBC787F-C724-41EF-8108-261265BB1DA3', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'747CFFD2-6F45-4BF0-8D50-7D7469AE9F6C', NULL, NULL)
GO

INSERT INTO [dbo].[System_RoleMenuItems] ([KeyId], [RolePermissionId], [MenuItemsId], [Deleted], [Created]) VALUES (N'1B108D3D-C360-4CE8-9330-4C1902635D14', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'C078B6E0-F762-4783-8143-1F921ECD3ED9', NULL, NULL)
GO

INSERT INTO [dbo].[System_RoleMenuItems] ([KeyId], [RolePermissionId], [MenuItemsId], [Deleted], [Created]) VALUES (N'C3A9AE9C-AF25-4BDC-AB2E-8F0705F25DA1', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'95332CD5-8159-45B8-83C8-A540F3279B0E', NULL, NULL)
GO


-- ----------------------------
-- Records of [System_RolePermission]
-- ----------------------------
INSERT INTO [dbo].[System_RolePermission] ([KeyId], [RoleName], [HandlerRole], [Deleted], [Created]) VALUES (N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'测试', N'Create|Read|Delete', NULL, NULL)
GO

