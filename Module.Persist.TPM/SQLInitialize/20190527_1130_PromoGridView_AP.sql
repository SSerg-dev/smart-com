-- точки доступа
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'GetPromoGridViews', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'GetPromoGridView', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'Patch', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'DeclinePromo', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('PromoGridViews', 'GetCanChangeStatePromoGridViews', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedPromoGridViews', 'GetDeletedPromoGridViews', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('DeletedPromoGridViews', 'GetDeletedPromoGridView', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('HistoricalPromoGridViews', 'GetHistoricalPromoGridView', 0, NULL);

DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] in ('PromoGridViews', 'DeletedPromoGridViews', 'HistoricalPromoGridViews'));

--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' and [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] in 
('PromoGridViews', 'DeletedPromoGridViews', 'HistoricalPromoGridViews') AND [Action] in 
('GetPromoGridViews', 'GetPromoGridView', 'Put', 'Post', 'Patch', 'ExportXLSX', 'GetCanChangeStatePromoes', 'Delete', 'GetHistoricalPromoGridViews', 'GetDeletedPromoGridViews', 'GetDeletedPromoGridView');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert'), Id FROM ACCESSPOINT WHERE [Resource] in 
('PromoGridViews', 'DeletedPromoGridViews', 'HistoricalPromoGridViews') AND [Action] in 
('GetPromoGridViews', 'GetPromoGridView', 'Put', 'Post', 'Patch', 'ExportXLSX', 'GetCanChangeStatePromoes', 'Delete', 'GetHistoricalPromoGridViews', 'GetDeletedPromoGridViews', 'GetDeletedPromoGridView');
-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing'), Id FROM ACCESSPOINT WHERE [Resource] in 
('PromoGridViews', 'DeletedPromoGridViews', 'HistoricalPromoGridViews') AND [Action] in 
('GetPromoGridViews', 'GetPromoGridView', 'Put', 'Post', 'Patch', 'ExportXLSX', 'GetCanChangeStatePromoes',  'Delete', 'GetHistoricalPromoGridViews', 'GetDeletedPromoGridViews', 'GetDeletedPromoGridView');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager'), Id FROM ACCESSPOINT WHERE [Resource] in 
('PromoGridViews', 'DeletedPromoGridViews', 'HistoricalPromoGridViews') AND [Action] in 
('GetPromoGridViews', 'GetPromoGridView', 'Put', 'Post', 'Patch', 'ExportXLSX', 'GetCanChangeStatePromoes', 'Delete', 'GetHistoricalPromoGridViews', 'GetDeletedPromoGridViews', 'GetDeletedPromoGridView');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance'), Id FROM ACCESSPOINT WHERE [Resource] in 
('PromoGridViews', 'DeletedPromoGridViews', 'HistoricalPromoGridViews') AND [Action] in 
('GetPromoGridViews', 'GetPromoGridView', 'ExportXLSX', 'GetCanChangeStatePromoes', 'GetHistoricalPromoGridViews', 'GetDeletedPromoGridViews', 'GetDeletedPromoGridView');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning'), Id FROM ACCESSPOINT WHERE [Resource] in 
('PromoGridViews', 'DeletedPromoGridViews', 'HistoricalPromoGridViews') AND [Action] in 
('GetPromoGridViews', 'GetPromoGridView', 'ExportXLSX', 'GetCanChangeStatePromoes', 'GetHistoricalPromoGridViews', 'GetDeletedPromoGridViews', 'GetDeletedPromoGridView');

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource] in 
('PromoGridViews', 'DeletedPromoGridViews', 'HistoricalPromoGridViews') AND [Action] in 
('GetPromoGridViews', 'GetPromoGridView', 'ExportXLSX', 'GetHistoricalPromoGridViews', 'GetDeletedPromoGridViews', 'GetDeletedPromoGridView');
