DECLARE ProductTreeFilterCursor CURSOR FAST_FORWARD
	FOR
		SELECT [Id]
			  ,[Filter]
		FROM [ProductTree] pt;

DECLARE
	@PromoTreeId INT,
	@PromoTreeFilter NVARCHAR(MAX),
	@ColumnName NVARCHAR(MAX),
	@AndValue NVARCHAR(MAX),
	@OrValue NVARCHAR(MAX),
	@Tmp NVARCHAR(MAX);
	DECLARE	@JsonKeys TABLE(
		AndKey NVARCHAR(MAX),
		OrKey NVARCHAR(MAX)
	);
	DECLARE	@Result TABLE(
		Id INT,
		NotExpected NVARCHAR(MAX)
	);

OPEN ProductTreeFilterCursor;
WHILE 1 = 1
BEGIN
	FETCH NEXT 
		FROM ProductTreeFilterCursor 
		INTO 
			@PromoTreeId,
			@PromoTreeFilter;

	IF (SELECT FETCH_STATUS FROM SYS.DM_EXEC_CURSORS(0) WHERE NAME = 'ProductTreeFilterCursor') <> 0
		BREAK;
	IF @PromoTreeFilter IS NULL OR @PromoTreeFilter = ''
		CONTINUE;

	DECLARE rColumnsCursor CURSOR FAST_FORWARD
	FOR
	SELECT	
		jAnd.[key] COLLATE DATABASE_DEFAULT AS AndKey,
		jOr.[key] COLLATE DATABASE_DEFAULT AS OrKey
	FROM OPENJSON(@PromoTreeFilter) 
		WITH (
			[and] NVARCHAR(MAX) '$.and' AS JSON, 
			[or] NVARCHAR(MAX) '$.or' AS JSON
		)
		OUTER APPLY OPENJSON([and]) 
			WITH (
				[andValue] NVARCHAR(MAX) '$' AS JSON 
			) 
			OUTER APPLY OPENJSON([andValue]) jAnd
		OUTER APPLY OPENJSON([or]) 
			WITH (
				[orValue] NVARCHAR(MAX) '$' AS JSON 
			) 
			OUTER APPLY OPENJSON([orValue]) jOr;
	OPEN rColumnsCursor;
	WHILE 1 = 1
	BEGIN
		FETCH NEXT 
			FROM rColumnsCursor 
			INTO 
				@AndValue,
				@OrValue;

		IF (SELECT FETCH_STATUS FROM SYS.DM_EXEC_CURSORS(0) WHERE NAME = 'rColumnsCursor') <> 0
			BREAK;

			SET @Tmp = (SELECT 
				sc.[name] COLLATE DATABASE_DEFAULT
			FROM syscolumns sc
				JOIN sysobjects so ON sc.id = so.id
			WHERE so.Name = 'Product' AND sc.[name] = @AndValue);
			IF @Tmp IS NULL AND @AndValue IS NOT NULL
				INSERT INTO @Result VALUES(@PromoTreeId, @AndValue);

			SET @Tmp = (SELECT 
				sc.[name] COLLATE DATABASE_DEFAULT
			FROM syscolumns sc
				JOIN sysobjects so ON sc.id = so.id
			WHERE so.Name = 'Product' AND sc.[name] = @OrValue);
			IF @Tmp IS NULL AND @OrValue IS NOT NULL
				INSERT INTO @Result VALUES(@PromoTreeId, @OrValue);
 	END;
	CLOSE rColumnsCursor;
	DEALLOCATE rColumnsCursor;
	
END;
CLOSE ProductTreeFilterCursor;
DEALLOCATE ProductTreeFilterCursor;

SELECT * FROM @Result;