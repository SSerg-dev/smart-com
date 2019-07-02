DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'PlanIncrementalReports') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'DeletedPlanIncrementalReports') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'HistoricalPlanIncrementalReports') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)

DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'PlanPostPromoEffectReports') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'DeletedPlanPostPromoEffectReports') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'HistoricalPlanPostPromoEffectReports') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)

DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'IncrementalPromoes') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'DeletedIncrementalPromoes') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'HistoricalIncrementalPromoes') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)

DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'ActualLSVs') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'DeletedActualLSVs') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'HistoricalActualLSVs') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)

DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'TradeInvestments') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'DeletedTradeInvestments') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'HistoricalTradeInvestments') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)

DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'COGSs') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'DeletedCOGSs') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = 'HistoricalCOGSs') 
AND RoleId IN (SELECT TOP 1 Id FROM [Role] WHERE SystemName = 'CustomerMarketing' AND [Disabled] = 0)

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT Id FROM [Role] WHERE LOWER([SystemName]) = 'CustomerMarketing' AND [Disabled] = 0), Id FROM ACCESSPOINT WHERE Resource + ':' + [Action] in (
	'PlanIncrementalReports:ExportXLSX',
	'PlanIncrementalReports:GetPlanIncrementalReports',
	'DeletedPlanIncrementalReports:GetDeletedPlanIncrementalReports',
	'HistoricalPlanIncrementalReports:GetHistoricalPlanIncrementalReports',
	
	'PlanPostPromoEffectReports:ExportXLSX',
	'PlanPostPromoEffectReports:GetPlanPostPromoEffectReports',
    'DeletedPlanPostPromoEffectReports:GetDeletedPlanPostPromoEffectReports',
	'HistoricalPlanPostPromoEffectReports:GetHistoricalPlanPostPromoEffectReports',

	'IncrementalPromoes:ExportXLSX',
	'IncrementalPromoes:GetIncrementalPromoes',
    'DeletedIncrementalPromoes:GetDeletedIncrementalPromoes',
	'HistoricalIncrementalPromoes:GetHistoricalIncrementalPromoes',

	'ActualLSVs:ExportXLSX',
	'ActualLSVs:GetActualLSVs',
    'DeletedActualLSVs:GetDeletedActualLSVs',
	'HistoricalActualLSVs:GetHistoricalActualLSVs',

	'TradeInvestments:ExportXLSX',
	'TradeInvestments:GetTradeInvestments',
    'DeletedTradeInvestments:GetDeletedTradeInvestments',
	'HistoricalTradeInvestments:GetHistoricalTradeInvestments',

	'COGSs:ExportXLSX',
	'COGSs:GetCOGSs',
    'DeletedCOGSs:GetDeletedCOGSs',
	'HistoricalCOGSs:GetHistoricalCOGSs'
);
