ALTER TABLE [dbo].[Product] DROP COLUMN [Technology];

GO
CREATE OR ALTER FUNCTION [dbo].[GetProductTechByCode]
(
	@technologyCode NVARCHAR(3),
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
ALTER TABLE [dbo].[Product]
    ADD [Technology] AS ([dbo].[GetProductTechByCode]([Tech_code], [SubBrand_code]));
GO
