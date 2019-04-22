namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_Filter : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTree", "Filter", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductTree", "Filter");
        }
    }
}
