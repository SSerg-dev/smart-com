namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTempPromoTable : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            if (defaultSchema.Equals("Jupiter"))
            {
                Sql($@"
                    {AddTIColumns}
                    go
					{UpdateProcUpdatePromo}
                    go
                ");
            }
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            if (defaultSchema.Equals("Jupiter"))
            {
                Sql($@"
                    {DropTIColumns}
                    go
                    {DownProcUpdatePromo}
                    go
                ");
            }
        }

        private string AddTIColumns =
        @"
            if not exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'BudgetYear'
            )
            alter table Jupiter.TEMP_PROMO add BudgetYear int null;

            if not exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'PlanAddTIShopperApproved'
            )
            alter table Jupiter.TEMP_PROMO add PlanAddTIShopperApproved float null;

            if not exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'PlanAddTIShopperCalculated'
            )
            alter table Jupiter.TEMP_PROMO add PlanAddTIShopperCalculated float null;

            if not exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'PlanAddTIMarketingApproved'
            )
            alter table Jupiter.TEMP_PROMO add PlanAddTIMarketingApproved float null;

            if not exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'ActualAddTIShopper'
            )
            alter table Jupiter.TEMP_PROMO add ActualAddTIShopper float null;

            if not exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'ActualAddTIMarketing'
            )
            alter table Jupiter.TEMP_PROMO add ActualAddTIMarketing float null;
        ";

        private string DropTIColumns =
        @"
            if exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'BudgetYear'
            )
            alter table Jupiter.TEMP_PROMO drop column BudgetYear;

            if exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'PlanAddTIShopperApproved'
            )
            alter table Jupiter.TEMP_PROMO drop column PlanAddTIShopperApproved;

            if exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'PlanAddTIShopperCalculated'
            )
            alter table Jupiter.TEMP_PROMO drop column PlanAddTIShopperCalculated;

            if exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'PlanAddTIMarketingApproved'
            )
            alter table Jupiter.TEMP_PROMO drop column PlanAddTIMarketingApproved;

            if exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'ActualAddTIShopper'
            )
            alter table Jupiter.TEMP_PROMO drop column ActualAddTIShopper;

            if exists (
                select * from INFORMATION_SCHEMA.COLUMNS where TABLE_NAME = 'TEMP_PROMO' and COLUMN_NAME = 'ActualAddTIMarketing'
            )
            alter table Jupiter.TEMP_PROMO drop column ActualAddTIMarketing;
        ";

		private string UpdateProcUpdatePromo =
		@"
        CREATE OR ALTER PROCEDURE [Jupiter].[UpdatePromo]
		AS
		BEGIN
			UPDATE [Jupiter].[Promo]
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
				[MechanicComment] = tp.[MechanicComment],
				[PlanInstoreMechanicDiscount] = tp.[PlanInstoreMechanicDiscount],
				[CalendarPriority] = tp.[CalendarPriority],
				[PlanPromoTIShopper] = tp.[PlanPromoTIShopper],
				[PlanPromoTIMarketing] = tp.[PlanPromoTIMarketing],
				[PlanPromoBranding] = tp.[PlanPromoBranding],
				[PlanPromoCost] = tp.[PlanPromoCost],
				[PlanPromoBTL] = tp.[PlanPromoBTL],
				[PlanPromoCostProduction] = tp.[PlanPromoCostProduction],
				[PlanPromoUpliftPercent] = tp.[PlanPromoUpliftPercent],
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
				[InvoiceTotal] = tp.[InvoiceTotal],
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
				[ActualAddTIMarketing] = tp.[ActualAddTIMarketing]
			FROM
				(SELECT * FROM [Jupiter].[TEMP_PROMO]) AS tp WHERE [Jupiter].[Promo].[Id] = tp.Id
			
			UPDATE [Jupiter].[PromoProduct]
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
				[InvoiceTotalProduct] = tpp.[InvoiceTotalProduct],
				[ActualProductBaselineCaseQty] = tpp.[ActualProductBaselineCaseQty]
			FROM
				(SELECT * FROM [Jupiter].[TEMP_PROMOPRODUCT]) AS tpp WHERE [Jupiter].[PromoProduct].[Id] = tpp.Id
		
			UPDATE [Jupiter].[PromoSupportPromo]
			SET
				[Disabled] = tpsp.[Disabled],
				[DeletedDate] = tpsp.[DeletedDate],
				[PromoId] = tpsp.[PromoId],
				[PromoSupportId] = tpsp.[PromoSupportId],
				[PlanCalculation] = tpsp.[PlanCalculation],
				[FactCalculation] = tpsp.[FactCalculation],
				[PlanCostProd] = tpsp.[PlanCostProd],
				[FactCostProd] = tpsp.[FactCostProd]
			FROM
				(SELECT * FROM [Jupiter].[TEMP_PROMOSUPPORTPROMO]) AS tpsp WHERE [Jupiter].[PromoSupportPromo].[Id] = tpsp.Id

			IF EXISTS (SELECT TOP 1 ProductId FROM [Jupiter].[ProductChangeIncident] WHERE NotificationProcessDate IS NULL)
			BEGIN
				INSERT INTO [Jupiter].[ProductChangeIncident]
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
					   (SELECT TOP 1 ProductId FROM [Jupiter].[ProductChangeIncident] WHERE NotificationProcessDate IS NULL)
					   ,GETDATE()
					   ,0
					   ,0
					   ,tpci.PromoId
					   ,tpci.AddedProductIds
					   ,tpci.ExcludedProductIds
					   ,1
					   ,0
				FROM [Jupiter].[TEMP_PRODUCTCHANGEINCIDENTS] AS tpci
			END
		END
        ";

		private string DownProcUpdatePromo =
		@"
        CREATE OR ALTER PROCEDURE [Jupiter].[UpdatePromo]
		AS
		BEGIN
			UPDATE [Jupiter].[Promo]
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
				[MechanicComment] = tp.[MechanicComment],
				[PlanInstoreMechanicDiscount] = tp.[PlanInstoreMechanicDiscount],
				[CalendarPriority] = tp.[CalendarPriority],
				[PlanPromoTIShopper] = tp.[PlanPromoTIShopper],
				[PlanPromoTIMarketing] = tp.[PlanPromoTIMarketing],
				[PlanPromoBranding] = tp.[PlanPromoBranding],
				[PlanPromoCost] = tp.[PlanPromoCost],
				[PlanPromoBTL] = tp.[PlanPromoBTL],
				[PlanPromoCostProduction] = tp.[PlanPromoCostProduction],
				[PlanPromoUpliftPercent] = tp.[PlanPromoUpliftPercent],
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
				[InvoiceTotal] = tp.[InvoiceTotal],
				[IsOnInvoice] = tp.[IsOnInvoice],
				[ActualPromoLSVSI] = tp.[ActualPromoLSVSI],
				[ActualPromoLSVSO] = tp.[ActualPromoLSVSO],
				[IsApolloExport] = tp.[IsApolloExport],
				[DeviationCoefficient] = tp.[DeviationCoefficient],
				[UseActualTI] = tp.[UseActualTI],
				[UseActualCOGS] = tp.[UseActualCOGS]
			FROM
				(SELECT * FROM [Jupiter].[TEMP_PROMO]) AS tp WHERE [Jupiter].[Promo].[Id] = tp.Id
			
			UPDATE [Jupiter].[PromoProduct]
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
				[InvoiceTotalProduct] = tpp.[InvoiceTotalProduct],
				[ActualProductBaselineCaseQty] = tpp.[ActualProductBaselineCaseQty]
			FROM
				(SELECT * FROM [Jupiter].[TEMP_PROMOPRODUCT]) AS tpp WHERE [Jupiter].[PromoProduct].[Id] = tpp.Id
		
			UPDATE [Jupiter].[PromoSupportPromo]
			SET
				[Disabled] = tpsp.[Disabled],
				[DeletedDate] = tpsp.[DeletedDate],
				[PromoId] = tpsp.[PromoId],
				[PromoSupportId] = tpsp.[PromoSupportId],
				[PlanCalculation] = tpsp.[PlanCalculation],
				[FactCalculation] = tpsp.[FactCalculation],
				[PlanCostProd] = tpsp.[PlanCostProd],
				[FactCostProd] = tpsp.[FactCostProd]
			FROM
				(SELECT * FROM [Jupiter].[TEMP_PROMOSUPPORTPROMO]) AS tpsp WHERE [Jupiter].[PromoSupportPromo].[Id] = tpsp.Id

			IF EXISTS (SELECT TOP 1 ProductId FROM [Jupiter].[ProductChangeIncident] WHERE NotificationProcessDate IS NULL)
			BEGIN
				INSERT INTO [Jupiter].[ProductChangeIncident]
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
					   (SELECT TOP 1 ProductId FROM [Jupiter].[ProductChangeIncident] WHERE NotificationProcessDate IS NULL)
					   ,GETDATE()
					   ,0
					   ,0
					   ,tpci.PromoId
					   ,tpci.AddedProductIds
					   ,tpci.ExcludedProductIds
					   ,1
					   ,0
				FROM [Jupiter].[TEMP_PRODUCTCHANGEINCIDENTS] AS tpci
			END
		END
        ";
	}
}
