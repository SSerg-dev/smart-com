SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE FillPreviousDayIncremental
AS
BEGIN		
	EXEC DropIncrementalIndex 0;						
	TRUNCATE TABLE [dbo].[PreviousDayIncremental];
	
	INSERT INTO [dbo].[PreviousDayIncremental]
		(DemandCode, DMDGroup, Id, IncrementalQty, LastChangeDate, ProductId, PromoId, Week) 
		SELECT DemandCode, DMDGroup, Id, IncrementalQty, LastChangeDate, ProductId, PromoId, Week FROM [dbo].[CurrentDayIncremental]; 
								
	EXEC CreateIncrementalIndex 0;
END;