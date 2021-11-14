namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_CopyScenario_Tables : DbMigration
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

        string SqlString = @"
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			create   function [DefaultSchemaSetting].[GetDistinctItems]
			(
			  @string nvarchar(max)  
			)
			returns nvarchar(max)
			as
			begin
			  declare @result nvarchar(max);
			  with cte as (select distinct(value) from string_split(@string, ';')  )
			  select @result = string_agg(value, ';') from cte
			  return @result
			end
			GO
			/****** Object:  Table [DefaultSchemaSetting].[TEMP_SCENARIO_BTL]    Script Date: 10/25/2021 5:28:42 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_BTL](
				[Id] [uniqueidentifier] NULL,
				[Disabled] [bit] NULL,
				[DeletedDate] [datetimeoffset](7) NULL,
				[Number] [int] NULL,
				[PlanBTLTotal] [float] NULL,
				[ActualBTLTotal] [float] NULL,
				[StartDate] [datetimeoffset](7) NULL,
				[EndDate] [datetimeoffset](7) NULL,
				[InvoiceNumber] [nvarchar](max) NULL,
				[EventId] [uniqueidentifier] NULL
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
			GO
			/****** Object:  Table [DefaultSchemaSetting].[TEMP_SCENARIO_BTLPROMO]    Script Date: 10/25/2021 5:28:42 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_BTLPROMO](
				[Id] [uniqueidentifier] NULL,
				[Disabled] [bit] NULL,
				[DeletedDate] [datetimeoffset](7) NULL,
				[BTLId] [uniqueidentifier] NULL,
				[PromoId] [uniqueidentifier] NULL,
				[ClientTreeId] [int] NULL,
				[PromoNumber] [int] NULL,
				[BTLNumber] [int] NULL
			) ON [PRIMARY]
			GO
			/****** Object:  Table [DefaultSchemaSetting].[TEMP_SCENARIO_PROMO]    Script Date: 10/25/2021 5:28:43 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_PROMO](
				[Id] [uniqueidentifier] NULL,
				[Disabled] [bit] NULL,
				[DeletedDate] [datetimeoffset](7) NULL,
				[BrandId] [uniqueidentifier] NULL,
				[BrandTechId] [uniqueidentifier] NULL,
				[PromoStatusId] [uniqueidentifier] NULL,
				[Name] [nvarchar](255) NULL,
				[StartDate] [datetimeoffset](7) NULL,
				[EndDate] [datetimeoffset](7) NULL,
				[DispatchesStart] [datetimeoffset](7) NULL,
				[DispatchesEnd] [datetimeoffset](7) NULL,
				[ColorId] [uniqueidentifier] NULL,
				[Number] [int] NULL,
				[RejectReasonId] [uniqueidentifier] NULL,
				[EventId] [uniqueidentifier] NULL,
				[MarsMechanicId] [uniqueidentifier] NULL,
				[MarsMechanicTypeId] [uniqueidentifier] NULL,
				[PlanInstoreMechanicId] [uniqueidentifier] NULL,
				[PlanInstoreMechanicTypeId] [uniqueidentifier] NULL,
				[MarsMechanicDiscount] [float] NULL,
				[OtherEventName] [nvarchar](255) NULL,
				[EventName] [nvarchar](max) NULL,
				[ClientHierarchy] [nvarchar](max) NULL,
				[ProductHierarchy] [nvarchar](max) NULL,
				[CreatorId] [uniqueidentifier] NULL,
				[MarsStartDate] [nvarchar](15) NULL,
				[MarsEndDate] [nvarchar](15) NULL,
				[MarsDispatchesStart] [nvarchar](15) NULL,
				[MarsDispatchesEnd] [nvarchar](15) NULL,
				[ClientTreeId] [int] NULL,
				[BaseClientTreeId] [int] NULL,
				[Mechanic] [nvarchar](255) NULL,
				[MechanicIA] [nvarchar](255) NULL,
				[BaseClientTreeIds] [nvarchar](400) NULL,
				[LastApprovedDate] [datetimeoffset](7) NULL,
				[TechnologyId] [uniqueidentifier] NULL,
				[NeedRecountUplift] [bit] NULL,
				[IsAutomaticallyApproved] [bit] NULL,
				[IsCMManagerApproved] [bit] NULL,
				[IsDemandPlanningApproved] [bit] NULL,
				[IsDemandFinanceApproved] [bit] NULL,
				[PlanPromoXSites] [float] NULL,
				[PlanPromoCatalogue] [float] NULL,
				[PlanPromoPOSMInClient] [float] NULL,
				[PlanPromoCostProdXSites] [float] NULL,
				[PlanPromoCostProdCatalogue] [float] NULL,
				[PlanPromoCostProdPOSMInClient] [float] NULL,
				[ActualPromoXSites] [float] NULL,
				[ActualPromoCatalogue] [float] NULL,
				[ActualPromoPOSMInClient] [float] NULL,
				[ActualPromoCostProdXSites] [float] NULL,
				[ActualPromoCostProdCatalogue] [float] NULL,
				[ActualPromoCostProdPOSMInClient] [float] NULL,
				[MechanicComment] [nvarchar](255) NULL,
				[PlanInstoreMechanicDiscount] [float] NULL,
				[CalendarPriority] [int] NULL,
				[PlanPromoTIShopper] [float] NULL,
				[PlanPromoTIMarketing] [float] NULL,
				[PlanPromoBranding] [float] NULL,
				[PlanPromoCost] [float] NULL,
				[PlanPromoBTL] [float] NULL,
				[PlanPromoCostProduction] [float] NULL,
				[PlanPromoUpliftPercent] [float] NULL,
				[PlanPromoIncrementalLSV] [float] NULL,
				[PlanPromoLSV] [float] NULL,
				[PlanPromoROIPercent] [float] NULL,
				[PlanPromoIncrementalNSV] [float] NULL,
				[PlanPromoNetIncrementalNSV] [float] NULL,
				[PlanPromoIncrementalMAC] [float] NULL,
				[ActualPromoTIShopper] [float] NULL,
				[ActualPromoTIMarketing] [float] NULL,
				[ActualPromoBranding] [float] NULL,
				[ActualPromoBTL] [float] NULL,
				[ActualPromoCostProduction] [float] NULL,
				[ActualPromoCost] [float] NULL,
				[ActualPromoUpliftPercent] [float] NULL,
				[ActualPromoIncrementalLSV] [float] NULL,
				[ActualPromoLSV] [float] NULL,
				[ActualPromoROIPercent] [float] NULL,
				[ActualPromoIncrementalNSV] [float] NULL,
				[ActualPromoNetIncrementalNSV] [float] NULL,
				[ActualPromoIncrementalMAC] [float] NULL,
				[ActualInStoreMechanicId] [uniqueidentifier] NULL,
				[ActualInStoreMechanicTypeId] [uniqueidentifier] NULL,
				[PromoDuration] [int] NULL,
				[DispatchDuration] [int] NULL,
				[InvoiceNumber] [nvarchar](max) NULL,
				[PlanPromoBaselineLSV] [float] NULL,
				[PlanPromoIncrementalBaseTI] [float] NULL,
				[PlanPromoIncrementalCOGS] [float] NULL,
				[PlanPromoTotalCost] [float] NULL,
				[PlanPromoNetIncrementalLSV] [float] NULL,
				[PlanPromoNetLSV] [float] NULL,
				[PlanPromoNetIncrementalMAC] [float] NULL,
				[PlanPromoIncrementalEarnings] [float] NULL,
				[PlanPromoNetIncrementalEarnings] [float] NULL,
				[PlanPromoNetROIPercent] [float] NULL,
				[PlanPromoNetUpliftPercent] [float] NULL,
				[ActualPromoBaselineLSV] [float] NULL,
				[ActualInStoreDiscount] [float] NULL,
				[ActualInStoreShelfPrice] [float] NULL,
				[ActualPromoIncrementalBaseTI] [float] NULL,
				[ActualPromoIncrementalCOGS] [float] NULL,
				[ActualPromoTotalCost] [float] NULL,
				[ActualPromoNetIncrementalLSV] [float] NULL,
				[ActualPromoNetLSV] [float] NULL,
				[ActualPromoNetIncrementalMAC] [float] NULL,
				[ActualPromoIncrementalEarnings] [float] NULL,
				[ActualPromoNetIncrementalEarnings] [float] NULL,
				[ActualPromoNetROIPercent] [float] NULL,
				[ActualPromoNetUpliftPercent] [float] NULL,
				[Calculating] [bit] NULL,
				[BlockInformation] [nvarchar](max) NULL,
				[PlanPromoBaselineBaseTI] [float] NULL,
				[PlanPromoBaseTI] [float] NULL,
				[PlanPromoNetNSV] [float] NULL,
				[ActualPromoBaselineBaseTI] [float] NULL,
				[ActualPromoBaseTI] [float] NULL,
				[ActualPromoNetNSV] [float] NULL,
				[ProductSubrangesList] [nvarchar](500) NULL,
				[ClientTreeKeyId] [int] NULL,
				[InOut] [bit] NULL,
				[PlanPromoNetIncrementalBaseTI] [float] NULL,
				[PlanPromoNetIncrementalCOGS] [float] NULL,
				[ActualPromoNetIncrementalBaseTI] [float] NULL,
				[ActualPromoNetIncrementalCOGS] [float] NULL,
				[PlanPromoNetBaseTI] [float] NULL,
				[PlanPromoNSV] [float] NULL,
				[ActualPromoNetBaseTI] [float] NULL,
				[ActualPromoNSV] [float] NULL,
				[ActualPromoLSVByCompensation] [float] NULL,
				[PlanInStoreShelfPrice] [float] NULL,
				[PlanPromoPostPromoEffectLSVW1] [float] NULL,
				[PlanPromoPostPromoEffectLSVW2] [float] NULL,
				[PlanPromoPostPromoEffectLSV] [float] NULL,
				[ActualPromoPostPromoEffectLSVW1] [float] NULL,
				[ActualPromoPostPromoEffectLSVW2] [float] NULL,
				[ActualPromoPostPromoEffectLSV] [float] NULL,
				[LoadFromTLC] [bit] NULL,
				[InOutProductIds] [nvarchar](max) NULL,
				[InOutExcludeAssortmentMatrixProductsButtonPressed] [bit] NULL,
				[DocumentNumber] [nvarchar](max) NULL,
				[LastChangedDate] [datetimeoffset](7) NULL,
				[LastChangedDateDemand] [datetimeoffset](7) NULL,
				[LastChangedDateFinance] [datetimeoffset](7) NULL,
				[RegularExcludedProductIds] [nvarchar](max) NULL,
				[AdditionalUserTimestamp] [nvarchar](100) NULL,
				[IsGrowthAcceleration] [bit] NULL,
				[PromoTypesId] [uniqueidentifier] NULL,
				[CreatorLogin] [nvarchar](max) NULL,
				[PlanTIBasePercent] [float] NULL,
				[PlanCOGSPercent] [float] NULL,
				[ActualTIBasePercent] [float] NULL,
				[ActualCOGSPercent] [float] NULL,
				[SumInvoice] [float] NULL,
				[IsOnInvoice] [bit] NULL,
				[ActualPromoLSVSI] [float] NULL,
				[ActualPromoLSVSO] [float] NULL,
				[IsApolloExport] [bit] NULL,
				[DeviationCoefficient] [float] NULL,
				[UseActualTI] [bit] NOT NULL,
				[UseActualCOGS] [bit] NOT NULL,
				[BudgetYear] [int] NULL,
				[PlanAddTIShopperApproved] [float] NULL,
				[PlanAddTIShopperCalculated] [float] NULL,
				[PlanAddTIMarketingApproved] [float] NULL,
				[ActualAddTIShopper] [float] NULL,
				[ActualAddTIMarketing] [float] NULL,
				[ProductSubrangesListRU] [nchar](500) NULL,
				[ManualInputSumInvoice] [bit] NULL
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
			GO
			/****** Object:  Table [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCT]    Script Date: 10/25/2021 5:28:43 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCT](
				[Id] [uniqueidentifier] NULL,
				[Disabled] [bit] NULL,
				[DeletedDate] [datetimeoffset](7) NULL,
				[ZREP] [nvarchar](255) NULL,
				[PromoId] [uniqueidentifier] NULL,
				[ProductId] [uniqueidentifier] NULL,
				[EAN_Case] [nvarchar](255) NULL,
				[PlanProductPCQty] [bigint] NULL,
				[PlanProductPCLSV] [float] NULL,
				[ActualProductPCQty] [bigint] NULL,
				[ActualProductUOM] [nvarchar](max) NULL,
				[ActualProductSellInPrice] [float] NULL,
				[ActualProductShelfDiscount] [float] NULL,
				[ActualProductPCLSV] [float] NULL,
				[ActualProductUpliftPercent] [float] NULL,
				[ActualProductIncrementalPCQty] [float] NULL,
				[ActualProductIncrementalPCLSV] [float] NULL,
				[ActualProductIncrementalLSV] [float] NULL,
				[PlanProductUpliftPercent] [float] NULL,
				[ActualProductLSV] [float] NULL,
				[PlanProductBaselineLSV] [float] NULL,
				[ActualProductPostPromoEffectQty] [float] NULL,
				[PlanProductPostPromoEffectQty] [float] NULL,
				[ProductEN] [nvarchar](max) NULL,
				[PlanProductCaseQty] [float] NULL,
				[PlanProductBaselineCaseQty] [float] NULL,
				[ActualProductCaseQty] [float] NULL,
				[PlanProductPostPromoEffectLSVW1] [float] NULL,
				[PlanProductPostPromoEffectLSVW2] [float] NULL,
				[PlanProductPostPromoEffectLSV] [float] NULL,
				[ActualProductPostPromoEffectLSV] [float] NULL,
				[PlanProductIncrementalCaseQty] [float] NULL,
				[ActualProductPostPromoEffectQtyW1] [float] NULL,
				[ActualProductPostPromoEffectQtyW2] [float] NULL,
				[PlanProductPostPromoEffectQtyW1] [float] NULL,
				[PlanProductPostPromoEffectQtyW2] [float] NULL,
				[PlanProductPCPrice] [float] NULL,
				[ActualProductBaselineLSV] [float] NULL,
				[EAN_PC] [nvarchar](255) NULL,
				[PlanProductCaseLSV] [float] NULL,
				[ActualProductLSVByCompensation] [float] NULL,
				[PlanProductLSV] [float] NULL,
				[PlanProductIncrementalLSV] [float] NULL,
				[AverageMarker] [bit] NULL,
				[Price] [float] NULL,
				[SumInvoiceProduct] [float] NULL,
				[ActualProductBaselineCaseQty] [float] NULL,
				[CreateDate] [datetimeoffset](7) NULL,
				[PromoNumber] [int] NULL
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
			GO
			/****** Object:  Table [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCTSCORRECTION]    Script Date: 10/25/2021 5:28:43 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCTSCORRECTION](
				[Id] [uniqueidentifier] NULL,
				[Disabled] [bit] NULL,
				[DeletedDate] [datetimeoffset](7) NULL,
				[PromoProductId] [uniqueidentifier] NULL,
				[PlanProductUpliftPercentCorrected] [float] NULL,
				[UserId] [uniqueidentifier] NULL,
				[CreateDate] [datetimeoffset](7) NULL,
				[ChangeDate] [datetimeoffset](7) NULL,
				[UserName] [nvarchar](max) NULL,
				[TempId] [nvarchar](max) NULL,
				[PromoNumber] [int] NULL,
				[ZREP] [nvarchar](255) NULL
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
			GO
			/****** Object:  Table [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCTTREE]    Script Date: 10/25/2021 5:28:44 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCTTREE](
				[Id] [uniqueidentifier] NULL,
				[Disabled] [bit] NULL,
				[DeletedDate] [datetimeoffset](7) NULL,
				[PromoId] [uniqueidentifier] NULL,
				[ProductTreeObjectId] [int] NULL,
				[PromoNumber] [int] NULL
			) ON [PRIMARY]
			GO
			/****** Object:  Table [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOSUPPORT]    Script Date: 10/25/2021 5:28:44 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOSUPPORT](
				[Id] [uniqueidentifier] NULL,
				[Disabled] [bit] NULL,
				[DeletedDate] [datetimeoffset](7) NULL,
				[ClientTreeId] [int] NULL,
				[BudgetSubItemId] [uniqueidentifier] NULL,
				[StartDate] [datetimeoffset](7) NULL,
				[EndDate] [datetimeoffset](7) NULL,
				[PlanQuantity] [int] NULL,
				[ActualQuantity] [int] NULL,
				[PlanCostTE] [float] NULL,
				[ActualCostTE] [float] NULL,
				[PlanProdCostPer1Item] [float] NULL,
				[ActualProdCostPer1Item] [float] NULL,
				[PlanProdCost] [float] NULL,
				[ActualProdCost] [float] NULL,
				[UserTimestamp] [nvarchar](max) NULL,
				[AttachFileName] [nvarchar](max) NULL,
				[BorderColor] [nvarchar](max) NULL,
				[Number] [int] NULL,
				[PONumber] [nvarchar](max) NULL,
				[InvoiceNumber] [nvarchar](max) NULL,
				[OffAllocation] [bit] NULL
			) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
			GO
			/****** Object:  Table [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOSUPPORTPROMO]    Script Date: 10/25/2021 5:28:44 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOSUPPORTPROMO](
				[Id] [uniqueidentifier] NULL,
				[Disabled] [bit] NULL,
				[DeletedDate] [datetimeoffset](7) NULL,
				[PromoId] [uniqueidentifier] NULL,
				[PromoSupportId] [uniqueidentifier] NULL,
				[PlanCalculation] [float] NULL,
				[FactCalculation] [float] NULL,
				[PlanCostProd] [float] NULL,
				[FactCostProd] [float] NULL,
				[PromoNumber] [int] NULL,
				[PromoSupportNumber] [int] NULL
			) ON [PRIMARY]
			GO
			ALTER TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_PROMO] ADD  DEFAULT ((0)) FOR [UseActualTI]
			GO
			ALTER TABLE [DefaultSchemaSetting].[TEMP_SCENARIO_PROMO] ADD  DEFAULT ((0)) FOR [UseActualCOGS]
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[AddScenarioBTL]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO

			CREATE   PROCEDURE [DefaultSchemaSetting].[AddScenarioBTL]
					AS
					BEGIN
						DISABLE TRIGGER [BTL_Increment_Number] ON [DefaultSchemaSetting].[BTL];

						INSERT INTO [DefaultSchemaSetting].[BTL]
								   ([Id]
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[Number]
								   ,[PlanBTLTotal]
								   ,[ActualBTLTotal]
								   ,[StartDate]
								   ,[EndDate]
								   ,[InvoiceNumber]
								   ,[EventId])
								select 
									NEWID()
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[Number]
								   ,[PlanBTLTotal]
								   ,[ActualBTLTotal]
								   ,[StartDate]
								   ,[EndDate]
								   ,[InvoiceNumber]
								   ,[EventId]
								from [DefaultSchemaSetting].[TEMP_SCENARIO_BTL];

						declare @newStartWith bigint;
						set @newStartWith = (select max(Number) + 1 from DefaultSchemaSetting.BTL);
						declare @sql nvarchar(max)
						set @sql = N'alter sequence [DefaultSchemaSetting].[BTLNumberSequence] restart with ' + cast(@newStartWith as nvarchar(50)) + ';';
						exec SP_EXECUTESQL @sql;

						ENABLE TRIGGER [BTL_Increment_Number] ON [DefaultSchemaSetting].[BTL];
					END
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[AddScenarioBTLPromo]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE   PROCEDURE [DefaultSchemaSetting].[AddScenarioBTLPromo]
					AS
					BEGIN
						INSERT INTO [DefaultSchemaSetting].[BTLPromo]
								   ([Id]
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[BTLId]
								   ,[PromoId]
								   ,[ClientTreeId])
								select 
									NEWID()
								   ,[Disabled]
								   ,[DeletedDate]
								   ,(SELECT Id FROM [DefaultSchemaSetting].[BTL] btl WHERE btl.Number = btlp.BTLNumber AND btl.Disabled = 0)
								   ,(SELECT Id FROM [DefaultSchemaSetting].[Promo] p WHERE p.Number = btlp.PromoNumber AND p.Disabled = 0)
								   ,[ClientTreeId]
								from [DefaultSchemaSetting].[TEMP_SCENARIO_BTLPROMO] btlp
					END
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[AddScenarioPromo]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO




			CREATE   PROCEDURE [DefaultSchemaSetting].[AddScenarioPromo]
					AS
					BEGIN
						DISABLE TRIGGER [Promo_increment_number] ON [DefaultSchemaSetting].[Promo];
						ENABLE TRIGGER [PromoScenario_ChangesIncident_Insert_Trigger] ON [DefaultSchemaSetting].[Promo];

						INSERT INTO [DefaultSchemaSetting].[Promo]
								   ([Id]
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[BrandId]
								   ,[BrandTechId]
								   ,[PromoStatusId]
								   ,[Name]
								   ,[StartDate]
								   ,[EndDate]
								   ,[DispatchesStart]
								   ,[DispatchesEnd]
								   ,[ColorId]
								   ,[Number]
								   ,[RejectReasonId]
								   ,[EventId]
								   ,[MarsMechanicId]
								   ,[MarsMechanicTypeId]
								   ,[PlanInstoreMechanicId]
								   ,[PlanInstoreMechanicTypeId]
								   ,[MarsMechanicDiscount]
								   ,[OtherEventName]
								   ,[EventName]
								   ,[ClientHierarchy]
								   ,[ProductHierarchy]
								   ,[CreatorId]
								   ,[MarsStartDate]
								   ,[MarsEndDate]
								   ,[MarsDispatchesStart]
								   ,[MarsDispatchesEnd]
								   ,[ClientTreeId]
								   ,[BaseClientTreeId]
								   ,[Mechanic]
								   ,[MechanicIA]
								   ,[BaseClientTreeIds]
								   ,[LastApprovedDate]
								   ,[TechnologyId]
								   ,[NeedRecountUplift]
								   ,[IsAutomaticallyApproved]
								   ,[IsCMManagerApproved]
								   ,[IsDemandPlanningApproved]
								   ,[IsDemandFinanceApproved]
								   ,[PlanPromoXSites]
								   ,[PlanPromoCatalogue]
								   ,[PlanPromoPOSMInClient]
								   ,[PlanPromoCostProdXSites]
								   ,[PlanPromoCostProdCatalogue]
								   ,[PlanPromoCostProdPOSMInClient]
								   ,[ActualPromoXSites]
								   ,[ActualPromoCatalogue]
								   ,[ActualPromoPOSMInClient]
								   ,[ActualPromoCostProdXSites]
								   ,[ActualPromoCostProdCatalogue]
								   ,[ActualPromoCostProdPOSMInClient]
								   ,[MechanicComment]
								   ,[PlanInstoreMechanicDiscount]
								   ,[CalendarPriority]
								   ,[PlanPromoTIShopper]
								   ,[PlanPromoTIMarketing]
								   ,[PlanPromoBranding]
								   ,[PlanPromoCost]
								   ,[PlanPromoBTL]
								   ,[PlanPromoCostProduction]
								   ,[PlanPromoUpliftPercent]
								   ,[PlanPromoIncrementalLSV]
								   ,[PlanPromoLSV]
								   ,[PlanPromoROIPercent]
								   ,[PlanPromoIncrementalNSV]
								   ,[PlanPromoNetIncrementalNSV]
								   ,[PlanPromoIncrementalMAC]
								   ,[ActualPromoTIShopper]
								   ,[ActualPromoTIMarketing]
								   ,[ActualPromoBranding]
								   ,[ActualPromoBTL]
								   ,[ActualPromoCostProduction]
								   ,[ActualPromoCost]
								   ,[ActualPromoUpliftPercent]
								   ,[ActualPromoIncrementalLSV]
								   ,[ActualPromoLSV]
								   ,[ActualPromoROIPercent]
								   ,[ActualPromoIncrementalNSV]
								   ,[ActualPromoNetIncrementalNSV]
								   ,[ActualPromoIncrementalMAC]
								   ,[ActualInStoreMechanicId]
								   ,[ActualInStoreMechanicTypeId]
								   ,[PromoDuration]
								   ,[DispatchDuration]
								   ,[InvoiceNumber]
								   ,[PlanPromoBaselineLSV]
								   ,[PlanPromoIncrementalBaseTI]
								   ,[PlanPromoIncrementalCOGS]
								   ,[PlanPromoTotalCost]
								   ,[PlanPromoNetIncrementalLSV]
								   ,[PlanPromoNetLSV]
								   ,[PlanPromoNetIncrementalMAC]
								   ,[PlanPromoIncrementalEarnings]
								   ,[PlanPromoNetIncrementalEarnings]
								   ,[PlanPromoNetROIPercent]
								   ,[PlanPromoNetUpliftPercent]
								   ,[ActualPromoBaselineLSV]
								   ,[ActualInStoreDiscount]
								   ,[ActualInStoreShelfPrice]
								   ,[ActualPromoIncrementalBaseTI]
								   ,[ActualPromoIncrementalCOGS]
								   ,[ActualPromoTotalCost]
								   ,[ActualPromoNetIncrementalLSV]
								   ,[ActualPromoNetLSV]
								   ,[ActualPromoNetIncrementalMAC]
								   ,[ActualPromoIncrementalEarnings]
								   ,[ActualPromoNetIncrementalEarnings]
								   ,[ActualPromoNetROIPercent]
								   ,[ActualPromoNetUpliftPercent]
								   ,[Calculating]
								   ,[BlockInformation]
								   ,[PlanPromoBaselineBaseTI]
								   ,[PlanPromoBaseTI]
								   ,[PlanPromoNetNSV]
								   ,[ActualPromoBaselineBaseTI]
								   ,[ActualPromoBaseTI]
								   ,[ActualPromoNetNSV]
								   ,[ProductSubrangesList]
								   ,[ClientTreeKeyId]
								   ,[InOut]
								   ,[PlanPromoNetIncrementalBaseTI]
								   ,[PlanPromoNetIncrementalCOGS]
								   ,[ActualPromoNetIncrementalBaseTI]
								   ,[ActualPromoNetIncrementalCOGS]
								   ,[PlanPromoNetBaseTI]
								   ,[PlanPromoNSV]
								   ,[ActualPromoNetBaseTI]
								   ,[ActualPromoNSV]
								   ,[ActualPromoLSVByCompensation]
								   ,[PlanInStoreShelfPrice]
								   ,[PlanPromoPostPromoEffectLSVW1]
								   ,[PlanPromoPostPromoEffectLSVW2]
								   ,[PlanPromoPostPromoEffectLSV]
								   ,[ActualPromoPostPromoEffectLSVW1]
								   ,[ActualPromoPostPromoEffectLSVW2]
								   ,[ActualPromoPostPromoEffectLSV]
								   ,[LoadFromTLC]
								   ,[InOutProductIds]
								   ,[InOutExcludeAssortmentMatrixProductsButtonPressed]
								   ,[DocumentNumber]
								   ,[LastChangedDate]
								   ,[LastChangedDateDemand]
								   ,[LastChangedDateFinance]
								   ,[RegularExcludedProductIds]
								   ,[AdditionalUserTimestamp]
								   ,[IsGrowthAcceleration]
								   ,[PromoTypesId]
								   ,[CreatorLogin]
								   ,[PlanTIBasePercent]
								   ,[PlanCOGSPercent]
								   ,[ActualTIBasePercent]
								   ,[ActualCOGSPercent]
								   ,[SumInvoice]
								   ,[IsOnInvoice]
								   ,[ActualPromoLSVSI]
								   ,[ActualPromoLSVSO]
								   ,[IsApolloExport]
								   ,[DeviationCoefficient]
								   ,[UseActualTI]
								   ,[UseActualCOGS]
								   ,[BudgetYear]
								   ,[PlanAddTIShopperApproved]
								   ,[PlanAddTIShopperCalculated]
								   ,[PlanAddTIMarketingApproved]
								   ,[ActualAddTIShopper]
								   ,[ActualAddTIMarketing]
								   ,[ProductSubrangesListRU]
								   ,[ManualInputSumInvoice])
								select 
									NEWID()
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[BrandId]
								   ,[BrandTechId]
								   ,[PromoStatusId]
								   ,[Name]
								   ,[StartDate]
								   ,[EndDate]
								   ,[DispatchesStart]
								   ,[DispatchesEnd]
								   ,[ColorId]
								   ,[Number]
								   ,[RejectReasonId]
								   ,[EventId]
								   ,[MarsMechanicId]
								   ,[MarsMechanicTypeId]
								   ,[PlanInstoreMechanicId]
								   ,[PlanInstoreMechanicTypeId]
								   ,[MarsMechanicDiscount]
								   ,[OtherEventName]
								   ,[EventName]
								   ,[ClientHierarchy]
								   ,[ProductHierarchy]
								   ,[CreatorId]
								   ,[MarsStartDate]
								   ,[MarsEndDate]
								   ,[MarsDispatchesStart]
								   ,[MarsDispatchesEnd]
								   ,[ClientTreeId]
								   ,[BaseClientTreeId]
								   ,[Mechanic]
								   ,[MechanicIA]
								   ,[BaseClientTreeIds]
								   ,[LastApprovedDate]
								   ,[TechnologyId]
								   ,[NeedRecountUplift]
								   ,[IsAutomaticallyApproved]
								   ,[IsCMManagerApproved]
								   ,[IsDemandPlanningApproved]
								   ,[IsDemandFinanceApproved]
								   ,[PlanPromoXSites]
								   ,[PlanPromoCatalogue]
								   ,[PlanPromoPOSMInClient]
								   ,[PlanPromoCostProdXSites]
								   ,[PlanPromoCostProdCatalogue]
								   ,[PlanPromoCostProdPOSMInClient]
								   ,[ActualPromoXSites]
								   ,[ActualPromoCatalogue]
								   ,[ActualPromoPOSMInClient]
								   ,[ActualPromoCostProdXSites]
								   ,[ActualPromoCostProdCatalogue]
								   ,[ActualPromoCostProdPOSMInClient]
								   ,[MechanicComment]
								   ,[PlanInstoreMechanicDiscount]
								   ,[CalendarPriority]
								   ,[PlanPromoTIShopper]
								   ,[PlanPromoTIMarketing]
								   ,[PlanPromoBranding]
								   ,[PlanPromoCost]
								   ,[PlanPromoBTL]
								   ,[PlanPromoCostProduction]
								   ,[PlanPromoUpliftPercent]
								   ,[PlanPromoIncrementalLSV]
								   ,[PlanPromoLSV]
								   ,[PlanPromoROIPercent]
								   ,[PlanPromoIncrementalNSV]
								   ,[PlanPromoNetIncrementalNSV]
								   ,[PlanPromoIncrementalMAC]
								   ,[ActualPromoTIShopper]
								   ,[ActualPromoTIMarketing]
								   ,[ActualPromoBranding]
								   ,[ActualPromoBTL]
								   ,[ActualPromoCostProduction]
								   ,[ActualPromoCost]
								   ,[ActualPromoUpliftPercent]
								   ,[ActualPromoIncrementalLSV]
								   ,[ActualPromoLSV]
								   ,[ActualPromoROIPercent]
								   ,[ActualPromoIncrementalNSV]
								   ,[ActualPromoNetIncrementalNSV]
								   ,[ActualPromoIncrementalMAC]
								   ,[ActualInStoreMechanicId]
								   ,[ActualInStoreMechanicTypeId]
								   ,[PromoDuration]
								   ,[DispatchDuration]
								   ,[InvoiceNumber]
								   ,[PlanPromoBaselineLSV]
								   ,[PlanPromoIncrementalBaseTI]
								   ,[PlanPromoIncrementalCOGS]
								   ,[PlanPromoTotalCost]
								   ,[PlanPromoNetIncrementalLSV]
								   ,[PlanPromoNetLSV]
								   ,[PlanPromoNetIncrementalMAC]
								   ,[PlanPromoIncrementalEarnings]
								   ,[PlanPromoNetIncrementalEarnings]
								   ,[PlanPromoNetROIPercent]
								   ,[PlanPromoNetUpliftPercent]
								   ,[ActualPromoBaselineLSV]
								   ,[ActualInStoreDiscount]
								   ,[ActualInStoreShelfPrice]
								   ,[ActualPromoIncrementalBaseTI]
								   ,[ActualPromoIncrementalCOGS]
								   ,[ActualPromoTotalCost]
								   ,[ActualPromoNetIncrementalLSV]
								   ,[ActualPromoNetLSV]
								   ,[ActualPromoNetIncrementalMAC]
								   ,[ActualPromoIncrementalEarnings]
								   ,[ActualPromoNetIncrementalEarnings]
								   ,[ActualPromoNetROIPercent]
								   ,[ActualPromoNetUpliftPercent]
								   ,[Calculating]
								   ,[BlockInformation]
								   ,[PlanPromoBaselineBaseTI]
								   ,[PlanPromoBaseTI]
								   ,[PlanPromoNetNSV]
								   ,[ActualPromoBaselineBaseTI]
								   ,[ActualPromoBaseTI]
								   ,[ActualPromoNetNSV]
								   ,[ProductSubrangesList]
								   ,[ClientTreeKeyId]
								   ,[InOut]
								   ,[PlanPromoNetIncrementalBaseTI]
								   ,[PlanPromoNetIncrementalCOGS]
								   ,[ActualPromoNetIncrementalBaseTI]
								   ,[ActualPromoNetIncrementalCOGS]
								   ,[PlanPromoNetBaseTI]
								   ,[PlanPromoNSV]
								   ,[ActualPromoNetBaseTI]
								   ,[ActualPromoNSV]
								   ,[ActualPromoLSVByCompensation]
								   ,[PlanInStoreShelfPrice]
								   ,[PlanPromoPostPromoEffectLSVW1]
								   ,[PlanPromoPostPromoEffectLSVW2]
								   ,[PlanPromoPostPromoEffectLSV]
								   ,[ActualPromoPostPromoEffectLSVW1]
								   ,[ActualPromoPostPromoEffectLSVW2]
								   ,[ActualPromoPostPromoEffectLSV]
								   ,[LoadFromTLC]
								   ,[InOutProductIds]
								   ,[InOutExcludeAssortmentMatrixProductsButtonPressed]
								   ,[DocumentNumber]
								   ,[LastChangedDate]
								   ,[LastChangedDateDemand]
								   ,[LastChangedDateFinance]
								   ,[RegularExcludedProductIds]
								   ,[AdditionalUserTimestamp]
								   ,[IsGrowthAcceleration]
								   ,[PromoTypesId]
								   ,[CreatorLogin]
								   ,[PlanTIBasePercent]
								   ,[PlanCOGSPercent]
								   ,[ActualTIBasePercent]
								   ,[ActualCOGSPercent]
								   ,[SumInvoice]
								   ,[IsOnInvoice]
								   ,[ActualPromoLSVSI]
								   ,[ActualPromoLSVSO]
								   ,[IsApolloExport]
								   ,[DeviationCoefficient]
								   ,[UseActualTI]
								   ,[UseActualCOGS]
								   ,[BudgetYear]
								   ,[PlanAddTIShopperApproved]
								   ,[PlanAddTIShopperCalculated]
								   ,[PlanAddTIMarketingApproved]
								   ,[ActualAddTIShopper]
								   ,[ActualAddTIMarketing]
								   ,[ProductSubrangesListRU]
								   ,[ManualInputSumInvoice] 
								from [DefaultSchemaSetting].[TEMP_SCENARIO_PROMO];

						declare @newStartWith bigint;
						set @newStartWith = (select max(Number) + 1 from DefaultSchemaSetting.Promo);
						declare @sql nvarchar(max)
						set @sql = N'alter sequence [DefaultSchemaSetting].[PromoNumberSequence] restart with ' + cast(@newStartWith as nvarchar(50)) + ';';
						exec SP_EXECUTESQL @sql;

						ENABLE TRIGGER [Promo_increment_number] ON [DefaultSchemaSetting].[Promo];
						DISABLE TRIGGER [PromoScenario_ChangesIncident_Insert_Trigger] ON [DefaultSchemaSetting].[Promo];
					END
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[AddScenarioPromoProduct]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO

			CREATE   PROCEDURE [DefaultSchemaSetting].[AddScenarioPromoProduct]
							AS
							BEGIN
								DECLARE AddScenarioPromoProductCursor CURSOR FAST_FORWARD
								FOR 
								SELECT
									tpp.[Disabled],
									tpp.[DeletedDate],
									tpp.[ZREP],
									(SELECT Id FROM [DefaultSchemaSetting].[Promo] p WHERE p.Number = tpp.PromoNumber AND p.Disabled = 0),
									tpp.[ProductId],
									tpp.[EAN_Case],
									tpp.[PlanProductPCQty],
									tpp.[PlanProductPCLSV],
									tpp.[ActualProductPCQty],
									tpp.[ActualProductUOM],
									tpp.[ActualProductSellInPrice],
									tpp.[ActualProductShelfDiscount],
									tpp.[ActualProductPCLSV],
									tpp.[ActualProductUpliftPercent],
									tpp.[ActualProductIncrementalPCQty],
									tpp.[ActualProductIncrementalPCLSV],
									tpp.[ActualProductIncrementalLSV],
									tpp.[PlanProductUpliftPercent],
									tpp.[ActualProductLSV],
									tpp.[PlanProductBaselineLSV],
									tpp.[ActualProductPostPromoEffectQty],
									tpp.[PlanProductPostPromoEffectQty],
									tpp.[ProductEN],
									tpp.[PlanProductCaseQty],
									tpp.[PlanProductBaselineCaseQty],
									tpp.[ActualProductCaseQty],
									tpp.[PlanProductPostPromoEffectLSVW1],
									tpp.[PlanProductPostPromoEffectLSVW2],
									tpp.[PlanProductPostPromoEffectLSV],
									tpp.[ActualProductPostPromoEffectLSV],
									tpp.[PlanProductIncrementalCaseQty],
									tpp.[ActualProductPostPromoEffectQtyW1],
									tpp.[ActualProductPostPromoEffectQtyW2],
									tpp.[PlanProductPostPromoEffectQtyW1],
									tpp.[PlanProductPostPromoEffectQtyW2],
									tpp.[PlanProductPCPrice],
									tpp.[ActualProductBaselineLSV],
									tpp.[EAN_PC],
									tpp.[PlanProductCaseLSV],
									tpp.[ActualProductLSVByCompensation],
									tpp.[PlanProductLSV],
									tpp.[PlanProductIncrementalLSV],
									tpp.[AverageMarker],
									tpp.[Price],
									tpp.[SumInvoiceProduct],
									tpp.[ActualProductBaselineCaseQty],
									tpp.[CreateDate]
								FROM [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCT] tpp;

								DECLARE
									@Disabled bit,
									@DeletedDate datetimeoffset(7),
									@ZREP nvarchar(255),
									@PromoId uniqueidentifier,
									@ProductId uniqueidentifier,
									@EAN_Case nvarchar(255),
									@PlanProductPCQty int,
									@PlanProductPCLSV float,
									@ActualProductPCQty int,
									@ActualProductUOM nvarchar(max),
									@ActualProductSellInPrice float,
									@ActualProductShelfDiscount float,
									@ActualProductPCLSV float,
									@ActualProductUpliftPercent float,
									@ActualProductIncrementalPCQty float,
									@ActualProductIncrementalPCLSV float,
									@ActualProductIncrementalLSV float,
									@PlanProductUpliftPercent float,
									@ActualProductLSV float,
									@PlanProductBaselineLSV float,
									@ActualProductPostPromoEffectQty float,
									@PlanProductPostPromoEffectQty float,
									@ProductEN nvarchar(max),
									@PlanProductCaseQty float,
									@PlanProductBaselineCaseQty float,
									@ActualProductCaseQty float,
									@PlanProductPostPromoEffectLSVW1 float,
									@PlanProductPostPromoEffectLSVW2 float,
									@PlanProductPostPromoEffectLSV float,
									@ActualProductPostPromoEffectLSV float,
									@PlanProductIncrementalCaseQty float,
									@ActualProductPostPromoEffectQtyW1 float,
									@ActualProductPostPromoEffectQtyW2 float,
									@PlanProductPostPromoEffectQtyW1 float,
									@PlanProductPostPromoEffectQtyW2 float,
									@PlanProductPCPrice float,
									@ActualProductBaselineLSV float,
									@EAN_PC nvarchar(255),
									@PlanProductCaseLSV float,
									@ActualProductLSVByCompensation float,
									@PlanProductLSV float,
									@PlanProductIncrementalLSV float,
									@AverageMarker bit,
									@Price float,
									@SumInvoiceProduct float,
									@ActualProductBaselineCaseQty float,
									@CreateDate datetimeoffset(7);

								OPEN AddScenarioPromoProductCursor;
								WHILE 1 = 1
								BEGIN
									FETCH NEXT 
										FROM AddScenarioPromoProductCursor 
										INTO
											@Disabled,
											@DeletedDate,
											@ZREP,
											@PromoId,
											@ProductId,
											@EAN_Case,
											@PlanProductPCQty,
											@PlanProductPCLSV,
											@ActualProductPCQty,
											@ActualProductUOM,
											@ActualProductSellInPrice,
											@ActualProductShelfDiscount,
											@ActualProductPCLSV,
											@ActualProductUpliftPercent,
											@ActualProductIncrementalPCQty,
											@ActualProductIncrementalPCLSV,
											@ActualProductIncrementalLSV,
											@PlanProductUpliftPercent,
											@ActualProductLSV,
											@PlanProductBaselineLSV,
											@ActualProductPostPromoEffectQty,
											@PlanProductPostPromoEffectQty,
											@ProductEN,
											@PlanProductCaseQty,
											@PlanProductBaselineCaseQty,
											@ActualProductCaseQty,
											@PlanProductPostPromoEffectLSVW1,
											@PlanProductPostPromoEffectLSVW2,
											@PlanProductPostPromoEffectLSV,
											@ActualProductPostPromoEffectLSV,
											@PlanProductIncrementalCaseQty,
											@ActualProductPostPromoEffectQtyW1,
											@ActualProductPostPromoEffectQtyW2,
											@PlanProductPostPromoEffectQtyW1,
											@PlanProductPostPromoEffectQtyW2,
											@PlanProductPCPrice,
											@ActualProductBaselineLSV,
											@EAN_PC,
											@PlanProductCaseLSV,
											@ActualProductLSVByCompensation,
											@PlanProductLSV,
											@PlanProductIncrementalLSV,
											@AverageMarker,
											@Price,
											@SumInvoiceProduct,
											@ActualProductBaselineCaseQty,
											@CreateDate;

									IF (SELECT FETCH_STATUS FROM SYS.DM_EXEC_CURSORS(0) WHERE NAME = 'AddScenarioPromoProductCursor') <> 0
										BREAK;
				
									INSERT INTO [DefaultSchemaSetting].[PromoProduct]
											VALUES (
												NEWID(),
												@Disabled,
												@DeletedDate,
												@ZREP,
												@PromoId,
												@ProductId,
												@EAN_Case,
												@PlanProductPCQty,
												@PlanProductPCLSV,
												@ActualProductPCQty,
												@ActualProductUOM,
												@ActualProductSellInPrice,
												@ActualProductShelfDiscount,
												@ActualProductPCLSV,
												@ActualProductUpliftPercent,
												@ActualProductIncrementalPCQty,
												@ActualProductIncrementalPCLSV,
												@ActualProductIncrementalLSV,
												@PlanProductUpliftPercent,
												@ActualProductLSV,
												@PlanProductBaselineLSV,
												@ActualProductPostPromoEffectQty,
												@PlanProductPostPromoEffectQty,
												@ProductEN,
												@PlanProductCaseQty,
												@PlanProductBaselineCaseQty,
												@ActualProductCaseQty,
												@PlanProductPostPromoEffectLSVW1,
												@PlanProductPostPromoEffectLSVW2,
												@PlanProductPostPromoEffectLSV,
												@ActualProductPostPromoEffectLSV,
												@PlanProductIncrementalCaseQty,
												@ActualProductPostPromoEffectQtyW1,
												@ActualProductPostPromoEffectQtyW2,
												@PlanProductPostPromoEffectQtyW1,
												@PlanProductPostPromoEffectQtyW2,
												@PlanProductPCPrice,
												@ActualProductBaselineLSV,
												@EAN_PC,
												@PlanProductCaseLSV,
												@ActualProductLSVByCompensation,
												@PlanProductLSV,
												@PlanProductIncrementalLSV,
												@AverageMarker,
												@Price,
												@SumInvoiceProduct,
												@ActualProductBaselineCaseQty,
												@CreateDate
											);
										END;
								CLOSE AddScenarioPromoProductCursor;
								DEALLOCATE AddScenarioPromoProductCursor;
		
							END;
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[AddScenarioPromoProductsCorrection]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO

			CREATE   PROCEDURE [DefaultSchemaSetting].[AddScenarioPromoProductsCorrection]
					AS
					BEGIN
						INSERT INTO [DefaultSchemaSetting].[PromoProductsCorrection]
								   ([Id]
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[PromoProductId]
								   ,[PlanProductUpliftPercentCorrected]
								   ,[UserId]
								   ,[CreateDate]
								   ,[ChangeDate]
								   ,[UserName]
								   ,[TempId])
								select 
									NEWID()
								   ,[Disabled]
								   ,[DeletedDate]
								   ,(SELECT Id FROM [DefaultSchemaSetting].[PromoProduct] pp 
										WHERE pp.PromoId = (SELECT Id FROM [DefaultSchemaSetting].[Promo] p WHERE p.Number = ppc.PromoNumber AND p.Disabled = 0) 
										AND pp.ZREP = ppc.ZREP AND pp.Disabled = 0
									)
								   ,[PlanProductUpliftPercentCorrected]
								   ,[UserId]
								   ,[CreateDate]
								   ,[ChangeDate]
								   ,[UserName]
								   ,[TempId] 
								from [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCTSCORRECTION] ppc
								where ppc.Disabled = 0
					END
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[AddScenarioPromoProductTree]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO


			CREATE   PROCEDURE [DefaultSchemaSetting].[AddScenarioPromoProductTree]
					AS
					BEGIN
						INSERT INTO [DefaultSchemaSetting].[PromoProductTree]
								   ([Id]
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[PromoId]
								   ,[ProductTreeObjectId])
								select 
									NEWID()
								   ,[Disabled]
								   ,[DeletedDate]
								   ,(SELECT Id FROM [DefaultSchemaSetting].[Promo] p WHERE p.Number = ppt.PromoNumber AND p.Disabled = 0)
								   ,[ProductTreeObjectId]
								from [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOPRODUCTTREE] ppt
					END
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[AddScenarioPromoSupport]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO




			CREATE   PROCEDURE [DefaultSchemaSetting].[AddScenarioPromoSupport]
					AS
					BEGIN
						DISABLE TRIGGER [PromoSupport_Increment_Number] ON [DefaultSchemaSetting].[PromoSupport];

						INSERT INTO [DefaultSchemaSetting].[PromoSupport]
								   ([Id]
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[ClientTreeId]
								   ,[BudgetSubItemId]
								   ,[StartDate]
								   ,[EndDate]
								   ,[PlanQuantity]
								   ,[ActualQuantity]
								   ,[PlanCostTE]
								   ,[ActualCostTE]
								   ,[PlanProdCostPer1Item]
								   ,[ActualProdCostPer1Item]
								   ,[PlanProdCost]
								   ,[ActualProdCost]
								   ,[UserTimestamp]
								   ,[AttachFileName]
								   ,[BorderColor]
								   ,[Number]
								   ,[PONumber]
								   ,[InvoiceNumber]
								   ,[OffAllocation])
								select 
									NEWID()
								   ,[Disabled]
								   ,[DeletedDate]
								   ,[ClientTreeId]
								   ,[BudgetSubItemId]
								   ,[StartDate]
								   ,[EndDate]
								   ,[PlanQuantity]
								   ,[ActualQuantity]
								   ,[PlanCostTE]
								   ,[ActualCostTE]
								   ,[PlanProdCostPer1Item]
								   ,[ActualProdCostPer1Item]
								   ,[PlanProdCost]
								   ,[ActualProdCost]
								   ,[UserTimestamp]
								   ,[AttachFileName]
								   ,[BorderColor]
								   ,[Number]
								   ,[PONumber]
								   ,[InvoiceNumber]
								   ,[OffAllocation]
								from [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOSUPPORT];

						declare @newStartWith bigint;
						set @newStartWith = (select max(Number) + 1 from DefaultSchemaSetting.PromoSupport);
						declare @sql nvarchar(max)
						set @sql = N'alter sequence [DefaultSchemaSetting].[PromoSupportNumberSequence] restart with ' + cast(@newStartWith as nvarchar(50)) + ';';
						exec SP_EXECUTESQL @sql;

						ENABLE TRIGGER [PromoSupport_Increment_Number] ON [DefaultSchemaSetting].[PromoSupport];
					END
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[AddScenarioPromoSupportPromo]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			CREATE   PROCEDURE [DefaultSchemaSetting].[AddScenarioPromoSupportPromo]
							AS
							BEGIN
								DECLARE AddScenarioPromoSupportPromoCursor CURSOR FAST_FORWARD
								FOR 
								SELECT
									psp.[Disabled]
								   ,psp.[DeletedDate]
								   ,(SELECT Id FROM [DefaultSchemaSetting].[Promo] p WHERE p.Number = psp.PromoNumber AND p.Disabled = 0)
								   ,(SELECT Id FROM [DefaultSchemaSetting].[PromoSupport] ps WHERE ps.Number = psp.PromoSupportNumber AND ps.Disabled = 0)
								   ,psp.[PlanCalculation]
								   ,psp.[FactCalculation]
								   ,psp.[PlanCostProd]
								   ,psp.[FactCostProd]
								FROM [DefaultSchemaSetting].[TEMP_SCENARIO_PROMOSUPPORTPROMO] psp;

								DECLARE
									@Disabled bit,
									@DeletedDate datetimeoffset(7),
									@PromoId uniqueidentifier,
									@PromoSupportId uniqueidentifier,
									@PlanCalculation float,
									@FactCalculation float,
									@PlanCostProd float,
									@FactCostProd float;

								OPEN AddScenarioPromoSupportPromoCursor;
								WHILE 1 = 1
								BEGIN
									FETCH NEXT 
										FROM AddScenarioPromoSupportPromoCursor 
										INTO
											@Disabled,
											@DeletedDate,
											@PromoId,
											@PromoSupportId,
											@PlanCalculation,
											@FactCalculation,
											@PlanCostProd,
											@FactCostProd;

									IF (SELECT FETCH_STATUS FROM SYS.DM_EXEC_CURSORS(0) WHERE NAME = 'AddScenarioPromoSupportPromoCursor') <> 0
										BREAK;

									IF (@PromoSupportId IS NOT NULL)
									BEGIN
										INSERT INTO [DefaultSchemaSetting].[PromoSupportPromo]
											VALUES (
												 NEWID()
												,@Disabled
												,@DeletedDate
												,@PromoId
												,@PromoSupportId
												,@PlanCalculation
												,@FactCalculation
												,@PlanCostProd
												,@FactCostProd
											);
									END ELSE CONTINUE
									END;
								CLOSE AddScenarioPromoSupportPromoCursor;
								DEALLOCATE AddScenarioPromoSupportPromoCursor;
		
							END;
			GO
			/****** Object:  StoredProcedure [DefaultSchemaSetting].[DisablePreviousScenarioData]    Script Date: 10/25/2021 5:28:45 PM ******/
			SET ANSI_NULLS ON
			GO
			SET QUOTED_IDENTIFIER ON
			GO
			create   procedure [DefaultSchemaSetting].[DisablePreviousScenarioData](
					@ClientList nvarchar(100) = N'',
					@BudgetYear int = 0
				)
				as
				if @ClientList <> N'' and @BudgetYear <> 0 begin
					update [DefaultSchemaSetting].[Promo] set [Disabled] = 1, [DeletedDate] = getdate()
					where ClientTreeId in (select * from string_split(@ClientList, ';')) and BudgetYear = @BudgetYear and Disabled = 0

					update [DefaultSchemaSetting].[PromoProduct] set [Disabled] = 1, [DeletedDate] = getdate()
					where PromoId in (select Id from [DefaultSchemaSetting].[Promo] where ClientTreeId in (select * from string_split(@ClientList, ';')) and BudgetYear = @BudgetYear) and Disabled = 0

					update [DefaultSchemaSetting].[PromoProductsCorrection] set [Disabled] = 1, [DeletedDate] = getdate()
					where PromoProductId in (select Id from [DefaultSchemaSetting].[PromoProduct] where PromoId in (select Id from [DefaultSchemaSetting].[Promo] where ClientTreeId in (select * from string_split(@ClientList, ';')) and BudgetYear = @BudgetYear)) and Disabled = 0

					update [DefaultSchemaSetting].[PromoProductTree] set [Disabled] = 1, [DeletedDate] = getdate()
					where PromoId in (select Id from [DefaultSchemaSetting].[Promo] where ClientTreeId in (select * from string_split(@ClientList, ';')) and BudgetYear = @BudgetYear) and Disabled = 0

					update [DefaultSchemaSetting].[PromoSupport] set [Disabled] = 1, [DeletedDate] = getdate()
					where Id in
					(
						select 
							 ps.Id
						from DefaultSchemaSetting.PromoSupport ps
						join DefaultSchemaSetting.PromoSupportPromo psp on psp.PromoSupportId = ps.Id and psp.Disabled = 0
						join DefaultSchemaSetting.Promo p on p.Id = psp.PromoId and p.Disabled = 0 and p.ClientTreeId in (select * from string_split(@ClientList, ';')) and p.BudgetYear = @BudgetYear
						where ps.Disabled = 0
						group by ps.Id
						having [DefaultSchemaSetting].[GetDistinctItems](string_agg(p.BudgetYear, ';')) = cast(@BudgetYear as nvarchar(50))
					)

					update [DefaultSchemaSetting].[PromoSupportPromo] set [Disabled] = 1, [DeletedDate] = getdate()
					where PromoId in (select Id from [DefaultSchemaSetting].[Promo] where ClientTreeId in (select * from string_split(@ClientList, ';')) and BudgetYear = @BudgetYear) and Disabled = 0

					update [DefaultSchemaSetting].[BTL] set [Disabled] = 1, [DeletedDate] = getdate()
					where Id in
					(
						select 
							 btl.Id
						from DefaultSchemaSetting.BTL btl
						join DefaultSchemaSetting.BTLPromo btlp on btlp.BTLId = btl.Id and btlp.Disabled = 0
						join DefaultSchemaSetting.Promo p on p.Id = btlp.PromoId and p.Disabled = 0 and p.ClientTreeId in (select * from string_split(@ClientList, ';')) and p.BudgetYear = @BudgetYear
						where btl.Disabled = 0
						group by btl.Id
						having [DefaultSchemaSetting].[GetDistinctItems](string_agg(p.BudgetYear, ';')) = cast(@BudgetYear as nvarchar(50))
					)

					update [DefaultSchemaSetting].[BTLPromo] set [Disabled] = 1, [DeletedDate] = getdate()
					where PromoId in (select Id from [DefaultSchemaSetting].[Promo] where ClientTreeId in (select * from string_split(@ClientList, ';')) and BudgetYear = @BudgetYear) and Disabled = 0
				end
			GO
			";
    }
}
