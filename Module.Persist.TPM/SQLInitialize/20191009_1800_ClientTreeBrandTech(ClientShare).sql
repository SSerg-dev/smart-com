DECLARE @ResourceName VARCHAR(MAX) = 'ClientTreeBrandTeches';

-- Удаляем роли у экшинов
DELETE AccessPointRole WHERE AccessPointId IN (SELECT Id FROM AccessPoint WHERE Resource = @ResourceName);
-- Удаляем экшины
DELETE AccessPoint WHERE Resource = @ResourceName;

-- Добавляем экшины
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES (@ResourceName, 'GetClientTreeBrandTech', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES (@ResourceName, 'GetClientTreeBrandTeches', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES (@ResourceName, 'ExportXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES (@ResourceName, 'DownloadTemplateXLSX', 0, NULL);
INSERT INTO AccessPoint (Resource, Action, Disabled, DeletedDate) VALUES (@ResourceName, 'FullImportXLSX', 0, NULL);

-- Добавляем все роли для всех экшинов
DECLARE @AccessPointCount INT = (SELECT COUNT(*) FROM AccessPoint WHERE Resource = @ResourceName AND Disabled = 0);
DECLARE @RoleCount INT = (SELECT COUNT(*) FROM Role WHERE Disabled = 0);
DECLARE @AccessPointCounter INT = 0;
WHILE @AccessPointCounter < @AccessPointCount
BEGIN
	DECLARE @AccessPointId UNIQUEIDENTIFIER;
	SELECT @AccessPointId = Id FROM AccessPoint WHERE Resource = @ResourceName AND Disabled = 0 ORDER BY Resource OFFSET @AccessPointCounter ROWS FETCH NEXT 1 ROWS ONLY;
	DECLARE @RoleCounter INT = 0;
	WHILE @RoleCounter < @RoleCount
	BEGIN
		DECLARE @RoleId UNIQUEIDENTIFIER;
		SELECT @RoleId = Id FROM Role WHERE Disabled = 0 ORDER BY SystemName OFFSET @RoleCounter ROWS FETCH NEXT 1 ROWS ONLY;
		INSERT INTO AccessPointRole ([Id], [AccessPointId], [RoleId]) VALUES (NEWID(), @AccessPointId, @RoleId);
		SET @RoleCounter = @RoleCounter + 1;
	END
	SET @AccessPointCounter = @AccessPointCounter + 1;
END

-- Убираем ненужные роли
DELETE AccessPointRole WHERE AccessPointId IN 
(SELECT Id FROM AccessPoint WHERE Resource = @ResourceName AND (Action = 'FullImportXLSX' OR Action = 'DownloadTemplateXLSX')) AND
RoleId IN (SELECT Id FROM Role WHERE SystemName != 'Administrator' AND SystemName != 'KeyAccountManager' AND SystemName != 'DemandPlanning' AND SystemName != 'FunctionalExpert')
GO

-- Очистка таблицы ClientTreeBrandTech
DELETE ClientTreeBrandTech;
GO

-- Скрипт заполнения Client Share
DECLARE @ClientTreeCount INT = (SELECT COUNT(*) FROM ClientTree WHERE EndDate IS NULL AND IsBaseClient = 1);
DECLARE @ClientTreeCounter INT = 0;

WHILE (@ClientTreeCounter < @ClientTreeCount)
BEGIN
	DECLARE @ClientTreeId INT;
	DECLARE @ObjectId VARCHAR(MAX);
	SELECT @ClientTreeId = Id, @ObjectId = ObjectId FROM ClientTree WHERE EndDate IS NULL AND IsBaseClient = 1 ORDER BY Id OFFSET @ClientTreeCounter ROWS FETCH NEXT 1 ROWS ONLY;
	
	IF @ObjectId IS NOT NULL 
	BEGIN
		DECLARE @ParentDemandCode VARCHAR(MAX) = NULL;
		DECLARE @ParentClientTreeType VARCHAR(MAX) = NULL;

		WHILE ((@ParentClientTreeType IS NULL OR @ParentClientTreeType != 'root') AND (@ParentDemandCode IS NULL OR LEN(@ParentDemandCode) = 0))
		BEGIN
			SELECT @ObjectId = parentId, @ParentClientTreeType = Type, @ParentDemandCode = DemandCode FROM ClientTree WHERE EndDate IS NULL AND ObjectId = @ObjectId;
		END

		IF @ParentDemandCode IS NOT NULL AND LEN(@ParentDemandCode) > 0
		BEGIN
			DECLARE @BrandTechCounter INT = 0;
			DECLARE @BrandTechCount INT = (SELECT COUNT(*) FROM BrandTech WHERE Disabled = 0)

			WHILE (@BrandTechCounter < @BrandTechCount)
			BEGIN
                DECLARE @BrandTechId UNIQUEIDENTIFIER;
                DECLARE @CurrentBrandTechName VARCHAR(MAX);

				SELECT @BrandTechId = Id, @CurrentBrandTechName = Name FROM BrandTech WHERE Disabled = 0 ORDER BY Id OFFSET @BrandTechCounter ROWS FETCH NEXT 1 ROWS ONLY;

				IF (SELECT COUNT(*) FROM ClientTreeBrandTech WHERE ParentClientTreeDemandCode = @ParentDemandCode AND ClientTreeId = @ClientTreeId AND BrandTechId = @BrandTechId AND Disabled = 0) = 0
				BEGIN
					INSERT INTO ClientTreeBrandTech ([Id], [ClientTreeId], [BrandTechId], [ParentClientTreeDemandCode], [Share], [CurrentBrandTechName], [DeletedDate], [Disabled]) VALUES (NEWID(), @ClientTreeId, @BrandTechId, @ParentDemandCode, 0, @CurrentBrandTechName, null, 0);
				END
				SET @BrandTechCounter = @BrandTechCounter + 1;
			END
		END
	END
	SET @ClientTreeCounter = @ClientTreeCounter + 1;
END
GO