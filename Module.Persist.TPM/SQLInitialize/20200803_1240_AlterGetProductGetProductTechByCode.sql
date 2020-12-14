ALTER TABLE [Product] DROP COLUMN [Technology];
GO

CREATE OR ALTER FUNCTION [GetProductTechByCode]
(
	@technologyCode NVARCHAR(20),
	@subBrandCode NVARCHAR(20)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)	
	Select @result = Technology.name From Technology
			Where ISNULL(Technology.Tech_code, '') = ISNULL(@technologyCode, '') 
					AND ISNULL(Technology.SubBrand_code, '') = ISNULL(@subBrandCode, '')
					AND Technology.Disabled = 0;
				
	RETURN @result
END
GO

ALTER TABLE [Product]
    ADD [Technology] AS ([GetProductTechByCode]([Tech_code], [SubBrand_code]));
GO