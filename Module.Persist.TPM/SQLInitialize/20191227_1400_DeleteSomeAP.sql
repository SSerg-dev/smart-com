DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='PromoProductsCorrections' AND [Action] = 'Put')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'KeyAccountManager'))
 
 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='PromoProductsCorrections' AND [Action] = 'Patch')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='PromoProductsCorrections' AND [Action] = 'Post')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='PromoProductsCorrections' AND [Action] = 'Delete')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='PromoProductsCorrections' AND [Action] = 'FullImportXLSX')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'KeyAccountManager'))
 
 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='PromoProductsCorrections' AND [Action] = 'DownloadTemplateXLSX')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'CustomerMarketing', 'CMManager', 'KeyAccountManager'))

 
 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='NonPromoEquipments' AND [Action] = 'Patch')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='NonPromoEquipments' AND [Action] = 'Patch')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='NonPromoEquipments' AND [Action] = 'FullImportXLSX')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='NonPromoEquipments' AND [Action] = 'DownloadTemplateXLSX')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='NonPromoEquipments' AND [Action] = 'Put')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='NonPromoEquipments' AND [Action] = 'Post')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))
 
 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='NonPromoEquipments' AND [Action] = 'Delete')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))

 
 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='EventClientTrees' AND [Action] = 'Post')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='EventClientTrees' AND [Action] = 'Put')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))

 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='EventClientTrees' AND [Action] = 'Delete')
 and RoleId IN (select Id from Role where SystemName IN ('DemandFinance', 'SuperReader', 'DemandPlanning', 'KeyAccountManager'))