--Правки по спецификации draft v2

--Для FunctionalExpert
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE Resource  in ('PromoStatuss') AND [Action] not like 'Get%') AND RoleId = '3F0B3688-2983-48D8-974C-3B541D1C12A4'

--Для CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '568DCBBC-1A61-40D4-88D1-059D4C3F1B8A', Id FROM ACCESSPOINT 
WHERE Resource in ('DeletedPromoes', 'HistoricalPromoes', 'ImportPromoes', 'Promoes',  'BaseClients', 
'MechanicTypes', 'HistoricalMechanicTypes', 'DeletedMechanicTypes',
'Events', 'HistoricalEvents','DeletedEvents',
'NoneNegoes','HistoricalNoneNegoes','DeletedNoneNegoes' )
and [Action] not like 'Get%';

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '568DCBBC-1A61-40D4-88D1-059D4C3F1B8A', Id FROM ACCESSPOINT 
WHERE Resource in ('PromoStatusChanges' )
and [Action] ='PromoStatusChangesByPromo';

--Для DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 'A8F7D3CA-B737-433B-9E1D-78E87BCEF853F', Id FROM ACCESSPOINT WHERE Resource in (
'DeletedPromoes', 'HistoricalPromoes', 'ImportPromoes', 'Promoes',  'BaseClients')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');


INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), 'A8F7D3CA-B737-433B-9E1D-78E87BCEF853F', Id FROM ACCESSPOINT 
WHERE Resource in ('PromoStatusChanges' )
and [Action] ='PromoStatusChangesByPromo';


--Для DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', Id FROM ACCESSPOINT WHERE Resource in (
'DeletedPromoes', 'HistoricalPromoes', 'ImportPromoes', 'Promoes',  'BaseClients')
and ([Action] like 'Get%' or [Action] ='ExportXLSX');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', Id FROM ACCESSPOINT 
WHERE Resource in ('PromoStatusChanges' )
and [Action] ='PromoStatusChangesByPromo';

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '1D227DD1-48AC-430F-9E30-66C8BD1BCAD1', Id FROM ACCESSPOINT WHERE Resource in (
'ClientTreeSharesViews')
and not ([Action] like 'Get%');

--Для KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '60009B10-D398-4686-B5C4-6D0032152AF4', Id FROM ACCESSPOINT WHERE Resource in (
'Products', 'DeletedProducts', 'HistoricalProducts',
'Brands','DeletedBrands','HistoricalBrands',
'Technologies','DeletedTechnologies','HistoricalTechnologies',
'BrandTeches','DeletedBrandTeches','HistoricalBrandTeches',
'Budgets','DeletedBudgets','HistoricalBudgets',
'BudgetItems','DeletedBudgetItems','HistoricalBudgetItems',
'Mechanics','DeletedMechanics','HistoricalMechanics',
'MechanicTypes','DeletedMechanicTypes','HistoricalMechanicTypes',
'PromoStatuss','DeletedPromoStatuss','HistoricalPromoStatuss',
'RejectReasons','DeletedRejectReasons','HistoricalRejectReasons',
'Colors','DeletedColors','HistoricalColors',
'Events','DeletedEvents','HistoricalEvents',
'NodeTypes','DeletedNodeTypes','HistoricalNodeTypes',
'NoneNegoes','DeletedNoneNegoes','HistoricalNoneNegoes') and ([Action] like 'Get%' or [Action] ='ExportXLSX');

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), '60009B10-D398-4686-B5C4-6D0032152AF4', Id FROM ACCESSPOINT 
WHERE Resource in ('PromoStatusChanges' )
and [Action] ='PromoStatusChangesByPromo';


