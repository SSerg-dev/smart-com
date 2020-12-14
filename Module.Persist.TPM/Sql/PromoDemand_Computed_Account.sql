----------------------------------------------------------------------------------------
-------------- SECTION: Функция вычисления BaseClientName Для PromoDemand --------------
----------------------------------------------------------------------------------------

CREATE FUNCTION [GetBaseClientName]
(
	@baseClientId int
)
RETURNS NVARCHAR(255) AS 
BEGIN
	Declare @result NVARCHAR(255)
	Select @result = [Name] From ClientTree
			Where ObjectId = @baseClientId 
				And (EndDate IS NULL OR EndDate > GETDATE())
				And StartDate < GETDATE()

	RETURN @result
END

----------------------------------------------------------------------------------------
----------------- SECTION: Преобразование столбца Account в вычисляемый ----------------
----------------------------------------------------------------------------------------

GO
ALTER TABLE [PromoDemand] DROP COLUMN [Account];

GO
ALTER TABLE [PromoDemand]
    ADD [Account] AS ([GetBaseClientName]([BaseClientObjectId]));
