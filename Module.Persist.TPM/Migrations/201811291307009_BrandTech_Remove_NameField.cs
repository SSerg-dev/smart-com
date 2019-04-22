namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandTech_Remove_NameField : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BrandTech", new[] { "BrandId" });
            DropIndex("dbo.BrandTech", new[] { "TechnologyId" });
            AddColumn("dbo.BrandTech", "BrandTechName", c => c.String());
            AlterColumn("dbo.BrandTech", "BrandId", c => c.Guid(nullable: false));
            AlterColumn("dbo.BrandTech", "TechnologyId", c => c.Guid(nullable: false));
            CreateIndex("dbo.BrandTech", "BrandId");
            CreateIndex("dbo.BrandTech", "TechnologyId");
            DropColumn("dbo.BrandTech", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.BrandTech", "Name", c => c.String(maxLength: 255));
            DropIndex("dbo.BrandTech", new[] { "TechnologyId" });
            DropIndex("dbo.BrandTech", new[] { "BrandId" });
            AlterColumn("dbo.BrandTech", "TechnologyId", c => c.Guid());
            AlterColumn("dbo.BrandTech", "BrandId", c => c.Guid());
            DropColumn("dbo.BrandTech", "BrandTechName");
            CreateIndex("dbo.BrandTech", "TechnologyId");
            CreateIndex("dbo.BrandTech", "BrandId");
        }
    }
}
