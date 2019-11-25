namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductChangeINcident_UpdateFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductChangeIncident", "RecalculatedPromoId", c => c.Guid(nullable: false));
            AddColumn("dbo.ProductChangeIncident", "AddedProductIds", c => c.String());
            AddColumn("dbo.ProductChangeIncident", "ExcludedProductIds", c => c.String());
            AddColumn("dbo.ProductChangeIncident", "IsChecked", c => c.Boolean(nullable: false));
            AddColumn("dbo.ProductChangeIncident", "IsRecalculated", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductChangeIncident", "IsRecalculated");
            DropColumn("dbo.ProductChangeIncident", "IsChecked");
            DropColumn("dbo.ProductChangeIncident", "ExcludedProductIds");
            DropColumn("dbo.ProductChangeIncident", "AddedProductIds");
            DropColumn("dbo.ProductChangeIncident", "RecalculatedPromoId");
        }
    }
}
