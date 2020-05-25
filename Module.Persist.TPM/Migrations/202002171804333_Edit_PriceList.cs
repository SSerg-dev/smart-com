namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_PriceList : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PriceList", "Price", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PriceList", "Price");
        }
    }
}
