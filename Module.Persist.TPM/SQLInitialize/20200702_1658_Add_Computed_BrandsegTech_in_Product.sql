ALTER TABLE [dbo].[Product] DROP COLUMN [Brandsegtech];

GO
CREATE OR ALTER FUNCTION [dbo].[GetProductBrandsegtechByCode]
(
	@brandCode NVARCHAR(3),
	@segmenCode NVARCHAR(2),
	@technologyCode NVARCHAR(3),
	@subBrandCode NVARCHAR(3)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	IF (@subBrandCode IS NULL OR @subBrandCode = '') BEGIN
		Select @result = CONCAT(Brand.Name, ' ', Technology.Name) From Brand, Technology
			Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Brand.Disabled = 0 AND Technology.Tech_code = @technologyCode
			AND Technology.Disabled = 0
	END
	ELSE BEGIN
		Select @result = CONCAT(Brand.Name, ' ', Technology.Name) From Brand, Technology
			Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Brand.Disabled = 0 AND Technology.Tech_code = @technologyCode
			AND Technology.SubBrand_code = @subBrandCode AND Technology.Disabled = 0
	END

	RETURN @result
END

GO
ALTER TABLE [dbo].[Product]
    ADD [Brandsegtech] AS ([dbo].[GetProductBrandsegtechByCode]([Brand_code], [Segmen_code], [Tech_code], [SubBrand_code]));
GO
