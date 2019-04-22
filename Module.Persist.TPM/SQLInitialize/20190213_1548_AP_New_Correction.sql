--Загрузка файлов экспорта
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] ='ExportDownload';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] ='ExportDownload';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] ='ExportDownload';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] ='ExportDownload';

--CanCreateBaseClient
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE Resource in (
'ClientTrees') and   [Action] ='CanCreateBaseClient';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource in (
'ClientTrees') and   [Action] ='CanCreateBaseClient';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource in (
'ClientTrees') and   [Action] ='CanCreateBaseClient';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in (
'ClientTrees') and   [Action] ='CanCreateBaseClient';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource in (
'ClientTrees') and   [Action] ='CanCreateBaseClient';

--Для KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource in (
'BaseClientTreeViews') and ([Action] like 'Get%' or [Action] ='ExportXLSX');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource in (
'ClientTreeSharesViews') and  ([Action] like 'Get%' or [Action] ='ExportXLSX');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource in (
'RetailTypes') and  ([Action] like 'Get%' or [Action] ='ExportXLSX');


--NoNego
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in (
'NoneNegoes') and   [Action] ='ExportXLSX';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource in (
'NoneNegoes') and   [Action] ='ExportXLSX';



