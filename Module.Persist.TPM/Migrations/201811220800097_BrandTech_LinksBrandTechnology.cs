namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BrandTech_LinksBrandTechnology : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BrandTech", new[] { "Name" });
            AddColumn("dbo.BrandTech", "BrandId", c => c.Guid());
            AddColumn("dbo.BrandTech", "TechnologyId", c => c.Guid());
            AlterColumn("dbo.BrandTech", "Name", c => c.String(maxLength: 255));
            CreateIndex("dbo.BrandTech", "BrandId");
            CreateIndex("dbo.BrandTech", "TechnologyId");
            AddForeignKey("dbo.BrandTech", "BrandId", "dbo.Brand", "Id");
            AddForeignKey("dbo.BrandTech", "TechnologyId", "dbo.Technology", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BrandTech", "TechnologyId", "dbo.Technology");
            DropForeignKey("dbo.BrandTech", "BrandId", "dbo.Brand");
            DropIndex("dbo.BrandTech", new[] { "TechnologyId" });
            DropIndex("dbo.BrandTech", new[] { "BrandId" });
            AlterColumn("dbo.BrandTech", "Name", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.BrandTech", "TechnologyId");
            DropColumn("dbo.BrandTech", "BrandId");
            CreateIndex("dbo.BrandTech", "Name", unique: true);
        }
    }
}
