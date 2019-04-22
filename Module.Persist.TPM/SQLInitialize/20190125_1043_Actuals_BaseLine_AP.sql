----Сами точки доступа

--Раскоммитить в случае наложения на чистую базу

--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('83279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'Actuals', 'GetActual', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('84279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'Actuals', 'GetActuals', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('88279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'Actuals', 'Put', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('8A279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'Actuals', 'Post', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('8C279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'Actuals', 'Patch', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('8E279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'Actuals', 'Delete', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('90279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'Actuals', 'ExportXLSX', '');
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('689E12A1-E91A-E911-8BBF-08606E18DF3F', 0, NULL, 'Actuals', 'FullImportXLSX', '');
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('85279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'DeletedActuals', 'GetDeletedActual', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('86279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'DeletedActuals', 'GetDeletedActuals', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('87279800-4018-E911-8BBF-08606E18DF3F', 0, NULL, 'HistoricalActuals', 'GetHistoricalActuals', NULL);


--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('D1B56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'BaseLines', 'GetBaseLine', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('D2B56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'BaseLines', 'GetBaseLines', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('D6B56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'BaseLines', 'Put', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('D8B56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'BaseLines', 'Post', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('DAB56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'BaseLines', 'Patch', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('DCB56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'BaseLines', 'Delete', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('DEB56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'BaseLines', 'ExportXLSX', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('679E12A1-E91A-E911-8BBF-08606E18DF3F', 0, NULL, 'BaseLines', 'FullImportXLSX', '');
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('D3B56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'DeletedBaseLines', 'GetDeletedBaseLine', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('D4B56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'DeletedBaseLines', 'GetDeletedBaseLines', NULL);
--INSERT INTO AccessPoint (Id, Disabled, DeletedDate, Resource, [Action], Description) VALUES('D5B56402-C418-E911-8BBF-08606E18DF3F', 0, NULL, 'HistoricalBaseLines', 'GetHistoricalBaseLines', NULL);


----Роли для администратора
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('CB01CD53-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '83279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('CE2A4563-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '84279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('E6305B90-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '85279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('E8305B90-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '86279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('5C344B9F-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '87279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('19736C6A-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '88279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('A08A3073-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '8A279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('8783DD79-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '8C279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('05E55780-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '8E279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('CC83D486-4118-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '90279800-4018-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('48C20635-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'D1B56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('A6EECA3C-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'D2B56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('8D78794F-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'D3B56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('3B918955-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'D4B56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('C428825B-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'D5B56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('8CEFCA3C-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'D6B56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('A4F0CA3C-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'D8B56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('94292443-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'DAB56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('752A2443-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'DCB56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('A7128249-C418-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'DEB56402-C418-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('7E558C05-E21A-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), 'E7A837E1-E11A-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('D52B8C2A-E21A-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '820173FA-E11A-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('03357F9B-EA1A-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '679E12A1-E91A-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('DCA8E6A3-EA1A-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '689E12A1-E91A-E911-8BBF-08606E18DF3F');
--INSERT INTO AccessPointRole (Id, RoleId, AccessPointId) VALUES('2D757EE5-ED1A-E911-8BBF-08606E18DF3F', (SELECT ID FROM Role Where SystemName = 'Administrator'), '800E3373-ED1A-E911-8BBF-08606E18DF3F');



--Добавление точек доступа для ролей не администратор

--Для FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '3F0B3688-2983-48D8-974C-3B541D1C12A4', Id FROM ACCESSPOINT WHERE Resource in ('Actuals','DeletedActuals',  'HistoricalActuals','BaseLines' ,'DeletedBaseLines', 'HistoricalBaseLines');

--Для CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '568DCBBC-1A61-40D4-88D1-059D4C3F1B8A', Id FROM ACCESSPOINT WHERE Resource  in ('Actuals','DeletedActuals',  'HistoricalActuals','BaseLines' ,'DeletedBaseLines', 'HistoricalBaseLines')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');


--Для DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 'A8F7D3CA-B737-433B-9E1D-78E87BCEF853F', Id FROM ACCESSPOINT WHERE Resource  in ('Actuals','DeletedActuals',  'HistoricalActuals','BaseLines' ,'DeletedBaseLines', 'HistoricalBaseLines')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

--Для DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', Id FROM ACCESSPOINT WHERE Resource in ('Actuals','DeletedActuals',  'HistoricalActuals','BaseLines' ,'DeletedBaseLines', 'HistoricalBaseLines');


----Удаление
----DELETE FROM AccessPointRole WHERE RoleId IN ('3F0B3688-2983-48D8-974C-3B541D1C12A4', '568DCBBC-1A61-40D4-88D1-059D4C3F1B8A', 'A8F7D3CA-B737-433B-9E1D-78E87BCEF853F', '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1') AND AccessPointId IN (SELECT ID FROM AccessPoint WHERE  Resource in ('Actuals','DeletedActuals',  'HistoricalActuals','BaseLines' ,'DeletedBaseLines', 'HistoricalBaseLines'));