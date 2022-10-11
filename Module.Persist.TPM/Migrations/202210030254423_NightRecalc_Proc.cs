namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NightRecalc_Proc : DbMigration
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

        string SqlString = $@"
			ALTER TABLE [DefaultSchemaSetting].[TEMP_PROMOPRODUCT] ADD TPMmode int
			GO
            ALTER   PROCEDURE [DefaultSchemaSetting].[AddNewPromoProduct]
				AS
				BEGIN
					DECLARE AddNewPromoProductCursor CURSOR FAST_FORWARD
					FOR 
					SELECT
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
						tpp.[SumInvoiceProduct],
						tpp.[ActualProductBaselineCaseQty],
						tpp.PlanProductBaselineVolume,
						tpp.PlanProductPostPromoEffectVolumeW1,
						tpp.PlanProductPostPromoEffectVolumeW2,
						tpp.PlanProductPostPromoEffectVolume,
						tpp.ActualProductQtySO
					FROM [DefaultSchemaSetting].[TEMP_PROMOPRODUCT] tpp;

					DECLARE 
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
						@PlanProductBaselineVolume float,
						@PlanProductPostPromoEffectVolumeW1 float,
						@PlanProductPostPromoEffectVolumeW2 float,
						@PlanProductPostPromoEffectVolume float,
						@ActualProductQtySO float,
						@TPMmode int;

					OPEN AddNewPromoProductCursor;
					WHILE 1 = 1
					BEGIN
						FETCH NEXT 
							FROM AddNewPromoProductCursor 
							INTO 
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
								@PlanProductBaselineVolume,
								@PlanProductPostPromoEffectVolumeW1,
								@PlanProductPostPromoEffectVolumeW2,
								@PlanProductPostPromoEffectVolume,
								@ActualProductQtySO;

						IF (SELECT FETCH_STATUS FROM SYS.DM_EXEC_CURSORS(0) WHERE NAME = 'AddNewPromoProductCursor') <> 0
							BREAK;

						SELECT @TPMmode = TPMmode FROM DefaultSchemaSetting.Promo WHERE Id = @PromoId
				
						INSERT INTO [DefaultSchemaSetting].[PromoProduct]
								VALUES (
									NEWID(),
									0,
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
									NULL,
									@PlanProductBaselineVolume,
									@PlanProductPostPromoEffectVolumeW1,
									@PlanProductPostPromoEffectVolumeW2,
									@PlanProductPostPromoEffectVolume,
									@ActualProductQtySO,
									@TPMmode
								);
							END;
					CLOSE AddNewPromoProductCursor;
					DEALLOCATE AddNewPromoProductCursor;
				END;
        ";
    }
}
