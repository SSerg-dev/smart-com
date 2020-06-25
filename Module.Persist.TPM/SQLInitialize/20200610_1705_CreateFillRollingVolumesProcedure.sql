CREATE OR ALTER PROCEDURE [dbo].[FillRollingVolumes] AS
  BEGIN
	   TRUNCATE TABLE [dbo].[RollingVolume];

	   DECLARE @today DATE = GETDATE();
	   DECLARE @currentWeekName NVARCHAR(255);
	   DECLARE @currentWeekStartDate DATE;
	   SELECT @currentWeekName = MarsWeekFullName FROM Dates WHERE OriginalDate = @today;
	   SELECT @currentWeekStartDate = MIN(OriginalDate) FROM Dates WHERE MarsWeekFullName = @currentWeekName;

    INSERT INTO [dbo].[RollingVolume]
           ([Id]
           ,[DemandGroup]
           ,[Week]
           ,[PlanProductIncrementalQTY]
           ,[Actuals]
           ,[OpenOrders]
           ,[Baseline]
           ,[ActualIncremental]
           ,[PreviousRollingVolumes]
           ,[Lock]
           ,[ProductId]
           ,[DMDGroup]
           ,[PromoDifference]
           ,[ActualOO]
           ,[PreliminaryRollingVolumes]
           ,[RollingVolumesTotal]
           ,[ManualRollingTotalVolumes])
		SELECT 
			NEWID(), 
			CONCAT(rvf.DMDGROUP, '_05_0125'),
			CAST(d.MarsYear AS NVARCHAR(4)) + 'P' + REPLACE(STR(d.MarsPeriod, 2), SPACE(1), '0') + d.MarsWeekName,
			rvf.PlanProductIncrementalQty, 
			rvf.ActualsQty, 
			rvf.OpenOrdersQty, 
			rvf.BaselineQty, 
			rvf.ActualIncrementalQty,
			rvf.PreviousRollingVolumesQty, 
			0, 
			p.Id, 
			ct.DMDGroup, 
			rvf.PromoDifferenceQty,
			rvf.ActualOpenOrdersQty,
			rvf.PreliminaryRollingVolumesQty,
			rvf.RollingVolumesQty,
			rvf.RollingVolumesQty

		FROM [dbo].[ROLLING_VOLUMES_FDM] AS rvf
		JOIN [dbo].[Product] AS p ON p.ZREP = rvf.ZREP
		JOIN [dbo].[Dates] AS d ON d.OriginalDate = rvf.WeekStartDate
		JOIN [dbo].[ClientTree] AS ct ON (ct.DemandCode = CONCAT(rvf.DMDGROUP, '_05_0125')
											AND ct.EndDate IS NULL)
		WHERE rvf.WeekStartDate = @currentWeekStartDate
   END




