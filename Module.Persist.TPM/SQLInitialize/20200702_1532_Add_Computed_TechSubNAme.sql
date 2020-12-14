ALTER TABLE [BrandTech] DROP COLUMN [TechSubName];

GO
CREATE OR ALTER FUNCTION [GetTechSubName]
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
ALTER TABLE [BrandTech]
    ADD [TechSubName] AS ([GetTechSubName]([TechnologyId]));
GO