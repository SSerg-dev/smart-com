namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_GA_to_IncrementalReport : DbMigration
    {
        public override void Up()
        {
            Sql("DROP VIEW [dbo].[PlanIncrementalReport]");
            Sql(@"CREATE VIEW [dbo].[PlanIncrementalReport] AS 
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
        
        public override void Down()
        {
            Sql("DROP VIEW [dbo].[PlanIncrementalReport]");
            Sql(@"CREATE VIEW [dbo].[PlanIncrementalReport] AS 
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
                p.DispatchesEnd as DispatchesEnd, '' as Week, ps.[Name] as [Status] FROM( Select pp.PromoId as PromoId,
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
