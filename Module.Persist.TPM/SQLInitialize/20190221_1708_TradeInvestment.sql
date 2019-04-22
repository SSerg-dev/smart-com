-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('TradeInvestments', 'GetTradeInvestments', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('TradeInvestments', 'GetCOGS', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('TradeInvestments', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('TradeInvestments', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('TradeInvestments', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('TradeInvestments', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('TradeInvestments', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('TradeInvestments', 'FullImportXLSX', 0, NULL);

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedTradeInvestments', 'GetDeletedTradeInvestments', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedTradeInvestments', 'GetDeletedTradeInvestment', 0, NULL);

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalTradeInvestments', 'GetHistoricalTradeInvestments', 0, NULL);


INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'TradeInvestments', 'DeletedTradeInvestments', 'HistoricalTradeInvestments' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'TradeInvestments', 'DeletedTradeInvestments', 'HistoricalTradeInvestments' );
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'TradeInvestments', 'DeletedTradeInvestments', 'HistoricalTradeInvestments' );