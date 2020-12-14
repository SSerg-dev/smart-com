namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IncrementalReport_Change_To_Dispatches : DbMigration
    {
        public override void Up()
        {
			Sql("DROP VIEW [PlanIncrementalReport]");
			Sql(@"CREATE VIEW [PlanIncrementalReport] AS 
SELECT NEWID() as Id, 
CONCAT(joined.ZREP, '_0125') as ZREP, 
joined.DemandCode as DemandCode, 
p.InOut as InOut,
FORMATMESSAGE('%s#%i', p.[Name], p.Number) as PromoNameId, 
'RU_0125' as LocApollo, '7' as TypeApollo,
'SHIP_LEWAND_CS' as ModelApollo, CONVERT(datetimeoffset(7), 
joined.WeekStartDate) as WeekStartDate, iif(joined.StartDay <> 1,
(joined.PlanProductCaseQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay),
(joined.PlanProductCaseQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductCaseQty,
iif(joined.StartDay <> 1,
(joined.PlanProductBaselineCaseQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay),
(joined.PlanProductBaselineCaseQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductBaselineCaseQty, 
iif(joined.StartDay <> 1,
(joined.PlanProductIncrementalLSV / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay),
(joined.PlanProductIncrementalLSV / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductIncrementalLSV, 
iif(joined.StartDay <> 1,
(joined.PlanProductBaselineLSV / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay),
(joined.PlanProductBaselineLSV / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductBaselineLSV,
p.PlanPromoUpliftPercent as PlanUplift, p.DispatchesStart as DispatchesStart,
p.DispatchesEnd as DispatchesEnd, '' as Week, ps.[Name] as [Status] FROM( Select pp.PromoId as PromoId,
pp.ZREP as ZREP, pp.PlanProductCaseQty as PlanProductCaseQty,
dc.DemandCode as DemandCode, MIN(dt.OriginalDate) as WeekStartDate,
MIN(MarsDay) as StartDay, MAX(MarsDay) as EndDay, pp.PlanProductBaselineCaseQty as PlanProductBaselineCaseQty,
pp.PlanProductIncrementalLSV as PlanProductIncrementalLSV,
pp.PlanProductBaselineLSV as PlanProductBaselineLSV FROM PromoProduct pp LEFT JOIN Promo p on pp.PromoId = p.Id 
LEFT JOIN DemandCodeView dc on p.ClientTreeId = dc.ObjectId 
INNER JOIN Dates dt on dt.OriginalDate >= p.StartDate and dt.OriginalDate <= p.EndDate 
where pp.[Disabled] = 0 and pp.PlanProductCaseQty > 0 and p.[Disabled] = 0 
GROUP BY pp.PromoId, pp.ZREP, pp.PlanProductCaseQty, pp.PlanProductBaselineCaseQty, pp.PlanProductIncrementalLSV, 
pp.PlanProductBaselineLSV, dc.DemandCode, dt.MarsYear, dt.MarsPeriod, dt.MarsWeek ) as joined 
LEFT JOIN Promo p on PromoId = p.Id LEFT JOIN PromoStatus ps on p.PromoStatusId = ps.Id");
		}
        
        public override void Down()
        {
			Sql("DROP VIEW [PlanIncrementalReport]");
			Sql(@"CREATE VIEW [PlanIncrementalReport] AS 
SELECT NEWID() as Id, 
CONCAT(joined.ZREP, '_0125') as ZREP, 
joined.DemandCode as DemandCode, 
p.InOut as InOut,
FORMATMESSAGE('%s#%i', p.[Name], p.Number) as PromoNameId, 
'RU_0125' as LocApollo, '7' as TypeApollo,
'SHIP_LEWAND_CS' as ModelApollo, CONVERT(datetimeoffset(7), 
joined.WeekStartDate) as WeekStartDate, iif(joined.StartDay <> 1,
(joined.PlanProductCaseQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay),
(joined.PlanProductCaseQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductCaseQty,
iif(joined.StartDay <> 1,
(joined.PlanProductBaselineCaseQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay),
(joined.PlanProductBaselineCaseQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductBaselineCaseQty, 
iif(joined.StartDay <> 1,
(joined.PlanProductIncrementalLSV / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay),
(joined.PlanProductIncrementalLSV / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductIncrementalLSV, 
iif(joined.StartDay <> 1,
(joined.PlanProductBaselineLSV / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay),
(joined.PlanProductBaselineLSV / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductBaselineLSV,
p.PlanPromoUpliftPercent as PlanUplift, p.StartDate as StartDate,
p.EndDate as EndDate, '' as Week, ps.[Name] as [Status] FROM( Select pp.PromoId as PromoId,
pp.ZREP as ZREP, pp.PlanProductCaseQty as PlanProductCaseQty,
dc.DemandCode as DemandCode, MIN(dt.OriginalDate) as WeekStartDate,
MIN(MarsDay) as StartDay, MAX(MarsDay) as EndDay, pp.PlanProductBaselineCaseQty as PlanProductBaselineCaseQty,
pp.PlanProductIncrementalLSV as PlanProductIncrementalLSV,
pp.PlanProductBaselineLSV as PlanProductBaselineLSV FROM PromoProduct pp LEFT JOIN Promo p on pp.PromoId = p.Id 
LEFT JOIN DemandCodeView dc on p.ClientTreeId = dc.ObjectId 
INNER JOIN Dates dt on dt.OriginalDate >= p.StartDate and dt.OriginalDate <= p.EndDate 
where pp.[Disabled] = 0 and pp.PlanProductCaseQty > 0 and p.[Disabled] = 0 
GROUP BY pp.PromoId, pp.ZREP, pp.PlanProductCaseQty, pp.PlanProductBaselineCaseQty, pp.PlanProductIncrementalLSV, 
pp.PlanProductBaselineLSV, dc.DemandCode, dt.MarsYear, dt.MarsPeriod, dt.MarsWeek ) as joined 
LEFT JOIN Promo p on PromoId = p.Id LEFT JOIN PromoStatus ps on p.PromoStatusId = ps.Id");
		}
    }
}
