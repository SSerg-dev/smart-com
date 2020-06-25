ALTER TABLE [dbo].[Product] DROP COLUMN [BrandsegTechSub];
GO

CREATE OR ALTER  FUNCTION [dbo].[GetProductBrandsegTechSubByCode]
(
	@brandCode NVARCHAR(3),
	@segmenCode NVARCHAR(2),
	@technologyCode NVARCHAR(3),
	@subBrandCode NVARCHAR(3)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	IF (SELECT COUNT(Brand.Id) FROM Brand WHERE Brand.Brand_code = @brandCode AND Brand.Brand_code IS NOT NULL AND Brand.Segmen_code = @segmenCode AND Brand.Segmen_code IS NOT NULL) > 0 
		AND (SELECT COUNT(Technology.Id) FROM Technology WHERE Technology.Tech_code = @technologyCode AND Technology.Tech_code IS NOT NULL) > 0
		BEGIN
			IF (@subBrandCode IS NULL OR @subBrandCode = '') BEGIN
				Select @result = CONCAT(Brand.Name, ' ', Technology.Name) From Brand, Technology
					Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Technology.Tech_code = @technologyCode AND (Technology.SubBrand_code IS NULL OR Technology.SubBrand_code = '')
						AND Brand.Disabled = 0 AND Technology.Disabled = 0
			END
			ELSE BEGIN
				Select @result = CONCAT(Brand.Name, ' ', Technology.Name, ' ', Technology.SubBrand) From Brand, Technology
					Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Technology.Tech_code = @technologyCode AND Technology.SubBrand_code = @subBrandCode
						AND Brand.Disabled = 0 AND Technology.Disabled = 0
			END
		END
	ELSE
		BEGIN
			RETURN NULL
		END

	RETURN @result
END
GO

ALTER TABLE [dbo].[Product]
    ADD [BrandsegTechSub] AS ([dbo].[GetProductBrandsegTechSubByCode]([Brand_code], [Segmen_code], [Tech_code], [SubBrand_code]));
GO