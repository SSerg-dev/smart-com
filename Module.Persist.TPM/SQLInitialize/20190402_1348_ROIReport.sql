-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoROIReports', 'GetPromoROIReports', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoROIReports', 'ExportXLSX', 0, NULL);



INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PromoROIReports' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PromoROIReports' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PromoROIReports' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PromoROIReports' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PromoROIReports' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PromoROIReports' );

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'PromoROIReports' );
