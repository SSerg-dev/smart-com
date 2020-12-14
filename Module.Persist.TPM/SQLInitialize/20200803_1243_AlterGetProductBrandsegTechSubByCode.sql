ALTER TABLE [Product] DROP COLUMN [BrandsegTechSub];
GO

CREATE OR ALTER FUNCTION [GetProductBrandsegTechSubByCode]
(
	@brandCode NVARCHAR(20),
	@segmenCode NVARCHAR(20),
	@technologyCode NVARCHAR(20),
	@subBrandCode NVARCHAR(20)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	DECLARE @result NVARCHAR(255)

	SELECT 
		@result = CONCAT(b.[Name], ' ', t.[Name], ' ', ISNULL(t.[SubBrand], '')) 
	FROM [Brand] b, [Technology] t
	WHERE 
		b.[Brand_code] = @brandCode AND b.[Segmen_code] = @segmenCode 
		AND b.[Disabled] = 0 AND t.[Disabled] = 0
		AND t.[Tech_code] = @technologyCode
		AND TRIM(ISNULL(t.[SubBrand_code], '')) = TRIM(ISNULL(@subBrandCode, '')) 

	RETURN TRIM(@result)
END
GO

ALTER TABLE [Product]
    ADD [BrandsegTechSub] AS ([GetProductBrandsegTechSubByCode]([Brand_code],[Segmen_code],[Tech_code],[SubBrand_code]));
GO