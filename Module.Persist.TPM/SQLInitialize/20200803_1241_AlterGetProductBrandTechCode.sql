ALTER TABLE [Product] DROP COLUMN [BrandTech_code]
GO

ALTER   FUNCTION [GetProductBrandTechCode]
(
	@brandCode NVARCHAR(20),
	@segmenCode NVARCHAR(20),
	@technologyCode NVARCHAR(20)
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

ALTER TABLE [Product]
    ADD [BrandTech_code] AS ([GetProductBrandTechCode]([Brand_code],[Segmen_code],[Tech_code]));
GO