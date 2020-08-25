CREATE OR ALTER FUNCTION JsonToWhereExpression
(
	@Filter NVARCHAR(MAX),
	@Cond NVARCHAR(3),
	@ParentCond NVARCHAR(3)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE 
		@Expression NVARCHAR(MAX),
		@Key NVARCHAR(MAX),
		@Value NVARCHAR(MAX),
		@Type INT,
		@KeyC NVARCHAR(MAX),
		@ValueC NVARCHAR(MAX),
		@TypeC INT,
		
		@Skip INT = 0,
		@RowCount INT;

		SELECT
			@Key = [key] COLLATE DATABASE_DEFAULT,
			@Value = [value] COLLATE DATABASE_DEFAULT
		FROM OPENJSON(@Filter)
		
		IF @Key = 'and' OR @Key = 'or'
		BEGIN
			IF @ParentCond = '' OR @ParentCond IS NULL
				SET @ParentCond = @Key;
			SET @Expression = CONCAT(@Expression, [dbo].[JsonToWhereExpression](@Value, @Key, @ParentCond))
		END;
		ELSE 
		BEGIN
			SET @RowCount = (SELECT COUNT(*) FROM OPENJSON(@Filter));
			WHILE @Skip < @RowCount
			BEGIN
				SELECT
					@Key = [key] COLLATE DATABASE_DEFAULT,
					@Value = [value] COLLATE DATABASE_DEFAULT
				FROM OPENJSON(@Filter)
					WITH (
						[json] NVARCHAR(MAX) '$' AS JSON
					)
					OUTER APPLY OPENJSON([json])
				ORDER BY [json]
				OFFSET (@Skip) ROWS FETCH NEXT (1) ROWS ONLY;

				IF @Key = 'and' OR @Key = 'or'
				BEGIN
					IF @ParentCond = '' OR @ParentCond IS NULL
						SET @ParentCond = @Key;
					SET @Expression = CONCAT(@Expression, ' (', [dbo].[JsonToWhereExpression](@Value, @Key, @ParentCond), ') ', @ParentCond, ' ')
				END;
				ELSE 
				BEGIN
					IF @Key IS NOT NULL
						SET @Expression = CONCAT(@Expression, [dbo].[JsonToCondition](@Key, @Value), ' ', @Cond, ' ');
				END;
				SET @Skip = @Skip + 1;
			END;
		END;

	RETURN LEFT(@Expression, LEN(@Expression) - LEN(@Cond));
END;
