ALTER TABLE [Product] DROP COLUMN [Technology];
GO
ALTER TABLE [Product] DROP COLUMN [BrandTech];
GO
----------------------------------------------------------------------------------------

-------------------------------- Product.Technology ------------------------------------
CREATE OR ALTER FUNCTION [GetProductTechByCode]
(
	@technologyCode NVARCHAR(3),
	@brandsegtech NVARCHAR(255)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	IF (CHARINDEX('App.Mix', @brandsegtech) = 0)
		BEGIN
			Select @result = Technology.name From Technology
					Where Technology.Tech_code = @technologyCode AND Technology.Disabled = 0 AND CHARINDEX('App.Mix', Technology.Name) = 0
		END
	ELSE 
		BEGIN
			Select @result = Technology.name From Technology
					Where Technology.Tech_code = @technologyCode AND Technology.Disabled = 0 AND CHARINDEX('App.Mix', Technology.Name) <> 0
		END
				
	RETURN @result
END
GO

--------------------------------- Product.BrandTech -----------------------------------
CREATE OR ALTER FUNCTION [GetProductBrandTechByCode]
(
	@brandCode NVARCHAR(3),
	@segmenCode NVARCHAR(2),
	@technologyCode NVARCHAR(3),
	@brandsegtech NVARCHAR(255)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	IF (CHARINDEX('App.Mix', @brandsegtech) = 0)
		BEGIN
			Select @result = CONCAT(Brand.name, ' ', Technology.name) From Brand, Technology
				Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Brand.Disabled = 0 AND Technology.Tech_code = @technologyCode AND Technology.Disabled = 0 AND CHARINDEX('App.Mix', Technology.Name) = 0
		END
	ELSE 
		BEGIN
			Select @result = CONCAT(Brand.name, ' ', Technology.name) From Brand, Technology
				Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Brand.Disabled = 0 AND Technology.Tech_code = @technologyCode AND Technology.Disabled = 0 AND CHARINDEX('App.Mix', Technology.Name) <> 0
		END
		
	RETURN @result
END
GO


----------------------------------------------------------------------------------------
-------------------- SECTION: Преобразование столбцов в вычисляемые --------------------
----------------------------------------------------------------------------------------
-------------------------------------- Product -----------------------------------------
ALTER TABLE [Product]
    ADD [Technology] AS ([GetProductTechByCode]([Tech_code], [Brandsegtech]));
GO
ALTER TABLE [Product]
    ADD [BrandTech] AS ([GetProductBrandTechByCode]([Brand_code], [Segmen_code], [Tech_code], [Brandsegtech]));
GO