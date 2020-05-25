SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE DropIncrementalIndex
@IsCurrent BIT
AS
BEGIN
	IF @IsCurrent = 1
	BEGIN
		IF EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_CurrentDayIncremental_References_NONCLUSTERED')
			DROP INDEX [IX_CurrentDayIncremental_References_NONCLUSTERED] ON [dbo].[CurrentDayIncremental];
	
		IF EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_CurrentDayIncremental_NONCLUSTERED')			
			DROP INDEX [IX_CurrentDayIncremental_NONCLUSTERED] ON [dbo].[CurrentDayIncremental];
	END;
	ELSE IF @IsCurrent = 0
	BEGIN
		IF EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_PreviousDayIncremental_References_NONCLUSTERED')
			DROP INDEX [IX_PreviousDayIncremental_References_NONCLUSTERED] ON [dbo].[PreviousDayIncremental];
			
		IF EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_PreviousDayIncremental_NONCLUSTERED')
			DROP INDEX IX_PreviousDayIncremental_NONCLUSTERED ON [dbo].[PreviousDayIncremental];
	END;
END;