namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_BrandFlagAbbr_SubmarkFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "BrandFlagAbbr", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "SubmarkFlag", c => c.String(maxLength: 255));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "SubmarkFlag");
            DropColumn("dbo.Product", "BrandFlagAbbr");
        }
    }
}
