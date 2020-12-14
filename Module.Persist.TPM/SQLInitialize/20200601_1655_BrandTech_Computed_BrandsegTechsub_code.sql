ALTER TABLE [BrandTech] DROP COLUMN [BrandsegTechsub_code];

GO
CREATE OR ALTER FUNCTION [GetBrandsegTechsub_code]
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
					IF (SELECT COUNT(Technology.Id) FROM Technology WHERE Technology.Id = @technologyId AND SubBrand_code IS NOT NULL AND SubBrand_code != '') > 0
					BEGIN
						Select @result = CONCAT(@result, '-', Technology.SubBrand_code) From Technology
							Where Technology.Id = @technologyId
					END
		END
	ELSE
		BEGIN
			RETURN NULL
		END

	RETURN @result
END

GO
ALTER TABLE [BrandTech]
    ADD [BrandsegTechsub_code] AS ([GetBrandsegTechsub_code]([BrandId], [TechnologyId]));
GO