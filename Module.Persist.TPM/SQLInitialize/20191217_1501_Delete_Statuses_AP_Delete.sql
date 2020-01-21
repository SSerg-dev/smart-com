 
 
 DELETE FROM AccessPointRole WHERE [AccessPointId] in (SELECT [Id] FROM [AccessPoint] WHERE [Resource] IN ('PromoStatuss') and [Action] IN ('Delete')) and [RoleId] in (select [Id] from [Role] where [SystemName] IN ('SupportAdministrator')) 