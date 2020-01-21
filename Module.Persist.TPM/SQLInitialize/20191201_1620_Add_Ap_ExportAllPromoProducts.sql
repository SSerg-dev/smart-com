DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE ([Resource] IN ('PromoProducts') AND [Action] IN ('SupportAdminExportXLSX')))
DELETE FROM AccessPoint WHERE ([Resource] IN ('PromoProducts') AND [Action] IN ('SupportAdminExportXLSX'))

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoProducts', 'SupportAdminExportXLSX', 0, NULL);
 
--SP
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT 
WHERE ([Resource] IN ('PromoProducts') AND [Action] IN ('SupportAdminExportXLSX'))
