CREATE OR ALTER FUNCTION JsonToCondition
(
	@PKey NVARCHAR(MAX),
	@Filter NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE 
		@Condition NVARCHAR(MAX),
		@SValue NVARCHAR(MAX),
		@Value NVARCHAR(MAX),
		@Key NVARCHAR(MAX),
		@Type INT,
		
		@Pref NVARCHAR(MAX) = '[dbo].[Product].[';
		
		IF @PKey IN (
			SELECT 
				sc.[name] COLLATE DATABASE_DEFAULT
			FROM syscolumns sc
				JOIN sysobjects so ON sc.id = so.id
			WHERE so.Name = 'Product'
		)
		BEGIN
			SELECT
				@Key = [key] COLLATE DATABASE_DEFAULT,
				@Value = [value] COLLATE DATABASE_DEFAULT,
				@Type = [type]
			FROM OPENJSON(@Filter);
			IF @Type = 1
				SET @SValue = '''' + @Value + '''';
			ELSE 
				SET @SValue = @Value;
			IF @Key = 'eq'
				SET @Condition = @Pref + @PKey + '] = ' + @SValue;
			ELSE IF @Key = 'ne'
				SET @Condition = @Pref + @PKey + '] <> ' + @SValue;
			ELSE IF @Key = 'isnull'
			BEGIN
				SET @Condition = @Pref + @PKey + '] IS NULL';
				IF @Type = 1
					SET @Condition = '(' + @Condition + ' OR ' + @Pref + @PKey + '] = '''')';
			END;
			ELSE IF @Key = 'notnull'
			BEGIN
				SET @Condition = @Pref + @PKey + '] IS NOT NULL';
				IF @Type = 1
					SET @Condition = '(' + @Condition + ' AND ' + @Pref + @PKey + '] <> '''')';
			END;
			ELSE IF @Key = 'contains'
				SET @Condition = @Pref + @PKey + '] LIKE ''%' + @Value + '%''';
			ELSE IF @Key = 'notcontains'
				SET @Condition = @Pref + @PKey + '] NOT LIKE ''%' + @Value + '%''';
			ELSE IF @Key = 'endswith'
				SET @Condition = @Pref + @PKey + '] LIKE ''%' + @Value + '''';
			ELSE IF @Key = 'ge'
				SET @Condition = @Pref + @PKey + '] >= ' + @SValue;
			ELSE IF @Key = ''
				SET @Condition = @Pref + @PKey + '] <= ' + @SValue;
			ELSE IF @Key = 'lt'
				SET @Condition = @Pref + @PKey + '] < ' + @SValue;
			ELSE IF @Key = 'gt'
				SET @Condition = @Pref + @PKey + '] > ' + @SValue;
			ELSE IF @Key = 'startswith'
				SET @Condition = @Pref + @PKey + '] LIKE ''' + @Value + '%''';
			ELSE IF @Key = 'in'
			BEGIN
				DECLARE
					@In NVARCHAR(MAX),
					@ValueI NVARCHAR(MAX),
					@SValueI NVARCHAR(MAX),
					@KeyI NVARCHAR(MAX),
					@TypeI INT,
					@Skip INT = 0,
					@RowCount INT;
				
				SET @RowCount = (SELECT COUNT(*) FROM OPENJSON(@Value));
				WHILE @Skip <= @RowCount
				BEGIN
					SELECT
						@KeyI = [key] COLLATE DATABASE_DEFAULT,
						@ValueI = [value] COLLATE DATABASE_DEFAULT,
						@TypeI = [type]
					FROM OPENJSON(@Value)
					WITH (
						[json] NVARCHAR(MAX) '$.values' AS JSON
					)
					OUTER APPLY OPENJSON([json]) 
					ORDER BY [json]
					OFFSET (@Skip) ROWS FETCH NEXT (1) ROWS ONLY;

					IF @TypeI = 1
						SET @SValueI = '''' + @ValueI + '''';
					ELSE 
						SET @SValueI = @ValueI;

					SET @In = CONCAT(@In, @SValueI, ', ');

					SET @Skip = @Skip + 1;
				END;
				SET @Condition = @Pref + @PKey + '] IN (' + LEFT(@In, LEN(@In) - 1) + ')';
			END;
		END;

	RETURN @Condition;
END;