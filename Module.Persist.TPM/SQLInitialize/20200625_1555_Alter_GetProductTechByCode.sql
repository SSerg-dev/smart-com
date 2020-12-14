ALTER TABLE [Product] DROP COLUMN [Technology];

GO
CREATE OR ALTER   FUNCTION [GetProductTechByCode]
(
	@technologyCode NVARCHAR(3),
	@brandsegtech NVARCHAR(255),
	@subBrandCode NVARCHAR(3)
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
    ADD [Technology] AS ([GetProductTechByCode]([Tech_code], [Brandsegtech], [SubBrand_code]));
GO