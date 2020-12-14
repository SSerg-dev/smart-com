ALTER TABLE [Product] DROP COLUMN [Brandsegtech]
GO

ALTER   FUNCTION [GetProductBrandsegtechByCode]
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
		@result = CONCAT(b.[Name], ' ', t.[Name]) 
	FROM [Brand] b, [Technology] t
	WHERE 
		b.[Brand_code] = @brandCode AND b.[Segmen_code] = @segmenCode 
		AND b.[Disabled] = 0 
		AND t.[Tech_code] = @technologyCode
		AND TRIM(ISNULL(t.[SubBrand_code], '')) = TRIM(ISNULL(@subBrandCode, '')) 
		AND t.[Disabled] = 0

	RETURN @result
END
GO

ALTER TABLE [Product]
    ADD [Brandsegtech] AS ([GetProductBrandsegtechByCode]([Brand_code],[Segmen_code],[Tech_code],[SubBrand_code]));
GO