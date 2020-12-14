namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class PlanIncrementalReportView : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE VIEW [DemandCodeView] AS Select ct.ObjectId, " +
                    "iif((ct.DemandCode IS NULL OR ct.DemandCode = ''), (SELECT TOP 1 DemandCode FROM ClientTree pct where pct.ObjectId = ct.parentId and(pct.StartDate <= SYSDATETIME() and(pct.EndDate IS NULL OR pct.EndDate >= SYSDATETIME()))), ct.DemandCode) as DemandCode " +
                    "From ClientTree ct " +
                    "where ct.StartDate <= SYSDATETIME() and(ct.EndDate IS NULL OR ct.EndDate >= SYSDATETIME());");
            Sql("CREATE VIEW [PlanIncrementalReport] AS SELECT " +
                    "NEWID() as Id, " +
                    "joined.ZREP as ZREP, " +
                    "joined.DemandCode as DemandCode, " +
                    "p.[Name] as PromoName, " +
                    "FORMATMESSAGE('%s#%i', p.[Name], p.Number) as PromoNameId, " +
                    "'RU_0125' as LocApollo, " +
                    "'7' as TypeApollo, " +
                    "'SHIP_LEWAND_CS' as ModelApollo, " +
                    "CONVERT(datetimeoffset(7), joined.WeekStartDate) as WeekStartDate, " +
                    "iif(joined.StartDay <> 1, (joined.PlanProductQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * (8 - joined.StartDay), (joined.PlanProductQty / DATEDIFF(DAY, p.StartDate, p.EndDate)) * joined.EndDay) as PlanProductQty, " +
                    "p.PlanPromoUpliftPercent as PlanUplift, " +
                    "p.StartDate as StartDate, " +
                    "p.EndDate as EndDate, " +
                    "ps.[Name] as [Status] " +
                     "FROM( " +
                        "Select " +
                            "pp.PromoId as PromoId, " +
                            "pp.ZREP as ZREP, " +
                            "pp.PlanProductQty as PlanProductQty, " +
                            "dc.DemandCode as DemandCode, " +
                            "MIN(dt.OriginalDate) as WeekStartDate, " +
                            "MIN(MarsDay) as StartDay, " +
                            "MAX(MarsDay) as EndDay " +
                            "FROM PromoProduct pp " +
                            "LEFT JOIN Promo p on pp.PromoId = p.Id " +
                            "LEFT JOIN DemandCodeView dc on p.ClientTreeId = dc.ObjectId " +
                            "INNER JOIN Dates dt on dt.OriginalDate >= p.StartDate and dt.OriginalDate <= p.EndDate " +
                            "where pp.[Disabled] = 0 " +
                                "and pp.PlanProductQty > 0 " +
                                "and p.[Disabled] = 0 " +
                            "GROUP BY pp.PromoId, pp.ZREP, pp.PlanProductQty, dc.DemandCode, dt.MarsYear, dt.MarsPeriod, dt.MarsWeek " +
                    ") as joined " +
                        "LEFT JOIN Promo p on PromoId = p.Id " +
                        "LEFT JOIN PromoStatus ps on p.PromoStatusId = ps.Id");
        }

        public override void Down()
        {
            Sql("DROP VIEW [PlanIncrementalReport]");
            Sql("DROP VIEW [DemandCodeView]");
        }
    }
}
