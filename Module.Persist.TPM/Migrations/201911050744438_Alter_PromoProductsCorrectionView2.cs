namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alter_PromoProductsCorrectionView2 : DbMigration
    {
        public override void Up()
        {
            Sql(@"
                ALTER VIEW [dbo].[PromoProductsView] AS 
                    SELECT 
		                pp.[Id]
                        , pp.[ZREP]
                        , pp.[ProductEN]
                        , pp.[PlanProductBaselineLSV]
		                , PlanProductUpliftPercent = 
			                IIF((SELECT TOP(1) [Id] FROM [dbo].[PromoProductsCorrection] WHERE [PromoProductId] = pp.[Id] and [Disabled] = 0 and [TempId] IS NULL) IS NOT NULL, 
			                (SELECT TOP(1) [PlanProductUpliftPercentCorrected] FROM [dbo].[PromoProductsCorrection] WHERE [PromoProductId] = pp.[Id] and [Disabled] = 0 and [TempId] IS NULL ORDER BY [ChangeDate] DESC), 
			                pp.[PlanProductUpliftPercent])
                        , pp.[PlanProductIncrementalLSV]
                        , pp.[PlanProductLSV]
                        , pp.[PlanProductBaselineCaseQty]
                        , pp.[PlanProductIncrementalCaseQty]
                        , pp.[PlanProductCaseQty]
                        , pp.[AverageMarker]
                        , IsCorrection = 
			                IIF((SELECT TOP(1) [Id] FROM [dbo].[PromoProductsCorrection] WHERE [PromoProductId] = pp.[Id] and [Disabled] = 0 and [TempId] IS NULL) IS NOT NULL,
			                CONVERT(bit, 1),
			                CONVERT(bit, 0))
                    FROM [dbo].[PromoProduct] pp
                    WHERE pp.[Disabled] = 0
            ");

        }
        
        public override void Down()
        {
            Sql(@"
                ALTER VIEW [dbo].[PromoProductsView] AS 
                    SELECT 
		                pp.[Id]
                        , pp.[ZREP]
                        , pp.[ProductEN]
                        , pp.[PlanProductBaselineLSV]
		                , PlanProductUpliftPercent = 
			                IIF((SELECT TOP(1) [Id] FROM [dbo].[PromoProductsCorrection] WHERE [PromoProductId] = pp.[Id] and [Disabled] = 0 and [TempId] IS NULL) IS NOT NULL, 
			                (SELECT TOP(1) [PlanProductUpliftPercentCorrected] FROM [dbo].[PromoProductsCorrection] WHERE [PromoProductId] = pp.[Id] and [Disabled] = 0 ORDER BY [ChangeDate] DESC), 
			                pp.[PlanProductUpliftPercent])
                        , pp.[PlanProductIncrementalLSV]
                        , pp.[PlanProductLSV]
                        , pp.[PlanProductBaselineCaseQty]
                        , pp.[PlanProductIncrementalCaseQty]
                        , pp.[PlanProductCaseQty]
                        , pp.[AverageMarker]
                        , IsCorrection = 
			                IIF((SELECT TOP(1) [Id] FROM [dbo].[PromoProductsCorrection] WHERE [PromoProductId] = pp.[Id] and [Disabled] = 0 and [TempId] IS NULL) IS NOT NULL,
			                CONVERT(bit, 1),
			                CONVERT(bit, 0))
                    FROM [dbo].[PromoProduct] pp
                    WHERE pp.[Disabled] = 0
            ");
        }
    }
}
