INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in (
'LoopHandlers') and   [Action] IN('Parameters', 'ReadLogFile');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] IN('ImportResultSuccessDownload', 'ImportResultWarningDownload', 'ImportResultErrorDownload');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE Resource in (
'LoopHandlers') and   [Action] IN('Parameters', 'ReadLogFile');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] IN('ImportResultSuccessDownload', 'ImportResultWarningDownload', 'ImportResultErrorDownload');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource in (
'LoopHandlers') and   [Action] IN('Parameters', 'ReadLogFile');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] IN('ImportResultSuccessDownload', 'ImportResultWarningDownload', 'ImportResultErrorDownload');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource in (
'LoopHandlers') and   [Action] IN('Parameters', 'ReadLogFile');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] IN('ImportResultSuccessDownload', 'ImportResultWarningDownload', 'ImportResultErrorDownload');


INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource in (
'LoopHandlers') and   [Action] IN('Parameters', 'ReadLogFile');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource in (
'File') and   [Action] IN('ImportResultSuccessDownload', 'ImportResultWarningDownload', 'ImportResultErrorDownload');