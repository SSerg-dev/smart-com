namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_Index_PriceList : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.PriceList", name: "Unique_ProductList", newName: "Unique_PriceList");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.PriceList", name: "Unique_PriceList", newName: "Unique_ProductList");
        }
    }
}
