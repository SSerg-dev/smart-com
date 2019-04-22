DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE Resource in ('PromoSupportPromoes'));

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE Resource in ('PromoSupportPromoes');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE Resource  in ('PromoSupportPromoes')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource  in ('PromoSupportPromoes')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource  in ('PromoSupportPromoes')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource  in ('PromoSupportPromoes')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in ('PromoSupportPromoes')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');