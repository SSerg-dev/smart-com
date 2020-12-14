ALTER TABLE [ProductTree] DROP COLUMN [FilterQuery]
GO

CREATE OR ALTER FUNCTION JsonFilterToSql
(
	@Filter NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	IF @Filter IS NULL OR @Filter = ''
		RETURN 'SELECT * FROM [Product]';

	RETURN 'SELECT * FROM [Product] WHERE ' + [JsonToWhereExpression](@Filter, '', '');
END;
GO

ALTER TABLE [ProductTree]
    ADD [FilterQuery] AS ([JsonFilterToSql]([Filter]));
GO