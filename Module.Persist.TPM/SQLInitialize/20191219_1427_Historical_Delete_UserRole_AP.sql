-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('HistoricalUserRoles', 'DeletedUserRoles') AND [Action] IN ('GetHistoricalUserRoles', 'GetDeletedUserRoles'));
DELETE FROM AccessPoint WHERE [Resource] IN ('HistoricalUserRoles', 'DeletedUserRoles') AND [Action] IN ('GetHistoricalUserRoles', 'GetDeletedUserRoles') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalUserRoles', 'GetHistoricalUserRoles', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedUserRoles', 'GetDeletedUserRoles', 0, NULL);

-- роли
--Administrator
--Historical 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='HistoricalUserRoles' AND [Action] IN ('GetHistoricalUserRoles');
--Deleted 
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='DeletedUserRoles' AND [Action] IN ('GetDeletedUserRoles');