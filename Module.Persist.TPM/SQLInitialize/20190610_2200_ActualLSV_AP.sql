-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'Patch', 'ExportXLSX'));
Delete FROM AccessPoint WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'Patch', 'ExportXLSX') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualLSVs', 'GetActualLSVs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualLSVs', 'GetActualLSV', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualLSVs', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualLSVs', 'ExportXLSX', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'Patch', 'ExportXLSX');


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'Patch', 'ExportXLSX');


-- CustomerMarketing
--INSERT INTO AccessPointRole
--(Id, RoleId, AccessPointId)
--SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'Patch', 'ExportXLSX');


-- KeyAccountManager
--INSERT INTO AccessPointRole
--(Id, RoleId, AccessPointId)
--SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'Patch', 'ExportXLSX');


-- DemandFinance
--INSERT INTO AccessPointRole
--(Id, RoleId, AccessPointId)
--SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'Patch', 'ExportXLSX');


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'Patch', 'ExportXLSX');


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualLSVs' AND [Action] IN ('GetActualLSVs', 'GetActualLSV', 'ExportXLSX');
