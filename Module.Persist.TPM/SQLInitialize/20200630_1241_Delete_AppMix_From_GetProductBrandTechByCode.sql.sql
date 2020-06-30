ALTER TABLE [dbo].[Product] DROP COLUMN [BrandTech];
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

ALTER TABLE [dbo].[Product]
    ADD [BrandTech] AS ([dbo].[GetProductBrandTechByCode]([Brand_code], [Segmen_code], [Tech_code]));
GO