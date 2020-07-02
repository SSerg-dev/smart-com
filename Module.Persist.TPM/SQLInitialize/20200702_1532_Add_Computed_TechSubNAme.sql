ALTER TABLE [dbo].[BrandTech] DROP COLUMN [TechSubName];

GO
CREATE OR ALTER FUNCTION [dbo].[GetTechSubName]
(
	@technologyId uniqueidentifier
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	Select @result = CONCAT(Technology.Name, ' ', Technology.SubBrand) From Technology
			Where Technology.Id = @technologyId

	RETURN TRIM(@result)
END

GO
ALTER TABLE [dbo].[BrandTech]
    ADD [TechSubName] AS ([dbo].[GetTechSubName]([TechnologyId]));
GO