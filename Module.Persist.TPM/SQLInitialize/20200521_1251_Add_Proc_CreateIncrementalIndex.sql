SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE CreateIncrementalIndex
@IsCurrent BIT
AS
BEGIN
	IF @IsCurrent = 1
	BEGIN
		IF NOT EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_CurrentDayIncremental_References_NONCLUSTERED')
			CREATE NONCLUSTERED INDEX [IX_CurrentDayIncremental_References_NONCLUSTERED] 
			ON [CurrentDayIncremental]
			(
				[PromoId] ASC,
				[ProductId] ASC
			)
			WITH 
			(
				PAD_INDEX = OFF, 
				STATISTICS_NORECOMPUTE = OFF, 
				SORT_IN_TEMPDB = OFF, 
				DROP_EXISTING = OFF, 
				ONLINE = OFF, 
				ALLOW_ROW_LOCKS = ON, 
				ALLOW_PAGE_LOCKS = ON
			) 
			ON [PRIMARY]
	
		IF NOT EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_CurrentDayIncremental_NONCLUSTERED')
			CREATE NONCLUSTERED INDEX [IX_CurrentDayIncremental_NONCLUSTERED]
			ON [CurrentDayIncremental] ([Id] ASC) 
			INCLUDE (
				[WEEK],
				[PromoId],
				[ProductId],
				[IncrementalQty],
				[LastChangeDate],
				[DemandCode],
				[DMDGroup]
			);
	END;
	ELSE IF @IsCurrent = 0
	BEGIN
		IF NOT EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_PreviousDayIncremental_References_NONCLUSTERED')
			CREATE NONCLUSTERED INDEX [IX_PreviousDayIncremental_References_NONCLUSTERED] 
			ON [PreviousDayIncremental]
			(
				[PromoId] ASC,
				[ProductId] ASC
			)
			WITH 
			(
				PAD_INDEX = OFF, 
				STATISTICS_NORECOMPUTE = OFF, 
				SORT_IN_TEMPDB = OFF, 
				DROP_EXISTING = OFF, 
				ONLINE = OFF, 
				ALLOW_ROW_LOCKS = ON, 
				ALLOW_PAGE_LOCKS = ON
			) 
			ON [PRIMARY]

		IF NOT EXISTS (SELECT * FROM SYS.INDEXES WHERE NAME = N'IX_PreviousDayIncremental_NONCLUSTERED')
			CREATE NONCLUSTERED INDEX [IX_PreviousDayIncremental_NONCLUSTERED]
			ON [PreviousDayIncremental] ([Id] ASC) 
			INCLUDE (
				[WEEK],
				[PromoId],
				[ProductId],
				[IncrementalQty],
				[LastChangeDate],
				[DemandCode],
				[DMDGroup]
			);
	END;
END;