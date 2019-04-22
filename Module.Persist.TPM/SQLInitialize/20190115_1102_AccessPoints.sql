DELETE FROM AccessPointRole WHERE RoleId IN ('3F0B3688-2983-48D8-974C-3B541D1C12A4','568DCBBC-1A61-40D4-88D1-059D4C3F1B8A','60009B10-D398-4686-B5C4-6D0032152AF4','A8F7D3CA-B737-433B-9E1D-78E87BCEF853F','1D227DD1-48AC-430F-9E30-66C8BD1BCAD1')


INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES(NEWID(), '3F0B3688-2983-48D8-974C-3B541D1C12A4', (SELECT MAX(ID) FROM ACCESSPOINT WHERE Resource = 'Security' AND Action = 'Get'));
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES(NEWID(), '568DCBBC-1A61-40D4-88D1-059D4C3F1B8A', (SELECT MAX(ID) FROM ACCESSPOINT WHERE Resource = 'Security' AND Action = 'Get'));
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES(NEWID(), '60009B10-D398-4686-B5C4-6D0032152AF4', (SELECT MAX(ID) FROM ACCESSPOINT WHERE Resource = 'Security' AND Action = 'Get'));
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES(NEWID(), 'A8F7D3CA-B737-433B-9E1D-78E87BCEF853F', (SELECT MAX(ID) FROM ACCESSPOINT WHERE Resource = 'Security' AND Action = 'Get'));
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
VALUES(NEWID(), '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', (SELECT MAX(ID) FROM ACCESSPOINT WHERE Resource = 'Security' AND Action = 'Get'));





--Äëÿ FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '3F0B3688-2983-48D8-974C-3B541D1C12A4', Id FROM ACCESSPOINT WHERE Resource not in ('AccessPointRoles','DeletedAccessPoints','HistoricalAccessPoints', 
'AccessPoints','LoopHandlers', 'UserDTOs', 'UserRoles', 'Roles', 'Constraints', 
'Settings', 'MailNotificationSettings',  'Security' );

--Äëÿ CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '568DCBBC-1A61-40D4-88D1-059D4C3F1B8A', Id FROM ACCESSPOINT WHERE Resource not in ('AccessPointRoles','DeletedAccessPoints','HistoricalAccessPoints',
'AccessPoints','LoopHandlers', 'UserDTOs', 'UserRoles', 'Roles', 'Constraints', 
'Settings', 'MailNotificationSettings',  'Security' )
and [Action] like 'Get%';


--Äëÿ DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 'A8F7D3CA-B737-433B-9E1D-78E87BCEF853F', Id FROM ACCESSPOINT WHERE Resource not in ('AccessPointRoles','DeletedAccessPoints','HistoricalAccessPoints',
'AccessPoints','LoopHandlers', 'UserDTOs', 'UserRoles', 'Roles', 'Constraints', 
'Settings', 'MailNotificationSettings', 'Security', 
'DeletedPromoes', 'HistoricalPromoes', 'ImportPromoes', 'Promoes')
and [Action] like 'Get%';

--Äëÿ DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', Id FROM ACCESSPOINT WHERE Resource not in ('AccessPointRoles','DeletedAccessPoints','HistoricalAccessPoints',
'AccessPoints','LoopHandlers', 'UserDTOs', 'UserRoles', 'Roles', 'Constraints', 
'Settings', 'MailNotificationSettings', 'Security', 
'DeletedPromoes', 'HistoricalPromoes', 'ImportPromoes', 'Promoes', 
'DeletedDemands', 'DemandDTOs', 'Demands', 'HistoricalDemands', 'PromoDemands')
and [Action] like 'Get%';

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', Id FROM ACCESSPOINT WHERE Resource in (
'DeletedDemands', 'DemandDTOs', 'Demands', 'HistoricalDemands', 'PromoDemands');


--Äëÿ KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '60009B10-D398-4686-B5C4-6D0032152AF4', Id FROM ACCESSPOINT WHERE Resource in (
'DeletedPromoes', 'HistoricalPromoes', 'ImportPromoes', 'Promoes', 'SingleLoopHandlers', 'BaseClients','UserLoopHandlers', 'BaseClientTreeViews');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '60009B10-D398-4686-B5C4-6D0032152AF4', Id FROM ACCESSPOINT WHERE Resource in (
'ProductTrees', 'ClientTrees') and [Action] like 'Get%';