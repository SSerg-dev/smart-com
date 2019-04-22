namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandTech_AddIndexUnique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BrandTech", new[] { "BrandId" });
            DropIndex("dbo.BrandTech", new[] { "TechnologyId" });
            CreateIndex("dbo.BrandTech", new[] { "BrandId", "TechnologyId", "Disabled", "DeletedDate" }, unique: true, name: "Unique_BrandTech");
        }
        
        public override void Down()
        {
            DropIndex("dbo.BrandTech", "Unique_BrandTech");
            CreateIndex("dbo.BrandTech", "TechnologyId");
            CreateIndex("dbo.BrandTech", "BrandId");
        }
    }
}
