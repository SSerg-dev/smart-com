-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PlanPostPromoEffectReports', 'GetPlanPostPromoEffectReports', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PlanPostPromoEffectReports', 'ExportXLSX', 0, NULL);



INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PlanPostPromoEffectReports' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PlanPostPromoEffectReports' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PlanPostPromoEffectReports' );