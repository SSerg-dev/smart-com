
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'BaseLines' ) and  ([Action] like 'Get%' or [Action] ='ExportXLSX');


INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'Products', 'Brands', 'Technologies', 'BrandTeches', 'RetailTypes') and  [Action] ='ExportXLSX';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'Products', 'Brands', 'Technologies', 'BrandTeches', 'RetailTypes' ) and   [Action] ='ExportXLSX';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'Products', 'Brands', 'Technologies', 'BrandTeches', 'RetailTypes' ) and   [Action] ='ExportXLSX';