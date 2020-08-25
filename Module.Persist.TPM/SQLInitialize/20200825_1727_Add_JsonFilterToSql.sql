ALTER TABLE [dbo].[ProductTree] DROP COLUMN [FilterQuery]
GO

CREATE OR ALTER FUNCTION JsonFilterToSql
(
	@Filter NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	IF @Filter IS NULL OR @Filter = ''
		RETURN 'SELECT * FROM [dbo].[Product]';

	RETURN 'SELECT * FROM [dbo].[Product] WHERE ' + [dbo].[JsonToWhereExpression](@Filter, '', '');
END;
GO

ALTER TABLE [dbo].[ProductTree]
    ADD [FilterQuery] AS ([dbo].[JsonFilterToSql]([Filter]));
GO