namespace Module.Persist.TPM.Migrations.Views
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
                              pr.ActualPromoBaselineLSV, pr.ActualPromoIncrementalLSV, pr.SumInvoice, pr.IsOnInvoice, pr.IsInExchange, pr.TPMmode, pr.IsPriceIncrease
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
			CREATE OR ALTER VIEW [DefaultSchemaSetting].[PromoProductCorrectionView]
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
					p.[IsApolloExport],
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

		public static string UpdatePromoPriceIncreaseROIReportViewString(string defaultSchema)
		{
			return UpdatePromoPriceIncreaseROIReportViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);
		}
		private static string UpdatePromoPriceIncreaseROIReportViewSqlString = @"
			DROP VIEW [DefaultSchemaSetting].[PromoPriceIncreaseROIReportView]
			GO

			CREATE   VIEW [DefaultSchemaSetting].[PromoPriceIncreaseROIReportView] AS
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
					ppi.[PlanPromoBaselineLSV],
					ppi.[PlanPromoIncrementalLSV],
					ppi.[PlanPromoLSV],
					ppi.[PlanPromoUpliftPercent],
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
					ppi.[PlanPromoPostPromoEffectLSV],
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
					p.[IsPriceIncrease],
					p.[IsApolloExport],
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
				LEFT JOIN [DefaultSchemaSetting].[PromoPriceIncrease] ppi
					ON p.Id = ppi.Id
				LEFT JOIN (
					SELECT 
						[PromoId],
						AVG(pppi.[PlanProductPCPrice]) AS PlanProductPCPriceAVG
					FROM [DefaultSchemaSetting].[PromoProductPriceIncrease] pppi
					JOIN [DefaultSchemaSetting].[PromoProduct] pp ON pp.Id = pppi.Id
					WHERE pppi.[Disabled] = 0 AND pppi.[PlanProductPCPrice] > 0
					GROUP BY [PromoId]
				) AS ppPCP
					ON ppPCP.[PromoId] = p.[Id]
			)

			SELECT * FROM PromoROIReport WHERE IsPriceIncrease = 1
			UNION
				
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
					p.[IsPriceIncrease],
					p.[IsApolloExport],
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

			WHERE IsPriceIncrease = 0
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
				pr.IsInExchange AS IsInExchange,
				pr.DispatchesStart AS PromoDispatchStartDate,
				ROW_NUMBER() OVER(PARTITION BY pr.Number, pp.ZREP ORDER BY ppc.TPMmode DESC) AS row_number

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

		public static string UpdatePromoProductCorrectionPriceIncreaseViewString(string defaultSchema)
		{
			return UpdatePromoProductCorrectionPriceIncreaseViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);
		}
		private static string UpdatePromoProductCorrectionPriceIncreaseViewSqlString = @"
			DROP VIEW [DefaultSchemaSetting].[PromoProductCorrectionPriceIncreaseView]
			GO
				CREATE VIEW [DefaultSchemaSetting].[PromoProductCorrectionPriceIncreaseView]
			AS
			SELECT
				ppcpi.Id AS Id,
				pr.Number AS Number,
				pr.ClientHierarchy AS ClientHierarchy,
				btech.BrandsegTechsub AS BrandTechName,
				pr.ProductSubrangesList AS ProductSubrangesList,
				mech.Name AS MarsMechanicName,
				ev.Name AS EventName,
				ps.SystemName AS PromoStatusSystemName,
				pr.MarsStartDate AS MarsStartDate,
				pr.MarsEndDate AS MarsEndDate,
				pppi.PlanProductBaselineLSV AS PlanProductBaselineLSV,
				pppi.PlanProductIncrementalLSV AS PlanProductIncrementalLSV,
				pppi.PlanProductLSV AS PlanProductLSV,
				pppi.ZREP AS ZREP,
				ppcpi.PlanProductUpliftPercentCorrected AS PlanProductUpliftPercentCorrected,
				ppcpi.CreateDate AS CreateDate,
				ppcpi.ChangeDate AS ChangeDate,
				ppcpi.UserName AS UserName,
				ppcpi.Disabled AS Disabled,
				pr.ClientTreeId AS ClientTreeId,
				pp.Id AS PromoProductId,
				ppcpi.UserId as UserId,
				ppcpi.TempId AS TempId,
				ps.Name AS PromoStatusName,
				pr.IsGrowthAcceleration AS IsGrowthAcceleration,
				pr.IsInExchange AS IsInExchange,
				pr.DispatchesStart AS PromoDispatchStartDate,
				pr.IsPriceIncrease AS IsPriceIncrease

			FROM 
				[DefaultSchemaSetting].PromoProductCorrectionPriceIncrease AS ppcpi INNER JOIN
                [DefaultSchemaSetting].PromoProductPriceIncrease AS pppi ON ppcpi.PromoProductPriceIncreaseId = pppi.Id INNER JOIN
				[DefaultSchemaSetting].PromoProduct AS pp ON pp.Id = pppi.PromoProductId INNER JOIN
                [DefaultSchemaSetting].PromoPriceIncrease AS prpi ON pppi.PromoPriceIncreaseId = prpi.Id INNER JOIN
				[DefaultSchemaSetting].Promo AS pr ON pr.Id = prpi.Id INNER JOIN
                [DefaultSchemaSetting].PromoStatus AS ps ON pr.PromoStatusId = ps.Id INNER JOIN
                [DefaultSchemaSetting].Event AS ev ON pr.EventId = ev.Id INNER JOIN
                [DefaultSchemaSetting].Mechanic AS mech ON pr.MarsMechanicId = mech.Id INNER JOIN
                [DefaultSchemaSetting].BrandTech AS btech ON pr.BrandTechId = btech.Id INNER JOIN
                [DefaultSchemaSetting].ClientTree AS cltr ON pr.ClientTreeKeyId = cltr.Id
			GO
		";

		public static string UpdateClientDashboardViewString(string defaultSchema)
		{
			return UpdateClientDashboardViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);
		}
		private static string UpdateClientDashboardViewSqlString = @"
					ALTER VIEW [DefaultSchemaSetting].[ClientDashboardView] AS
			WITH
				Years
			AS
				(SELECT [BudgetYear] AS [Year]
				FROM [DefaultSchemaSetting].[Promo] (NOLOCK)
				GROUP BY [BudgetYear]),
	
				YEEF
			AS
				-- TODO: remove distinct
				(SELECT DISTINCT MIN(YEE_LSV) AS YEE_LSV, MIN(DMR_PLAN_LSV) AS PlanLSV, MIN(YTD_LSV) AS YTD_LSV, G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR 
					FROM [DefaultSchemaSetting].[YEAR_END_ESTIMATE_FDM]  (NOLOCK)
					GROUP BY G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR),
	
				Shares
			AS
				(SELECT [ClientTreeId], [BrandTechId], [Share], [ParentClientTreeDemandCode]
				FROM [DefaultSchemaSetting].[ClientTreeBrandTech]
				WHERE [DefaultSchemaSetting].[ClientTreeBrandTech].[Disabled] = 0
					AND [DefaultSchemaSetting].[ClientTreeBrandTech].[ParentClientTreeDemandCode] 
								IN (SELECT DemandCode FROM [DefaultSchemaSetting].[ClientTree] 
								WHERE [GHierarchyCode] IS NOT NULL AND [GHierarchyCode] != '')),

				CrossJoinBrandTechClientTree
			AS
				(SELECT 
					BrandTech.Id AS BTId, BrandTech.[BrandsegTechsub] AS BTName, [BrandsegTechsub_code] AS BTCode, BrandId AS BrandId,
					CT.[Name] AS CTName, CT.ObjectId AS ObjectId, CT.Id AS CTId, CT.[FullPathName] AS ClientHierarchy,
					Y.[Year],
					YEE.YEE_LSV AS YEE_LSV, YEE.PlanLSV AS PlanLSV, YEE.YTD_LSV AS YTD_LSV
				FROM [DefaultSchemaSetting].[BrandTech] (NOLOCK)
				CROSS JOIN [DefaultSchemaSetting].[ClientTree] AS CT (NOLOCK)
				CROSS JOIN Years AS Y (NOLOCK)
				LEFT JOIN YEEF AS YEE (NOLOCK) ON YEE.YEAR = Y.[Year]
					AND YEE.BRAND_SEG_TECH_CODE = [BrandsegTechsub_code]
					AND YEE.G_HIERARCHY_ID LIKE [DefaultSchemaSetting].[ClientDashboardGetParentGHierarchyCode](CT.ObjectId)
				WHERE 
					CT.EndDate IS NULL AND CT.IsBaseClient = 1
					AND [DefaultSchemaSetting].[BrandTech].[Disabled] = 0),

				PromoParams
			AS 
				(SELECT pr.ClientTreeId AS ClientTreeId, MIN(pr.ClientTreeKeyId) AS ClientTreeKeyId, MIN(pr.ClientHierarchy) AS ClientHierarchy, pr.BudgetYear AS [Year], pr.BrandTechId AS BrandTechId, pr.PromoStatusId AS Status,

					SUM(CASE WHEN pr.EndDate <= GETDATE() THEN pr.PromoDuration ELSE
					(CASE WHEN pr.StartDate < GETDATE() AND pr.EndDate > GETDATE() THEN DATEDIFF(day, pr.StartDate, GETDATE()) ELSE
					0 END) END) AS promoDays,

					/* YTD Closed */
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoLSV ELSE 0 END) AS ActualPromoLSV,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoTIShopper ELSE 0 END) AS ActualPromoTIShopper,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoTIMarketing ELSE 0 END) AS ActualPromoTIMarketing,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoCostProduction ELSE 0 END) AS ActualPromoCostProduction,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoBranding ELSE 0 END) AS ActualPromoBranding,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoBTL ELSE 0 END) AS ActualPromoBTL,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoNetIncrementalEarnings ELSE 0 END) AS ActualPromoIncrementalEarnings,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoCost ELSE 0 END) AS ActualPromoCost,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoNetIncrementalNSV ELSE 0 END) AS ActualPromoIncrementalNSV,
					SUM(CASE WHEN CS.SystemName = 'Closed' THEN pr.ActualPromoNSV ELSE 0 END) AS ActualPromoNSV,
					/* YEE Finished */
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoLSV ELSE 0 END) AS PlanPromoLSVFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoNetIncrementalEarnings ELSE 0 END) AS PlanPromoIncrementalEarningsFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoCost ELSE 0 END) AS PlanPromoCostFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoNetIncrementalNSV ELSE 0 END) AS PlanPromoIncrementalNSVFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoTIShopper ELSE 0 END) AS PlanPromoTIShopperFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoTIMarketing ELSE 0 END) AS PlanPromoTIMarketingFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoBTL ELSE 0 END) AS PlanPromoBTLFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoBranding ELSE 0 END) AS PlanPromoBrandingFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoCostProduction ELSE 0 END) AS PlanPromoCostProductionFinished,
					SUM(CASE WHEN CS.SystemName = 'Finished' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSVFinished,
					/* YEE */
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoLSV ELSE 0 END) AS PlanPromoLSV,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNetIncrementalEarnings ELSE 0 END) AS PlanPromoIncrementalEarnings,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoCost ELSE 0 END) AS PlanPromoCost,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNetIncrementalNSV ELSE 0 END) AS PlanPromoIncrementalNSV,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoTIShopper ELSE 0 END) AS PlanPromoTIShopper,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoTIMarketing ELSE 0 END) AS PlanPromoTIMarketing,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoBTL ELSE 0 END) AS PlanPromoBTL,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoBranding ELSE 0 END) AS PlanPromoBranding,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoCostProduction ELSE 0 END) AS PlanPromoCostProduction,
					SUM(CASE WHEN CS.SystemName != 'Closed' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSV
				FROM [DefaultSchemaSetting].[Promo] AS pr (NOLOCK)
				LEFT JOIN [DefaultSchemaSetting].[PromoStatus] AS CS (NOLOCK) ON CS.Id = pr.PromoStatusId
				WHERE pr.Disabled = 0 AND CS.SystemName IN ('Started', 'Approved', 'Closed', 'Finished', 'Planned') AND pr.TPMmode = 0
				GROUP BY pr.ClientTreeId, pr.BrandTechId, pr.BudgetYear, pr.PromoStatusId),
	
				GroupedPP
			AS
				(SELECT pp.ClientTreeId AS ClientTreeId, MIN(pp.ClientTreeKeyId) AS ClientTreeKeyId, MIN(pp.ClientHierarchy) AS ClientHierarchy, pp.YEAR AS [Year], pp.BrandTechId AS BrandTechId,
					SUM( promoDays) as promoDays,

					/* YTD Closed */
					SUM( ActualPromoLSV)AS ActualPromoLSV ,
					SUM( ActualPromoTIShopper)as ActualPromoTIShopper ,
					SUM( ActualPromoTIMarketing)as ActualPromoTIMarketing,
					SUM( ActualPromoCostProduction)as ActualPromoCostProduction,
					SUM( ActualPromoBranding)as ActualPromoBranding,
					SUM( ActualPromoBTL)as ActualPromoBTL,
					SUM( ActualPromoIncrementalEarnings)as ActualPromoIncrementalEarnings,
					SUM( ActualPromoCost)as ActualPromoCost,
					SUM( ActualPromoIncrementalNSV)as ActualPromoIncrementalNSV,
					SUM( ActualPromoNSV)as ActualPromoNSV,
					/* YEE Finished */
					SUM( PlanPromoLSVFinished)as PlanPromoLSVFinished,
					SUM( PlanPromoIncrementalEarningsFinished)as PlanPromoIncrementalEarningsFinished,
					SUM( PlanPromoCostFinished)as PlanPromoCostFinished,
					SUM( PlanPromoIncrementalNSVFinished)as PlanPromoIncrementalNSVFinished,
					SUM( PlanPromoTIShopperFinished)as PlanPromoTIShopperFinished,
					SUM( PlanPromoTIMarketingFinished)as PlanPromoTIMarketingFinished,
					SUM( PlanPromoBTLFinished)as PlanPromoBTLFinished,
					SUM( PlanPromoBrandingFinished)as PlanPromoBrandingFinished,
					SUM( PlanPromoCostProductionFinished)as PlanPromoCostProductionFinished,
					SUM( PlanPromoNSVFinished)as PlanPromoNSVFinished,
					/* YEE */
					SUM( PlanPromoLSV)as PlanPromoLSV,
					SUM( PlanPromoIncrementalEarnings)as PlanPromoIncrementalEarnings,
					SUM( PlanPromoCost)as PlanPromoCost,
					SUM( PlanPromoIncrementalNSV)as PlanPromoIncrementalNSV,
					SUM( PlanPromoTIShopper)as PlanPromoTIShopper,
					SUM( PlanPromoTIMarketing)as PlanPromoTIMarketing,
					SUM( PlanPromoBTL)as PlanPromoBTL,
					SUM( PlanPromoBranding)as PlanPromoBranding,
					SUM( PlanPromoCostProduction)as PlanPromoCostProduction,
					SUM( PlanPromoNSV) as PlanPromoNSV
				FROM PromoParams AS pp
				GROUP BY pp.ClientTreeId, pp.BrandTechId, pp.YEAR),

				NonPromo
			AS
				(SELECT nps.ClientTreeId AS ClientTreeKeyId,
						YEAR(nps.StartDate) AS [Year],
						SUM(CASE WHEN nps.ActualCostTE IS NOT NULL THEN nps.ActualCostTE ELSE 0 END) AS ActualCostTE,
						SUM(CASE WHEN (nps.EndDate < CAST(GETDATE() AS DATE)) AND (nps.ActualCostTE IS NULL OR nps.ActualCostTE = 0) 
								THEN nps.PlanCostTE ELSE 0 END) AS PlanCostTEYTD,
						SUM(CASE WHEN nps.ActualCostTE IS NULL OR nps.ActualCostTE = 0 THEN nps.PlanCostTE ELSE 0 END) AS PlanCostTEYEE
					FROM [DefaultSchemaSetting].[NonPromoSupport] AS nps (NOLOCK)
					WHERE nps.[Disabled] = 0
					GROUP BY nps.ClientTreeId, YEAR(nps.StartDate))

			SELECT
				NEWID() AS Id, MIN(CD.Id) AS HistoryId, MAX(BTCT.ObjectId) AS ObjectId, MIN(BTCT.ClientHierarchy) AS ClientHierarchy, MAX(BTCT.BTId) AS BrandTechId, MAX(BTCT.[Year]) AS Year,
				-- TODO: replace with join
				MIN(BTCT.BTName) AS BrandsegTechsubName, 
				-- TODO: replace with join
				(SELECT MIN([LogoFileName]) FROM [DefaultSchemaSetting].[ProductTree] WHERE [DefaultSchemaSetting].[ProductTree].BrandId = MIN(BTCT.BrandId)) AS LogoFileName,
				CASE WHEN (SUM(pp.promoDays) IS NULL) THEN 0
					ELSE ROUND(SUM(pp.promoDays)/7, 0) END AS PromoWeeks,
				CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0)
						AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished)) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished)) IS NOT NULL 
					THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) ELSE 0 END AS VodYTD,
				CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV)) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV)) IS NOT NULL 
						AND [DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
					THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) ELSE 0 END AS VodYEE,
				CASE WHEN (SUM(pp.ActualPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoLSV) END AS ActualPromoLSV,
				CASE WHEN (SUM(pp.PlanPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.PlanPromoLSV) END AS PlanPromoLSV,
				CASE WHEN (SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished) END AS ActualPromoCost,
				CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished) END AS ActualPromoIncrementalEarnings,
				CASE WHEN (SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) END AS TotalPromoCost,
				CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) IS NULL) THEN 0 ELSE
					SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) END AS TotalPromoIncrementalEarnings,
				/* Shopper TI */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ShopperTiPlanPercent) END AS ShopperTiPlanPercent,
					-- Plan% * PlanLSV / 100
				CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
					THEN 0 ELSE (MAX(CD.ShopperTiPlanPercent) * MAX(CD.PlanLSV) / 100) END AS ShopperTiPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)
				CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) END AS ShopperTiYTD,
					-- YTD / YTD with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
						AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) != 0 
						AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS ShopperTiYTDPercent,
					-- Sum of Actual (closed) + Sum of Plan (approved & planned & started & finished)
				CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) END AS ShopperTiYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) != 0 AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS ShopperTiYEEPercent,
				/* Marketing TI */ 
					-- From ClientDashboard
				CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.MarketingTiPlanPercent) END AS MarketingTiPlanPercent,
					-- Plan% * PlanLSV / 100
				CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
					THEN 0 ELSE (MAX(CD.MarketingTiPlanPercent) * MAX(CD.PlanLSV) / 100) END AS MarketingTiPlan,
					-- Promo TI cost YTD + Non promo TI cost YTD
				(CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
						THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END
					+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
						THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END) AS MarketingTiYTD,
					-- (Promo TI cost YTD + Non promo TI cost YTD) / YTD with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0)
					THEN (CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
							THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END
						+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
							THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END) / [DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) * 100
					ELSE 0 END) AS MarketingTiYTDPercent,
					-- Promo TI cost YEE + Non promo TI cost YEE
				(CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END
					+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
						THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END) AS MarketingTiYEE,
					-- (Promo TI cost YEE + Non promo TI cost YEE) / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0)
					THEN (CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
							THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END
						+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
							THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END) / [DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) * 100
					ELSE 0 END) AS MarketingTiYEEPercent,
				/* Promo TI Cost*/
					-- From ClientDashboard
				CASE WHEN (MAX(CD.PromoTiCostPlanPercent) IS NULL) 
					THEN 0 ELSE MAX(CD.PromoTiCostPlanPercent) END AS PromoTiCostPlanPercent,
					-- Plan% * PlanLSV / 100
				CASE WHEN (MAX(CD.PromoTiCostPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
					THEN 0 ELSE (MAX(CD.PromoTiCostPlanPercent) * MAX(CD.PlanLSV) / 100) END AS PromoTiCostPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)
				CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END AS PromoTiCostYTD,
					-- YTD / YTD with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
						AND (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished)) != 0 
						AND (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS PromoTiCostYTDPercent,
					-- Sum of Actual (closed) + Sum of Plan (approved & planned & started & finished)
				CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END AS PromoTiCostYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) != 0 
						AND SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS PromoTiCostYEEPercent,
				/* Non Promo TI Cost */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.NonPromoTiCostPlanPercent) IS NULL) 
					THEN 0 ELSE MAX(CD.NonPromoTiCostPlanPercent) END AS NonPromoTiCostPlanPercent,
					-- Plan% * PlanLSV / 100
				CASE WHEN (MAX(CD.NonPromoTiCostPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
					THEN 0 ELSE (MAX(CD.NonPromoTiCostPlanPercent) * MAX(CD.PlanLSV) / 100) END AS NonPromoTiCostPlan,
					-- Sum of Actual + Sum of Plan (end date < now & Actual = 0/null) (rules are observed in NonPromo select)
				CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
					THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END AS NonPromoTiCostYTD,
					-- YTD / YTD with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0  
						AND (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD)) != 0 
						AND (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD)) IS NOT NULL) 
					THEN ((MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS NonPromoTiCostYTDPercent,
					-- Sum of Actual + Sum of Plan (Actual = 0/null) (rules are observed in NonPromo select)
				CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
					THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END AS NonPromoTiCostYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) != 0 
						AND MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NOT NULL) 
					THEN ((MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS NonPromoTiCostYEEPercent,
				/* Production */
					-- Plan / PlanLSV
				CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN MAX(CD.ProductionPlan)/MAX(CD.PlanLSV) IS NULL
					THEN 0 ELSE (MAX(CD.ProductionPlan)/MAX(CD.PlanLSV)) * 100 END) END AS ProductionPlanPercent,
					-- From ClientDashboard
				CASE WHEN (MAX(CD.ProductionPlan) IS NULL) THEN 0 ELSE MAX(CD.ProductionPlan) END AS ProductionPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)		
				CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) IS NULL) THEN 0 
					ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) END AS ProductionYTD,
					-- YTD / YTD with Share * 100
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0
							AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) != 0 
							AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) * 100)
					ELSE 0 END) AS ProductionYTDPercent,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) END AS ProductionYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) != 0 
						AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS ProductionYEEPercent,
				/* Branding */
					-- Plan / PlanLSV
				CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BrandingPlan)/MAX(CD.PlanLSV)) IS NULL 
					THEN 0 ELSE (MAX(CD.BrandingPlan)/MAX(CD.PlanLSV) * 100) END) END AS BrandingPlanPercent,
					-- From ClientDashboard
				CASE WHEN (MAX(CD.BrandingPlan) IS NULL) THEN 0 ELSE MAX(CD.BrandingPlan) END AS BrandingPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)	
				CASE WHEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NULL) 
					THEN 0 ELSE (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) END AS BrandingYTD,
					-- YTD / YTD with Share * 100
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0
						AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) != 0 
						AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS BrandingYTDPercent,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) END AS BrandingYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) != 0 AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS BrandingYEEPercent,
				/* BTL */
					-- Plan / PlanLSV
				CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BTLPlan)/MAX(CD.PlanLSV)) IS NULL 
					THEN 0 ELSE (MAX(CD.BTLPlan)/MAX(CD.PlanLSV) * 100) END) END AS BTLPlanPercent,
					-- From ClientDashboard
				CASE WHEN (MAX(CD.BTLPlan) IS NULL) THEN 0 ELSE MAX(CD.BTLPlan) END AS BTLPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)	
				CASE WHEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NULL) 
					THEN 0 ELSE (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) END AS BTLYTD,
					-- YTD / YTD with Share * 100
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
						AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) != 0
						AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS BTLYTDPercent,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) END AS BTLYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) != 0 
						AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) * 100)
					ELSE 0 END) AS BTLYEEPercent,
				/* ROI */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.ROIPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ROIPlanPercent) END AS ROIPlanPercent,
					-- ([Sum of Actual Net Incremental Earnings (closed) + Sum of Plan Net Incremental Earnings (finished)] 
					--	/ [Sum of Actual Promo Cost (closed) + Sum of Plan Promo Cost (finished)] + 1) * 100
				CASE WHEN ((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)) IS NULL) 
						OR ((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)) = 0) 
						OR ((((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished))
							/((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)))+1)*100) IS NULL) 
					THEN 0 ELSE
						(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished))
						/((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)))+1)*100)
					END AS ROIYTDPercent,
					-- ([Sum of Actual Net Incremental Earnings (closed) + Sum of Plan Net Incremental Earnings (approved, planned, started � finished)] 
					--	/ [Sum of Actual Promo Cost (closed) + Sum of Plan Promo Cost (approved, planned, started � finished)] +1 ) * 100
				CASE WHEN ((SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)) IS NULL) 
						OR ((SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)) = 0) 
						OR ((((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
							/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)))+1)*100) IS NULL 
					THEN 0 ELSE
					(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
						/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost))+1)*100) 
					END AS ROIYEEPercent,
				/* LSV */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.PlanLSV) IS NULL) THEN 0 ELSE MAX(CD.PlanLSV) END AS LSVPlan,
					-- YTD with Share
				[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) AS LSVYTD,
					-- YEE with Share
				[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) AS LSVYEE,
				/* Incremental NSV */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.IncrementalNSVPlan) IS NULL) THEN 0 ELSE MAX(CD.IncrementalNSVPlan) END AS IncrementalNSVPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)	
				CASE WHEN ((SUM(pp.ActualPromoIncrementalNSV) + SUM(pp.PlanPromoIncrementalNSVFinished)) IS NULL) THEN 0
					ELSE (SUM(pp.ActualPromoIncrementalNSV) + SUM(pp.PlanPromoIncrementalNSVFinished)) END AS IncrementalNSVYTD,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) END AS IncrementalNSVYEE,
				/* Promo NSV */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.PromoNSVPlan) IS NULL) THEN 0 ELSE MAX(CD.PromoNSVPlan) END AS PromoNSVPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)	
				CASE WHEN ((SUM(pp.ActualPromoNSV) + SUM(pp.PlanPromoNSVFinished)) IS NULL) 
					THEN 0 ELSE (SUM(pp.ActualPromoNSV) + SUM(pp.PlanPromoNSVFinished)) END AS PromoNSVYTD,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) END AS PromoNSVYEE		
			FROM CrossJoinBrandTechClientTree AS BTCT (NOLOCK)
			LEFT JOIN PromoParams AS pp (NOLOCK) ON BTCT.BTId = pp.BrandTechId AND BTCT.ObjectId = pp.ClientTreeId AND BTCT.YEAR = pp.YEAR
			LEFT JOIN [DefaultSchemaSetting].[ClientDashboard] AS CD (NOLOCK) ON CD.ClientTreeId = BTCT.ObjectId AND CD.BrandTechId = BTCT.BTId AND CD.Year = BTCT.[Year]
			LEFT JOIN NonPromo AS np (NOLOCK) ON BTCT.CTId = np.ClientTreeKeyId AND BTCT.[Year] = np.Year
			LEFT JOIN Shares AS SHARES (NOLOCK) ON BTCT.CTId = SHARES.ClientTreeId AND 
				SHARES.[BrandTechId] = BTCT.BTId
			GROUP BY BTCT.ObjectId, BTCT.BTId, BTCT.[Year]
		GO
		";

		public static string UpdateClientDashboardRSViewString(string defaultSchema)
		{
			return UpdateClientDashboardRSViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);
		}
		private static string UpdateClientDashboardRSViewSqlString = @"
					CREATE VIEW [DefaultSchemaSetting].[ClientDashboardRSView] AS
			WITH
				Years
			AS
				(SELECT [BudgetYear] AS [Year]
				FROM [DefaultSchemaSetting].[Promo] (NOLOCK)
				GROUP BY [BudgetYear]),
	
				YEEF
			AS
				-- TODO: remove distinct
				(SELECT DISTINCT MIN(YEE_LSV) AS YEE_LSV, MIN(DMR_PLAN_LSV) AS PlanLSV, MIN(YTD_LSV) AS YTD_LSV, G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR 
					FROM [DefaultSchemaSetting].[YEAR_END_ESTIMATE_FDM]  (NOLOCK)
					GROUP BY G_HIERARCHY_ID, BRAND_SEG_TECH_CODE, YEAR),
	
				Shares
			AS
				(SELECT [ClientTreeId], [BrandTechId], [Share], [ParentClientTreeDemandCode]
				FROM [DefaultSchemaSetting].[ClientTreeBrandTech]
				WHERE [DefaultSchemaSetting].[ClientTreeBrandTech].[Disabled] = 0
					AND [DefaultSchemaSetting].[ClientTreeBrandTech].[ParentClientTreeDemandCode] 
								IN (SELECT DemandCode FROM [DefaultSchemaSetting].[ClientTree] 
								WHERE [GHierarchyCode] IS NOT NULL AND [GHierarchyCode] != '')),

				CrossJoinBrandTechClientTree
			AS
				(SELECT 
					BrandTech.Id AS BTId, BrandTech.[BrandsegTechsub] AS BTName, [BrandsegTechsub_code] AS BTCode, BrandId AS BrandId,
					CT.[Name] AS CTName, CT.ObjectId AS ObjectId, CT.Id AS CTId, CT.[FullPathName] AS ClientHierarchy,
					Y.[Year],
					YEE.YEE_LSV AS YEE_LSV, YEE.PlanLSV AS PlanLSV, YEE.YTD_LSV AS YTD_LSV
				FROM [DefaultSchemaSetting].[BrandTech] (NOLOCK)
				CROSS JOIN [DefaultSchemaSetting].[ClientTree] AS CT (NOLOCK)
				CROSS JOIN Years AS Y (NOLOCK)
				LEFT JOIN YEEF AS YEE (NOLOCK) ON YEE.YEAR = Y.[Year]
					AND YEE.BRAND_SEG_TECH_CODE = [BrandsegTechsub_code]
					AND YEE.G_HIERARCHY_ID LIKE [DefaultSchemaSetting].[ClientDashboardGetParentGHierarchyCode](CT.ObjectId)
				WHERE 
					CT.EndDate IS NULL AND CT.IsBaseClient = 1
					AND [DefaultSchemaSetting].[BrandTech].[Disabled] = 0),

				PromoFilter
			AS
				(SELECT * FROM ( SELECT
				pr.ClientTreeId,
				pr.ClientTreeKeyId,
				pr.ClientHierarchy,
				pr.BudgetYear,
				pr.BrandTechId,
				pr.PromoStatusId,
				pr.EndDate,
				pr.PromoDuration,
				pr.StartDate,
				pr.ActualPromoLSV,
				pr.ActualPromoTIShopper, 
				pr.ActualPromoTIMarketing,
				pr.ActualPromoCostProduction,
				pr.ActualPromoBranding,
				pr.ActualPromoBTL,
				pr.ActualPromoNetIncrementalEarnings,
				pr.ActualPromoCost,
				pr.ActualPromoNetIncrementalNSV,
				pr.ActualPromoNSV,
				pr.PlanPromoLSV,
				pr.PlanPromoNetIncrementalEarnings,
				pr.PlanPromoCost,
				pr.PlanPromoNetIncrementalNSV,
				pr.PlanPromoTIShopper, 
				pr.PlanPromoTIMarketing,
				pr.PlanPromoBTL,
				pr.PlanPromoBranding,
				pr.PlanPromoCostProduction,
				pr.PlanPromoNSV,
				CS.SystemName,
				ROW_NUMBER() OVER(PARTITION BY pr.Number ORDER BY TPMmode DESC) AS row_number

				FROM [DefaultSchemaSetting].[Promo] AS pr (NOLOCK)
				LEFT JOIN [DefaultSchemaSetting].[PromoStatus] AS CS (NOLOCK) ON CS.Id = pr.PromoStatusId

				WHERE pr.Disabled = 0 AND CS.SystemName IN ('Started', 'Approved', 'Closed', 'Finished', 'Planned')
				) query
				WHERE row_number = 1),

				PromoParams
			AS 
				(SELECT pr.ClientTreeId AS ClientTreeId, MIN(pr.ClientTreeKeyId) AS ClientTreeKeyId, MIN(pr.ClientHierarchy) AS ClientHierarchy, pr.BudgetYear AS [Year], pr.BrandTechId AS BrandTechId, pr.PromoStatusId AS Status,

					SUM(CASE WHEN pr.EndDate <= GETDATE() THEN pr.PromoDuration ELSE
					(CASE WHEN pr.StartDate < GETDATE() AND pr.EndDate > GETDATE() THEN DATEDIFF(day, pr.StartDate, GETDATE()) ELSE
					0 END) END) AS promoDays,

					/* YTD Closed */
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoLSV ELSE 0 END) AS ActualPromoLSV,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoTIShopper ELSE 0 END) AS ActualPromoTIShopper,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoTIMarketing ELSE 0 END) AS ActualPromoTIMarketing,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoCostProduction ELSE 0 END) AS ActualPromoCostProduction,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoBranding ELSE 0 END) AS ActualPromoBranding,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoBTL ELSE 0 END) AS ActualPromoBTL,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoNetIncrementalEarnings ELSE 0 END) AS ActualPromoIncrementalEarnings,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoCost ELSE 0 END) AS ActualPromoCost,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoNetIncrementalNSV ELSE 0 END) AS ActualPromoIncrementalNSV,
					SUM(CASE WHEN pr.SystemName = 'Closed' THEN pr.ActualPromoNSV ELSE 0 END) AS ActualPromoNSV,
					/* YEE Finished */
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoLSV ELSE 0 END) AS PlanPromoLSVFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoNetIncrementalEarnings ELSE 0 END) AS PlanPromoIncrementalEarningsFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoCost ELSE 0 END) AS PlanPromoCostFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoNetIncrementalNSV ELSE 0 END) AS PlanPromoIncrementalNSVFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoTIShopper ELSE 0 END) AS PlanPromoTIShopperFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoTIMarketing ELSE 0 END) AS PlanPromoTIMarketingFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoBTL ELSE 0 END) AS PlanPromoBTLFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoBranding ELSE 0 END) AS PlanPromoBrandingFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoCostProduction ELSE 0 END) AS PlanPromoCostProductionFinished,
					SUM(CASE WHEN pr.SystemName = 'Finished' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSVFinished,
					/* YEE */
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoLSV ELSE 0 END) AS PlanPromoLSV,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoNetIncrementalEarnings ELSE 0 END) AS PlanPromoIncrementalEarnings,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoCost ELSE 0 END) AS PlanPromoCost,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoNetIncrementalNSV ELSE 0 END) AS PlanPromoIncrementalNSV,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoTIShopper ELSE 0 END) AS PlanPromoTIShopper,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoTIMarketing ELSE 0 END) AS PlanPromoTIMarketing,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoBTL ELSE 0 END) AS PlanPromoBTL,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoBranding ELSE 0 END) AS PlanPromoBranding,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoCostProduction ELSE 0 END) AS PlanPromoCostProduction,
					SUM(CASE WHEN pr.SystemName != 'Closed' THEN pr.PlanPromoNSV ELSE 0 END) AS PlanPromoNSV
				FROM PromoFilter AS pr (NOLOCK)
				GROUP BY pr.ClientTreeId, pr.BrandTechId, pr.BudgetYear, pr.PromoStatusId),
	
				GroupedPP
			AS
				(SELECT pp.ClientTreeId AS ClientTreeId, MIN(pp.ClientTreeKeyId) AS ClientTreeKeyId, MIN(pp.ClientHierarchy) AS ClientHierarchy, pp.YEAR AS [Year], pp.BrandTechId AS BrandTechId,
					SUM( promoDays) as promoDays,

					/* YTD Closed */
					SUM( ActualPromoLSV)AS ActualPromoLSV ,
					SUM( ActualPromoTIShopper)as ActualPromoTIShopper ,
					SUM( ActualPromoTIMarketing)as ActualPromoTIMarketing,
					SUM( ActualPromoCostProduction)as ActualPromoCostProduction,
					SUM( ActualPromoBranding)as ActualPromoBranding,
					SUM( ActualPromoBTL)as ActualPromoBTL,
					SUM( ActualPromoIncrementalEarnings)as ActualPromoIncrementalEarnings,
					SUM( ActualPromoCost)as ActualPromoCost,
					SUM( ActualPromoIncrementalNSV)as ActualPromoIncrementalNSV,
					SUM( ActualPromoNSV)as ActualPromoNSV,
					/* YEE Finished */
					SUM( PlanPromoLSVFinished)as PlanPromoLSVFinished,
					SUM( PlanPromoIncrementalEarningsFinished)as PlanPromoIncrementalEarningsFinished,
					SUM( PlanPromoCostFinished)as PlanPromoCostFinished,
					SUM( PlanPromoIncrementalNSVFinished)as PlanPromoIncrementalNSVFinished,
					SUM( PlanPromoTIShopperFinished)as PlanPromoTIShopperFinished,
					SUM( PlanPromoTIMarketingFinished)as PlanPromoTIMarketingFinished,
					SUM( PlanPromoBTLFinished)as PlanPromoBTLFinished,
					SUM( PlanPromoBrandingFinished)as PlanPromoBrandingFinished,
					SUM( PlanPromoCostProductionFinished)as PlanPromoCostProductionFinished,
					SUM( PlanPromoNSVFinished)as PlanPromoNSVFinished,
					/* YEE */
					SUM( PlanPromoLSV)as PlanPromoLSV,
					SUM( PlanPromoIncrementalEarnings)as PlanPromoIncrementalEarnings,
					SUM( PlanPromoCost)as PlanPromoCost,
					SUM( PlanPromoIncrementalNSV)as PlanPromoIncrementalNSV,
					SUM( PlanPromoTIShopper)as PlanPromoTIShopper,
					SUM( PlanPromoTIMarketing)as PlanPromoTIMarketing,
					SUM( PlanPromoBTL)as PlanPromoBTL,
					SUM( PlanPromoBranding)as PlanPromoBranding,
					SUM( PlanPromoCostProduction)as PlanPromoCostProduction,
					SUM( PlanPromoNSV) as PlanPromoNSV
				FROM PromoParams AS pp
				GROUP BY pp.ClientTreeId, pp.BrandTechId, pp.YEAR),

				NonPromo
			AS
				(SELECT nps.ClientTreeId AS ClientTreeKeyId,
						YEAR(nps.StartDate) AS [Year],
						SUM(CASE WHEN nps.ActualCostTE IS NOT NULL THEN nps.ActualCostTE ELSE 0 END) AS ActualCostTE,
						SUM(CASE WHEN (nps.EndDate < CAST(GETDATE() AS DATE)) AND (nps.ActualCostTE IS NULL OR nps.ActualCostTE = 0) 
								THEN nps.PlanCostTE ELSE 0 END) AS PlanCostTEYTD,
						SUM(CASE WHEN nps.ActualCostTE IS NULL OR nps.ActualCostTE = 0 THEN nps.PlanCostTE ELSE 0 END) AS PlanCostTEYEE
					FROM [DefaultSchemaSetting].[NonPromoSupport] AS nps (NOLOCK)
					WHERE nps.[Disabled] = 0
					GROUP BY nps.ClientTreeId, YEAR(nps.StartDate))

			SELECT
				NEWID() AS Id, MIN(CD.Id) AS HistoryId, MAX(BTCT.ObjectId) AS ObjectId, MIN(BTCT.ClientHierarchy) AS ClientHierarchy, MAX(BTCT.BTId) AS BrandTechId, MAX(BTCT.[Year]) AS Year,
				-- TODO: replace with join
				MIN(BTCT.BTName) AS BrandsegTechsubName, 
				-- TODO: replace with join
				(SELECT MIN([LogoFileName]) FROM [DefaultSchemaSetting].[ProductTree] WHERE [DefaultSchemaSetting].[ProductTree].BrandId = MIN(BTCT.BrandId)) AS LogoFileName,
				CASE WHEN (SUM(pp.promoDays) IS NULL) THEN 0
					ELSE ROUND(SUM(pp.promoDays)/7, 0) END AS PromoWeeks,
				CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0)
						AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished)) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished)) IS NOT NULL 
					THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSVFinished))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) ELSE 0 END AS VodYTD,
				CASE WHEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV)) != 0 AND (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV)) IS NOT NULL 
						AND [DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
					THEN (SUM(pp.ActualPromoLSV) + SUM(pp.PlanPromoLSV))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) ELSE 0 END AS VodYEE,
				CASE WHEN (SUM(pp.ActualPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.ActualPromoLSV) END AS ActualPromoLSV,
				CASE WHEN (SUM(pp.PlanPromoLSV) IS NULL) THEN 0 ELSE SUM(pp.PlanPromoLSV) END AS PlanPromoLSV,
				CASE WHEN (SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished) END AS ActualPromoCost,
				CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished) END AS ActualPromoIncrementalEarnings,
				CASE WHEN (SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost) END AS TotalPromoCost,
				CASE WHEN (SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) IS NULL) THEN 0 ELSE
					SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings) END AS TotalPromoIncrementalEarnings,
				/* Shopper TI */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ShopperTiPlanPercent) END AS ShopperTiPlanPercent,
					-- Plan% * PlanLSV / 100
				CASE WHEN (MAX(CD.ShopperTiPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
					THEN 0 ELSE (MAX(CD.ShopperTiPlanPercent) * MAX(CD.PlanLSV) / 100) END AS ShopperTiPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)
				CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished) END AS ShopperTiYTD,
					-- YTD / YTD with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
						AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) != 0 
						AND (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopperFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS ShopperTiYTDPercent,
					-- Sum of Actual (closed) + Sum of Plan (approved & planned & started & finished)
				CASE WHEN (SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) END AS ShopperTiYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) != 0 AND SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoTIShopper) + SUM(pp.PlanPromoTIShopper))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS ShopperTiYEEPercent,
				/* Marketing TI */ 
					-- From ClientDashboard
				CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.MarketingTiPlanPercent) END AS MarketingTiPlanPercent,
					-- Plan% * PlanLSV / 100
				CASE WHEN (MAX(CD.MarketingTiPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
					THEN 0 ELSE (MAX(CD.MarketingTiPlanPercent) * MAX(CD.PlanLSV) / 100) END AS MarketingTiPlan,
					-- Promo TI cost YTD + Non promo TI cost YTD
				(CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
						THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END
					+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
						THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END) AS MarketingTiYTD,
					-- (Promo TI cost YTD + Non promo TI cost YTD) / YTD with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0)
					THEN (CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
							THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END
						+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
							THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END) / [DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) * 100
					ELSE 0 END) AS MarketingTiYTDPercent,
					-- Promo TI cost YEE + Non promo TI cost YEE
				(CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END
					+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
						THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END) AS MarketingTiYEE,
					-- (Promo TI cost YEE + Non promo TI cost YEE) / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0)
					THEN (CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
							THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END
						+ CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
							THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END) / [DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) * 100
					ELSE 0 END) AS MarketingTiYEEPercent,
				/* Promo TI Cost*/
					-- From ClientDashboard
				CASE WHEN (MAX(CD.PromoTiCostPlanPercent) IS NULL) 
					THEN 0 ELSE MAX(CD.PromoTiCostPlanPercent) END AS PromoTiCostPlanPercent,
					-- Plan% * PlanLSV / 100
				CASE WHEN (MAX(CD.PromoTiCostPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
					THEN 0 ELSE (MAX(CD.PromoTiCostPlanPercent) * MAX(CD.PlanLSV) / 100) END AS PromoTiCostPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)
				CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished) END AS PromoTiCostYTD,
					-- YTD / YTD with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
						AND (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished)) != 0 
						AND (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketingFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS PromoTiCostYTDPercent,
					-- Sum of Actual (closed) + Sum of Plan (approved & planned & started & finished)
				CASE WHEN (SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) END AS PromoTiCostYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) != 0 
						AND SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoTIMarketing) + SUM(pp.PlanPromoTIMarketing))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS PromoTiCostYEEPercent,
				/* Non Promo TI Cost */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.NonPromoTiCostPlanPercent) IS NULL) 
					THEN 0 ELSE MAX(CD.NonPromoTiCostPlanPercent) END AS NonPromoTiCostPlanPercent,
					-- Plan% * PlanLSV / 100
				CASE WHEN (MAX(CD.NonPromoTiCostPlanPercent) IS NULL OR MAX(CD.PlanLSV) IS NULL)
					THEN 0 ELSE (MAX(CD.NonPromoTiCostPlanPercent) * MAX(CD.PlanLSV) / 100) END AS NonPromoTiCostPlan,
					-- Sum of Actual + Sum of Plan (end date < now & Actual = 0/null) (rules are observed in NonPromo select)
				CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) IS NULL) 
					THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD) END AS NonPromoTiCostYTD,
					-- YTD / YTD with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0  
						AND (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD)) != 0 
						AND (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD)) IS NOT NULL) 
					THEN ((MAX(np.ActualCostTE) + MAX(np.PlanCostTEYTD))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS NonPromoTiCostYTDPercent,
					-- Sum of Actual + Sum of Plan (Actual = 0/null) (rules are observed in NonPromo select)
				CASE WHEN (MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NULL) 
					THEN 0 ELSE MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) END AS NonPromoTiCostYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) != 0 
						AND MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE) IS NOT NULL) 
					THEN ((MAX(np.ActualCostTE) + MAX(np.PlanCostTEYEE))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS NonPromoTiCostYEEPercent,
				/* Production */
					-- Plan / PlanLSV
				CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN MAX(CD.ProductionPlan)/MAX(CD.PlanLSV) IS NULL
					THEN 0 ELSE (MAX(CD.ProductionPlan)/MAX(CD.PlanLSV)) * 100 END) END AS ProductionPlanPercent,
					-- From ClientDashboard
				CASE WHEN (MAX(CD.ProductionPlan) IS NULL) THEN 0 ELSE MAX(CD.ProductionPlan) END AS ProductionPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)		
				CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) IS NULL) THEN 0 
					ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished) END AS ProductionYTD,
					-- YTD / YTD with Share * 100
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0
							AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) != 0 
							AND (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProductionFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) * 100)
					ELSE 0 END) AS ProductionYTDPercent,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) END AS ProductionYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) != 0 
						AND SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoCostProduction) + SUM(pp.PlanPromoCostProduction))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS ProductionYEEPercent,
				/* Branding */
					-- Plan / PlanLSV
				CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BrandingPlan)/MAX(CD.PlanLSV)) IS NULL 
					THEN 0 ELSE (MAX(CD.BrandingPlan)/MAX(CD.PlanLSV) * 100) END) END AS BrandingPlanPercent,
					-- From ClientDashboard
				CASE WHEN (MAX(CD.BrandingPlan) IS NULL) THEN 0 ELSE MAX(CD.BrandingPlan) END AS BrandingPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)	
				CASE WHEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NULL) 
					THEN 0 ELSE (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) END AS BrandingYTD,
					-- YTD / YTD with Share * 100
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0
						AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) != 0 
						AND (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBrandingFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS BrandingYTDPercent,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) END AS BrandingYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) != 0 AND SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoBranding) + SUM(pp.PlanPromoBranding))/([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)))) * 100
					ELSE 0 END) AS BrandingYEEPercent,
				/* BTL */
					-- Plan / PlanLSV
				CASE WHEN (MAX(CD.PlanLSV) IS NULL OR MAX(CD.PlanLSV) = 0) THEN 0 ELSE (CASE WHEN (MAX(CD.BTLPlan)/MAX(CD.PlanLSV)) IS NULL 
					THEN 0 ELSE (MAX(CD.BTLPlan)/MAX(CD.PlanLSV) * 100) END) END AS BTLPlanPercent,
					-- From ClientDashboard
				CASE WHEN (MAX(CD.BTLPlan) IS NULL) THEN 0 ELSE MAX(CD.BTLPlan) END AS BTLPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)	
				CASE WHEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NULL) 
					THEN 0 ELSE (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) END AS BTLYTD,
					-- YTD / YTD with Share * 100
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) != 0 
						AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) != 0
						AND (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished)) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTLFinished))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share))) * 100
					ELSE 0 END) AS BTLYTDPercent,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) END AS BTLYEE,
					-- YEE / YEE with Share
				(CASE WHEN ([DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) != 0
						AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) != 0 
						AND SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL) IS NOT NULL) 
					THEN ((SUM(pp.ActualPromoBTL) + SUM(pp.PlanPromoBTL))/[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) * 100)
					ELSE 0 END) AS BTLYEEPercent,
				/* ROI */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.ROIPlanPercent) IS NULL) THEN 0 ELSE MAX(CD.ROIPlanPercent) END AS ROIPlanPercent,
					-- ([Sum of Actual Net Incremental Earnings (closed) + Sum of Plan Net Incremental Earnings (finished)] 
					--	/ [Sum of Actual Promo Cost (closed) + Sum of Plan Promo Cost (finished)] + 1) * 100
				CASE WHEN ((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)) IS NULL) 
						OR ((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)) = 0) 
						OR ((((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished))
							/((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)))+1)*100) IS NULL) 
					THEN 0 ELSE
						(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarningsFinished))
						/((SUM(pp.ActualPromoCost) + SUM(pp.PlanPromoCostFinished)))+1)*100)
					END AS ROIYTDPercent,
					-- ([Sum of Actual Net Incremental Earnings (closed) + Sum of Plan Net Incremental Earnings (approved, planned, started � finished)] 
					--	/ [Sum of Actual Promo Cost (closed) + Sum of Plan Promo Cost (approved, planned, started � finished)] +1 ) * 100
				CASE WHEN ((SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)) IS NULL) 
						OR ((SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)) = 0) 
						OR ((((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
							/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost)))+1)*100) IS NULL 
					THEN 0 ELSE
					(((SUM(pp.ActualPromoIncrementalEarnings) + SUM(pp.PlanPromoIncrementalEarnings))
						/(SUM(pp.ActualPromoCost)+SUM(pp.PlanPromoCost))+1)*100) 
					END AS ROIYEEPercent,
				/* LSV */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.PlanLSV) IS NULL) THEN 0 ELSE MAX(CD.PlanLSV) END AS LSVPlan,
					-- YTD with Share
				[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YTD_LSV), MAX(SHARES.Share)) AS LSVYTD,
					-- YEE with Share
				[DefaultSchemaSetting].[ClientDashboardGetYTDwithShare](MAX(BTCT.YEE_LSV), MAX(SHARES.Share)) AS LSVYEE,
				/* Incremental NSV */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.IncrementalNSVPlan) IS NULL) THEN 0 ELSE MAX(CD.IncrementalNSVPlan) END AS IncrementalNSVPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)	
				CASE WHEN ((SUM(pp.ActualPromoIncrementalNSV) + SUM(pp.PlanPromoIncrementalNSVFinished)) IS NULL) THEN 0
					ELSE (SUM(pp.ActualPromoIncrementalNSV) + SUM(pp.PlanPromoIncrementalNSVFinished)) END AS IncrementalNSVYTD,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoIncrementalNSV)+SUM(pp.PlanPromoIncrementalNSV) END AS IncrementalNSVYEE,
				/* Promo NSV */
					-- From ClientDashboard
				CASE WHEN (MAX(CD.PromoNSVPlan) IS NULL) THEN 0 ELSE MAX(CD.PromoNSVPlan) END AS PromoNSVPlan,
					-- Sum of Actual (closed) + Sum of Plan (finished)	
				CASE WHEN ((SUM(pp.ActualPromoNSV) + SUM(pp.PlanPromoNSVFinished)) IS NULL) 
					THEN 0 ELSE (SUM(pp.ActualPromoNSV) + SUM(pp.PlanPromoNSVFinished)) END AS PromoNSVYTD,
					-- YTD + Sum of Plan (approved & planned & started)
				CASE WHEN (SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) IS NULL) 
					THEN 0 ELSE SUM(pp.ActualPromoNSV)+SUM(pp.PlanPromoNSV) END AS PromoNSVYEE		
			FROM CrossJoinBrandTechClientTree AS BTCT (NOLOCK)
			LEFT JOIN PromoParams AS pp (NOLOCK) ON BTCT.BTId = pp.BrandTechId AND BTCT.ObjectId = pp.ClientTreeId AND BTCT.YEAR = pp.YEAR
			LEFT JOIN [DefaultSchemaSetting].[ClientDashboard] AS CD (NOLOCK) ON CD.ClientTreeId = BTCT.ObjectId AND CD.BrandTechId = BTCT.BTId AND CD.Year = BTCT.[Year]
			LEFT JOIN NonPromo AS np (NOLOCK) ON BTCT.CTId = np.ClientTreeKeyId AND BTCT.[Year] = np.Year
			LEFT JOIN Shares AS SHARES (NOLOCK) ON BTCT.CTId = SHARES.ClientTreeId AND 
				SHARES.[BrandTechId] = BTCT.BTId
			GROUP BY BTCT.ObjectId, BTCT.BTId, BTCT.[Year]
		GO
		";

		public static string UpdatePromoProductPriceIncreasesViewString(string defaultSchema)
		{
			return UpdatePromoProductPriceIncreasesViewSqlString.Replace("DefaultSchemaSetting", defaultSchema);
		}
		private static string UpdatePromoProductPriceIncreasesViewSqlString = @"
					CREATE VIEW [DefaultSchemaSetting].[PromoProductPriceIncreasesView] AS 
                    SELECT 
		                pp.[Id]
                        , pp.[ZREP]
                        , pp.[ProductEN]
                        , pp.[PlanProductBaselineLSV]
		                , PlanProductUpliftPercent = 
			                IIF((SELECT TOP(1) [Id] FROM [DefaultSchemaSetting].[PromoProductCorrectionPriceIncrease] WHERE [PromoProductPriceIncreaseId] = pp.[Id] and [Disabled] = 0 and [TempId] IS NULL) IS NOT NULL, 
			                (SELECT TOP(1) [PlanProductUpliftPercentCorrected] FROM [DefaultSchemaSetting].[PromoProductCorrectionPriceIncrease] WHERE [PromoProductPriceIncreaseId] = pp.[Id] and [Disabled] = 0 and [TempId] IS NULL ORDER BY [ChangeDate] DESC), 
			                pp.[PlanProductUpliftPercent])
                        , pp.[PlanProductIncrementalLSV]
                        , pp.[PlanProductLSV]
                        , pp.[PlanProductBaselineCaseQty]
                        , pp.[PlanProductIncrementalCaseQty]
                        , pp.[PlanProductCaseQty]
                        , pp.[AverageMarker]
                        , IsCorrection = 
			                IIF((SELECT TOP(1) [Id] FROM [DefaultSchemaSetting].[PromoProductCorrectionPriceIncrease] WHERE [Id] = pp.[Id] and [Disabled] = 0 and [TempId] IS NULL) IS NOT NULL,
			                CONVERT(bit, 1),
			                CONVERT(bit, 0))
                    FROM [DefaultSchemaSetting].[PromoProductPriceIncrease] pp
                    WHERE pp.[Disabled] = 0
			GO
		";
	}
}
