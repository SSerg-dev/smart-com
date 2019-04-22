namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductTree_BrandIdTechnologyId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductTree", "BrandId", c => c.Guid());
            AddColumn("dbo.ProductTree", "TechnologyId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductTree", "TechnologyId");
            DropColumn("dbo.ProductTree", "BrandId");
        }
    }
}
