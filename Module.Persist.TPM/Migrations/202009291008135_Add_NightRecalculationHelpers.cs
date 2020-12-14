namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_NightRecalculationHelpers : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            if (defaultSchema.Equals("Jupiter"))
            {
                Sql($@"
                    IF NOT EXISTS 
                        ( 
                            SELECT  
                                *
                            FROM sys.schemas
                            WHERE   name = 'Jupiter' 
                        )
                        EXEC('CREATE SCHEMA [Jupiter]');
                    GO

                    {AddBlockedPromo}
                    GO
					{AddFileBuffer}
					GO
                    {AddNewBaseline}
                    GO
                    {AddNewPromoProduct}
                    GO
					{GetOutComingFiles}
					GO
                    {CreateNightProcessingWaitHandler}
                    GO
					{SetIncidentsProcessDate}
                    GO
                    {SetNightProcessingProgressDown}
                    GO
                    {SetNightProcessingProgressUp}
                    GO
                    {UpdatePromo}
                    GO
					{UpdateBaseLine}
                    GO
					{UpdateFileBufferStatus}
					GO
					{EnableBaseLine}
                    GO
					{DisableBaseLine}
                    GO
                    {TEMP_BASELINE}
                    GO
                    {TEMP_BLOCKED_PROMO}
                    GO
                    {TEMP_PROMO}
                    GO
                    {TEMP_PROMOPRODUCT}
                    GO
                    {TEMP_PROMOSUPPORTPROMO}
                    GO
					{TEMP_PRODUCTCHANGEINCIDENTS}
					GO
                    {UnblockPromo}
                    GO
					{GetUnprocessedBaselineFiles}
					GO
					{UpdatePromoProductsCorrection}
					GO
                ");
            }
        }

        public override void Down()
        {
            Sql(@"
            IF EXISTS 
                ( 
                    SELECT  
                        *
                    FROM sys.schemas
                    WHERE   name = 'Jupiter' 
                )
            BEGIN
				IF OBJECT_ID('Jupiter.UnblockPromo', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[UnblockPromo]

				IF OBJECT_ID('Jupiter.TEMP_PROMOSUPPORTPROMO', 'U') IS NOT NULL
					DROP TABLE [Jupiter].[TEMP_PROMOSUPPORTPROMO]

				IF OBJECT_ID('Jupiter.TEMP_PROMOPRODUCT', 'U') IS NOT NULL
					DROP TABLE [Jupiter].[TEMP_PROMOPRODUCT]

				IF OBJECT_ID('Jupiter.TEMP_PROMO', 'U') IS NOT NULL
					DROP TABLE [Jupiter].[TEMP_PROMO]

				IF OBJECT_ID('Jupiter.TEMP_BLOCKED_PROMO', 'U') IS NOT NULL
					DROP TABLE [Jupiter].[TEMP_BLOCKED_PROMO]

				IF OBJECT_ID('Jupiter.TEMP_BASELINE', 'U') IS NOT NULL
					DROP TABLE [Jupiter].[TEMP_BASELINE]

				IF OBJECT_ID('Jupiter.UpdatePromo', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[UpdatePromo]

				IF OBJECT_ID('Jupiter.SetNightProcessingProgressUp', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[SetNightProcessingProgressUp]

				IF OBJECT_ID('Jupiter.SetNightProcessingProgressDown', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[SetNightProcessingProgressDown]

				IF OBJECT_ID('Jupiter.CreateNightProcessingWaitHandler', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[CreateNightProcessingWaitHandler]

				IF OBJECT_ID('Jupiter.AddNewPromoProduct', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[AddNewPromoProduct]

				IF OBJECT_ID('Jupiter.AddNewBaseline', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[AddNewBaseline]

				IF OBJECT_ID('Jupiter.UpdateBaseLine', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[UpdateBaseLine]

				IF OBJECT_ID('Jupiter.EnableBaseLine', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[EnableBaseLine]

				IF OBJECT_ID('Jupiter.DisableBaseLine', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[DisableBaseLine]

				IF OBJECT_ID('Jupiter.AddBlockedPromo', 'P') IS NOT NULL
					DROP PROCEDURE [Jupiter].[AddBlockedPromo]
            END;
            ");
        }

        private string AddBlockedPromo =
        @"
            CREATE OR ALTER PROCEDURE [Jupiter].[AddBlockedPromo] AS
            BEGIN
                INSERT INTO [Jupiter].[BlockedPromo]
                        ([Id]
                        ,[Disabled]
                        ,[DeletedDate]
                        ,[PromoId]
                        ,[HandlerId]
                        ,[CreateDate])
	            SELECT
		            NEWID(),
		            0,
		            NULL,
		            tbp.PromoId,
		            tbp.HandlerId,
		            GETDATE()

	            FROM [Jupiter].[TEMP_BLOCKED_PROMO] AS tbp
            END
        ";

		private string AddFileBuffer =
		@"
			CREATE OR ALTER PROCEDURE [Jupiter].[AddFileBuffer] (
				@FileName NVARCHAR(max) = N'',
				@ProcessDate DATETIMEOFFSET,
				@HandlerId UNIQUEIDENTIFIER
			)
				AS

				DECLARE @InterfaceId UNIQUEIDENTIFIER;
				SELECT @InterfaceId = Id FROM [Jupiter].[Interface] WHERE [Name] = 'BASELINE_APOLLO';

				IF @HandlerId IS NULL BEGIN
					SET @ProcessDate = GETDATE()
				END

				INSERT INTO [Jupiter].[FileBuffer]
						([Id]
						,[CreateDate]
						,[InterfaceId]
						,[UserId]
						,[HandlerId]
						,[FileName]
						,[Status]
						,[ProcessDate])
					VALUES
						(NEWID()
						,@ProcessDate
						,@InterfaceId
						,NULL
						,@HandlerId
						,@FileName
						,'NONE'
						,NULL)
		";


		private string AddNewBaseline =
		@"
        CREATE OR ALTER PROCEDURE [Jupiter].[AddNewBaseline]
        AS
        BEGIN
	        INSERT INTO [Jupiter].[BaseLine]
                   ([Id]
                   ,[Disabled]
                   ,[DeletedDate]
                   ,[StartDate]
                   ,[Type]
                   ,[ProductId]
                   ,[LastModifiedDate]
                   ,[DemandCode]
                   ,[InputBaselineQTY]
                   ,[SellInBaselineQTY]
                   ,[SellOutBaselineQTY]
                   ,[NeedProcessing])
	        SELECT
		        NEWID(),
		        0,
		        NULL,
		        [StartDate] = TODATETIMEOFFSET ( tb.[StartDate] , '+03:00' ),
		        1,
		        tb.ProductId,
		        GETDATE(),
		        tb.DemandCode,
		        tb.InputBaselineQTY,
		        NULL,
		        NULL,
		        1
	        FROM [Jupiter].[TEMP_BASELINE] AS tb
        END
        ";
        private string AddNewPromoProduct =
        @"
        CREATE OR ALTER PROCEDURE [Jupiter].[AddNewPromoProduct]
        AS
        BEGIN
	        INSERT INTO [Jupiter].[PromoProduct]
                   ([Id]
                   ,[Disabled]
                   ,[DeletedDate]
                   ,[ZREP]
                   ,[PromoId]
                   ,[ProductId]
                   ,[EAN_Case]
                   ,[PlanProductPCQty]
                   ,[PlanProductPCLSV]
                   ,[ActualProductPCQty]
                   ,[ActualProductUOM]
                   ,[ActualProductSellInPrice]
                   ,[ActualProductShelfDiscount]
                   ,[ActualProductPCLSV]
                   ,[ActualProductUpliftPercent]
                   ,[ActualProductIncrementalPCQty]
                   ,[ActualProductIncrementalPCLSV]
                   ,[ActualProductIncrementalLSV]
                   ,[PlanProductUpliftPercent]
                   ,[ActualProductLSV]
                   ,[PlanProductBaselineLSV]
                   ,[ActualProductPostPromoEffectQty]
                   ,[PlanProductPostPromoEffectQty]
                   ,[ProductEN]
                   ,[PlanProductCaseQty]
                   ,[PlanProductBaselineCaseQty]
                   ,[ActualProductCaseQty]
                   ,[PlanProductPostPromoEffectLSVW1]
                   ,[PlanProductPostPromoEffectLSVW2]
                   ,[PlanProductPostPromoEffectLSV]
                   ,[ActualProductPostPromoEffectLSV]
                   ,[PlanProductIncrementalCaseQty]
                   ,[ActualProductPostPromoEffectQtyW1]
                   ,[ActualProductPostPromoEffectQtyW2]
                   ,[PlanProductPostPromoEffectQtyW1]
                   ,[PlanProductPostPromoEffectQtyW2]
                   ,[PlanProductPCPrice]
                   ,[ActualProductBaselineLSV]
                   ,[EAN_PC]
                   ,[PlanProductCaseLSV]
                   ,[ActualProductLSVByCompensation]
                   ,[PlanProductLSV]
                   ,[PlanProductIncrementalLSV]
                   ,[AverageMarker]
                   ,[Price]
                   ,[InvoiceTotalProduct]
                   ,[ActualProductBaselineCaseQty])
	        SELECT
		        NEWID(),
		        0,
		        tpp.[DeletedDate],
		        tpp.[ZREP],
		        tpp.[PromoId],
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
		        tpp.[InvoiceTotalProduct],
		        tpp.[ActualProductBaselineCaseQty]

	        FROM [Jupiter].[TEMP_PROMOPRODUCT] AS tpp
        END
        ";

		private string GetOutComingFiles =
		@"
			CREATE OR ALTER PROCEDURE [Jupiter].[GetOutComingFiles] 
			AS

				DECLARE @InterfaceId UNIQUEIDENTIFIER;
				SELECT @InterfaceId = Id FROM [Jupiter].[Interface] WHERE [Name] = 'INCREMENTAL_TO_APOLLO';

				SELECT REPLACE([FileName], N'.dat', CONCAT(N'_', FORMAT([CreateDate], N'yyyyMMdd_HHmmss'), N'.dat')) AS [FileName], [CreateDate]
				FROM [Jupiter].[FileBuffer] 
				WHERE InterfaceId = @InterfaceId AND Status = 'NONE'
				ORDER BY [CreateDate]
		";

		private string SetIncidentsProcessDate =
		@"
			CREATE OR ALTER PROCEDURE [Jupiter].[SetIncidentsProcessDate]
				AS
				UPDATE [Jupiter].[ChangesIncident] SET ProcessDate = GETDATE() WHERE [Disabled] = 1 AND ProcessDate IS NULL
				UPDATE [Jupiter].[ProductChangeIncident] SET RecalculationProcessDate = GETDATE() WHERE [Disabled] = 1 AND RecalculationProcessDate IS NULL
		";

		private string CreateNightProcessingWaitHandler =
        @"
        CREATE OR ALTER PROCEDURE [Jupiter].[CreateNightProcessingWaitHandler](
	        @HandlerId nvarchar(100) = N''
        )
        AS

        DECLARE @handlerName VARCHAR(255) = 'Module.Host.TPM.Handlers.NightProcessingWaitHandler'
        DECLARE @NewHandlerId nvarchar(100)
        IF @HandlerId = N'' BEGIN
	        SET @NewHandlerId = NEWID()
        END ELSE BEGIN
	        SET @NewHandlerId = CONVERT(uniqueidentifier, @HandlerId)
        END
	
        INSERT INTO [Jupiter].[LoopHandler] (
	        [Id],
	        [Description],
	        [Name],
	        [ExecutionPeriod],
	        [ExecutionMode],
	        [CreateDate],
	        [LastExecutionDate],
	        [NextExecutionDate],
	        [ConfigurationName],
	        [Status],
	        [RunGroup],
	        [UserId],
	        [RoleId]
        )
        VALUES (
	        @NewHandlerId,
	        N'Wait while night proccess is in progress ',
	        @handlerName,
	        NULL,
	        'SINGLE',
	        SYSDATETIME(),
	        NULL,
	        NULL,
	        'PROCESSING',
	        'WAITING',
	        NULL,
	        NULL,
	        NULL
        )
        ";
        private string SetNightProcessingProgressDown =
		@"
        CREATE OR ALTER PROCEDURE [Jupiter].[SetNightProcessingProgressDown] (@Value nvarchar(100) = 0)
        AS
        UPDATE [Jupiter].[JobFlag] SET [Value] = @Value WHERE [Prefix] = 'NightProcessingProgress'
        ";
        private string SetNightProcessingProgressUp =
        @"
        CREATE OR ALTER PROCEDURE [Jupiter].[SetNightProcessingProgressUp]
        AS
        UPDATE [Jupiter].[JobFlag] SET [Value] = 1 WHERE [Prefix] = 'NightProcessingProgress'
        ";
        private string UpdatePromo =
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
				[DeviationCoefficient] = tp.[DeviationCoefficient]
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
		private string UpdateBaseLine =
		@"
        CREATE OR ALTER PROCEDURE [Jupiter].[UpdateBaseLine]
		AS
		BEGIN
			UPDATE [Jupiter].[BaseLine]
			SET 
				 [Disabled] = tb.[Disabled]
				,[DeletedDate] = tb.[DeletedDate]
				,[StartDate] = TODATETIMEOFFSET ( tb.[StartDate] , '+03:00' )
				,[Type] = tb.[Type]
				,[ProductId] = tb.[ProductId]
				,[LastModifiedDate] = TODATETIMEOFFSET ( tb.[LastModifiedDate] , '+03:00' )
				,[DemandCode] = tb.[DemandCode]
				,[InputBaselineQTY] = tb.[InputBaselineQTY]
				,[SellInBaselineQTY] = tb.[SellInBaselineQTY]
				,[SellOutBaselineQTY] = tb.[SellOutBaselineQTY]
				,[NeedProcessing] = tb.[NeedProcessing]
			FROM
				(SELECT * FROM [Jupiter].[TEMP_BASELINE]) AS tb WHERE [Jupiter].[BaseLine].[Id] = tb.Id
		END
        ";

		private string UpdateFileBufferStatus =
		@"
			CREATE OR ALTER PROCEDURE [Jupiter].[UpdateFileBufferStatus] (
					@Success NVARCHAR(100) = 0,
					@InterfaceName NVARCHAR(255) = N'',
					@CreateDate DATETIMEOFFSET
				)
			AS
				DECLARE @InterfaceId UNIQUEIDENTIFIER;
				SELECT @InterfaceId = Id FROM [Jupiter].[Interface] WHERE [Name] = @InterfaceName;

				IF(@Success = 1)
					UPDATE [Jupiter].[FileBuffer] SET [Status] = 'COMPLETE', [ProcessDate] = GETDATE() WHERE CreateDate = @CreateDate AND InterfaceId = @InterfaceId
				ELSE
					UPDATE [Jupiter].[FileBuffer] SET [Status] = 'ERROR', [ProcessDate] = GETDATE() WHERE CreateDate = @CreateDate AND InterfaceId = @InterfaceId
		";

		private string EnableBaseLine =
		@"
        CREATE OR ALTER PROCEDURE [Jupiter].[EnableBaseLine]
		AS
		BEGIN
			ENABLE TRIGGER [BaseLine_ChangesIncident_Insert_Update_Trigger] ON [Jupiter].[BaseLine];
			ALTER INDEX IX_BaseLine_NonClustered ON [Jupiter].[BaseLine] REBUILD;
		END
        ";
		private string DisableBaseLine =
		@"
        CREATE OR ALTER PROCEDURE [Jupiter].[DisableBaseLine]
		AS
		BEGIN
			DISABLE TRIGGER [BaseLine_ChangesIncident_Insert_Update_Trigger] ON[Jupiter].[BaseLine]
			ALTER INDEX IX_BaseLine_NonClustered ON [Jupiter].[BaseLine] DISABLE
		END
        ";
		private string TEMP_BASELINE =
		@"
        CREATE TABLE [Jupiter].[TEMP_BASELINE](
			[Id] [uniqueidentifier] NULL,
			[Disabled] [bit] NULL,
			[DeletedDate] [datetimeoffset](7) NULL,
			[StartDate] [datetimeoffset](7) NULL,
			[Type] [int] NULL,
			[ProductId] [uniqueidentifier] NULL,
			[LastModifiedDate] [datetimeoffset](7) NULL,
			[DemandCode] [nvarchar](255) NULL,
			[InputBaselineQTY] [float] NULL,
			[SellInBaselineQTY] [float] NULL,
			[SellOutBaselineQTY] [float] NULL,
			[NeedProcessing] [bit] NULL,
		) ON [PRIMARY]
        ";
        private string TEMP_BLOCKED_PROMO =
		@"
        CREATE TABLE [Jupiter].[TEMP_BLOCKED_PROMO](
			[PromoId] [uniqueidentifier] NULL,
			[HandlerId] [uniqueidentifier] NULL
		)ON [PRIMARY]
        ";
        private string TEMP_PROMO =
		@"
        CREATE TABLE [Jupiter].[TEMP_PROMO](
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
			[InvoiceTotal] [float] NULL,
			[IsOnInvoice] [bit] NULL,
			[ActualPromoLSVSI] [float] NULL,
			[ActualPromoLSVSO] [float] NULL,
			[IsApolloExport] [bit] NULL,
			[DeviationCoefficient] [float] NULL,
		) ON [PRIMARY] 
        ";
        private string TEMP_PROMOPRODUCT =
		@"
        CREATE TABLE [Jupiter].[TEMP_PROMOPRODUCT](
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
			[InvoiceTotalProduct] [float] NULL,
			[ActualProductBaselineCaseQty] [float] NULL,
		) ON [PRIMARY]
        ";
        private string TEMP_PROMOSUPPORTPROMO =
		@"
        CREATE TABLE [Jupiter].[TEMP_PROMOSUPPORTPROMO](
			[Id] [uniqueidentifier] NULL,
			[Disabled] [bit] NULL,
			[DeletedDate] [datetimeoffset](7) NULL,
			[PromoId] [uniqueidentifier] NULL,
			[PromoSupportId] [uniqueidentifier] NULL,
			[PlanCalculation] [float] NULL,
			[FactCalculation] [float] NULL,
			[PlanCostProd] [float] NULL,
			[FactCostProd] [float] NULL,
		) ON [PRIMARY]
        ";

		private string TEMP_PRODUCTCHANGEINCIDENTS =
		@"
		CREATE TABLE [Jupiter].[TEMP_PRODUCTCHANGEINCIDENTS](
					[PromoId] [uniqueidentifier] NOT NULL,
					[AddedProductIds] [nvarchar](max) NULL,
					[ExcludedProductIds] [nvarchar](max) NULL,
				) ON [PRIMARY]
		";

		private string UnblockPromo =
		@"
        CREATE PROCEDURE [Jupiter].[UnblockPromo](
			@HandlerId nvarchar(100) = N''
		)
		AS
		IF @HandlerId <> N'' BEGIN
			UPDATE [BlockedPromo] SET [Disabled] = 1, [DeletedDate] = GETDATE()
			WHERE [HandlerId] = @HandlerId
		END
        ";

		private string GetUnprocessedBaselineFiles =
		@"
			CREATE OR ALTER PROCEDURE [Jupiter].[GetUnprocessedBaselineFiles] 
			AS

				DECLARE @InterfaceId UNIQUEIDENTIFIER;
				SELECT @InterfaceId = Id FROM [Jupiter].[Interface] WHERE [Name] = 'BASELINE_APOLLO';

				SELECT REPLACE([FileName], N'.dat', CONCAT(N'_', FORMAT([CreateDate], N'yyyyMMdd_HHmmss'), N'.dat')) AS [FileName], [CreateDate]
				FROM [Jupiter].[FileBuffer] 
				WHERE InterfaceId = @InterfaceId AND Status = 'NONE'
				ORDER BY [CreateDate]
		";

		private string UpdatePromoProductsCorrection =
		@"
			CREATE OR ALTER PROCEDURE [Jupiter].[UpdatePromoProductsCorrection]
					AS
					BEGIN
						UPDATE Jupiter.PromoProductsCorrection SET [Disabled] = 1, DeletedDate = GETDATE()
						WHERE PromoProductId IN (SELECT Id FROM Jupiter.PromoProduct WHERE [Disabled] = 1)
						AND [Disabled] = 0
					END
		";
	}
}
