﻿namespace Module.Persist.TPM.Migrations.Views
{
    /// <summary>
    /// Имя схемы менять на DefaultSchemaSetting
    /// </summary>
    public static class ViewMigrations
    {
        public static string GetPromoGridViewString(string defaultSchema)
        {            
            return SqlString.Replace("DefaultSchemaSetting", defaultSchema); ;
        }
        private static readonly string SqlString = @" 
             ALTER VIEW [DefaultSchemaSetting].[PromoGridView]
                AS
            SELECT pr.Id, pr.Name, pr.Number, pr.Disabled, pr.Mechanic, pr.CreatorId, pr.MechanicIA, pr.ClientTreeId, pr.ClientHierarchy, pr.MarsMechanicDiscount, pr.IsDemandFinanceApproved, pr.IsDemandPlanningApproved, pr.IsCMManagerApproved, pr.IsGAManagerApproved,
                              pr.PlanInstoreMechanicDiscount, pr.EndDate, pr.StartDate, pr.DispatchesEnd, pr.DispatchesStart, pr.MarsEndDate, pr.MarsStartDate, pr.MarsDispatchesEnd, pr.MarsDispatchesStart, pr.BudgetYear, bnd.Name AS BrandName, 
                              bt.BrandsegTechsub AS BrandTechName, ev.Name AS PromoEventName, ps.Name AS PromoStatusName, ps.Color AS PromoStatusColor, mmc.Name AS MarsMechanicName, mmt.Name AS MarsMechanicTypeName, 
                              pim.Name AS PlanInstoreMechanicName, ps.SystemName AS PromoStatusSystemName, pimt.Name AS PlanInstoreMechanicTypeName, pr.PlanPromoTIShopper, pr.PlanPromoTIMarketing, pr.PlanPromoXSites, pr.PlanPromoCatalogue, 
                              pr.PlanPromoPOSMInClient, pr.ActualPromoUpliftPercent, pr.ActualPromoTIShopper, pr.ActualPromoTIMarketing, pr.ActualPromoXSites, pr.ActualPromoCatalogue, pr.ActualPromoPOSMInClient, 
                              CAST(ROUND(CAST(pr.PlanPromoUpliftPercent AS DECIMAL(18, 3)), 2) AS FLOAT) AS PlanPromoUpliftPercent, pr.PlanPromoROIPercent, pr.ActualPromoNetIncrementalNSV, pr.ActualPromoIncrementalNSV, pr.ActualPromoROIPercent, 
                              pr.ProductHierarchy, pr.PlanPromoNetIncrementalNSV, pr.PlanPromoIncrementalNSV, pr.InOut, CAST(ROUND(CAST(pr.PlanPromoIncrementalLSV / 1000000.0 AS DECIMAL(18, 3)), 2) AS FLOAT) AS PlanPromoIncrementalLSV, 
                              CAST(ROUND(CAST(pr.PlanPromoBaselineLSV / 1000000.0 AS DECIMAL(18, 3)), 2) AS FLOAT) AS PlanPromoBaselineLSV, pr.LastChangedDate, pr.LastChangedDateFinance, pr.LastChangedDateDemand, pts.Name AS PromoTypesName, 
                              pr.IsGrowthAcceleration, pr.IsApolloExport, CAST(CAST(pr.DeviationCoefficient * 100 AS DECIMAL) AS FLOAT) AS DeviationCoefficient, pr.ActualPromoLSVByCompensation, pr.PlanPromoLSV, pr.ActualPromoLSV, 
                              pr.ActualPromoBaselineLSV, pr.ActualPromoIncrementalLSV, pr.SumInvoice, pr.IsOnInvoice, pr.IsInExchange, pr.TPMmode
            FROM     DefaultSchemaSetting.Promo AS pr LEFT OUTER JOIN
                              DefaultSchemaSetting.Event AS ev ON pr.EventId = ev.Id LEFT OUTER JOIN
                              DefaultSchemaSetting.Brand AS bnd ON pr.BrandId = bnd.Id LEFT OUTER JOIN
                              DefaultSchemaSetting.BrandTech AS bt ON pr.BrandTechId = bt.Id LEFT OUTER JOIN
                              DefaultSchemaSetting.PromoStatus AS ps ON pr.PromoStatusId = ps.Id LEFT OUTER JOIN
                              DefaultSchemaSetting.Mechanic AS mmc ON pr.MarsMechanicId = mmc.Id LEFT OUTER JOIN
                              DefaultSchemaSetting.Mechanic AS pim ON pr.PlanInstoreMechanicId = pim.Id LEFT OUTER JOIN
                              DefaultSchemaSetting.MechanicType AS mmt ON pr.MarsMechanicTypeId = mmt.Id LEFT OUTER JOIN
                              DefaultSchemaSetting.MechanicType AS pimt ON pr.PlanInstoreMechanicTypeId = pimt.Id LEFT OUTER JOIN
                              DefaultSchemaSetting.PromoTypes AS pts ON pr.PromoTypesId = pts.Id
            ";

        public static string GetPromoViewString(string defaultSchema)
        {
            return SqlPromoViewString.Replace("DefaultSchemaSetting", defaultSchema); ;
        }
        private static readonly string SqlPromoViewString = @" 
             ALTER VIEW [DefaultSchemaSetting].[PromoView]
            AS
            SELECT
                pr.Id,
				pr.Disabled,
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
				pr.TPMmode,
				CAST(0 AS bit) AS IsOnHold,
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

            UNION

            SELECT
                cp.Id,
				cp.Disabled,
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
				0,
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
            ";

        public static string GetPromoRSViewString(string defaultSchema)
        {
            return SqlPromoRSViewString.Replace("DefaultSchemaSetting", defaultSchema); ;
        }
        private static readonly string SqlPromoRSViewString = @" 
             CREATE OR ALTER VIEW [DefaultSchemaSetting].[PromoRSView]
            AS
			select * from (
            SELECT
                pr.Id,
				pr.Disabled,
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
				pr.TPMmode,
				CAST(0 AS bit) AS IsOnHold,
                'mars' AS CompetitorName,
                'mars' AS CompetitorBrandTechName,
                ISNULL(pr.ActualInStoreShelfPrice, 0) AS Price, 
                ISNULL(pr.ActualInStoreDiscount, 0) AS Discount,
                [DefaultSchemaSetting].[GetPromoSubrangesById](pr.Id) as Subranges,
				ROW_NUMBER() OVER(PARTITION BY pr.Number ORDER BY TPMmode DESC) AS row_number

            FROM
                [DefaultSchemaSetting].Promo AS pr LEFT OUTER JOIN
                [DefaultSchemaSetting].PromoStatus AS ps ON pr.PromoStatusId = ps.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].PromoTypes AS pt ON pr.PromoTypesId = pt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Color AS cl ON pr.ColorId = cl.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Mechanic AS mmc ON pr.MarsMechanicId = mmc.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].MechanicType AS mmt ON pr.MarsMechanicTypeId = mmt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Event AS ev ON pr.EventId = ev.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].BrandTech AS bt ON pr.BrandTechId = bt.Id) query
			WHERE 
				row_number = 1

            UNION

            SELECT
                cp.Id,
				cp.Disabled,
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
				0,
				CAST(0 AS bit),
                c.[Name], 
                cbt.BrandTech, 
                cp.Price, cp.Discount, 
                '' as Subranges,
				1 as row_number

            FROM    
                [DefaultSchemaSetting].CompetitorPromo AS cp LEFT OUTER JOIN
                [DefaultSchemaSetting].ClientTree AS ct ON cp.ClientTreeObjectId = ct.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].CompetitorBrandTech AS cbt ON cp.CompetitorBrandTechId = cbt.Id LEFT OUTER JOIN
                [DefaultSchemaSetting].Competitor AS c ON cp.CompetitorId = c.Id
            ";

        public static string GetPromoInsertTriggerString(string defaultSchema)
        {
            return PromoInsertTriggerSqlString.Replace("DefaultSchemaSetting", defaultSchema); ;
        }
        private static readonly string PromoInsertTriggerSqlString = @" 
             ALTER TRIGGER [DefaultSchemaSetting].[Promo_increment_number] ON [DefaultSchemaSetting].[Promo] AFTER INSERT AS
                BEGIN

					If (SELECT Number FROM INSERTED) > 0 
					Begin
						Return
					End
    				UPDATE Promo SET Number = (SELECT ISNULL((SELECT MAX(Number) FROM Promo WHERE Number < 999999), 0) + 1) FROM Inserted WHERE Promo.Id = Inserted.Id;
                END
            ";

        public static string GetPromoROIReportViewString(string defaultSchema)
        {
            return PromoROIReportViewSqlString.Replace("DefaultSchemaSetting", defaultSchema); ;
        }
        private static readonly string PromoROIReportViewSqlString = @" 
             ALTER VIEW [DefaultSchemaSetting].[PromoROIReportView] AS
			WITH
				CheckPromoStatuses
			AS
			(
				SELECT
					[value] AS [Status]
				FROM STRING_SPLIT(ISNULL(
				(
					SELECT
						s.[Value]
					FROM [DefaultSchemaSetting].[Setting] s
					WHERE s.[Name] = 'ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST'
				), 'Finished,Closed'), ',')  
			),
				PreviousYearsPromoId
			AS
			(
				SELECT
					p.[Id]
				FROM [DefaultSchemaSetting].[Promo] p
				INNER JOIN [DefaultSchemaSetting].[PromoStatus] ps 
					ON ps.[Id] = p.[PromoStatusId]
				WHERE
					p.[Disabled] = 0
					AND p.[StartDate] IS NOT NULL AND YEAR(p.[StartDate]) <> YEAR(GETDATE())
					AND ps.[Name] IN (SELECT * FROM CheckPromoStatuses)
			),
				PromoROIReport
			AS
			(
				SELECT 
					p.[ClientTreeKeyId],
					p.[BaseClientTreeId],
					p.[BaseClientTreeIds],
					p.[NeedRecountUplift],
					p.[LastApprovedDate],
					p.[Name],
					p.[ClientHierarchy],
					p.[ProductHierarchy],
					p.[LastChangedDate],
					p.[LastChangedDateDemand],
					p.[LastChangedDateFinance],
					p.[DispatchesStart],
					p.[DispatchesEnd],
					p.[DispatchDuration],
					p.[Mechanic],
					p.[MechanicIA],
					p.[MarsStartDate],
					p.[MarsEndDate],
					p.[MarsDispatchesStart],
					p.[MarsDispatchesEnd],
					p.[BudgetYear],
					p.[OtherEventName],
					p.[CalendarPriority],
					p.[PlanPromoPostPromoEffectLSVW1],
					p.[PlanPromoPostPromoEffectLSVW2],
					p.[ActualPromoPostPromoEffectLSVW1],
					p.[ActualPromoPostPromoEffectLSVW2],
					p.[IsAutomaticallyApproved],
					p.[IsCMManagerApproved],
					p.[IsGAManagerApproved],
					p.[IsDemandPlanningApproved],
					p.[IsDemandFinanceApproved],
					'' AS [ProductTreeObjectIds],
					p.[Calculating],
					p.[BlockInformation],
					p.[Id],
					p.[Number],
					p.[ClientTreeId],
					p.[ProductSubrangesList],
					p.[MarsMechanicDiscount],
					p.[MechanicComment],
					p.[StartDate],
					p.[EndDate],
					p.[PromoDuration],
					p.[InOut],
					p.[IsGrowthAcceleration],
					p.[TPMmode],
					p.[PlanInstoreMechanicDiscount],
					p.[PlanInStoreShelfPrice],
					p.[PlanPromoBaselineLSV],
					p.[PlanPromoIncrementalLSV],
					p.[PlanPromoLSV],
					p.[PlanPromoUpliftPercent],
					p.[PlanPromoTIShopper],
					p.[PlanPromoTIMarketing],
					p.[PlanPromoXSites],
					p.[PlanPromoCatalogue],
					p.[PlanPromoPOSMInClient],
					p.[PlanPromoBranding],
					p.[PlanPromoBTL],
					p.[PlanPromoCostProduction],
					p.[PlanPromoCostProdXSites],
					p.[PlanPromoCostProdCatalogue],
					p.[PlanPromoCostProdPOSMInClient],
					p.[PlanPromoCost],
					p.[PlanPromoIncrementalBaseTI],
					p.[PlanPromoNetIncrementalBaseTI],
					p.[PlanPromoIncrementalCOGS],
					p.[PlanPromoNetIncrementalCOGS],
					p.[PlanPromoTotalCost],
					p.[PlanPromoPostPromoEffectLSV],
					p.[PlanPromoNetIncrementalLSV],
					p.[PlanPromoNetLSV],
					p.[PlanPromoBaselineBaseTI],
					p.[PlanPromoBaseTI],
					p.[PlanPromoNetBaseTI],
					p.[PlanPromoNSV],
					p.[PlanPromoNetNSV],
					p.[PlanPromoIncrementalNSV],
					p.[PlanPromoNetIncrementalNSV],
					p.[PlanPromoIncrementalMAC],
					p.[PlanPromoNetIncrementalMAC],
					p.[PlanPromoIncrementalEarnings],
					p.[PlanPromoNetIncrementalEarnings],
					p.[PlanPromoROIPercent],
					p.[PlanPromoNetROIPercent],
					p.[PlanPromoNetUpliftPercent],
					p.[ActualInStoreDiscount],
					p.[ActualInStoreShelfPrice],
					p.[InvoiceNumber],
					p.[ActualPromoBaselineLSV],
					p.[ActualPromoIncrementalLSV],
					p.[ActualPromoLSVByCompensation],
					p.[ActualPromoLSV],
					p.[ActualPromoUpliftPercent],
					p.[ActualPromoNetUpliftPercent],
					p.[ActualPromoTIShopper],
					p.[ActualPromoTIMarketing],
					p.[ActualPromoXSites],
					p.[ActualPromoCatalogue],
					p.[ActualPromoPOSMInClient],
					p.[ActualPromoBranding],
					p.[ActualPromoBTL],
					p.[ActualPromoCostProduction],
					p.[ActualPromoCostProdXSites],
					p.[ActualPromoCostProdCatalogue],
					p.[ActualPromoCostProdPOSMInClient],
					p.[ActualPromoCost],
					p.[ActualPromoIncrementalBaseTI],
					p.[ActualPromoNetIncrementalBaseTI],
					p.[ActualPromoIncrementalCOGS],
					p.[ActualPromoNetIncrementalCOGS],
					p.[ActualPromoTotalCost],
					p.[ActualPromoPostPromoEffectLSV],
					p.[ActualPromoNetIncrementalLSV],
					p.[ActualPromoNetLSV],
					p.[ActualPromoIncrementalNSV],
					p.[ActualPromoNetIncrementalNSV],
					p.[ActualPromoBaselineBaseTI],
					p.[ActualPromoBaseTI],
					p.[ActualPromoNetBaseTI],
					p.[ActualPromoNSV],
					p.[ActualPromoNetNSV],
					p.[ActualPromoIncrementalMAC],
					p.[ActualPromoNetIncrementalMAC],
					p.[ActualPromoIncrementalEarnings],
					p.[ActualPromoNetIncrementalEarnings],
					p.[ActualPromoROIPercent],
					p.[ActualPromoNetROIPercent],
					p.[SumInvoice],
					p.[ActualAddTIMarketing],
					p.[PlanAddTIMarketingApproved],
					p.[ActualAddTIShopper],
					p.[PlanAddTIShopperApproved],
					p.[PlanAddTIShopperCalculated],
					p.[PlanPromoIncrementalMACLSV],
					p.[PlanPromoNetIncrementalMACLSV],
					p.[ActualPromoIncrementalMACLSV],
					p.[ActualPromoNetIncrementalMACLSV],
					p.[PlanPromoIncrementalEarningsLSV],
					p.[PlanPromoNetIncrementalEarningsLSV],
					p.[ActualPromoIncrementalEarningsLSV],
					p.[ActualPromoNetIncrementalEarningsLSV],
					p.[PlanPromoROIPercentLSV],
					p.[PlanPromoNetROIPercentLSV],
					p.[ActualPromoROIPercentLSV],
					p.[ActualPromoNetROIPercentLSV],
					p.[PlanPromoIncrementalCOGSTn],
					p.[PlanPromoNetIncrementalCOGSTn],
					p.[ActualPromoIncrementalCOGSTn],
					p.[ActualPromoNetIncrementalCOGSTn],
					p.[PlanPromoBaselineVolume],
					p.[PlanPromoIncrementalVolume],
					p.[PlanPromoNetIncrementalVolume],
					p.[ActualPromoVolume],
					p.[ActualPromoIncrementalVolume],
					p.[ActualPromoNetIncrementalVolume],
					ct.[Name]						AS [ClientName],
					e.[Name]						AS [EventName],
					ps.[Name]						AS [PromoStatusName],
					inM.[Name]						AS [PlanInstoreMechanicName],
					inMT.[Name]						AS [PlanInstoreMechanicTypeName],
					ppPCP.[PlanProductPCPriceAVG]	AS [PCPrice],
					m.[Name]						AS [MarsMechanicName],
					mt.[Name]						AS [MarsMechanicTypeName],
					aM.[Name]						AS [ActualInStoreMechanicName],
					aMT.[Name]						AS [ActualInStoreMechanicTypeName],
					pt.[Name]						AS [PromoTypesName],
				
					TRIM((SELECT TOP(1) 
						[value] 
					FROM STRING_SPLIT(p.[ClientHierarchy]
					, '>')))									    AS [Client1LevelName],

					TRIM((SELECT TOP(1) 
						T.*  
					FROM (
						SELECT TOP(2) 
							[value] 
						FROM STRING_SPLIT(p.[ClientHierarchy], '>')
						) AS T
					ORDER BY T.[value] ASC))						AS [Client2LevelName],

					CASE WHEN b.[Name] IS NULL OR b.[Name] = ''
						THEN btB.[Name]
						ELSE b.[Name]
					END												AS [BrandName],
					CASE WHEN t.[Name] IS NULL OR t.[Name] = ''
						THEN btT.[Name]
						ELSE t.[Name]
					END												AS [TechnologyName],
					CASE WHEN t.[SubBrand] IS NULL OR t.[SubBrand] = ''
						THEN btT.[SubBrand]
						ELSE t.[SubBrand]
					END												AS [SubName],

					CASE WHEN p.[Id] IN (SELECT * FROM PreviousYearsPromoId)
						THEN p.[ActualTIBasePercent]
						ELSE p.[PlanTIBasePercent]
					END									AS [TIBasePercent],

					CASE WHEN p.[Id] IN (SELECT * FROM PreviousYearsPromoId)
						THEN p.[ActualCOGSPercent]
						ELSE p.[PlanCOGSPercent]
					END									AS [COGSPercent],

					CASE WHEN p.[Id] IN (SELECT * FROM PreviousYearsPromoId)
						THEN p.[ActualCOGSTn]
						ELSE p.[PlanCOGSTn]
					END									AS [COGSTn]
				FROM [DefaultSchemaSetting].[Promo] p
				INNER JOIN [DefaultSchemaSetting].[ClientTree] ct 
					ON ct.[Id] = p.[ClientTreeKeyId]
				INNER JOIN [DefaultSchemaSetting].[PromoStatus] ps
					ON ps.[Id] = p.[PromoStatusId]
				INNER JOIN [DefaultSchemaSetting].[PromoTypes] pt
					ON pt.[Id] = p.[PromoTypesId]

				LEFT JOIN [DefaultSchemaSetting].[Brand] b
					ON b.[Id] = p.[BrandId]
				LEFT JOIN [DefaultSchemaSetting].[Technology] t
					ON t.[Id] = p.[TechnologyId]

				LEFT JOIN [DefaultSchemaSetting].[BrandTech] bt
					ON bt.[Id] = p.[BrandTechId]
				LEFT JOIN [DefaultSchemaSetting].[Brand] btB
					ON btB.[Id] = bt.[BrandId]
				LEFT JOIN [DefaultSchemaSetting].[Technology] btT
					ON btT.[Id] = bt.[TechnologyId]

				LEFT JOIN [DefaultSchemaSetting].[Event] e
					ON e.[Id] = p.[EventId]
		
				LEFT JOIN [DefaultSchemaSetting].[Mechanic] inM
					ON inM.[Id] = p.[PlanInstoreMechanicId]
				LEFT JOIN [DefaultSchemaSetting].[MechanicType] inMT
					ON inMT.[Id] = p.[PlanInstoreMechanicTypeId]
		
				LEFT JOIN [DefaultSchemaSetting].[Mechanic] m
					ON m.[Id] = p.[MarsMechanicId]
				LEFT JOIN [DefaultSchemaSetting].[MechanicType] mt
					ON mt.[Id] = p.[MarsMechanicTypeId]
		
				LEFT JOIN [DefaultSchemaSetting].[Mechanic] aM
					ON aM.[Id] = p.[ActualInStoreMechanicId]
				LEFT JOIN [DefaultSchemaSetting].[MechanicType] aMT
					ON aMT.[Id] = p.[ActualInStoreMechanicTypeId]

				LEFT JOIN (
					SELECT 
						[PromoId],
						AVG([PlanProductPCPrice]) AS PlanProductPCPriceAVG
					FROM [DefaultSchemaSetting].[PromoProduct]
					WHERE [Disabled] = 0 AND [PlanProductPCPrice] > 0
					GROUP BY [PromoId]
				) AS ppPCP
					ON ppPCP.[PromoId] = p.[Id]

				WHERE 
					p.[Disabled] = 0  
			)

			SELECT * FROM PromoROIReport
            ";

		public static string CreatePromoProductCorrectionViewString(string defaultSchema)
        {
			return CreatePromoProductCorrectionViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);
        }
		private static string CreatePromoProductCorrectionViewSqlString = @"
			CREATE VIEW [DefaultSchemaSetting].[PromoProductCorrectionView]
			AS
			SELECT
				ppc.Id AS Id,
				pr.Number AS Number,
				pr.ClientHierarchy AS ClientHierarchy,
				btech.BrandsegTechsub AS BrandTechName,
				pr.ProductSubrangesList AS ProductSubrangesList,
				mech.Name AS MarsMechanicName,
				ev.Name AS EventName,
				ps.SystemName AS PromoStatusSystemName,
				pr.MarsStartDate AS MarsStartDate,
				pr.MarsEndDate AS MarsEndDate,
				pp.PlanProductBaselineLSV AS PlanProductBaselineLSV,
				pp.PlanProductIncrementalLSV AS PlanProductIncrementalLSV,
				pp.PlanProductLSV AS PlanProductLSV,
				pp.ZREP AS ZREP,
				ppc.PlanProductUpliftPercentCorrected AS PlanProductUpliftPercentCorrected,
				ppc.CreateDate AS CreateDate,
				ppc.ChangeDate AS ChangeDate,
				ppc.UserName AS UserName,
				ppc.Disabled AS Disabled,
				pr.ClientTreeId AS ClientTreeId,
				ppc.PromoProductId AS PromoProductId,
				ppc.UserId as UserId,				
				ppc.TPMmode AS TPMmode,
				ppc.TempId AS TempId

			FROM 
				[DefaultSchemaSetting].PromoProductsCorrection AS ppc INNER JOIN
                [DefaultSchemaSetting].PromoProduct AS pp ON ppc.PromoProductId = pp.Id INNER JOIN
                [DefaultSchemaSetting].Promo AS pr ON pp.PromoId = pr.Id INNER JOIN
                [DefaultSchemaSetting].PromoStatus AS ps ON pr.PromoStatusId = ps.Id INNER JOIN
                [DefaultSchemaSetting].Event AS ev ON pr.EventId = ev.Id INNER JOIN
                [DefaultSchemaSetting].Mechanic AS mech ON pr.MarsMechanicId = mech.Id INNER JOIN
                [DefaultSchemaSetting].BrandTech AS btech ON pr.BrandTechId = btech.Id INNER JOIN
                [DefaultSchemaSetting].ClientTree AS cltr ON pr.ClientTreeKeyId = cltr.Id
		";

		public static string DropPromoProductCorrectionViewString(string defaultSchema)
		{
			return DropPromoProductCorrectionViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);

		}
		private static string DropPromoProductCorrectionViewSqlString = @"
			DROP VIEW [DefaultSchemaSetting].[PromoProductCorrectionView]
		";

		public static string UpdatePromoROIReportViewString(string defaultSchema)
        {
			return UpdatePromoROIReportViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);
		}
		private static string UpdatePromoROIReportViewSqlString = @"
			ALTER VIEW [DefaultSchemaSetting].[PromoROIReportView] AS
			WITH
				CheckPromoStatuses
			AS
			(
				SELECT
					[value] AS [Status]
				FROM STRING_SPLIT(ISNULL(
				(
					SELECT
						s.[Value]
					FROM [DefaultSchemaSetting].[Setting] s
					WHERE s.[Name] = 'ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST'
				), 'Finished,Closed'), ',')  
			),
				PreviousYearsPromoId
			AS
			(
				SELECT
					p.[Id]
				FROM [DefaultSchemaSetting].[Promo] p
				INNER JOIN [DefaultSchemaSetting].[PromoStatus] ps 
					ON ps.[Id] = p.[PromoStatusId]
				WHERE
					p.[Disabled] = 0
					AND p.[StartDate] IS NOT NULL AND YEAR(p.[StartDate]) <> YEAR(GETDATE())
					AND ps.[Name] IN (SELECT * FROM CheckPromoStatuses)
			),
				PromoROIReport
			AS
			(
				SELECT 
					p.[ClientTreeKeyId],
					p.[BaseClientTreeId],
					p.[BaseClientTreeIds],
					p.[NeedRecountUplift],
					p.[LastApprovedDate],
					p.[Name],
					p.[ClientHierarchy],
					p.[ProductHierarchy],
					p.[LastChangedDate],
					p.[LastChangedDateDemand],
					p.[LastChangedDateFinance],
					p.[DispatchesStart],
					p.[DispatchesEnd],
					p.[DispatchDuration],
					p.[Mechanic],
					p.[MechanicIA],
					p.[MarsStartDate],
					p.[MarsEndDate],
					p.[MarsDispatchesStart],
					p.[MarsDispatchesEnd],
					p.[BudgetYear],
					p.[OtherEventName],
					p.[CalendarPriority],
					p.[PlanPromoPostPromoEffectLSVW1],
					p.[PlanPromoPostPromoEffectLSVW2],
					p.[ActualPromoPostPromoEffectLSVW1],
					p.[ActualPromoPostPromoEffectLSVW2],
					p.[IsAutomaticallyApproved],
					p.[IsCMManagerApproved],
					p.[IsGAManagerApproved],
					p.[IsDemandPlanningApproved],
					p.[IsDemandFinanceApproved],
					'' AS [ProductTreeObjectIds],
					p.[Calculating],
					p.[BlockInformation],
					p.[Id],
					p.[Number],
					p.[ClientTreeId],
					p.[ProductSubrangesList],
					p.[MarsMechanicDiscount],
					p.[MechanicComment],
					p.[StartDate],
					p.[EndDate],
					p.[PromoDuration],
					p.[InOut],
					p.[IsGrowthAcceleration],
					p.[TPMmode],
					p.[PlanInstoreMechanicDiscount],
					p.[PlanInStoreShelfPrice],
					p.[PlanPromoBaselineLSV],
					p.[PlanPromoIncrementalLSV],
					p.[PlanPromoLSV],
					p.[PlanPromoUpliftPercent],
					p.[PlanPromoTIShopper],
					p.[PlanPromoTIMarketing],
					p.[PlanPromoXSites],
					p.[PlanPromoCatalogue],
					p.[PlanPromoPOSMInClient],
					p.[PlanPromoBranding],
					p.[PlanPromoBTL],
					p.[PlanPromoCostProduction],
					p.[PlanPromoCostProdXSites],
					p.[PlanPromoCostProdCatalogue],
					p.[PlanPromoCostProdPOSMInClient],
					p.[PlanPromoCost],
					p.[PlanPromoIncrementalBaseTI],
					p.[PlanPromoNetIncrementalBaseTI],
					p.[PlanPromoIncrementalCOGS],
					p.[PlanPromoNetIncrementalCOGS],
					p.[PlanPromoTotalCost],
					p.[PlanPromoPostPromoEffectLSV],
					p.[PlanPromoNetIncrementalLSV],
					p.[PlanPromoNetLSV],
					p.[PlanPromoBaselineBaseTI],
					p.[PlanPromoBaseTI],
					p.[PlanPromoNetBaseTI],
					p.[PlanPromoNSV],
					p.[PlanPromoNetNSV],
					p.[PlanPromoIncrementalNSV],
					p.[PlanPromoNetIncrementalNSV],
					p.[PlanPromoIncrementalMAC],
					p.[PlanPromoNetIncrementalMAC],
					p.[PlanPromoIncrementalEarnings],
					p.[PlanPromoNetIncrementalEarnings],
					p.[PlanPromoROIPercent],
					p.[PlanPromoNetROIPercent],
					p.[PlanPromoNetUpliftPercent],
					p.[ActualInStoreDiscount],
					p.[ActualInStoreShelfPrice],
					p.[InvoiceNumber],
					p.[ActualPromoBaselineLSV],
					p.[ActualPromoIncrementalLSV],
					p.[ActualPromoLSVByCompensation],
					p.[ActualPromoLSV],
					p.[ActualPromoUpliftPercent],
					p.[ActualPromoNetUpliftPercent],
					p.[ActualPromoTIShopper],
					p.[ActualPromoTIMarketing],
					p.[ActualPromoXSites],
					p.[ActualPromoCatalogue],
					p.[ActualPromoPOSMInClient],
					p.[ActualPromoBranding],
					p.[ActualPromoBTL],
					p.[ActualPromoCostProduction],
					p.[ActualPromoCostProdXSites],
					p.[ActualPromoCostProdCatalogue],
					p.[ActualPromoCostProdPOSMInClient],
					p.[ActualPromoCost],
					p.[ActualPromoIncrementalBaseTI],
					p.[ActualPromoNetIncrementalBaseTI],
					p.[ActualPromoIncrementalCOGS],
					p.[ActualPromoNetIncrementalCOGS],
					p.[ActualPromoTotalCost],
					p.[ActualPromoPostPromoEffectLSV],
					p.[ActualPromoNetIncrementalLSV],
					p.[ActualPromoNetLSV],
					p.[ActualPromoIncrementalNSV],
					p.[ActualPromoNetIncrementalNSV],
					p.[ActualPromoBaselineBaseTI],
					p.[ActualPromoBaseTI],
					p.[ActualPromoNetBaseTI],
					p.[ActualPromoNSV],
					p.[ActualPromoNetNSV],
					p.[ActualPromoIncrementalMAC],
					p.[ActualPromoNetIncrementalMAC],
					p.[ActualPromoIncrementalEarnings],
					p.[ActualPromoNetIncrementalEarnings],
					p.[ActualPromoROIPercent],
					p.[ActualPromoNetROIPercent],
					p.[SumInvoice],
					p.[ActualAddTIMarketing],
					p.[PlanAddTIMarketingApproved],
					p.[ActualAddTIShopper],
					p.[PlanAddTIShopperApproved],
					p.[PlanAddTIShopperCalculated],
					p.[PlanPromoIncrementalMACLSV],
					p.[PlanPromoNetIncrementalMACLSV],
					p.[ActualPromoIncrementalMACLSV],
					p.[ActualPromoNetIncrementalMACLSV],
					p.[PlanPromoIncrementalEarningsLSV],
					p.[PlanPromoNetIncrementalEarningsLSV],
					p.[ActualPromoIncrementalEarningsLSV],
					p.[ActualPromoNetIncrementalEarningsLSV],
					p.[PlanPromoROIPercentLSV],
					p.[PlanPromoNetROIPercentLSV],
					p.[ActualPromoROIPercentLSV],
					p.[ActualPromoNetROIPercentLSV],
					p.[PlanPromoIncrementalCOGSTn],
					p.[PlanPromoNetIncrementalCOGSTn],
					p.[ActualPromoIncrementalCOGSTn],
					p.[ActualPromoNetIncrementalCOGSTn],
					p.[PlanPromoBaselineVolume],
					p.[PlanPromoIncrementalVolume],
					p.[PlanPromoNetIncrementalVolume],
					p.[ActualPromoVolume],
					p.[ActualPromoIncrementalVolume],
					p.[ActualPromoNetIncrementalVolume],
					p.[Disabled],
					ct.[Name]						AS [ClientName],
					e.[Name]						AS [EventName],
					ps.[Name]						AS [PromoStatusName],
					inM.[Name]						AS [PlanInstoreMechanicName],
					inMT.[Name]						AS [PlanInstoreMechanicTypeName],
					ppPCP.[PlanProductPCPriceAVG]	AS [PCPrice],
					m.[Name]						AS [MarsMechanicName],
					mt.[Name]						AS [MarsMechanicTypeName],
					aM.[Name]						AS [ActualInStoreMechanicName],
					aMT.[Name]						AS [ActualInStoreMechanicTypeName],
					pt.[Name]						AS [PromoTypesName],
				
					TRIM((SELECT TOP(1) 
						[value] 
					FROM STRING_SPLIT(p.[ClientHierarchy]
					, '>')))									    AS [Client1LevelName],

					TRIM((SELECT TOP(1) 
						T.*  
					FROM (
						SELECT TOP(2) 
							[value] 
						FROM STRING_SPLIT(p.[ClientHierarchy], '>')
						) AS T
					ORDER BY T.[value] ASC))						AS [Client2LevelName],

					CASE WHEN b.[Name] IS NULL OR b.[Name] = ''
						THEN btB.[Name]
						ELSE b.[Name]
					END												AS [BrandName],
					CASE WHEN t.[Name] IS NULL OR t.[Name] = ''
						THEN btT.[Name]
						ELSE t.[Name]
					END												AS [TechnologyName],
					CASE WHEN t.[SubBrand] IS NULL OR t.[SubBrand] = ''
						THEN btT.[SubBrand]
						ELSE t.[SubBrand]
					END												AS [SubName],

					CASE WHEN p.[Id] IN (SELECT * FROM PreviousYearsPromoId)
						THEN p.[ActualTIBasePercent]
						ELSE p.[PlanTIBasePercent]
					END									AS [TIBasePercent],

					CASE WHEN p.[Id] IN (SELECT * FROM PreviousYearsPromoId)
						THEN p.[ActualCOGSPercent]
						ELSE p.[PlanCOGSPercent]
					END									AS [COGSPercent],

					CASE WHEN p.[Id] IN (SELECT * FROM PreviousYearsPromoId)
						THEN p.[ActualCOGSTn]
						ELSE p.[PlanCOGSTn]
					END									AS [COGSTn]
				FROM [DefaultSchemaSetting].[Promo] p
				INNER JOIN [DefaultSchemaSetting].[ClientTree] ct 
					ON ct.[Id] = p.[ClientTreeKeyId]
				INNER JOIN [DefaultSchemaSetting].[PromoStatus] ps
					ON ps.[Id] = p.[PromoStatusId]
				INNER JOIN [DefaultSchemaSetting].[PromoTypes] pt
					ON pt.[Id] = p.[PromoTypesId]

				LEFT JOIN [DefaultSchemaSetting].[Brand] b
					ON b.[Id] = p.[BrandId]
				LEFT JOIN [DefaultSchemaSetting].[Technology] t
					ON t.[Id] = p.[TechnologyId]

				LEFT JOIN [DefaultSchemaSetting].[BrandTech] bt
					ON bt.[Id] = p.[BrandTechId]
				LEFT JOIN [DefaultSchemaSetting].[Brand] btB
					ON btB.[Id] = bt.[BrandId]
				LEFT JOIN [DefaultSchemaSetting].[Technology] btT
					ON btT.[Id] = bt.[TechnologyId]

				LEFT JOIN [DefaultSchemaSetting].[Event] e
					ON e.[Id] = p.[EventId]
		
				LEFT JOIN [DefaultSchemaSetting].[Mechanic] inM
					ON inM.[Id] = p.[PlanInstoreMechanicId]
				LEFT JOIN [DefaultSchemaSetting].[MechanicType] inMT
					ON inMT.[Id] = p.[PlanInstoreMechanicTypeId]
		
				LEFT JOIN [DefaultSchemaSetting].[Mechanic] m
					ON m.[Id] = p.[MarsMechanicId]
				LEFT JOIN [DefaultSchemaSetting].[MechanicType] mt
					ON mt.[Id] = p.[MarsMechanicTypeId]
		
				LEFT JOIN [DefaultSchemaSetting].[Mechanic] aM
					ON aM.[Id] = p.[ActualInStoreMechanicId]
				LEFT JOIN [DefaultSchemaSetting].[MechanicType] aMT
					ON aMT.[Id] = p.[ActualInStoreMechanicTypeId]

				LEFT JOIN (
					SELECT 
						[PromoId],
						AVG([PlanProductPCPrice]) AS PlanProductPCPriceAVG
					FROM [DefaultSchemaSetting].[PromoProduct]
					WHERE [Disabled] = 0 AND [PlanProductPCPrice] > 0
					GROUP BY [PromoId]
				) AS ppPCP
					ON ppPCP.[PromoId] = p.[Id]
			)

			SELECT * FROM PromoROIReport
            
		GO
		";

		public static string UpdatePromoProductCorrectionViewString(string defaultSchema)
		{
			return UpdatePromoProductCorrectionViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);
		}
		private static string UpdatePromoProductCorrectionViewSqlString = @"
		ALTER VIEW [DefaultSchemaSetting].[PromoProductCorrectionView]
			AS
			SELECT
				ppc.Id AS Id,
				pr.Number AS Number,
				pr.ClientHierarchy AS ClientHierarchy,
				btech.BrandsegTechsub AS BrandTechName,
				pr.ProductSubrangesList AS ProductSubrangesList,
				mech.Name AS MarsMechanicName,
				ev.Name AS EventName,
				ps.SystemName AS PromoStatusSystemName,
				pr.MarsStartDate AS MarsStartDate,
				pr.MarsEndDate AS MarsEndDate,
				pp.PlanProductBaselineLSV AS PlanProductBaselineLSV,
				pp.PlanProductIncrementalLSV AS PlanProductIncrementalLSV,
				pp.PlanProductLSV AS PlanProductLSV,
				pp.ZREP AS ZREP,
				ppc.PlanProductUpliftPercentCorrected AS PlanProductUpliftPercentCorrected,
				ppc.CreateDate AS CreateDate,
				ppc.ChangeDate AS ChangeDate,
				ppc.UserName AS UserName,
				ppc.Disabled AS Disabled,
				pr.ClientTreeId AS ClientTreeId,
				ppc.PromoProductId AS PromoProductId,
				ppc.UserId as UserId,				
				ppc.TPMmode AS TPMmode,
				ppc.TempId AS TempId,
				ps.Name AS PromoStatusName,
				pr.IsGrowthAcceleration AS IsGrowthAcceleration,
				pr.IsInExchange AS IsInExchange

			FROM 
				[DefaultSchemaSetting].PromoProductsCorrection AS ppc INNER JOIN
                [DefaultSchemaSetting].PromoProduct AS pp ON ppc.PromoProductId = pp.Id INNER JOIN
                [DefaultSchemaSetting].Promo AS pr ON pp.PromoId = pr.Id INNER JOIN
                [DefaultSchemaSetting].PromoStatus AS ps ON pr.PromoStatusId = ps.Id INNER JOIN
                [DefaultSchemaSetting].Event AS ev ON pr.EventId = ev.Id INNER JOIN
                [DefaultSchemaSetting].Mechanic AS mech ON pr.MarsMechanicId = mech.Id INNER JOIN
                [DefaultSchemaSetting].BrandTech AS btech ON pr.BrandTechId = btech.Id INNER JOIN
                [DefaultSchemaSetting].ClientTree AS cltr ON pr.ClientTreeKeyId = cltr.Id
		
		GO
		";

	}
}
