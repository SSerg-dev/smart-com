INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'BaseLines', 'DeletedBaseLines', 'HistoricalBaseLines');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'BaseLines', 'DeletedBaseLines', 'HistoricalBaseLines');
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource in (
'BaseLines', 'DeletedBaseLines', 'HistoricalBaseLines') and  ([Action] like 'Get%' or [Action] ='ExportXLSX');