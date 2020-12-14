ALTER TABLE [Product] DROP COLUMN [Brand]
GO

CREATE OR ALTER FUNCTION [GetProductBrandByCode]
(
	@brandCode NVARCHAR(20),
	@segmenCode NVARCHAR(20)
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	Select @result = Brand.name From Brand
				Where Brand.Brand_code = @brandCode AND Brand.Segmen_code = @segmenCode AND Brand.Disabled = 0
				
	RETURN @result
END
GO

ALTER TABLE [Product]
    ADD [Brand] AS ([GetProductBrandByCode]([Brand_code],[Segmen_code]));
GO