CREATE OR ALTER FUNCTION GetDemandCode 
(
	@ParentId INT, 
	@DemandCode NVARCHAR(MAX)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE
		@ObjectId INT,
		@Type NVARCHAR(MAX);
		
	WHILE (@DemandCode IS NULL OR @DemandCode = '') AND (@Type IS NULL OR @Type <> 'root')
	BEGIN		
		SELECT TOP(1)
			@ObjectId = ct.[ObjectId],
			@ParentId = ct.[parentId],
			@Type = ct.[Type]
		FROM [dbo].[ClientTree] ct
		WHERE ct.[ObjectId] = @ParentId AND ct.[EndDate] IS NULL;
		
		SELECT TOP(1)
			@DemandCode = ct.[DemandCode]
		FROM [dbo].[ClientTree] ct
		WHERE ct.[ObjectId] = @ObjectId AND ct.[EndDate] IS NULL;
	END;

	RETURN @DemandCode;
END;