-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('ActualTradeInvestments', 'HistoricalActualTradeInvestments', 'DeletedActualTradeInvestments') AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetHistoricalActualTradeInvestments', 'GetDeletedActualTradeInvestments'));
DELETE FROM AccessPoint WHERE [Resource] IN ('ActualTradeInvestments', 'HistoricalActualTradeInvestments', 'DeletedActualTradeInvestments') AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete', 'GetHistoricalActualTradeInvestments', 'GetDeletedActualTradeInvestments') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'GetActualTradeInvestments', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'GetActualTradeInvestment', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'FullImportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'DownloadTemplateXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalActualTradeInvestments', 'GetHistoricalActualTradeInvestments', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedActualTradeInvestments', 'GetDeletedActualTradeInvestments', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');

--Support Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'Patch', 'ExportXLSX', 'FullImportXLSX', 'DownloadTemplateXLSX', 'Put', 'Post', 'Delete');
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');


-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'ExportXLSX'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'ExportXLSX'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');


-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'ExportXLSX'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');


-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'ExportXLSX'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');


-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('GetActualTradeInvestments', 'GetActualTradeInvestment', 'ExportXLSX'); 
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalActualTradeInvestments' AND [Action] IN ('GetHistoricalActualTradeInvestments');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedActualTradeInvestments' AND [Action] IN ('GetDeletedActualTradeInvestments');
