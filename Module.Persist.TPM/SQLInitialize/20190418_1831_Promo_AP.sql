-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('Promoes', 'GetCanChangeStatePromoes', 0, NULL);

DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action]='GetCanChangeStatePromoes');

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action]='GetCanChangeStatePromoes';

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action]='GetCanChangeStatePromoes';
-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action]='GetCanChangeStatePromoes';

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action]='GetCanChangeStatePromoes';

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action]='GetCanChangeStatePromoes';

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE [Resource]='Promoes' AND [Action]='GetCanChangeStatePromoes';