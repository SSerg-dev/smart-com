namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_PromoView4 : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlStringUp = SqlStringUp.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlStringUp);
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlStringDown = SqlStringUp.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlStringDown);
        }
        private string SqlStringUp = @"
            DROP VIEW [DefaultSchemaSetting].[PromoView]
            GO
           CREATE VIEW [DefaultSchemaSetting].[PromoView]
            AS
            SELECT
                pr.Id,
                pr.Name,
                pr.IsOnInvoice,
                mmc.Name AS MarsMechanicName,
                mmt.Name AS MarsMechanicTypeName,
				CASE
					WHEN LEN(pr.MechanicComment) > 30 THEN SUBSTRING(pr.MechanicComment,0,29) + '...'
						ELSE pr.MechanicComment
				END as MechanicComment,
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
				pr.IsInExchange,
				pr.MasterPromoId,
                'mars' AS CompetitorName,
                'mars' AS CompetitorBrandTechName,
                ISNULL(pr.ActualInStoreShelfPrice, 0) AS Price, 
                ISNULL(pr.ActualInStoreDiscount, 0) AS Discount,
                [DefaultSchemaSetting].[GetPromoSubrangesById](pr.Id) as Subranges

            FROM
                [DefaultSchemaSetting].Promo AS pr LEFT OUTER JOIN
                [DefaultSchemaSetting].PromoStatus AS ps ON pr.PromoStatusId = ps.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].PromoTypes AS pt ON pr.PromoTypesId = pt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Color AS cl ON pr.ColorId = cl.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Mechanic AS mmc ON pr.MarsMechanicId = mmc.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].MechanicType AS mmt ON pr.MarsMechanicTypeId = mmt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Event AS ev ON pr.EventId = ev.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].BrandTech AS bt ON pr.BrandTechId = bt.Id
            WHERE   (pr.Disabled = 0)

            UNION

            SELECT
                cp.Id,
                cp.Name,
                CAST(0 AS bit),
                '',
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
                cp.Number, cbt.BrandTech, 
                '', 
                CAST(0 AS bit), 
                'Competitor', 
                'FD01', 
                CAST(0 AS bit), 
				CAST(0 AS bit), 
				NULL,
                c.[Name], 
                cbt.BrandTech, 
                cp.Price, cp.Discount, 
                '' as Subranges

            FROM    
                [DefaultSchemaSetting].CompetitorPromo AS cp LEFT OUTER JOIN
                [DefaultSchemaSetting].ClientTree AS ct ON cp.ClientTreeObjectId = ct.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].CompetitorBrandTech AS cbt ON cp.CompetitorBrandTechId = cbt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Competitor AS c ON cp.CompetitorId = c.Id
            WHERE   (cp.Disabled = 0)
        ";
        private string SqlStringDown = @"
            DROP VIEW [DefaultSchemaSetting].[PromoView]
            GO
           CREATE VIEW [DefaultSchemaSetting].[PromoView]
            AS
            SELECT
                pr.Id,
                pr.Name,
                pr.IsOnInvoice,
                mmc.Name AS MarsMechanicName,
                mmt.Name AS MarsMechanicTypeName,
				CASE
					WHEN LEN(pr.MechanicComment) > 30 THEN SUBSTRING(pr.MechanicComment,0,29) + '...'
						ELSE pr.MechanicComment
				END as MechanicComment,
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
                ISNULL(pr.ActualInStoreShelfPrice, 0) AS Price, 
                ISNULL(pr.ActualInStoreDiscount, 0) AS Discount,
                [DefaultSchemaSetting].[GetPromoSubrangesById](pr.Id) as Subranges

            FROM
                [DefaultSchemaSetting].Promo AS pr LEFT OUTER JOIN
                [DefaultSchemaSetting].PromoStatus AS ps ON pr.PromoStatusId = ps.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].PromoTypes AS pt ON pr.PromoTypesId = pt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Color AS cl ON pr.ColorId = cl.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Mechanic AS mmc ON pr.MarsMechanicId = mmc.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].MechanicType AS mmt ON pr.MarsMechanicTypeId = mmt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Event AS ev ON pr.EventId = ev.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].BrandTech AS bt ON pr.BrandTechId = bt.Id
            WHERE   (pr.Disabled = 0)

            UNION

            SELECT
                cp.Id,
                cp.Name,
                CAST(0 AS bit),
                '',
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
                cp.Number, cbt.BrandTech, 
                '', 
                CAST(0 AS bit), 
                'Competitor', 
                'FD01', 
                CAST(0 AS bit), 
                c.[Name], 
                cbt.BrandTech, 
                cp.Price, cp.Discount, 
                '' as Subranges

            FROM    
                [DefaultSchemaSetting].CompetitorPromo AS cp LEFT OUTER JOIN
                [DefaultSchemaSetting].ClientTree AS ct ON cp.ClientTreeObjectId = ct.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].CompetitorBrandTech AS cbt ON cp.CompetitorBrandTechId = cbt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Competitor AS c ON cp.CompetitorId = c.Id
            WHERE   (cp.Disabled = 0)
        ";
    }
}
