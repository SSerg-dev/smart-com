using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module.Persist.TPM.Migrations.Procedures
{
    public static class StoredProcedureMigrations
    {
        public static string UpdateSICalculation(string defaultSchema)
        {
            return SqlString.Replace("DefaultSchemaSetting", defaultSchema);
        }
        private static readonly string SqlString = @"
        ALTER   PROCEDURE [DefaultSchemaSetting].[SI_Calculation] AS
           BEGIN
               UPDATE BaseLine SET 
			        SellInBaselineQTY = t.SellInQty
		        FROM
			        (SELECT b.Id AS baseLineId, b.SellInBaseLineQTY AS SellInQty
			        FROM [DefaultSchemaSetting].[BaseLine] AS b
			        WHERE b.NeedProcessing = 1 AND b.Disabled = 0 AND YEAR(b.StartDate) <> 9999) AS t
		        WHERE Id = baseLineId
           END
        GO
        ";
        public static string UpdatePromo(string defaultSchema)
        {
            return UpdatePromoSqlString.Replace("DefaultSchemaSetting", defaultSchema);
        }
        private static readonly string UpdatePromoSqlString = @"
		ALTER   PROCEDURE [DefaultSchemaSetting].[UpdatePromo]
				AS
				BEGIN
					UPDATE [DefaultSchemaSetting].[Promo]
					SET
						[Disabled] = tp.[Disabled],
						[DeletedDate] = tp.[DeletedDate],
						[BrandId] = tp.[BrandId],
						[BrandTechId] = tp.[BrandTechId],
						[PromoStatusId] = tp.[PromoStatusId],
						[Name] = tp.[Name],
						[StartDate] = TODATETIMEOFFSET ( tp.[StartDate] , '+03:00' ),
						[EndDate] = TODATETIMEOFFSET ( tp.[EndDate] , '+03:00' ),
						[DispatchesStart] = TODATETIMEOFFSET ( tp.[DispatchesStart] , '+03:00' ),
						[DispatchesEnd] = TODATETIMEOFFSET ( tp.[DispatchesEnd] , '+03:00' ),
						[ColorId] = tp.[ColorId],
						[Number] = tp.[Number],
						[RejectReasonId] = tp.[RejectReasonId],
						[EventId] = tp.[EventId],
						[MarsMechanicId] = tp.[MarsMechanicId],
						[MarsMechanicTypeId] = tp.[MarsMechanicTypeId],
						[PlanInstoreMechanicId] = tp.[PlanInstoreMechanicId],
						[PlanInstoreMechanicTypeId] = tp.[PlanInstoreMechanicTypeId],
						[MarsMechanicDiscount] = tp.[MarsMechanicDiscount],
						[OtherEventName] = tp.[OtherEventName],
						[EventName] = tp.[EventName],
						[ClientHierarchy] = tp.[ClientHierarchy],
						[ProductHierarchy] = tp.[ProductHierarchy],
						[CreatorId] = tp.[CreatorId],
						[MarsStartDate] = tp.[MarsStartDate],
						[MarsEndDate] = tp.[MarsEndDate],
						[MarsDispatchesStart] = tp.[MarsDispatchesStart],
						[MarsDispatchesEnd] = tp.[MarsDispatchesEnd],
						[ClientTreeId] = tp.[ClientTreeId],
						[BaseClientTreeId] = tp.[BaseClientTreeId],
						[Mechanic] = tp.[Mechanic],
						[MechanicIA] = tp.[MechanicIA],
						[BaseClientTreeIds] = tp.[BaseClientTreeIds],
						[LastApprovedDate] = TODATETIMEOFFSET ( tp.[LastApprovedDate] , '+03:00' ),
						[TechnologyId] = tp.[TechnologyId],
						[NeedRecountUplift] = tp.[NeedRecountUplift],
						[IsAutomaticallyApproved] = tp.[IsAutomaticallyApproved],
						[IsCMManagerApproved] = tp.[IsCMManagerApproved],
						[IsDemandPlanningApproved] = tp.[IsDemandPlanningApproved],
						[IsDemandFinanceApproved] = tp.[IsDemandFinanceApproved],
						[PlanPromoXSites] = tp.[PlanPromoXSites],
						[PlanPromoCatalogue] = tp.[PlanPromoCatalogue],
						[PlanPromoPOSMInClient] = tp.[PlanPromoPOSMInClient],
						[PlanPromoCostProdXSites] = tp.[PlanPromoCostProdXSites],
						[PlanPromoCostProdCatalogue] = tp.[PlanPromoCostProdCatalogue],
						[PlanPromoCostProdPOSMInClient] = tp.[PlanPromoCostProdPOSMInClient],
						[ActualPromoXSites] = tp.[ActualPromoXSites],
						[ActualPromoCatalogue] = tp.[ActualPromoCatalogue],
						[ActualPromoPOSMInClient] = tp.[ActualPromoPOSMInClient],
						[ActualPromoCostProdXSites] = tp.[ActualPromoCostProdXSites],
						[ActualPromoCostProdCatalogue] = tp.[ActualPromoCostProdCatalogue],
						[ActualPromoCostProdPOSMInClient] = tp.[ActualPromoCostProdPOSMInClient],
						--[MechanicComment] = tp.[MechanicComment],
						[PlanInstoreMechanicDiscount] = tp.[PlanInstoreMechanicDiscount],
						[CalendarPriority] = tp.[CalendarPriority],
						[PlanPromoTIShopper] = tp.[PlanPromoTIShopper],
						[PlanPromoTIMarketing] = tp.[PlanPromoTIMarketing],
						[PlanPromoBranding] = tp.[PlanPromoBranding],
						[PlanPromoCost] = tp.[PlanPromoCost],
						[PlanPromoBTL] = tp.[PlanPromoBTL],
						[PlanPromoCostProduction] = tp.[PlanPromoCostProduction],
						[PlanPromoUpliftPercent] = tp.[PlanPromoUpliftPercent],
						[PlanPromoUpliftPercentPI] = tp.[PlanPromoUpliftPercentPI],
						[PlanPromoIncrementalLSV] = tp.[PlanPromoIncrementalLSV],
						[PlanPromoLSV] = tp.[PlanPromoLSV],
						[PlanPromoROIPercent] = tp.[PlanPromoROIPercent],
						[PlanPromoIncrementalNSV] = tp.[PlanPromoIncrementalNSV],
						[PlanPromoNetIncrementalNSV] = tp.[PlanPromoNetIncrementalNSV],
						[PlanPromoIncrementalMAC] = tp.[PlanPromoIncrementalMAC],
						[ActualPromoTIShopper] = tp.[ActualPromoTIShopper],
						[ActualPromoTIMarketing] = tp.[ActualPromoTIMarketing],
						[ActualPromoBranding] = tp.[ActualPromoBranding],
						[ActualPromoBTL] = tp.[ActualPromoBTL],
						[ActualPromoCostProduction] = tp.[ActualPromoCostProduction],
						[ActualPromoCost] = tp.[ActualPromoCost],
						[ActualPromoUpliftPercent] = tp.[ActualPromoUpliftPercent],
						[ActualPromoIncrementalLSV] = tp.[ActualPromoIncrementalLSV],
						[ActualPromoLSV] = tp.[ActualPromoLSV],
						[ActualPromoROIPercent] = tp.[ActualPromoROIPercent],
						[ActualPromoIncrementalNSV] = tp.[ActualPromoIncrementalNSV],
						[ActualPromoNetIncrementalNSV] = tp.[ActualPromoNetIncrementalNSV],
						[ActualPromoIncrementalMAC] = tp.[ActualPromoIncrementalMAC],
						[ActualInStoreMechanicId] = tp.[ActualInStoreMechanicId],
						[ActualInStoreMechanicTypeId] = tp.[ActualInStoreMechanicTypeId],
						[PromoDuration] = tp.[PromoDuration],
						[DispatchDuration] = tp.[DispatchDuration],
						[InvoiceNumber] = tp.[InvoiceNumber],
						[PlanPromoBaselineLSV] = tp.[PlanPromoBaselineLSV],
						[PlanPromoIncrementalBaseTI] = tp.[PlanPromoIncrementalBaseTI],
						[PlanPromoIncrementalCOGS] = tp.[PlanPromoIncrementalCOGS],
						[PlanPromoTotalCost] = tp.[PlanPromoTotalCost],
						[PlanPromoNetIncrementalLSV] = tp.[PlanPromoNetIncrementalLSV],
						[PlanPromoNetLSV] = tp.[PlanPromoNetLSV],
						[PlanPromoNetIncrementalMAC] = tp.[PlanPromoNetIncrementalMAC],
						[PlanPromoIncrementalEarnings] = tp.[PlanPromoIncrementalEarnings],
						[PlanPromoNetIncrementalEarnings] = tp.[PlanPromoNetIncrementalEarnings],
						[PlanPromoNetROIPercent] = tp.[PlanPromoNetROIPercent],
						[PlanPromoNetUpliftPercent] = tp.[PlanPromoNetUpliftPercent],
						[ActualPromoBaselineLSV] = tp.[ActualPromoBaselineLSV],
						[ActualInStoreDiscount] = tp.[ActualInStoreDiscount],
						[ActualInStoreShelfPrice] = tp.[ActualInStoreShelfPrice],
						[ActualPromoIncrementalBaseTI] = tp.[ActualPromoIncrementalBaseTI],
						[ActualPromoIncrementalCOGS] = tp.[ActualPromoIncrementalCOGS],
						[ActualPromoTotalCost] = tp.[ActualPromoTotalCost],
						[ActualPromoNetIncrementalLSV] = tp.[ActualPromoNetIncrementalLSV],
						[ActualPromoNetLSV] = tp.[ActualPromoNetLSV],
						[ActualPromoNetIncrementalMAC] = tp.[ActualPromoNetIncrementalMAC],
						[ActualPromoIncrementalEarnings] = tp.[ActualPromoIncrementalEarnings],
						[ActualPromoNetIncrementalEarnings] = tp.[ActualPromoNetIncrementalEarnings],
						[ActualPromoNetROIPercent] = tp.[ActualPromoNetROIPercent],
						[ActualPromoNetUpliftPercent] = tp.[ActualPromoNetUpliftPercent],
						[Calculating] = tp.[Calculating],
						[BlockInformation] = tp.[BlockInformation],
						[PlanPromoBaselineBaseTI] = tp.[PlanPromoBaselineBaseTI],
						[PlanPromoBaseTI] = tp.[PlanPromoBaseTI],
						[PlanPromoNetNSV] = tp.[PlanPromoNetNSV],
						[ActualPromoBaselineBaseTI] = tp.[ActualPromoBaselineBaseTI],
						[ActualPromoBaseTI] = tp.[ActualPromoBaseTI],
						[ActualPromoNetNSV] = tp.[ActualPromoNetNSV],
						[ProductSubrangesList] = tp.[ProductSubrangesList],
						[ClientTreeKeyId] = tp.[ClientTreeKeyId],
						[InOut] = tp.[InOut],
						[PlanPromoNetIncrementalBaseTI] = tp.[PlanPromoNetIncrementalBaseTI],
						[PlanPromoNetIncrementalCOGS] = tp.[PlanPromoNetIncrementalCOGS],
						[ActualPromoNetIncrementalBaseTI] = tp.[ActualPromoNetIncrementalBaseTI],
						[ActualPromoNetIncrementalCOGS] = tp.[ActualPromoNetIncrementalCOGS],
						[PlanPromoNetBaseTI] = tp.[PlanPromoNetBaseTI],
						[PlanPromoNSV] = tp.[PlanPromoNSV],
						[ActualPromoNetBaseTI] = tp.[ActualPromoNetBaseTI],
						[ActualPromoNSV] = tp.[ActualPromoNSV],
						[ActualPromoLSVByCompensation] = tp.[ActualPromoLSVByCompensation],
						[PlanInStoreShelfPrice] = tp.[PlanInStoreShelfPrice],
						[PlanPromoPostPromoEffectLSVW1] = tp.[PlanPromoPostPromoEffectLSVW1],
						[PlanPromoPostPromoEffectLSVW2] = tp.[PlanPromoPostPromoEffectLSVW2],
						[PlanPromoPostPromoEffectLSV] = tp.[PlanPromoPostPromoEffectLSV],
						[ActualPromoPostPromoEffectLSVW1] = tp.[ActualPromoPostPromoEffectLSVW1],
						[ActualPromoPostPromoEffectLSVW2] = tp.[ActualPromoPostPromoEffectLSVW2],
						[ActualPromoPostPromoEffectLSV] = tp.[ActualPromoPostPromoEffectLSV],
						[LoadFromTLC] = tp.[LoadFromTLC],
						[InOutProductIds] = tp.[InOutProductIds],
						[InOutExcludeAssortmentMatrixProductsButtonPressed] = tp.[InOutExcludeAssortmentMatrixProductsButtonPressed],
						[DocumentNumber] = tp.[DocumentNumber],
						[LastChangedDate] = TODATETIMEOFFSET ( tp.[LastChangedDate] , '+03:00' ),
						[LastChangedDateDemand] = TODATETIMEOFFSET ( tp.[LastChangedDateDemand] , '+03:00' ),
						[LastChangedDateFinance] = TODATETIMEOFFSET ( tp.[LastChangedDateFinance] , '+03:00' ),
						[RegularExcludedProductIds] = tp.[RegularExcludedProductIds],
						[AdditionalUserTimestamp] = tp.[AdditionalUserTimestamp],
						[IsGrowthAcceleration] = tp.[IsGrowthAcceleration],
						[PromoTypesId] = tp.[PromoTypesId],
						[CreatorLogin] = tp.[CreatorLogin],
						[PlanTIBasePercent] = tp.[PlanTIBasePercent],
						[PlanCOGSPercent] = tp.[PlanCOGSPercent],
						[ActualTIBasePercent] = tp.[ActualTIBasePercent],
						[ActualCOGSPercent] = tp.[ActualCOGSPercent],
						[SumInvoice] = tp.[SumInvoice],
						[IsOnInvoice] = tp.[IsOnInvoice],
						[ActualPromoLSVSI] = tp.[ActualPromoLSVSI],
						[ActualPromoLSVSO] = tp.[ActualPromoLSVSO],
						[IsApolloExport] = tp.[IsApolloExport],
						[DeviationCoefficient] = tp.[DeviationCoefficient],
						[UseActualTI] = tp.[UseActualTI],
						[UseActualCOGS] = tp.[UseActualCOGS],
						[BudgetYear] = tp.[BudgetYear],
						[PlanAddTIShopperApproved] = tp.[PlanAddTIShopperApproved],
						[PlanAddTIShopperCalculated] = tp.[PlanAddTIShopperCalculated],
						[PlanAddTIMarketingApproved] = tp.[PlanAddTIMarketingApproved],
						[ActualAddTIShopper] = tp.[ActualAddTIShopper],
						[ActualAddTIMarketing] = tp.[ActualAddTIMarketing],
						[PlanPromoBaselineVolume] = tp.[PlanPromoBaselineVolume],
						[PlanPromoPostPromoEffectVolume] = tp.[PlanPromoPostPromoEffectVolume],
						[PlanPromoPostPromoEffectVolumeW1] = tp.[PlanPromoPostPromoEffectVolumeW1],
						[PlanPromoPostPromoEffectVolumeW2] = tp.[PlanPromoPostPromoEffectVolumeW2],
						[PlanPromoIncrementalVolume] = tp.[PlanPromoIncrementalVolume],
						[PlanPromoNetIncrementalVolume] = tp.[PlanPromoNetIncrementalVolume],
						[ActualPromoBaselineVolume] = tp.[ActualPromoBaselineVolume],
						[ActualPromoPostPromoEffectVolume] = tp.[ActualPromoPostPromoEffectVolume],
						[ActualPromoVolumeByCompensation] = tp.[ActualPromoVolumeByCompensation],
						[ActualPromoVolumeSI] = tp.[ActualPromoVolumeSI],
						[ActualPromoVolume] = tp.[ActualPromoVolume],
						[ActualPromoIncrementalVolume] = tp.[ActualPromoIncrementalVolume],
						[ActualPromoNetIncrementalVolume] = tp.[ActualPromoNetIncrementalVolume],
						[PlanPromoIncrementalCOGSTn] = tp.[PlanPromoIncrementalCOGSTn],
						[PlanPromoNetIncrementalCOGSTn] = tp.[PlanPromoNetIncrementalCOGSTn],
						[ActualPromoIncrementalCOGSTn] = tp.[ActualPromoIncrementalCOGSTn],
						[ActualPromoNetIncrementalCOGSTn] = tp.[ActualPromoNetIncrementalCOGSTn],
						[IsLSVBased] = tp.[IsLSVBased],
						[PlanPromoIncrementalMACLSV] = tp.[PlanPromoIncrementalMACLSV],
						[PlanPromoNetIncrementalMACLSV] = tp.[PlanPromoNetIncrementalMACLSV],
						[ActualPromoIncrementalMACLSV] = tp.[ActualPromoIncrementalMACLSV],
						[ActualPromoNetIncrementalMACLSV] = tp.[ActualPromoNetIncrementalMACLSV],
						[PlanPromoIncrementalEarningsLSV] = tp.[PlanPromoIncrementalEarningsLSV],
						[PlanPromoNetIncrementalEarningsLSV] = tp.[PlanPromoNetIncrementalEarningsLSV],
						[ActualPromoIncrementalEarningsLSV] = tp.[ActualPromoIncrementalEarningsLSV],
						[ActualPromoNetIncrementalEarningsLSV] = tp.[ActualPromoNetIncrementalEarningsLSV],
						[PlanPromoROIPercentLSV] = tp.[PlanPromoROIPercentLSV],
						[PlanPromoNetROIPercentLSV] = tp.[PlanPromoNetROIPercentLSV],
						[ActualPromoROIPercentLSV] = tp.[ActualPromoROIPercentLSV],
						[ActualPromoNetROIPercentLSV] = tp.[ActualPromoNetROIPercentLSV],
						[PlanCOGSTn] = tp.[PlanCOGSTn],
						[ActualCOGSTn] = tp.[ActualCOGSTn],
						[PlanPromoVolume] = tp.[PlanPromoVolume],
						[PlanPromoNSVtn] = [PlanPromoNSVtn],
						[ActualPromoNSVtn] = [ActualPromoNSVtn]
					FROM
						(SELECT * FROM [DefaultSchemaSetting].[TEMP_PROMO]) AS tp WHERE [DefaultSchemaSetting].[Promo].[Id] = tp.Id
			
					UPDATE [DefaultSchemaSetting].[PromoPriceIncrease]
					SET
						[PlanPromoUpliftPercent] = tp.[PlanPromoUpliftPercent],
						[PlanPromoBaselineLSV] = tp.[PlanPromoBaselineLSV],
						[PlanPromoIncrementalLSV] = tp.[PlanPromoIncrementalLSV],
						[PlanPromoLSV] = tp.[PlanPromoLSV],
						[PlanPromoPostPromoEffectLSV] = tp.[PlanPromoPostPromoEffectLSV],
						[PlanPromoPostPromoEffectLSVW1] = tp.[PlanPromoPostPromoEffectLSVW1],
						[PlanPromoPostPromoEffectLSVW2] = tp.[PlanPromoPostPromoEffectLSVW2],
						[PlanPromoPostPromoEffectVolumeW1] = tp.[PlanPromoPostPromoEffectVolumeW1],
						[PlanPromoPostPromoEffectVolumeW2] = tp.[PlanPromoPostPromoEffectVolumeW2],
						[PlanPromoPostPromoEffectVolume] = tp.[PlanPromoPostPromoEffectVolume],
						[PlanPromoBaselineVolume] = tp.[PlanPromoBaselineVolume],
						[PlanPromoTIShopper] = tp.[PlanPromoTIShopper],
						[PlanPromoCost] = tp.[PlanPromoCost],
						[PlanPromoIncrementalCOGS] = tp.[PlanPromoIncrementalCOGS],
						[PlanPromoBaseTI] = tp.[PlanPromoBaseTI],
						[PlanPromoIncrementalBaseTI] = tp.[PlanPromoIncrementalBaseTI],
						[PlanPromoTotalCost] = tp.[PlanPromoTotalCost],
						[PlanPromoNetIncrementalLSV] = tp.[PlanPromoNetIncrementalLSV],
						[PlanPromoNetLSV] = tp.[PlanPromoNetLSV],
						[PlanPromoNetIncrementalBaseTI] = tp.[PlanPromoNetIncrementalBaseTI],
						[PlanPromoNetIncrementalCOGS] = tp.[PlanPromoNetIncrementalCOGS],
						[PlanPromoNetBaseTI] = tp.[PlanPromoNetBaseTI],
						[PlanPromoBaselineBaseTI] = tp.[PlanPromoBaselineBaseTI],
						[PlanPromoNSV] = tp.[PlanPromoNSV],
						[PlanPromoIncrementalNSV] = tp.[PlanPromoIncrementalNSV],
						[PlanPromoNetIncrementalNSV] = tp.[PlanPromoNetIncrementalNSV],
						[PlanPromoNetIncrementalMAC] = tp.[PlanPromoNetIncrementalMAC],
						[PlanPromoIncrementalVolume] = tp.[PlanPromoIncrementalVolume],
						[PlanPromoNetIncrementalVolume] = tp.[PlanPromoNetIncrementalVolume],
						[PlanPromoNetNSV] = tp.[PlanPromoNetNSV],
						[PlanPromoIncrementalCOGSTn] = tp.[PlanPromoIncrementalCOGSTn],
						[PlanPromoNetIncrementalCOGSTn] = tp.[PlanPromoNetIncrementalCOGSTn],
						[PlanPromoIncrementalMAC] = tp.[PlanPromoIncrementalMAC],
						[PlanPromoIncrementalEarnings] = tp.[PlanPromoIncrementalEarnings],
						[PlanPromoNetIncrementalEarnings] = tp.[PlanPromoNetIncrementalEarnings],
						[PlanPromoROIPercent] = tp.[PlanPromoROIPercent],
						[PlanPromoNetROIPercent] = tp.[PlanPromoNetROIPercent],
						[PlanPromoNetIncrementalMACLSV] = tp.[PlanPromoNetIncrementalMACLSV],
						[PlanPromoIncrementalMACLSV] = tp.[PlanPromoIncrementalMACLSV],
						[PlanPromoIncrementalEarningsLSV] = tp.[PlanPromoIncrementalEarningsLSV],
						[PlanPromoNetIncrementalEarningsLSV] = tp.[PlanPromoNetIncrementalEarningsLSV],
						[PlanPromoROIPercentLSV] = tp.[PlanPromoROIPercentLSV],
						[PlanPromoNetROIPercentLSV] = tp.[PlanPromoNetROIPercentLSV],
						[PlanAddTIShopperCalculated] = tp.[PlanAddTIShopperCalculated],
						[PlanAddTIShopperApproved] = tp.[PlanAddTIShopperApproved],
						[PlanPromoNetUpliftPercent] = tp.[PlanPromoNetUpliftPercent],
						[PlanPromoVolume] = tp.[PlanPromoVolume],
						[PlanPromoNSVtn] = [PlanPromoNSVtn],
						[ActualPromoNSVtn] = [ActualPromoNSVtn]
					FROM
						(SELECT * FROM [DefaultSchemaSetting].[TEMP_PROMO_PRICEINCREASE]) AS tp WHERE [DefaultSchemaSetting].[PromoPriceIncrease].[Id] = tp.Id

					UPDATE [DefaultSchemaSetting].[PromoProduct]
					SET
						[Disabled] = tpp.[Disabled],
						[DeletedDate] = tpp.[DeletedDate],
						[ZREP] = tpp.[ZREP],
						[PromoId] = tpp.[PromoId],
						[ProductId] = tpp.[ProductId],
						[EAN_Case] = tpp.[EAN_Case],
						[PlanProductPCQty] = TRY_CAST(tpp.[PlanProductPCQty] AS INT),
						[PlanProductPCLSV] = tpp.[PlanProductPCLSV],
						[ActualProductPCQty] = TRY_CAST(tpp.[ActualProductPCQty] AS INT),
						[ActualProductUOM] = tpp.[ActualProductUOM],
						[ActualProductSellInPrice] = tpp.[ActualProductSellInPrice],
						[ActualProductShelfDiscount] = tpp.[ActualProductShelfDiscount],
						[ActualProductPCLSV] = tpp.[ActualProductPCLSV],
						[ActualProductUpliftPercent] = tpp.[ActualProductUpliftPercent],
						[ActualProductIncrementalPCQty] = tpp.[ActualProductIncrementalPCQty],
						[ActualProductIncrementalPCLSV] = tpp.[ActualProductIncrementalPCLSV],
						[ActualProductIncrementalLSV] = tpp.[ActualProductIncrementalLSV],
						[PlanProductUpliftPercent] = tpp.[PlanProductUpliftPercent],
						[ActualProductLSV] = tpp.[ActualProductLSV],
						[PlanProductBaselineLSV] = tpp.[PlanProductBaselineLSV],
						[ActualProductPostPromoEffectQty] = tpp.[ActualProductPostPromoEffectQty],
						[PlanProductPostPromoEffectQty] = tpp.[PlanProductPostPromoEffectQty],
						[ProductEN] = tpp.[ProductEN],
						[PlanProductCaseQty] = tpp.[PlanProductCaseQty],
						[PlanProductBaselineCaseQty] = tpp.[PlanProductBaselineCaseQty],
						[ActualProductCaseQty] = tpp.[ActualProductCaseQty],
						[PlanProductPostPromoEffectLSVW1] = tpp.[PlanProductPostPromoEffectLSVW1],
						[PlanProductPostPromoEffectLSVW2] = tpp.[PlanProductPostPromoEffectLSVW2],
						[PlanProductPostPromoEffectLSV] = tpp.[PlanProductPostPromoEffectLSV],
						[ActualProductPostPromoEffectLSV] = tpp.[ActualProductPostPromoEffectLSV],
						[PlanProductIncrementalCaseQty] = tpp.[PlanProductIncrementalCaseQty],
						[ActualProductPostPromoEffectQtyW1] = tpp.[ActualProductPostPromoEffectQtyW1],
						[ActualProductPostPromoEffectQtyW2] = tpp.[ActualProductPostPromoEffectQtyW2],
						[PlanProductPostPromoEffectQtyW1] = tpp.[PlanProductPostPromoEffectQtyW1],
						[PlanProductPostPromoEffectQtyW2] = tpp.[PlanProductPostPromoEffectQtyW2],
						[PlanProductPCPrice] = tpp.[PlanProductPCPrice],
						[ActualProductBaselineLSV] = tpp.[ActualProductBaselineLSV],
						[EAN_PC] = tpp.[EAN_PC],
						[PlanProductCaseLSV] = tpp.[PlanProductCaseLSV],
						[ActualProductLSVByCompensation] = tpp.[ActualProductLSVByCompensation],
						[PlanProductLSV] = tpp.[PlanProductLSV],
						[PlanProductIncrementalLSV] = tpp.[PlanProductIncrementalLSV],
						[AverageMarker] = tpp.[AverageMarker],
						[Price] = tpp.[Price],
						[SumInvoiceProduct] = tpp.[SumInvoiceProduct],
						[ActualProductBaselineCaseQty] = tpp.[ActualProductBaselineCaseQty],
						[PlanProductBaselineVolume] = tpp.[PlanProductBaselineVolume],
						[PlanProductPostPromoEffectVolumeW1] = tpp.[PlanProductPostPromoEffectVolumeW1],
						[PlanProductPostPromoEffectVolumeW2] = tpp.[PlanProductPostPromoEffectVolumeW2],
						[PlanProductPostPromoEffectVolume] = tpp.[PlanProductPostPromoEffectVolume],
						[ActualProductQtySO] = tpp.[ActualProductQtySO]
					FROM
						(SELECT * FROM [DefaultSchemaSetting].[TEMP_PROMOPRODUCT]) AS tpp WHERE [DefaultSchemaSetting].[PromoProduct].[Id] = tpp.Id
					
					UPDATE DefaultSchemaSetting.Promo 
					SET IsPriceIncrease = 0

					UPDATE p
					SET p.IsPriceIncrease = 1
					from DefaultSchemaSetting.Promo p
					INNER JOIN DefaultSchemaSetting.PromoStatus ps on ps.id = p.PromoStatusId
					INNER JOIN DefaultSchemaSetting.PromoProduct pp on pp.PromoId = p.id and pp.Disabled = 0
					INNER JOIN DefaultSchemaSetting.PriceList pl on pl.ProductId = pp.ProductId AND pl.Disabled = 0
																AND pl.FuturePriceMarker = 1 AND pl.StartDate <= p.DispatchesStart
																AND pl.EndDate >= p.DispatchesStart
																AND pl.ClientTreeId = p.ClientTreeKeyId
					where p.Disabled = 0 AND ps.SystemName in ('OnApproval','Planned','Approved','DraftPublished')

					UPDATE [DefaultSchemaSetting].[PromoProductPriceIncrease]
					SET
						[PlanProductCaseQty] = tpp.[PlanProductCaseQty],
						[PlanProductPCQty] = tpp.[PlanProductPCQty],
						[PlanProductCaseLSV] = tpp.[PlanProductCaseLSV],
						[PlanProductPCLSV] = tpp.[PlanProductPCLSV],
						[PlanProductBaselineLSV] = tpp.[PlanProductBaselineLSV],
						[ActualProductBaselineLSV] = tpp.[ActualProductBaselineLSV],
						[PlanProductIncrementalLSV] = tpp.[PlanProductIncrementalLSV],
						[PlanProductLSV] = tpp.[PlanProductLSV],
						[PlanProductBaselineCaseQty] = tpp.[PlanProductBaselineCaseQty],
						[Price] = tpp.[Price],
						[PlanProductPCPrice] = tpp.[PlanProductPCPrice],
						[ActualProductPCQty] = tpp.[ActualProductPCQty],
						[ActualProductCaseQty] = tpp.[ActualProductCaseQty],
						[ActualProductUOM] = tpp.[ActualProductUOM],
						[ActualProductSellInPrice] = tpp.[ActualProductSellInPrice],
						[ActualProductShelfDiscount] = tpp.[ActualProductShelfDiscount],
						[ActualProductPCLSV] = tpp.[ActualProductPCLSV],
						[ActualProductUpliftPercent] = tpp.[ActualProductUpliftPercent],
						[ActualProductIncrementalPCQty] = tpp.[ActualProductIncrementalPCQty],
						[ActualProductIncrementalPCLSV] = tpp.[ActualProductIncrementalPCLSV],
						[ActualProductIncrementalLSV] = tpp.[ActualProductIncrementalLSV],
						[PlanProductPostPromoEffectLSVW1] = tpp.[PlanProductPostPromoEffectLSVW1],
						[PlanProductPostPromoEffectLSVW2] = tpp.[PlanProductPostPromoEffectLSVW2],
						[PlanProductPostPromoEffectLSV] = tpp.[PlanProductPostPromoEffectLSV],
						[ActualProductPostPromoEffectLSV] = tpp.[ActualProductPostPromoEffectLSV],
						[PlanProductIncrementalCaseQty] = tpp.[PlanProductIncrementalCaseQty],
						[PlanProductUpliftPercent] = tpp.[PlanProductUpliftPercent],
						[ActualProductLSV] = tpp.[ActualProductLSV],
						[ActualProductPostPromoEffectQtyW1] = tpp.[ActualProductPostPromoEffectQtyW1],
						[ActualProductPostPromoEffectQtyW2] = tpp.[ActualProductPostPromoEffectQtyW2],
						[ActualProductPostPromoEffectQty] = tpp.[ActualProductPostPromoEffectQty],
						[PlanProductPostPromoEffectQtyW1] = tpp.[PlanProductPostPromoEffectQtyW1],
						[PlanProductPostPromoEffectQtyW2] = tpp.[PlanProductPostPromoEffectQtyW2],
						[PlanProductPostPromoEffectQty] = tpp.[PlanProductPostPromoEffectQty],
						[ActualProductLSVByCompensation] = tpp.[ActualProductLSVByCompensation],
						[ActualProductBaselineCaseQty] = tpp.[ActualProductBaselineCaseQty],
						[SumInvoiceProduct] = tpp.[SumInvoiceProduct],
						[PlanProductBaselineVolume] = tpp.[PlanProductBaselineVolume],
						[PlanProductPostPromoEffectVolumeW1] = tpp.[PlanProductPostPromoEffectVolumeW1],
						[PlanProductPostPromoEffectVolumeW2] = tpp.[PlanProductPostPromoEffectVolumeW2],
						[PlanProductPostPromoEffectVolume] = tpp.[PlanProductPostPromoEffectVolume],
						[ActualProductQtySO] = tpp.[ActualProductQtySO]
					FROM
						(SELECT * FROM [DefaultSchemaSetting].[TEMP_PROMOPRODUCT_PRICEINCREASE]) AS tpp WHERE [DefaultSchemaSetting].[PromoProductPriceIncrease].[Id] = tpp.Id

					UPDATE [DefaultSchemaSetting].[PromoSupportPromo]
					SET
						[Disabled] = CASE WHEN tpsp.[DeletedDate] IS NOT NULL THEN 1 ELSE 0 END,
						[DeletedDate] = tpsp.[DeletedDate],
						[PromoId] = tpsp.[PromoId],
						[PromoSupportId] = tpsp.[PromoSupportId],
						[PlanCalculation] = tpsp.[PlanCalculation],
						[FactCalculation] = tpsp.[FactCalculation],
						[PlanCostProd] = tpsp.[PlanCostProd],
						[FactCostProd] = tpsp.[FactCostProd]
					FROM
						(SELECT * FROM [DefaultSchemaSetting].[TEMP_PROMOSUPPORTPROMO]) AS tpsp WHERE [DefaultSchemaSetting].[PromoSupportPromo].[Id] = tpsp.Id

					IF EXISTS (SELECT TOP 1 ProductId FROM [DefaultSchemaSetting].[ProductChangeIncident] WHERE NotificationProcessDate IS NULL)
					BEGIN
						INSERT INTO [DefaultSchemaSetting].[ProductChangeIncident]
							   ([ProductId]
							   ,[CreateDate]
							   ,[IsCreate]
							   ,[IsDelete]
							   ,[RecalculatedPromoId]
							   ,[AddedProductIds]
							   ,[ExcludedProductIds]
							   ,[IsRecalculated]
							   ,[Disabled])
						SELECT
							   (SELECT TOP 1 ProductId FROM [DefaultSchemaSetting].[ProductChangeIncident] WHERE NotificationProcessDate IS NULL)
							   ,GETDATE()
							   ,0
							   ,0
							   ,tpci.PromoId
							   ,tpci.AddedProductIds
							   ,tpci.ExcludedProductIds
							   ,1
							   ,0
						FROM [DefaultSchemaSetting].[TEMP_PRODUCTCHANGEINCIDENTS] AS tpci
					END
				END
        GO
        ";
    }
}
