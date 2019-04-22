namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_UOM_PC2Case : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "UOM_PC2Case", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "UOM_PC2Case");
        }
    }
}
