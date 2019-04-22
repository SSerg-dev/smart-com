namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Color_Add_UniqueIndex : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Color", new[] { "BrandTechId" });
            CreateIndex("dbo.Color", new[] { "BrandTechId", "Disabled", "DeletedDate" }, unique: true, name: "Unique_Color");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Color", "Unique_Color");
            CreateIndex("dbo.Color", "BrandTechId");
        }
    }
}
