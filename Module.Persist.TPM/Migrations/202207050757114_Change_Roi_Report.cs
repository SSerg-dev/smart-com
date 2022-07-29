namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_Roi_Report : DbMigration
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
		private string SqlString =
@"
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
			GO
        ";
	}
}
