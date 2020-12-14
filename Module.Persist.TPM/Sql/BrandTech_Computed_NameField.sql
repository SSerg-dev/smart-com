----------------------------------------------------------------------------------------
-------------------- SECTION: Функция вычисления Name Для BrandTech --------------------
----------------------------------------------------------------------------------------

CREATE OR ALTER FUNCTION [GetBrandTechName]
(
	@brandId uniqueidentifier,
	@technologyId uniqueidentifier
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	Select @result = CONCAT(Brand.Name, ' ', Technology.Name) From Brand, Technology
			Where Brand.Id = @brandId And Technology.Id = @technologyId

	RETURN @result
END

----------------------------------------------------------------------------------------
------------------ SECTION: Преобразование столбца Name в вычисляемый ------------------
----------------------------------------------------------------------------------------

GO
ALTER TABLE [BrandTech] DROP COLUMN [Name];

GO
ALTER TABLE [BrandTech]
    ADD [Name] AS ([GetBrandTechName]([BrandId], [TechnologyId]));
