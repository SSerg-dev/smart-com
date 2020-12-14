namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncrementalReport_View_Fixes : DbMigration
    {
        public override void Up()
        {
            Sql(@"ALTER VIEW [PlanIncrementalReport]
AS
  SELECT NEWID()                                    AS Id,
         CONCAT(joined.ZREP, '_0125')               AS ZREP,
         joined.DemandCode                          AS DemandCode,
         p.InOut                                    AS InOut,
         FORMATMESSAGE('%s#%i', p.[Name], p.Number) AS PromoNameId,
         'RU_0125'                                  AS LocApollo,
         '7'                                        AS TypeApollo,
         'SHIP_LEWAND_CS'                           AS ModelApollo,

         CONVERT(DATETIMEOFFSET(7), IIF((SELECT COUNT(*) FROM Dates WHERE OriginalDate = joined.WeekStartDate AND MarsDay = 1) > 0, joined.WeekStartDate, (SELECT TOP(1) OriginalDate FROM Dates WHERE OriginalDate < joined.WeekStartDate AND OriginalDate > DATEADD(DAY, -7, joined.WeekStartDate) AND MarsDay = 1))) 
		 AS WeekStartDate,

         IIF(joined.StartDay <> 1, (joined.PlanProductIncrementalCaseQty / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1)) * (8 - joined.StartDay), 
								   IIF(joined.EndDay = 7, joined.PlanProductIncrementalCaseQty / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1) * joined.EndDay, 
														  joined.PlanProductIncrementalCaseQty / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1) * (joined.EndDay + 1)))
		 AS PlanProductIncrementalCaseQty,
         IIF(joined.StartDay <> 1, (joined.PlanProductBaselineCaseQty / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1)) * (8 -joined.StartDay), 
								   IIF(joined.EndDay = 7, joined.PlanProductBaselineCaseQty / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1) * joined.EndDay, 
														  joined.PlanProductBaselineCaseQty / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1) * (joined.EndDay + 1))) 
		 AS PlanProductBaselineCaseQty,
		 IIF(joined.StartDay <> 1, (joined.PlanProductIncrementalLSV / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1)) * (8 - joined.StartDay), 
								   IIF(joined.EndDay = 7, joined.PlanProductIncrementalLSV / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1) * joined.EndDay, 
														  joined.PlanProductIncrementalLSV / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1) * (joined.EndDay + 1)))
		 AS PlanProductIncrementalLSV,
		 IIF(joined.StartDay <> 1, (joined.PlanProductBaselineLSV / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1)) * (8 - joined.StartDay), 
								   IIF(joined.EndDay = 7, joined.PlanProductBaselineLSV / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1) * joined.EndDay, 
														  joined.PlanProductBaselineLSV / (DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd) + 1) * (joined.EndDay + 1)))                 
		 AS PlanProductBaselineLSV,

         p.PlanPromoUpliftPercent					AS PlanUplift,
         p.DispatchesStart							AS DispatchesStart,
         p.DispatchesEnd							AS DispatchesEnd,
         p.IsGrowthAcceleration						AS IsGrowthAcceleration,
         ''											AS [Week],
         ps.[Name]									AS [Status]

  FROM  (SELECT pp.PromoId								AS PromoId,
                pp.ZREP									AS ZREP,
                pp.PlanProductIncrementalCaseQty		AS PlanProductIncrementalCaseQty,
                dc.DemandCode							AS DemandCode,
                Min(dt.OriginalDate)					AS WeekStartDate,
                Min(dt.MarsDay)							AS StartDay,
                Max(dt.MarsDay)							AS EndDay,
                pp.PlanProductBaselineCaseQty			AS PlanProductBaselineCaseQty,
                pp.PlanProductIncrementalLSV			AS PlanProductIncrementalLSV,
                pp.PlanProductBaselineLSV				AS PlanProductBaselineLSV
         FROM   PromoProduct pp
                LEFT JOIN Promo p
                       ON pp.PromoId = p.Id
                LEFT JOIN DemandCodeView dc
                       ON p.ClientTreeId = dc.ObjectId
                INNER JOIN Dates dt
                        ON dt.OriginalDate >= p.DispatchesStart
                           AND dt.OriginalDate <= p.DispatchesEnd
         WHERE  pp.[Disabled] = 0
                AND pp.PlanProductIncrementalCaseQty > 0
                AND p.[Disabled] = 0
         GROUP  BY pp.PromoId,
                   pp.ZREP,
                   pp.PlanProductIncrementalCaseQty,
                   pp.PlanProductBaselineCaseQty,
                   pp.PlanProductIncrementalLSV,
                   pp.PlanProductBaselineLSV,
                   dc.DemandCode,
                   dt.MarsYear,
                   dt.MarsPeriod,
                   dt.MarsWeek) AS joined
        LEFT JOIN Promo p
               ON PromoId = p.Id
        LEFT JOIN PromoStatus ps
               ON p.PromoStatusId = ps.Id");
        }

        public override void Down()
        {
            Sql(@"ALTER VIEW [PlanIncrementalReport] AS 
                SELECT NEWID() as Id, 
                CONCAT(joined.ZREP, '_0125') as ZREP, 
                joined.DemandCode as DemandCode, 
                p.InOut as InOut,
                FORMATMESSAGE('%s#%i', p.[Name], p.Number) as PromoNameId, 
                'RU_0125' as LocApollo, '7' as TypeApollo,
                'SHIP_LEWAND_CS' as ModelApollo, 
				CONVERT(datetimeoffset(7), 
				iif((SELECT COUNT(*) FROM Dates WHERE OriginalDate = joined.WeekStartDate AND MarsDay = 1) > 0, joined.WeekStartDate, (SELECT TOP(1) OriginalDate FROM Dates WHERE OriginalDate < joined.WeekStartDate AND OriginalDate > DATEADD(DAY, -7, joined.WeekStartDate) AND MarsDay = 1))) as WeekStartDate, 
				iif(joined.StartDay <> 1,
                (joined.PlanProductIncrementalCaseQty / DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd)) * (8 - joined.StartDay),
                (joined.PlanProductIncrementalCaseQty / DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd)) * joined.EndDay) as PlanProductIncrementalCaseQty,
                iif(joined.StartDay <> 1,
                (joined.PlanProductBaselineCaseQty / DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd)) * (8 - joined.StartDay),
                (joined.PlanProductBaselineCaseQty / DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd)) * joined.EndDay) as PlanProductBaselineCaseQty, 
                iif(joined.StartDay <> 1,
                (joined.PlanProductIncrementalLSV / DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd)) * (8 - joined.StartDay),
                (joined.PlanProductIncrementalLSV / DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd)) * joined.EndDay) as PlanProductIncrementalLSV, 
                iif(joined.StartDay <> 1,
                (joined.PlanProductBaselineLSV / DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd)) * (8 - joined.StartDay),
                (joined.PlanProductBaselineLSV / DATEDIFF(DAY, p.DispatchesStart, p.DispatchesEnd)) * joined.EndDay) as PlanProductBaselineLSV,
                p.PlanPromoUpliftPercent as PlanUplift, p.DispatchesStart as DispatchesStart,
                p.DispatchesEnd as DispatchesEnd, p.IsGrowthAcceleration as IsGrowthAcceleration, '' as Week, ps.[Name] as [Status] FROM( Select pp.PromoId as PromoId,
                pp.ZREP as ZREP, pp.PlanProductIncrementalCaseQty as PlanProductIncrementalCaseQty,
                dc.DemandCode as DemandCode,
			    MIN(dt.OriginalDate) as WeekStartDate,
                MIN(dt.MarsDay) as StartDay,
				MAX(dt.MarsDay) as EndDay,
				pp.PlanProductBaselineCaseQty as PlanProductBaselineCaseQty,
                pp.PlanProductIncrementalLSV as PlanProductIncrementalLSV,
                pp.PlanProductBaselineLSV as PlanProductBaselineLSV 
				FROM PromoProduct pp LEFT JOIN Promo p on pp.PromoId = p.Id
                LEFT JOIN DemandCodeView dc on p.ClientTreeId = dc.ObjectId
                INNER JOIN Dates dt on dt.OriginalDate >= p.DispatchesStart and dt.OriginalDate <= p.DispatchesEnd
                where pp.[Disabled] = 0 and pp.PlanProductIncrementalCaseQty > 0 and p.[Disabled] = 0
                GROUP BY pp.PromoId, pp.ZREP, pp.PlanProductIncrementalCaseQty, pp.PlanProductBaselineCaseQty, pp.PlanProductIncrementalLSV,
                pp.PlanProductBaselineLSV, dc.DemandCode, dt.MarsYear, dt.MarsPeriod, dt.MarsWeek ) as joined
                LEFT JOIN Promo p on PromoId = p.Id LEFT JOIN PromoStatus ps on p.PromoStatusId = ps.Id");
        }
    }
}
