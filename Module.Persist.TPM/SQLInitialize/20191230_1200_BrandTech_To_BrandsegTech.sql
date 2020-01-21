ALTER TABLE [dbo].[BrandTech] DROP COLUMN [BrandTech_code];
GO
----------------------------------------------------------------------------------------
-------------------- SECTION: Функция вычисления BrandTech_code ------------------------
----------------------------------------------------------------------------------------

------------------------------------- BrandTech ----------------------------------------
CREATE OR ALTER FUNCTION [dbo].[GetBrandTechCode]
(
	@brandId uniqueidentifier,
	@technologyId uniqueidentifier
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	IF (SELECT COUNT(Brand.Id) FROM Brand WHERE Brand.Id = @brandId AND Brand.Brand_code IS NOT NULL) > 0 AND (SELECT COUNT(Technology.Id) FROM Technology WHERE Technology.Id = @technologyId AND Technology.Tech_code IS NOT NULL) > 0
		BEGIN
			Select @result = CONCAT(Brand.Brand_code, '-', Brand.Segmen_code, '-', Technology.Tech_code) From Brand, Technology
				Where Brand.Id = @brandId AND Technology.Id = @technologyId
		END
	ELSE
		BEGIN
			RETURN NULL
		END

	RETURN @result
END
GO

----------------------------------------------------------------------------------------
-------------------- SECTION: Преобразование столбцов в вычисляемые --------------------
----------------------------------------------------------------------------------------
	
------------------------------------- BrandTech ----------------------------------------
ALTER TABLE [dbo].[BrandTech]
    ADD [BrandTech_code] AS ([dbo].[GetBrandTechCode]([BrandId], [TechnologyId]));
GO