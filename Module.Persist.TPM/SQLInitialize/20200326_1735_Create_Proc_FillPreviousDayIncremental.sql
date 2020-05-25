CREATE OR ALTER PROCEDURE FillPreviousDayIncremental
AS
BEGIN
	IF EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_PreviousDayIncremental_NONCLUSTERED')
		DROP INDEX IX_PreviousDayIncremental_NONCLUSTERED ON [dbo].[PreviousDayIncremental];
							
	TRUNCATE TABLE [dbo].[PreviousDayIncremental];
	INSERT INTO [dbo].[PreviousDayIncremental]
		(DemandCode, DMDGroup, Id, IncrementalQty, LastChangeDate, ProductId, PromoId, Week) 
		SELECT DemandCode, DMDGroup, Id, IncrementalQty, LastChangeDate, ProductId, PromoId, Week FROM [dbo].[CurrentDayIncremental]; 
								
	IF NOT EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_PreviousDayIncremental_NONCLUSTERED')
		CREATE NONCLUSTERED INDEX IX_PreviousDayIncremental_NONCLUSTERED
		ON [dbo].[PreviousDayIncremental] (Id ASC) 
		INCLUDE (
			WEEK,
			PromoId,
			ProductId,
			IncrementalQty,
			LastChangeDate,
			DemandCode,
			DMDGroup
		);
END;