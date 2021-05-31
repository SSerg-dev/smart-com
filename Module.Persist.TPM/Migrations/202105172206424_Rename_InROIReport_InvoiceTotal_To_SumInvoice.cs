namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Rename_InROIReport_InvoiceTotal_To_SumInvoice : DbMigration
    {
        public override void Up()
        {
            Sql(sqlstring);
        }
        
        public override void Down()
        {
        }

		private string sqlstring =
			@"
			CREATE OR ALTER VIEW [Jupiter].[PromoROIReportView] AS
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
					FROM [Jupiter].[Setting] s
					WHERE s.[Name] = 'ACTUAL_COGSTI_CHECK_PROMO_STATUS_LIST'
				), 'Finished,Closed'), ',')  
			),
				PreviousYearsPromoId
			AS
			(
				SELECT
					p.[Id]
				FROM [Jupiter].[Promo] p
				INNER JOIN [Jupiter].[PromoStatus] ps 
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
					END									AS [COGSPercent]
				FROM [Jupiter].[Promo] p
				INNER JOIN [Jupiter].[ClientTree] ct 
					ON ct.[Id] = p.[ClientTreeKeyId]
				INNER JOIN [Jupiter].[PromoStatus] ps
					ON ps.[Id] = p.[PromoStatusId]
				INNER JOIN [Jupiter].[PromoTypes] pt
					ON pt.[Id] = p.[PromoTypesId]

				LEFT JOIN [Jupiter].[Brand] b
					ON b.[Id] = p.[BrandId]
				LEFT JOIN [Jupiter].[Technology] t
					ON t.[Id] = p.[TechnologyId]

				LEFT JOIN [Jupiter].[BrandTech] bt
					ON bt.[Id] = p.[BrandTechId]
				LEFT JOIN [Jupiter].[Brand] btB
					ON btB.[Id] = bt.[BrandId]
				LEFT JOIN [Jupiter].[Technology] btT
					ON btT.[Id] = bt.[TechnologyId]

				LEFT JOIN [Jupiter].[Event] e
					ON e.[Id] = p.[EventId]
		
				LEFT JOIN [Jupiter].[Mechanic] inM
					ON inM.[Id] = p.[PlanInstoreMechanicId]
				LEFT JOIN [Jupiter].[MechanicType] inMT
					ON inMT.[Id] = p.[PlanInstoreMechanicTypeId]
		
				LEFT JOIN [Jupiter].[Mechanic] m
					ON m.[Id] = p.[MarsMechanicId]
				LEFT JOIN [Jupiter].[MechanicType] mt
					ON mt.[Id] = p.[MarsMechanicTypeId]
		
				LEFT JOIN [Jupiter].[Mechanic] aM
					ON aM.[Id] = p.[ActualInStoreMechanicId]
				LEFT JOIN [Jupiter].[MechanicType] aMT
					ON aMT.[Id] = p.[ActualInStoreMechanicTypeId]

				LEFT JOIN (
					SELECT 
						[PromoId],
						AVG([PlanProductPCPrice]) AS PlanProductPCPriceAVG
					FROM [Jupiter].[PromoProduct]
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
