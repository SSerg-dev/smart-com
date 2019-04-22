-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('RetailTypes', 'FullImportXLSX', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('RetailTypes') AND [Action]='FullImportXLSX';


--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('RetailTypes') AND [Action]='FullImportXLSX';

