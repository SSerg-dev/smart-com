ALTER TABLE [dbo].[BrandTech] DROP COLUMN [BrandsegTechsub];

GO
CREATE OR ALTER FUNCTION [dbo].[GetBrandsegTechsubName]
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
ALTER TABLE [dbo].[BrandTech]
    ADD [BrandsegTechsub] AS ([dbo].[GetBrandsegTechsubName]([BrandId], [TechnologyId]));
GO