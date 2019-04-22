INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource = 'Promoes' and [Action] ='ExportSchedule';

INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND DeletedDate IS NULL), Id FROM ACCESSPOINT WHERE Resource = 'Promoes' and [Action] ='ExportSchedule';