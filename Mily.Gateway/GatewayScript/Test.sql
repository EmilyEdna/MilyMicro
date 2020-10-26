-- ----------------------------
-- Records of [Sys_Admin]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_Admin] ON
GO

INSERT INTO [dbo].[Sys_Admin] ([Id], [AdminName], [Account], [PassWord], [RolePermissionId], [Deleted], [Created]) VALUES (N'1', N'admin', N'lzh', N'123', N'1', N'0', N'2020-10-23 18:10:59.003')
GO

SET IDENTITY_INSERT [dbo].[Sys_Admin] OFF
GO


-- ----------------------------
-- Records of [Sys_Log]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_Log] ON
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'1', N'*', N'1', N'2020-10-23 18:10:58.817', N'RolePermission', N'新增操作', N'【*】对【RolePermission】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:58】', N'0', N'2020-10-23 18:10:58.883')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'2', N'*', N'1', N'2020-10-23 18:10:59.023', N'Administrator', N'新增操作', N'【*】对【Administrator】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.023')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'3', N'*', N'1', N'2020-10-23 18:10:59.113', N'MenuItems', N'新增操作', N'【*】对【MenuItems】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.113')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'4', N'*', N'1', N'2020-10-23 18:10:59.207', N'MenuItems', N'新增操作', N'【*】对【MenuItems】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.207')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'5', N'*', N'1', N'2020-10-23 18:10:59.310', N'MenuItems', N'新增操作', N'【*】对【MenuItems】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.310')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'6', N'*', N'1', N'2020-10-23 18:10:59.407', N'MenuFeatures', N'新增操作', N'【*】对【MenuFeatures】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.407')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'7', N'*', N'1', N'2020-10-23 18:10:59.510', N'MenuFeaturesRouter', N'新增操作', N'【*】对【MenuFeaturesRouter】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.510')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'8', N'*', N'1', N'2020-10-23 18:10:59.607', N'RoleMenuFeatures', N'新增操作', N'【*】对【RoleMenuFeatures】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.607')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'9', N'*', N'1', N'2020-10-23 18:10:59.707', N'MenuItemsRouter', N'新增操作', N'【*】对【MenuItemsRouter】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.707')
GO

INSERT INTO [dbo].[Sys_Log] ([Id], [Hnadler], [HandleType], [HandleTime], [HandleObject], [HandleName], [HandleObvious], [Deleted], [Created]) VALUES (N'10', N'*', N'1', N'2020-10-23 18:10:59.803', N'RoleMenuItems', N'新增操作', N'【*】对【RoleMenuItems】表进行了【新增操作】，操作时间为：【2020/10/23 18:10:59】', N'0', N'2020-10-23 18:10:59.803')
GO

SET IDENTITY_INSERT [dbo].[Sys_Log] OFF
GO

-- ----------------------------
-- Records of [Sys_MenuFeatures]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_MenuFeatures] ON
GO

INSERT INTO [dbo].[Sys_MenuFeatures] ([Id], [MenuItemId], [FeatName], [Icon], [EnableOrDisable], [Deleted], [Created]) VALUES (N'1', N'3', N'新增', N'el-icon-plus', N'1', N'0', N'2020-10-23 18:10:59.383')
GO

INSERT INTO [dbo].[Sys_MenuFeatures] ([Id], [MenuItemId], [FeatName], [Icon], [EnableOrDisable], [Deleted], [Created]) VALUES (N'2', N'3', N'编辑', N'el-icon-edit', N'1', N'0', N'2020-10-23 18:10:59.383')
GO

INSERT INTO [dbo].[Sys_MenuFeatures] ([Id], [MenuItemId], [FeatName], [Icon], [EnableOrDisable], [Deleted], [Created]) VALUES (N'3', N'3', N'删除', N'el-icon-delete', N'1', N'0', N'2020-10-23 18:10:59.383')
GO

INSERT INTO [dbo].[Sys_MenuFeatures] ([Id], [MenuItemId], [FeatName], [Icon], [EnableOrDisable], [Deleted], [Created]) VALUES (N'4', N'3', N'批量删除', N'el-icon-delete', N'1', N'0', N'2020-10-23 18:10:59.387')
GO

SET IDENTITY_INSERT [dbo].[Sys_MenuFeatures] OFF
GO

-- ----------------------------
-- Records of [Sys_MenuFeaturesRouter]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_MenuFeaturesRouter] ON
GO

INSERT INTO [dbo].[Sys_MenuFeaturesRouter] ([Id], [MenuFeatId], [Title], [PathRoad], [PathRouter], [Deleted], [Created]) VALUES (N'1', N'0', N'新增', N'Menu/AddMenu', N'Menu/Handler/AddMenu.vue', N'0', N'2020-10-23 18:10:59.490')
GO

SET IDENTITY_INSERT [dbo].[Sys_MenuFeaturesRouter] OFF
GO

-- ----------------------------
-- Records of [Sys_MenuItems]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_MenuItems] ON
GO

INSERT INTO [dbo].[Sys_MenuItems] ([Id], [Icon], [Title], [Lv], [Parent], [ParentId], [Deleted], [Created]) VALUES (N'1', N'el-icon-lx-home', N'系统管理', N'1', N'1', NULL, N'0', N'2020-10-23 18:10:59.093')
GO

INSERT INTO [dbo].[Sys_MenuItems] ([Id], [Icon], [Title], [Lv], [Parent], [ParentId], [Deleted], [Created]) VALUES (N'2', N'el-icon-lx-settings', N'菜单管理', N'2', N'1', N'1', N'0', N'2020-10-23 18:10:59.190')
GO

INSERT INTO [dbo].[Sys_MenuItems] ([Id], [Icon], [Title], [Lv], [Parent], [ParentId], [Deleted], [Created]) VALUES (N'3', NULL, N'后台菜单', N'3', N'0', N'2', N'0', N'2020-10-23 18:10:59.290')
GO

SET IDENTITY_INSERT [dbo].[Sys_MenuItems] OFF
GO

-- ----------------------------
-- Records of [Sys_MenuItemsRouter]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_MenuItemsRouter] ON
GO

INSERT INTO [dbo].[Sys_MenuItemsRouter] ([Id], [MenuItemId], [Title], [PathRoad], [PathRouter], [Deleted], [Created]) VALUES (N'1', N'3', N'后台菜单', N'SysMenu', N'Menu/SysMenu.vue', N'0', N'2020-10-23 18:10:59.683')
GO

SET IDENTITY_INSERT [dbo].[Sys_MenuItemsRouter] OFF
GO

-- ----------------------------
-- Records of [Sys_RoleMenuFeatures]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_RoleMenuFeatures] ON
GO

INSERT INTO [dbo].[Sys_RoleMenuFeatures] ([Id], [RolePermissionId], [MenuItemId], [MenuFeatId], [Deleted], [Created]) VALUES (N'1', N'1', N'3', N'1', N'0', N'2020-10-23 18:10:59.587')
GO

INSERT INTO [dbo].[Sys_RoleMenuFeatures] ([Id], [RolePermissionId], [MenuItemId], [MenuFeatId], [Deleted], [Created]) VALUES (N'2', N'1', N'3', N'2', N'0', N'2020-10-23 18:10:59.587')
GO

INSERT INTO [dbo].[Sys_RoleMenuFeatures] ([Id], [RolePermissionId], [MenuItemId], [MenuFeatId], [Deleted], [Created]) VALUES (N'3', N'1', N'3', N'3', N'0', N'2020-10-23 18:10:59.587')
GO

INSERT INTO [dbo].[Sys_RoleMenuFeatures] ([Id], [RolePermissionId], [MenuItemId], [MenuFeatId], [Deleted], [Created]) VALUES (N'4', N'1', N'3', N'4', N'0', N'2020-10-23 18:10:59.587')
GO

SET IDENTITY_INSERT [dbo].[Sys_RoleMenuFeatures] OFF
GO

-- ----------------------------
-- Records of [Sys_RoleMenuItems]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_RoleMenuItems] ON
GO

INSERT INTO [dbo].[Sys_RoleMenuItems] ([Id], [RolePermissionId], [MenuItemsId], [Deleted], [Created]) VALUES (N'1', N'1', N'1', N'0', N'2020-10-23 18:10:59.783')
GO

INSERT INTO [dbo].[Sys_RoleMenuItems] ([Id], [RolePermissionId], [MenuItemsId], [Deleted], [Created]) VALUES (N'2', N'1', N'2', N'0', N'2020-10-23 18:10:59.783')
GO

INSERT INTO [dbo].[Sys_RoleMenuItems] ([Id], [RolePermissionId], [MenuItemsId], [Deleted], [Created]) VALUES (N'3', N'1', N'3', N'0', N'2020-10-23 18:10:59.783')
GO

SET IDENTITY_INSERT [dbo].[Sys_RoleMenuItems] OFF
GO

-- ----------------------------
-- Records of [Sys_RolePermission]
-- ----------------------------
SET IDENTITY_INSERT [dbo].[Sys_RolePermission] ON
GO

INSERT INTO [dbo].[Sys_RolePermission] ([Id], [RoleName], [RoleType], [Deleted], [Created]) VALUES (N'1', N'超级管理员', N'0', N'0', N'2020-10-23 18:10:58.790')
GO

SET IDENTITY_INSERT [dbo].[Sys_RolePermission] OFF
GO