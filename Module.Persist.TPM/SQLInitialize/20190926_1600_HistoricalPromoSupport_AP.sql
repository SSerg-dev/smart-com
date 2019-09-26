-- Вкладка PromoSupport
DECLARE @ResourceName VARCHAR(MAX) = 'HistoricalPromoSupports';
DECLARE @Action VARCHAR(MAX) = 'GetHistoricalPromoSupports';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES (@ResourceName, @Action, 0, NULL);
DECLARE @AccessPointId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM AccessPoint WHERE Resource = @ResourceName AND Action = @Action AND Disabled = 0);

-- Добавляем все роли
DECLARE @RoleCount INT = (SELECT COUNT(*) FROM Role WHERE Disabled = 0);
DECLARE @RoleCounter INT = 0;
WHILE @RoleCounter < @RoleCount
BEGIN
	DECLARE @RoleId UNIQUEIDENTIFIER = (SELECT Id FROM Role WHERE Disabled = 0 ORDER BY SystemName OFFSET @RoleCounter ROWS FETCH NEXT 1 ROWS ONLY);
	INSERT INTO AccessPointRole ([Id], [AccessPointId], [RoleId]) VALUES (NEWID(), @AccessPointId, @RoleId);
	SET @RoleCounter = @RoleCounter + 1;
END

-- Вкладка CostProduction
DECLARE @ResourceName VARCHAR(MAX) = 'HistoricalCostProductions';
DECLARE @Action VARCHAR(MAX) = 'GetHistoricalCostProductions';

INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES (@ResourceName, @Action, 0, NULL);
DECLARE @AccessPointId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM AccessPoint WHERE Resource = @ResourceName AND Action = @Action AND Disabled = 0);

-- Добавляем все роли
DECLARE @RoleCount INT = (SELECT COUNT(*) FROM Role WHERE Disabled = 0);
DECLARE @RoleCounter INT = 0;
WHILE @RoleCounter < @RoleCount
BEGIN
	DECLARE @RoleId UNIQUEIDENTIFIER = (SELECT Id FROM Role WHERE Disabled = 0 ORDER BY SystemName OFFSET @RoleCounter ROWS FETCH NEXT 1 ROWS ONLY);
	INSERT INTO AccessPointRole ([Id], [AccessPointId], [RoleId]) VALUES (NEWID(), @AccessPointId, @RoleId);
	SET @RoleCounter = @RoleCounter + 1;
END
