-- ----------------------------
-- Records of [System_Administrator]
-- ----------------------------
INSERT INTO [dbo].[System_Administrator]  VALUES (N'9490009D-FE18-45C6-A113-D059167EA608', N'lzh', N'admin', N'123', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'0', NULL)
GO

-- ----------------------------
-- Records of [System_MenuFeatures]
-- ----------------------------
INSERT INTO [dbo].[System_MenuFeatures]  VALUES (N'07C1F0A2-28A6-48DC-98C3-2A4F32E183E9', N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'新增', N'el-icon-plus', N'1', N'0', NULL)
GO

INSERT INTO [dbo].[System_MenuFeatures]  VALUES (N'36872190-4A83-4C83-9BCC-54787936CFD5', N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'编辑', N'el-icon-edit', N'1', N'0', NULL)
GO

INSERT INTO [dbo].[System_MenuFeatures]  VALUES (N'0729C15A-4DA2-433E-9691-AC47AFECAC6D', N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'删除', N'el-icon-delete', N'1', N'0', NULL)
GO

INSERT INTO [dbo].[System_MenuFeatures]  VALUES (N'5BFD1062-EBA8-4059-B118-F56AFC01175E', N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'批量删除', N'el-icon-delete', N'1', N'0', NULL)
GO

-- ----------------------------
-- Records of [System_MenuFeaturesRouter]
-- ----------------------------
INSERT INTO [dbo].[System_MenuFeaturesRouter]  VALUES (N'B4E59939-3A23-4414-9658-1DA5034E0529', N'07C1F0A2-28A6-48DC-98C3-2A4F32E183E9', N'新增', N'Menu/AddMenu', N'Menu/Handler/AddMenu.vue', N'0', NULL)
GO


-- ----------------------------
-- Records of [System_MenuItems]
-- ----------------------------
INSERT INTO [dbo].[System_MenuItems]  VALUES (N'C078B6E0-F762-4783-8143-1F921ECD3ED9', NULL, N'后台菜单', N'3', N'747CFFD2-6F45-4BF0-8D50-7D7469AE9F6C', N'0', N'0', NULL)
GO

INSERT INTO [dbo].[System_MenuItems]  VALUES (N'747CFFD2-6F45-4BF0-8D50-7D7469AE9F6C', N'el-icon-lx-settings', N'菜单管理', N'2', N'95332CD5-8159-45B8-83C8-A540F3279B0E', N'1', N'0', NULL)
GO

INSERT INTO [dbo].[System_MenuItems]  VALUES (N'95332CD5-8159-45B8-83C8-A540F3279B0E', N'el-icon-lx-home', N'系统管理', N'1', NULL, N'1', N'0', NULL)
GO


-- ----------------------------
-- Records of [System_MenuItemsRouter]
-- ----------------------------
INSERT INTO [dbo].[System_MenuItemsRouter]  VALUES (N'9766B4C6-FBDF-4819-BCF1-71A06AAF1395', N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'后台菜单', N'SysMenu', N'Menu/SysMenu.vue', N'0', NULL)
GO


-- ----------------------------
-- Records of [System_RoleMenuFeatures]
-- ----------------------------
INSERT INTO [dbo].[System_RoleMenuFeatures]  VALUES (N'9995BA04-18F9-4C50-B6BA-9627D6168B6F', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39',N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'36872190-4A83-4C83-9BCC-54787936CFD5', N'0', NULL)
GO

INSERT INTO [dbo].[System_RoleMenuFeatures]  VALUES (N'4AD785D9-2ED8-4414-850F-BAFC61544940', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39',N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'0729C15A-4DA2-433E-9691-AC47AFECAC6D', N'0', NULL)
GO

INSERT INTO [dbo].[System_RoleMenuFeatures]  VALUES (N'0A0796AC-B942-48E6-80AE-C2A8DD137C6A', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39',N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'5BFD1062-EBA8-4059-B118-F56AFC01175E', N'0', NULL)
GO

INSERT INTO [dbo].[System_RoleMenuFeatures]  VALUES (N'1F273FEF-2008-48E0-BDEE-D13A31274026', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39',N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'07C1F0A2-28A6-48DC-98C3-2A4F32E183E9', N'0', NULL)
GO


-- ----------------------------
-- Records of [System_RoleMenuItems]
-- ----------------------------
INSERT INTO [dbo].[System_RoleMenuItems]  VALUES (N'CBBC787F-C724-41EF-8108-261265BB1DA3', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'747CFFD2-6F45-4BF0-8D50-7D7469AE9F6C', N'0', NULL)
GO

INSERT INTO [dbo].[System_RoleMenuItems]  VALUES (N'1B108D3D-C360-4CE8-9330-4C1902635D14', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'C078B6E0-F762-4783-8143-1F921ECD3ED9', N'0', NULL)
GO

INSERT INTO [dbo].[System_RoleMenuItems]  VALUES (N'C3A9AE9C-AF25-4BDC-AB2E-8F0705F25DA1', N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'95332CD5-8159-45B8-83C8-A540F3279B0E', N'0', NULL)
GO


-- ----------------------------
-- Records of [System_RolePermission]
-- ----------------------------
INSERT INTO [dbo].[System_RolePermission]  VALUES (N'D3DCAE43-77D0-4D53-83CD-63099C7F9D39', N'超级管理员', N'0', NULL, N'0')
GO
