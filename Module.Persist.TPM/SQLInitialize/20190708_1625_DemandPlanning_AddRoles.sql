-- удаление дублированных полей
DELETE FROM [AccessPointRole] 
WHERE ([AccessPointId] IN (SELECT Id FROM [AccessPoint] WHERE [Resource]='ClientTrees' AND [Action] IN ('Post', 'Delete', 'UpdateNode', 'Move', 'DeleteLogo', 'UploadLogoFile')))
AND ([RoleId] = (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'));
DELETE FROM [AccessPointRole] WHERE [AccessPointId] IN (SELECT Id FROM [AccessPoint] WHERE [Resource]='ProductTrees' AND [Action] IN ('Post', 'Delete', 'DeleteNode', 'ApplyProductFilter', 'UpdateNode', 'Move', 'DeleteLogo', 'UploadLogo'))
AND ([RoleId] = (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'));


/* ClientTrees */

--Post
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ClientTrees' AND [Action] = 'Post';

--Delete
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ClientTrees' AND [Action] = 'Delete';

--UpdateNode
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ClientTrees' AND [Action] = 'UpdateNode';

--Move
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ClientTrees' AND [Action] = 'Move';

--DeleteLogo
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ClientTrees' AND [Action] = 'DeleteLogo';

--UploadLogoFile
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ClientTrees' AND [Action] = 'UploadLogoFile';



/* ProductTrees */

--Post
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ProductTrees' AND [Action] = 'Post';

--Delete
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ProductTrees' AND [Action] = 'Delete';

--DeleteNode
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ProductTrees' AND [Action] = 'DeleteNode';

--ApplyProductFilter
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ProductTrees' AND [Action] = 'ApplyProductFilter';

--UpdateNode
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ProductTrees' AND [Action] = 'UpdateNode';

--Move
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ProductTrees' AND [Action] = 'Move';

--DeleteLogo
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ProductTrees' AND [Action] = 'DeleteLogo';

--UploadLogo
INSERT INTO [AccessPointRole]
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT [Id] FROM [Role] WHERE [SystemName] = 'DemandPlanning' AND [Disabled] = 'false'), [Id] FROM [ACCESSPOINT] WHERE [Resource] ='ProductTrees' AND [Action] = 'UploadLogoFile';
