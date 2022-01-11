namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RPA_AddProcedure : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");

            Sql($@"            
                CREATE TABLE {defaultSchema}.[RPAParameters](
	            [RPAId] [uniqueidentifier] NOT NULL,
	            [TasksToComplete] [nvarchar](max) NOT NULL
                ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
                GO
                CREATE OR ALTER PROCEDURE {defaultSchema}.[UpdatePromoProduct]
                (@RPAId nvarchar(max))
                    AS
                    BEGIN
				    DECLARE @query nvarchar(max)
                    SET @query = N'UPDATE {defaultSchema}.[PromoProduct]
                    SET
                        [Disabled] = tpp.[Disabled],
                        [DeletedDate] = tpp.[DeletedDate],
                        [ZREP] = tpp.[ZREP],
                        [PromoId] = tpp.[PromoId],
                        [ProductId] = tpp.[ProductId],
                        [EAN_Case] = tpp.[EAN_Case],
                        [PlanProductPCQty] = TRY_CAST(TRY_CAST(tpp.[PlanProductPCQty] as float) as int),
                        [PlanProductPCLSV] = tpp.[PlanProductPCLSV],
                        [ActualProductPCQty] = TRY_CAST(TRY_CAST(tpp.[ActualProductPCQty] as float) as int),
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
                        [ActualProductBaselineCaseQty] = tpp.[ActualProductBaselineCaseQty]
                        FROM
					    (SELECT * FROM {defaultSchema}.[TEMP_RPA_PROMOPRODUCT' + @RPAId + ']) AS tpp WHERE {defaultSchema}.[PromoProduct].[Id] = tpp.Id'

					    EXEC sp_executesql @query
                        END
                    GO
                    
                    CREATE OR ALTER PROCEDURE {defaultSchema}.[LaunchRecalculationTasks]
                    (@RPAId nvarchar(max))
                        AS
                        BEGIN
                        DECLARE @buff nvarchar(max)
					    DECLARE @query nvarchar(max)
					    SELECT @buff = TasksToComplete FROM {defaultSchema}.[RPAParameters] WHERE RPAId = @RPAId
					    IF @buff = '' OR @buff = ''''
						    BEGIN
						    return;
						    END 
					    select @query = N'UPDATE {defaultSchema}.[LoopHandler]
                        SET
                        [Status] = ''WAITING''WHERE {defaultSchema}.[LoopHandler].[Id] IN ('+@buff+')'
					    exec sp_executesql @query
					    END
                    GO
                    
                    CREATE OR ALTER PROCEDURE {defaultSchema}.[RemoveTasksAndBlockedPromo]
                    (@RPAId nvarchar(max))
                        AS
                        BEGIN
                        DECLARE @tasks nvarchar(max)
					    DECLARE @promoes nvarchar(max)
					    DECLARE @query nvarchar(max)
					    SELECT @tasks = TasksToComplete FROM {defaultSchema}.[RPAParameters] WHERE RPAId = @RPAId
					    IF @tasks = '' OR @tasks = ''''
						    BEGIN
						    return;
						    END 
					    select @query = N'UPDATE {defaultSchema}.[LoopHandler]
                        SET
                        [Status] = ''ERROR'' WHERE {defaultSchema}.[LoopHandler].[Id] IN ('+@tasks+')'
					    exec sp_executesql @query
					    select @query = N'UPDATE {defaultSchema}.[BlockedPromo]
                            SET
                                [Disabled] = 1 , [DeletedDate] = GETDATE()
                            WHERE {defaultSchema}.[BlockedPromo].[HandlerId] IN ('+@tasks+')'
					    exec sp_executesql @query
					    END
                    GO

                    CREATE OR ALTER PROCEDURE {defaultSchema}.[RpaPipeActual_UpdateRPAStatus]
                    (       
	                    @RPAId nvarchar(max),
					    @Status nvarchar(max)
                    )
                        AS
                        BEGIN

                            SET NOCOUNT ON

                            UPDATE 
		                        {defaultSchema}.[RPA]
	                        SET 
		                        Status = @Status
	                        WHERE 
		                        Id = @RPAId
                        END
                    GO

                    CREATE OR ALTER PROCEDURE {defaultSchema}.[RpaPipeActual_UpdateRPA]
                    (   
	                    @RPAId nvarchar(max),
	                    @LogFileURL nvarchar(max)
                    )
                        AS
                        BEGIN
	                        DECLARE @dropTableQuery nvarchar(max)

                            UPDATE 
		                        {defaultSchema}.[RPA]
	                        SET 		                
		                        LogURL = @LogFileURL
	                        WHERE 
		                        Id = @RPAId
				
					        SET @dropTableQuery = N'DROP TABLE {defaultSchema}.' + QUOTENAME('TEMP_RPA_PROMOPRODUCT'+@RPAId)
					        EXEC sp_executesql @dropTableQuery

                        END
                    GO

                    CREATE OR ALTER PROCEDURE {defaultSchema}.[RpaPipeEvent_UpdateRPA]
                    (   
	                    @RPAId nvarchar(max),
	                    @RunPipeId nvarchar(max),
					    @LogFileURL nvarchar(max)
                    )
                    AS
                    BEGIN
	                    DECLARE @dropTableQuery nvarchar(max)

                        UPDATE 
		                    {defaultSchema}.[RPA]
	                    SET 		                
		                    LogURL = @LogFileURL
	                    WHERE 
		                    Id = @RPAId
				
					    SET @dropTableQuery = N'DROP TABLE {defaultSchema}.' + QUOTENAME('TempEventTestStage'+@RunPipeId)
					    EXEC sp_executesql @dropTableQuery

                    END
                    GO
                    "
                );
        }
        
        public override void Down()
        {
        }
    }
}
