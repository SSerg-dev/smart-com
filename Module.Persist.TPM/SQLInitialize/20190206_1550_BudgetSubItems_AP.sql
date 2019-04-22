-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE Resource in ('BudgetSubItems','DeletedBudgetSubItems', 'HistoricalBudgetSubItems'));

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM Role Where SystemName = 'Administrator'), Id FROM ACCESSPOINT WHERE Resource in ('BudgetSubItems','DeletedBudgetSubItems', 'HistoricalBudgetSubItems');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '3F0B3688-2983-48D8-974C-3B541D1C12A4', Id FROM ACCESSPOINT WHERE Resource in ('BudgetSubItems','DeletedBudgetSubItems',  'HistoricalBudgetSubItems');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '568DCBBC-1A61-40D4-88D1-059D4C3F1B8A', Id FROM ACCESSPOINT WHERE Resource  in ('BudgetSubItems','DeletedBudgetSubItems', 'HistoricalBudgetSubItems')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '60009b10-d398-4686-b5c4-6d0032152af4', Id FROM ACCESSPOINT WHERE Resource  in ('BudgetSubItems','DeletedBudgetSubItems', 'HistoricalBudgetSubItems')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 'A8F7D3CA-B737-433B-9E1D-78E87BCEF853F', Id FROM ACCESSPOINT WHERE Resource  in ('BudgetSubItems','DeletedBudgetSubItems', 'HistoricalBudgetSubItems')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', Id FROM ACCESSPOINT WHERE Resource in ('BudgetSubItems','DeletedBudgetSubItems', 'HistoricalBudgetSubItems')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');