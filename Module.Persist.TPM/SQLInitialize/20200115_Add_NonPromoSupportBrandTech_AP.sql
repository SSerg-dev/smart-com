-- точки доступа
-- избавляемся от дуближа
DELETE FROM AccessPointRole WHERE AccessPointId in (SELECT Id FROM ACCESSPOINT WHERE [Resource] IN ('NonPromoSupportBrandTeches') AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList'));
DELETE FROM AccessPoint WHERE [Resource] IN ('NonPromoSupportBrandTeches') AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList') AND [Disabled] = 'false';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTeches', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupportBrandTeches', 'Put', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupportBrandTeches', 'Post', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupportBrandTeches', 'Delete', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES ('NonPromoSupportBrandTeches', 'ModifyNonPromoSupportBrandTechList', 0, NULL);

-- роли
--Administrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'Administrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');

--SupportAdministrator
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SupportAdministrator' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');

--FunctionalExpert
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'FunctionalExpert' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');

-- CustomerMarketing
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CustomerMarketing' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');

-- CMManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'CMManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');

-- KeyAccountManager
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'KeyAccountManager' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');

-- DemandFinance
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandFinance' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');

-- DemandPlanning
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'DemandPlanning' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');

-- SuperReader
INSERT INTO AccessPointRole
(Id, RoleId, AccessPointId)
SELECT NEWID(), (SELECT ID FROM [Role] Where SystemName = 'SuperReader' AND [Disabled] = 'false'), Id FROM ACCESSPOINT WHERE [Resource]='NonPromoSupportBrandTeches' AND [Action] IN ('GetNonPromoSupportBrandTeches', 'GetNonPromoSupportBrandTech', 'Put', 'Post', 'Delete', 'ModifyNonPromoSupportBrandTechList');