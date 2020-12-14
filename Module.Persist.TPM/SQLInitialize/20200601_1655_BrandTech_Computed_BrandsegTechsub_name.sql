ALTER TABLE [BrandTech] DROP COLUMN [BrandsegTechsub];

GO
CREATE OR ALTER FUNCTION [GetBrandsegTechsubName]
(
	@brandId uniqueidentifier,
	@technologyId uniqueidentifier
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	Select @result = CONCAT(Brand.Name, ' ', Technology.Name, ' ', Technology.SubBrand) From Brand, Technology
			Where Brand.Id = @brandId And Technology.Id = @technologyId

	RETURN @result
END

GO
ALTER TABLE [BrandTech]
    ADD [BrandsegTechsub] AS ([GetBrandsegTechsubName]([BrandId], [TechnologyId]));
GO