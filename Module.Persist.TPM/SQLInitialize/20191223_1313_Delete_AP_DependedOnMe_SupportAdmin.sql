 
 
DELETE FROM AccessPointRole where AccessPointId = (select Id from AccessPoint  WHERE [Resource]='Promoes' AND [Action] = 'GetCanChangeStatePromoes')
 and RoleId = (select Id from Role where SystemName = 'SupportAdministrator')
 
 