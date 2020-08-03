ALTER TABLE [dbo].[Product] DROP COLUMN [BrandsegTechSub_code]
GO

ALTER   FUNCTION [dbo].[GetProductBrandsegTechsubCode]
(
	@brandCode NVARCHAR(20),
	@segmenCode NVARCHAR(20),
	@technologyCode NVARCHAR(20),
	@subBrandCode NVARCHAR(20)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	IF (SELECT COUNT(Brand.Id) FROM Brand WHERE Brand.Brand_code = @brandCode AND Brand.Brand_code IS NOT NULL AND Brand.Segmen_code = @segmenCode AND Brand.Segmen_code IS NOT NULL AND Brand.Disabled = 0) > 0
		AND (SELECT COUNT(Technology.Id) FROM Technology WHERE Technology.Tech_code = @technologyCode AND Technology.Tech_code IS NOT NULL AND Technology.Disabled = 0) > 0
		BEGIN
			Select @result = CONCAT(Brand.Brand_code, '-', Brand.Segmen_code, '-', Technology.Tech_code) From Brand, Technology
				Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Technology.Tech_code = @technologyCode AND Technology.Disabled = 0 AND Brand.Disabled = 0
					IF (@subBrandCode IS NOT NULL AND @subBrandCode != '')
					BEGIN
						Select @result = CONCAT(@result, '-', @subBrandCode)
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
    ADD [BrandsegTechSub_code] AS ([dbo].[GetProductBrandsegTechsubCode]([Brand_code],[Segmen_code],[Tech_code],[SubBrand_code]));
GO