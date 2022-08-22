namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Edit_Copy_Settings : DbMigration
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

        private string SqlString = @" 
            UPDATE [Setting].[PARAMETERS_Jupiter]
	            SET
		            [Value] = 'Jupiter.Promo;Jupiter.PromoStatus;Jupiter.PromoProduct;Jupiter.Product;Jupiter.ProductTree;Jupiter.PromoProductTree;Jupiter.PriceList;Jupiter.BaseLine;Jupiter.ChangesIncident;Jupiter.ClientTreeBrandTech;Jupiter.ClientTree;Jupiter.ClientTreeHierarchyView;Jupiter.PromoProductsCorrection;Jupiter.IncrementalPromo;Jupiter.PromoStatus;Jupiter.COGS;Jupiter.PlanCOGSTn;Jupiter.TradeInvestment;Jupiter.BTL;Jupiter.BTLPromo;Jupiter.PromoSupport;Jupiter.PromoSupportPromo;Jupiter.BudgetItem;Jupiter.BudgetSubItem;Jupiter.AssortmentMatrix;Jupiter.BrandTech;Jupiter.ActualCOGS;Jupiter.ActualCOGSTn;Jupiter.ActualTradeInvestment;Jupiter.ProductChangeIncident;Jupiter.ServiceInfo;Jupiter.PlanIncrementalReport;Jupiter.RollingVolume;Jupiter.RATIShopper'
	            WHERE 
                    [Key] = 'PromoCalculationEntites'
";
    }
}
