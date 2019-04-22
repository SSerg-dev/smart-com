--NoneNegoes/IsValidPeriod
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE Resource in (
'NoneNegoes') and   [Action] ='IsValidPeriod';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource in (
'NoneNegoes') and   [Action] ='IsValidPeriod';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in (
'NoneNegoes') and   [Action] ='IsValidPeriod';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource in (
'NoneNegoes') and   [Action] ='IsValidPeriod';


--In Directory Export
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE Resource in (
'Budgets',
'BudgetItems',
'BudgetSubItems',
'Mechanics',
'MechanicTypes',
'PromoStatuss',
'RejectReasons',
'Events') and   [Action] ='ExportXLSX';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE Resource in (
'Budgets',
'BudgetItems',
'BudgetSubItems',
'Mechanics',
'MechanicTypes',
'PromoStatuss',
'RejectReasons',
'Events') and   [Action] ='ExportXLSX';
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE Resource in (
'Budgets',
'BudgetItems',
'BudgetSubItems',
'Mechanics',
'MechanicTypes',
'PromoStatuss',
'RejectReasons',
'Events') and   [Action] ='ExportXLSX';