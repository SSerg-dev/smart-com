ALTER TABLE [dbo].[BrandTech] DROP COLUMN [BrandTech_code];
GO

ALTER TABLE [dbo].[Product] DROP COLUMN [Brand];
GO
ALTER TABLE [dbo].[Product] DROP COLUMN [Technology];
GO
ALTER TABLE [dbo].[Product] DROP COLUMN [BrandTech];
GO
ALTER TABLE [dbo].[Product] DROP COLUMN [BrandTech_code];
GO
ALTER TABLE [dbo].[Product] DROP COLUMN [BrandsegTech_code];
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
			Select @result = CONCAT(Brand.Brand_code, '-', Technology.Tech_code) From Brand, Technology
				Where Brand.Id = @brandId AND Technology.Id = @technologyId
		END
	ELSE
		BEGIN
			RETURN NULL
		END

	RETURN @result
END
GO

----------------------------------- Product.Brand -------------------------------------
CREATE OR ALTER FUNCTION [dbo].[GetProductBrandByCode]
(
	@brandCode NVARCHAR(3),
	@segmenCode NVARCHAR(2)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	Select @result = Brand.name From Brand
				Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Brand.Disabled = 0
				
	RETURN @result
END
GO

-------------------------------- Product.Technology ------------------------------------
CREATE OR ALTER FUNCTION [dbo].[GetProductTechByCode]
(
	@technologyCode NVARCHAR(3)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	Select @result = Technology.name From Technology
				Where Technology.Tech_code = @technologyCode AND Technology.Disabled = 0
				
	RETURN @result
END
GO

--------------------------------- Product.BrandTech -----------------------------------
CREATE OR ALTER FUNCTION [dbo].[GetProductBrandTechByCode]
(
	@brandCode NVARCHAR(3),
	@segmenCode NVARCHAR(2),
	@technologyCode NVARCHAR(3)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	Select @result = CONCAT(Brand.name, ' ', Technology.name) From Brand, Technology
				Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Brand.Disabled = 0 AND Technology.Tech_code = @technologyCode AND Technology.Disabled = 0

	RETURN @result
END
GO

--------------------------------- Product.BrandTech_code -----------------------------------
CREATE OR ALTER FUNCTION [dbo].[GetProductBrandTechCode]
(
	@brandCode NVARCHAR(3),
	@segmenCode NVARCHAR(2),
	@technologyCode NVARCHAR(3)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	IF (SELECT COUNT(Brand.Id) FROM Brand WHERE Brand.Brand_code = @brandCode AND Brand.Brand_code IS NOT NULL AND Brand.Segmen_code = @segmenCode AND Brand.Segmen_code IS NOT NULL) > 0 AND (SELECT COUNT(Technology.Id) FROM Technology WHERE Technology.Tech_code = @technologyCode AND Technology.Tech_code IS NOT NULL) > 0
		BEGIN
			Select @result = CONCAT(Brand.Brand_code, '-', Technology.Tech_code) From Brand, Technology
				Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Technology.Tech_code = @technologyCode
		END
	ELSE
		BEGIN
			RETURN NULL
		END

	RETURN @result
END
GO

--------------------------------- Product.BrandsegTech_code -----------------------------------
CREATE OR ALTER FUNCTION [dbo].[GetProductBrandsegTechCode]
(
	@brandCode NVARCHAR(3),
	@segmenCode NVARCHAR(2),
	@technologyCode NVARCHAR(3)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	IF (SELECT COUNT(Brand.Id) FROM Brand WHERE Brand.Brand_code = @brandCode AND Brand.Brand_code IS NOT NULL AND Brand.Segmen_code = @segmenCode AND Brand.Segmen_code IS NOT NULL) > 0 AND (SELECT COUNT(Technology.Id) FROM Technology WHERE Technology.Tech_code = @technologyCode AND Technology.Tech_code IS NOT NULL) > 0
		BEGIN
			Select @result = CONCAT(Brand.Brand_code, '-', Brand.Segmen_code, '-', Technology.Tech_code) From Brand, Technology
				Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Technology.Tech_code = @technologyCode
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

-------------------------------------- Product -----------------------------------------
ALTER TABLE [dbo].[Product]
    ADD [Brand] AS ([dbo].[GetProductBrandByCode]([Brand_code], [Segmen_code]));
GO
ALTER TABLE [dbo].[Product]
    ADD [Technology] AS ([dbo].[GetProductTechByCode]([Tech_code]));
GO
ALTER TABLE [dbo].[Product]
    ADD [BrandTech] AS ([dbo].[GetProductBrandTechByCode]([Brand_code], [Segmen_code], [Tech_code]));
GO
ALTER TABLE [dbo].[Product]
    ADD [BrandTech_code] AS ([dbo].[GetProductBrandTechCode]([Brand_code], [Segmen_code], [Tech_code]));
GO
ALTER TABLE [dbo].[Product]
    ADD [BrandsegTech_code] AS ([dbo].[GetProductBrandsegTechCode]([Brand_code], [Segmen_code], [Tech_code]));
GO
