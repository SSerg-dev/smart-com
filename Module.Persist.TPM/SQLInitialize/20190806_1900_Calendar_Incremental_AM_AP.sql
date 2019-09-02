-- точки доступа
-- избавляемся от дуближа
-- IncrementalPromoes
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX'));
Delete FROM AccessPoint WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'GetIncrementalPromo', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'GetIncrementalPromoes', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('IncrementalPromoes', 'DownloadTemplateXLSX', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX');

--DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX');

--KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX');

--DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX');

--CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX');

--CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'Put', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX');

--SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='IncrementalPromoes' AND [Action] IN ('GetIncrementalPromo', 'GetIncrementalPromoes', 'ExportXLSX');


-- точки доступа
-- избавляемся от дуближа
-- AssortmentMatrices
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX'));
Delete FROM AccessPoint WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'GetAssortmentMatrices', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'GetAssortmentMatrix', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'DownloadTemplateXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('AssortmentMatrices', 'FullImportXLSX', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX');

--DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX');

--KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX');

--DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX');

--CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX');

--CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'Put', 'Post', 'Patch', 'Delete', 'ExportXLSX', 'DownloadTemplateXLSX', 'FullImportXLSX');

--SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='AssortmentMatrices' AND [Action] IN ('GetAssortmentMatrices', 'GetAssortmentMatrix', 'ExportXLSX');


-- точки доступа
-- избавляемся от дуближа
-- SchedulerClientTreeDTOs
DELETE FROM [dbo].[AccessPointRole] WHERE [AccessPointId] in (SELECT [Id] FROM [dbo].[AccessPoint] WHERE [Resource] = 'SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs');
DELETE FROM [dbo].[AccessPoint] WHERE [Resource] = 'SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';


INSERT INTO [dbo].[AccessPoint] (Resource, Action, Disabled, DeletedDate) VALUES ('SchedulerClientTreeDTOs', 'GetSchedulerClientTreeDTOs', 0, NULL);

-- Administrator
INSERT INTO [dbo].[AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] WHERE [SystemName] = 'Administrator' and [Disabled] = 0), [Id] FROM [dbo].[AccessPoint] WHERE [Resource]='SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';

-- FunctionalExpert
INSERT INTO [dbo].[AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] WHERE [SystemName] = 'FunctionalExpert' and [Disabled] = 0), [Id] FROM [dbo].[AccessPoint] WHERE [Resource]='SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';

-- CustomerMarketing
INSERT INTO [dbo].[AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] WHERE [SystemName] = 'CustomerMarketing' and [Disabled] = 0), [Id] FROM [dbo].[AccessPoint] WHERE [Resource]='SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';

-- CMManager
INSERT INTO [dbo].[AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] WHERE [SystemName] = 'CMManager' and [Disabled] = 0), [Id] FROM [dbo].[AccessPoint] WHERE [Resource]='SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';

-- KeyAccountManager
INSERT INTO [dbo].[AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] WHERE [SystemName] = 'KeyAccountManager' and [Disabled] = 0), [Id] FROM [dbo].[AccessPoint] WHERE [Resource]='SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';

-- DemandPlanning
INSERT INTO [dbo].[AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] WHERE [SystemName] = 'DemandPlanning' and [Disabled] = 0), [Id] FROM [dbo].[AccessPoint] WHERE [Resource]='SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';

-- DemandFinance
INSERT INTO [dbo].[AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] WHERE [SystemName] = 'DemandFinance' and [Disabled] = 0), [Id] FROM [dbo].[AccessPoint] WHERE [Resource]='SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';

-- SuperReader
INSERT INTO [dbo].[AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] WHERE [SystemName] = 'SuperReader' and [Disabled] = 0), [Id] FROM [dbo].[AccessPoint] WHERE [Resource]='SchedulerClientTreeDTOs' and [Action] = 'GetSchedulerClientTreeDTOs';