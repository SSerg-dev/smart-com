DECLARE @resource_name VARCHAR(255) = 'ClientDashboardViews';
DECLARE @action_export_xlsx VARCHAR(255) = 'ExportXLSX';
DECLARE @action_full_import_xlsx VARCHAR(255) = 'FullImportXLSX';
DECLARE @action_download_template_xlsx VARCHAR(255) = 'DownloadTemplateXLSX';

DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]=@resource_name AND [Action] IN (@action_export_xlsx, @action_full_import_xlsx, @action_download_template_xlsx));
DELETE FROM AccessPoint WHERE [Resource]=@resource_name AND [Action] IN (@action_export_xlsx, @action_full_import_xlsx, @action_download_template_xlsx);

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ClientDashboardViews', @action_export_xlsx, 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ClientDashboardViews', @action_full_import_xlsx, 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ClientDashboardViews', @action_download_template_xlsx, 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx, @action_full_import_xlsx, @action_download_template_xlsx);


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx);


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx);

-- Customer Marketing Manager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx);


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx, @action_full_import_xlsx, @action_download_template_xlsx);


-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx, @action_full_import_xlsx, @action_download_template_xlsx);


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx);


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx);

-- SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] = @resource_name AND [Action] IN (@action_export_xlsx, @action_full_import_xlsx, @action_download_template_xlsx);