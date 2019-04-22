-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('COGSs', 'GetCOGSs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('COGSs', 'GetCOGS', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('COGSs', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('COGSs', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('COGSs', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('COGSs', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('COGSs', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('COGSs', 'FullImportXLSX', 0, NULL);

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedCOGSs', 'GetDeletedCOGSs', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedCOGSs', 'GetDeletedCOGS', 0, NULL);

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalCOGSs', 'GetHistoricalCOGSs', 0, NULL);


INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'COGSs', 'DeletedCOGSs', 'HistoricalCOGSs' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'COGSs', 'DeletedCOGSs', 'HistoricalCOGSs' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'COGSs', 'DeletedCOGSs', 'HistoricalCOGSs' );