DECLARE 
    @SchemaFrom NVARCHAR(MAX) = 'dbo',
    @SchemaTo NVARCHAR(MAX) = 'Jupiter';

IF NOT EXISTS 
    ( 
        SELECT  
            *
        FROM sys.schemas
        WHERE   name = @SchemaTo 
    )
    EXEC('CREATE SCHEMA [' + @SchemaTo + ']');

CREATE TABLE #JupiterObjects (
    [Name] NVARCHAR(MAX),
    [Type] NVARCHAR(MAX)
);

INSERT INTO #JupiterObjects([Name], [Type])
(
    SELECT  object_name(o.object_id), o.Type
    FROM sys.Objects o
    INNER JOIN sys.Schemas s on o.schema_id = s.schema_id
    WHERE s.Name = @SchemaFrom 
    And (o.Type = 'U' Or o.Type = 'P' Or o.Type = 'V' Or o.Type = 'FN')
)

CREATE TABLE #ComputedFieldsUpdate (
    DropSQL NVARCHAR(MAX),
    AddSQL NVARCHAR(MAX)
);
INSERT INTO #ComputedFieldsUpdate(DropSQL, AddSQL)
(
    SELECT 
        'ALTER TABLE [' + @SchemaFrom + '].[' + object_name(c.object_id) + '] DROP COLUMN [' + c.name + '];',
        'ALTER TABLE [' + @SchemaTo + '].[' + object_name(c.object_id) + '] ADD [' + c.name + '] AS ' + REPLACE(definition, @SchemaFrom, @SchemaTo) + ';'
    FROM sys.computed_columns c
    JOIN sys.objects o on o.object_id = c.object_id
    JOIN sys.Schemas s on o.schema_id = s.schema_id
    WHERE s.Name = @SchemaFrom
        AND object_name(c.object_id) IN (SELECT [Name] FROM #JupiterObjects)
);

DECLARE 
    @DropColumnSQL NVARCHAR(MAX),
    @AddColumnSQL NVARCHAR(MAX),
    @ObjectName NVARCHAR(MAX),
    @ObjectType NVARCHAR(MAX),
    @Query NVARCHAR(MAX);
DECLARE ComputedFieldsCursor CURSOR FAST_FORWARD
    FOR SELECT * FROM #ComputedFieldsUpdate;
DECLARE ObjectsCursor CURSOR FAST_FORWARD
    FOR SELECT * FROM #JupiterObjects;

-- drop all computed fields
OPEN ComputedFieldsCursor;
FETCH NEXT FROM ComputedFieldsCursor INTO @DropColumnSQL, @AddColumnSQL;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXECUTE sp_executesql @DropColumnSQL
    FETCH NEXT FROM ComputedFieldsCursor INTO @DropColumnSQL, @AddColumnSQL;
END;
CLOSE ComputedFieldsCursor;

-- transfer schema
DECLARE 
    @ObjectTypeName NVARCHAR(MAX),
    @DropQuery NVARCHAR(MAX);
CREATE TABLE #SqlLines (
    [Line] NVARCHAR(MAX)
);

OPEN ObjectsCursor;
FETCH NEXT FROM ObjectsCursor INTO  @ObjectName, @ObjectType;
WHILE @@FETCH_STATUS = 0
BEGIN
    IF  EXISTS 
        (
            SELECT  
                * 
            FROM sys.Objects o
            INNER JOIN sys.Schemas s ON o.schema_id = s.schema_id
            WHERE s.Name = @SchemaFrom
                AND object_name(o.object_id) = @ObjectName
        )
        AND NOT EXISTS
        (
            SELECT  
                * 
            FROM sys.Objects o
            INNER JOIN sys.Schemas s ON o.schema_id = s.schema_id
            WHERE s.Name = @SchemaTo
                AND object_name(o.object_id) = @ObjectName
        )
    BEGIN
        SET @Query = 'ALTER SCHEMA ' + @SchemaTo + ' TRANSFER OBJECT::' + @SchemaFrom + '.[' + @ObjectName + ']';        
        EXECUTE sp_executesql @Query
    END;
    IF  EXISTS 
        (
            SELECT  
                * 
            FROM sys.Objects o
            INNER JOIN sys.Schemas s ON o.schema_id = s.schema_id
            WHERE s.Name = @SchemaFrom
                AND object_name(o.object_id) = @ObjectName
                AND @ObjectName = '__MigrationHistory'
        ) 
        AND EXISTS 
        (
            SELECT  
                * 
            FROM sys.Objects o
            INNER JOIN sys.Schemas s ON o.schema_id = s.schema_id
            WHERE s.Name = @SchemaTo
                AND object_name(o.object_id) = @ObjectName
                AND @ObjectName = '__MigrationHistory'
        ) 
        BEGIN
            SET @Query = 
            'INSERT INTO [' + @SchemaTo + '].[__MigrationHistory] 
                SELECT * FROM [' + @SchemaFrom + '].[__MigrationHistory]
            DROP TABLE [' + @SchemaFrom + '].[__MigrationHistory];';

            EXECUTE sp_executesql @Query;
        END;
    FETCH NEXT FROM ObjectsCursor INTO  @ObjectName, @ObjectType;
END;
CLOSE ObjectsCursor;
DEALLOCATE ObjectsCursor;
DROP TABLE #SqlLines

-- restore computed fields
OPEN ComputedFieldsCursor;
FETCH NEXT FROM ComputedFieldsCursor INTO  @DropColumnSQL, @AddColumnSQL;
WHILE @@FETCH_STATUS = 0
BEGIN
    EXECUTE sp_executesql @AddColumnSQL
    FETCH NEXT FROM ComputedFieldsCursor INTO @DropColumnSQL, @AddColumnSQL;
END;
CLOSE ComputedFieldsCursor;
DEALLOCATE ComputedFieldsCursor;

-- clear tmp tabels
DROP TABLE #JupiterObjects;
DROP TABLE #ComputedFieldsUpdate;
GO