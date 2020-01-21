
 DELETE FROM AccessPointRole where AccessPointId IN (select Id from AccessPoint  WHERE [Resource]='PromoSupportPromoes' AND [Action] = 'ManageSubItems')
 and RoleId IN (select Id from Role where SystemName IN ('DemandPlanning', 'SuperReader', 'DemandFinance'))
