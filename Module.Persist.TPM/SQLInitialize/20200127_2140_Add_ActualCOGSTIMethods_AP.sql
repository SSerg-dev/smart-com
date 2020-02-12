-- точки доступа
-- избавляемся от дублей
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] = 'ActualCOGSs' AND [Action] IN ('IsCOGSRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualCOGSChangeIncidents') AND [Disabled] = 'false');
DELETE FROM AccessPoint WHERE [Resource] = 'ActualCOGSs' AND [Action] IN ('IsCOGSRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualCOGSChangeIncidents') AND [Disabled] = 'false';
DELETE FROM AccessPointRole WHERE AccessPointId IN (SELECT Id FROM ACCESSPOINT WHERE [Resource] = 'ActualTradeInvestments' AND [Action] IN ('IsTIRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualTIChangeIncidents') AND [Disabled] = 'false');
DELETE FROM AccessPoint WHERE [Resource] = 'ActualTradeInvestments' AND [Action] IN ('IsTIRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualTIChangeIncidents') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'IsCOGSRecalculatePreviousYearButtonAvailable', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'PreviousYearPromoList', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualCOGSs', 'CreateActualCOGSChangeIncidents', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'IsTIRecalculatePreviousYearButtonAvailable', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'PreviousYearPromoList', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('ActualTradeInvestments', 'CreateActualTIChangeIncidents', 0, NULL);

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('IsCOGSRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualCOGSChangeIncidents');

--Support Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('IsCOGSRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualCOGSChangeIncidents');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('IsCOGSRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualCOGSChangeIncidents');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualCOGSs' AND [Action] IN ('IsCOGSRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualCOGSChangeIncidents');

--TI
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('IsTIRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualTIChangeIncidents');

--Support Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('IsTIRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualTIChangeIncidents');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('IsTIRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualTIChangeIncidents');

-- FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='ActualTradeInvestments' AND [Action] IN ('IsTIRecalculatePreviousYearButtonAvailable', 'PreviousYearPromoList', 'CreateActualTIChangeIncidents');