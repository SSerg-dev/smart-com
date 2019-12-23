namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_AddFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Product", "Brand", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "Brand_code", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "Technology", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "Tech_code", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "BrandTech", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "BrandTech_code", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "Segmen_code", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "Division", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Product", "Division");
            DropColumn("dbo.Product", "Segmen_code");
            DropColumn("dbo.Product", "BrandTech_code");
            DropColumn("dbo.Product", "BrandTech");
            DropColumn("dbo.Product", "Tech_code");
            DropColumn("dbo.Product", "Technology");
            DropColumn("dbo.Product", "Brand_code");
            DropColumn("dbo.Product", "Brand");
        }
    }
}
