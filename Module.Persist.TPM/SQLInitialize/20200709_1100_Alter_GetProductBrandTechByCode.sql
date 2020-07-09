﻿ALTER TABLE [dbo].[Product] DROP COLUMN [BrandTech];
GO

CREATE OR ALTER FUNCTION [dbo].[GetProductBrandTechByCode]
(
	@brandCode NVARCHAR(3),
	@segmenCode NVARCHAR(2),
	@technologyCode NVARCHAR(3),
	@subBrandCode NVARCHAR(3)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	DECLARE @result NVARCHAR(255);
	
	SELECT 
		@result = CONCAT(b.[name], ' ', t.[name]) 
	FROM [dbo].[Brand] b, [dbo].[Technology] t
	WHERE 
		b.[Brand_code] = @brandCode AND b.[Segmen_code] = @segmenCode AND b.[Disabled] = 0 
		AND t.[Tech_code] = @technologyCode
		AND TRIM(ISNULL(t.[SubBrand_code], '')) = TRIM(ISNULL(@subBrandCode, '')) 
		AND t.[Disabled] = 0;

	RETURN @result;
END
GO

ALTER TABLE [dbo].[Product]
    ADD [BrandTech] AS ([dbo].[GetProductBrandTechByCode]([Brand_code], [Segmen_code], [Tech_code], [SubBrand_code]));
GO