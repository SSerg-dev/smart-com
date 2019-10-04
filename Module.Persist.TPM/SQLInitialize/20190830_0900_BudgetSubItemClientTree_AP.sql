-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE Resource in ('BudgetSubItemClientTrees'));
Delete FROM AccessPoint WHERE ([Resource] = 'BudgetSubItemClientTrees' AND [Disabled] = 'false');

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BudgetSubItemClientTrees', 'GetBudgetSubItemClientTrees', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BudgetSubItemClientTrees', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BudgetSubItemClientTrees', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BudgetSubItemClientTrees', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('BudgetSubItemClientTrees', 'Delete', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE Resource in ('BudgetSubItemClientTrees');
