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
	IF (SELECT COUNT(Brand.Id) FROM Brand WHERE Brand.Brand_code = @brandCode AND Brand.Brand_code IS NOT NULL AND Brand.Segmen_code = @segmenCode AND Brand.Segmen_code IS NOT NULL AND Brand.Disabled = 0) > 0
		AND (SELECT COUNT(Technology.Id) FROM Technology WHERE Technology.Tech_code = @technologyCode AND Technology.Tech_code IS NOT NULL AND Technology.Disabled = 0) > 0
		BEGIN
			Select @result = Technology.SubBrand From Brand, Technology
				Where Technology.Tech_code = @technologyCode AND Technology.SubBrand_code = @subBrandCode
					AND Technology.Disabled = 0
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