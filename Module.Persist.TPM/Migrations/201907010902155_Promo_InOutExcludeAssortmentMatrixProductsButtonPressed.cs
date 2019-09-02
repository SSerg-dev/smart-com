namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Promo_InOutExcludeAssortmentMatrixProductsButtonPressed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Promo", "InOutExcludeAssortmentMatrixProductsButtonPressed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Promo", "InOutExcludeAssortmentMatrixProductsButtonPressed");
        }
    }
}
