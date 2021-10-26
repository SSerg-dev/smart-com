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
                CREATE SCHEMA RPA_Setting
                               
                CREATE PROCEDURE {defaultSchema}.[UpdatePromoProduct]
                AS
                BEGIN
                    UPDATE {defaultSchema}.[PromoProduct]
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
                    [ActualProductBaselineCaseQty] = tpp.[ActualProductBaselineCaseQty]
                    FROM
                    (SELECT * FROM {defaultSchema}.[TEMP_PROMOPRODUCT]) AS tpp WHERE {defaultSchema}.[PromoProduct].[Id] = tpp.Id
                    END
                        
                    CREATE PROCEDURE {defaultSchema}.[LaunchRecalculationTasks]
                    AS
                    BEGIN
                    UPDATE {defaultSchema}.[LoopHandler]
                    SET
                    [Status] = 'WAITING'
                    WHERE {defaultSchema}.[LoopHandler].[Id] IN (SELECT [TasksToComplete] FROM [RPA_Setting].[PARAMETERS])
                    END
                    GO"
                    );}
        
        public override void Down()
        {
        }
    }
}
