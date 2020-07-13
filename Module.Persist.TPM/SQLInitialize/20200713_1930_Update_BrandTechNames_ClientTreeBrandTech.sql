DECLARE [BrandTechCursor] CURSOR FAST_FORWARD
	FOR SELECT bt.[Id], bt.[BrandsegTechsub] FROM [dbo].[BrandTech] bt;

DECLARE 
	@BTID UNIQUEIDENTIFIER,
	@BTName NVARCHAR(MAX);

OPEN [BrandTechCursor];
WHILE 1 = 1
BEGIN
	FETCH NEXT
		FROM [BrandTechCursor]
		INTO 		
			@BTID,
			@BTName;
			
	IF (SELECT FETCH_STATUS FROM SYS.DM_EXEC_CURSORS(0) WHERE NAME = 'BrandTechCursor') <> 0
		BREAK;

	UPDATE [dbo].[ClientTreeBrandTech] SET [CurrentBrandTechName] = @BTName WHERE [BrandTechId] = @BTID;
END;

CLOSE [BrandTechCursor];
DEALLOCATE [BrandTechCursor];