/****** Script for SelectTopNRows command from SSMS  ******/
 
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('Products', 'GetProductInPromo', 0, NULL);
 

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] IN ('Products') and [Action] IN('GetProductInPromo')
