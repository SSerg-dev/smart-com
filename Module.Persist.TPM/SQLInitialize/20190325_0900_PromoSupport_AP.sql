-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports');
Delete FROM AccessPoint WHERE [Resource]='PromoSupports' AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'GetPromoSupportGroup', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'SetUserTimestamp', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'UploadFile', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'DownloadFile', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'GetUserTimestamp', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'GetPromoSupport', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'GetPromoSupports', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoSupports', 'PatchCostProduction', 0, NULL);


DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' AND [Action] IN ('Post', 'Patch', 'Delete'));

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' 
	AND [Action] IN ('Put', 'Post', 'Patch', 'Delete', 'GetPromoSupportGroup', 'SetUserTimestamp', 'UploadFile', 'DownloadFile', 'GetUserTimestamp', 'ExportXLSX', 'GetPromoSupport', 'GetPromoSupports', 'PatchCostProduction');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' 
	AND [Action] IN ('Post', 'Patch', 'Delete');


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' 
	AND [Action] IN ('Put', 'Post', 'Patch', 'Delete', 'GetPromoSupportGroup', 'SetUserTimestamp', 'UploadFile', 'DownloadFile', 'GetUserTimestamp', 'ExportXLSX', 'GetPromoSupport', 'GetPromoSupports', 'PatchCostProduction');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' 
	AND [Action] IN ('Post', 'Patch', 'Delete');


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' 
	AND [Action] IN ('Put', 'Post', 'Patch', 'Delete', 'GetPromoSupportGroup', 'SetUserTimestamp', 'UploadFile', 'DownloadFile', 'GetUserTimestamp', 'ExportXLSX', 'GetPromoSupport', 'GetPromoSupports', 'PatchCostProduction');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' 
	AND [Action] IN ('Post', 'Patch', 'Delete');


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' 
	AND [Action] IN ('Put', 'Post', 'Patch', 'Delete', 'GetPromoSupportGroup', 'SetUserTimestamp', 'UploadFile', 'DownloadFile', 'GetUserTimestamp', 'ExportXLSX', 'GetPromoSupport', 'GetPromoSupports');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupportPromoes' 
	AND [Action] IN ('Post', 'Patch', 'Delete');


-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' 
	AND [Action] IN ('GetPromoSupportGroup', 'SetUserTimestamp', 'DownloadFile', 'GetUserTimestamp', 'ExportXLSX', 'GetPromoSupport', 'GetPromoSupports');


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' 
	AND [Action] IN ('GetPromoSupportGroup', 'SetUserTimestamp', 'DownloadFile', 'GetUserTimestamp', 'ExportXLSX', 'GetPromoSupport', 'GetPromoSupports');


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='PromoSupports' 
	AND [Action] IN ('GetPromoSupportGroup', 'SetUserTimestamp', 'DownloadFile', 'GetUserTimestamp', 'ExportXLSX', 'GetPromoSupport', 'GetPromoSupports');
