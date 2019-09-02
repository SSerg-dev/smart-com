DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] = 'SchedulerClientTreeDTOs');
DELETE FROM AccessPoint WHERE [Resource] = 'SchedulerClientTreeDTOs';

/* Action - 'GetSchedulerClientTreeDTOs' */
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('SchedulerClientTreeDTOs', 'GetSchedulerClientTreeDTOs', 0, NULL);

-- Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='SchedulerClientTreeDTOs';

-- FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE [Resource]='SchedulerClientTreeDTOs';

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE [Resource]='SchedulerClientTreeDTOs';

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager'), Id FROM ACCESSPOINT WHERE [Resource]='SchedulerClientTreeDTOs';

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE [Resource]='SchedulerClientTreeDTOs';

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE [Resource]='SchedulerClientTreeDTOs';


/* Action - 'Post' */
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('SchedulerClientTreeDTOs', 'Post', 0, NULL);

-- Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE ([Resource] = 'SchedulerClientTreeDTOs' AND [Action] = 'Post' AND [Disabled] = 0);

-- FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE ([Resource] = 'SchedulerClientTreeDTOs' AND [Action] = 'Post' AND [Disabled] = 0);

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE ([Resource] = 'SchedulerClientTreeDTOs' AND [Action] = 'Post' AND [Disabled] = 0);

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE ([Resource] = 'SchedulerClientTreeDTOs' AND [Action] = 'Post' AND [Disabled] = 0);

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE ([Resource] = 'SchedulerClientTreeDTOs' AND [Action] = 'Post' AND [Disabled] = 0);