namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_PromoView : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            
        }


        private string SqlString = @"
            SELECT
                pr.Id,
                pr.Name,
                pr.IsOnInvoice,
                mmc.Name AS MarsMechanicName,
                mmt.Name AS MarsMechanicTypeName,
                pr.MarsMechanicDiscount,
                cl.SystemName AS ColorSystemName,
                ps.Color AS PromoStatusColor,
                ps.SystemName AS PromoStatusSystemName,
                ps.Name AS PromoStatusName,
                pr.CreatorId,
                pr.ClientTreeId,
                pr.BaseClientTreeIds,
                pr.StartDate,
                DATEADD(SECOND, 86399, pr.EndDate) AS EndDate,
                pr.DispatchesStart, 
                pr.MarsStartDate,
                pr.MarsEndDate,
                pr.MarsDispatchesStart,
                pr.MarsDispatchesEnd,
                pr.CalendarPriority,
                pr.IsApolloExport,
                CAST(CAST(pr.DeviationCoefficient * 100 AS DECIMAL) AS FLOAT) AS DeviationCoefficient,
                pr.Number,
                bt.BrandsegTechsub AS BrandTechName,
                ev.Name AS EventName,
                pr.InOut,
                pt.SystemName AS TypeName,
                pt.Glyph AS TypeGlyph,
                pr.IsGrowthAcceleration,
                'mars' AS CompetitorName,
                'mars' AS CompetitorBrandTechName,
                0 AS Price,
                0 AS Discount
            FROM
                Jupiter.Promo AS pr LEFT OUTER JOIN
                Jupiter.PromoStatus AS ps ON pr.PromoStatusId = ps.Id LEFT OUTER JOIN
                Jupiter.PromoTypes AS pt ON pr.PromoTypesId = pt.Id LEFT OUTER JOIN
                Jupiter.Color AS cl ON pr.ColorId = cl.Id LEFT OUTER JOIN
                Jupiter.Mechanic AS mmc ON pr.MarsMechanicId = mmc.Id LEFT OUTER JOIN
                Jupiter.MechanicType AS mmt ON pr.MarsMechanicTypeId = mmt.Id LEFT OUTER JOIN
                Jupiter.Event AS ev ON pr.EventId = ev.Id LEFT OUTER JOIN
                Jupiter.BrandTech AS bt ON pr.BrandTechId = bt.Id
            WHERE        (pr.Disabled = 0)
            UNION
            SELECT
                cp.Id,
                cp.Name,
                CAST(0 AS bit),
                '',
                '',
                cp.Discount,
                cbt.Color,
                '#FFFFFF',
                'Finished',
                'Finished',
                NULL,
                ct.ObjectId,
                CAST(ct.ObjectId AS nvarchar),
                cp.StartDate,
                DATEADD(SECOND, 86399, cp.EndDate),
                cp.StartDate,
                cp.MarsStartDate,
                cp.MarsEndDate,
                cp.MarsDispatchesStart,
                cp.MarsDispatchesEnd,
                '3', 
                0, 
                0, 
                cp.Number, cbt.BrandTech, '', CAST(0 AS bit), 'Competitor', 'FD01', CAST(0 AS bit), c.[Name], cbt.BrandTech, cp.Price, cp.Discount
FROM            Jupiter.CompetitorPromo AS cp LEFT OUTER JOIN
                Jupiter.ClientTree AS ct ON cp.ClientTreeObjectId = ct.Id LEFT OUTER JOIN
                Jupiter.CompetitorBrandTech AS cbt ON cp.CompetitorBrandTechId = cbt.Id LEFT OUTER JOIN
                         Jupiter.Competitor AS c ON cp.CompetitorId = c.Id
WHERE        (cp.Disabled = 0)
        ";
    }
}
