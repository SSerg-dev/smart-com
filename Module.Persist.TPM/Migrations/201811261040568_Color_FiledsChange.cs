namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Color_FiledsChange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Color", "AgeGroupId", "dbo.AgeGroup");
            DropForeignKey("dbo.Color", "BrandId", "dbo.Brand");
            DropForeignKey("dbo.Color", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Color", "FormatId", "dbo.Format");
            DropForeignKey("dbo.Color", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Color", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.Color", "SegmentId", "dbo.Segment");
            DropForeignKey("dbo.Color", "SubrangeId", "dbo.Subrange");
            DropForeignKey("dbo.Color", "TechHighLevelId", "dbo.TechHighLevel");
            DropForeignKey("dbo.Color", "TechnologyId", "dbo.Technology");
            DropForeignKey("dbo.Color", "VarietyId", "dbo.Variety");
            DropIndex("dbo.Color", new[] { "ProductId" });
            DropIndex("dbo.Color", new[] { "CategoryId" });
            DropIndex("dbo.Color", new[] { "BrandId" });
            DropIndex("dbo.Color", new[] { "SegmentId" });
            DropIndex("dbo.Color", new[] { "TechnologyId" });
            DropIndex("dbo.Color", new[] { "TechHighLevelId" });
            DropIndex("dbo.Color", new[] { "ProgramId" });
            DropIndex("dbo.Color", new[] { "FormatId" });
            DropIndex("dbo.Color", new[] { "SubrangeId" });
            DropIndex("dbo.Color", new[] { "AgeGroupId" });
            DropIndex("dbo.Color", new[] { "VarietyId" });
            DropColumn("dbo.Color", "ProductId");
            DropColumn("dbo.Color", "CategoryId");
            DropColumn("dbo.Color", "BrandId");
            DropColumn("dbo.Color", "SegmentId");
            DropColumn("dbo.Color", "TechnologyId");
            DropColumn("dbo.Color", "TechHighLevelId");
            DropColumn("dbo.Color", "ProgramId");
            DropColumn("dbo.Color", "FormatId");
            DropColumn("dbo.Color", "SubrangeId");
            DropColumn("dbo.Color", "AgeGroupId");
            DropColumn("dbo.Color", "VarietyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Color", "VarietyId", c => c.Guid());
            AddColumn("dbo.Color", "AgeGroupId", c => c.Guid());
            AddColumn("dbo.Color", "SubrangeId", c => c.Guid());
            AddColumn("dbo.Color", "FormatId", c => c.Guid());
            AddColumn("dbo.Color", "ProgramId", c => c.Guid());
            AddColumn("dbo.Color", "TechHighLevelId", c => c.Guid());
            AddColumn("dbo.Color", "TechnologyId", c => c.Guid());
            AddColumn("dbo.Color", "SegmentId", c => c.Guid());
            AddColumn("dbo.Color", "BrandId", c => c.Guid());
            AddColumn("dbo.Color", "CategoryId", c => c.Guid());
            AddColumn("dbo.Color", "ProductId", c => c.Guid());
            CreateIndex("dbo.Color", "VarietyId");
            CreateIndex("dbo.Color", "AgeGroupId");
            CreateIndex("dbo.Color", "SubrangeId");
            CreateIndex("dbo.Color", "FormatId");
            CreateIndex("dbo.Color", "ProgramId");
            CreateIndex("dbo.Color", "TechHighLevelId");
            CreateIndex("dbo.Color", "TechnologyId");
            CreateIndex("dbo.Color", "SegmentId");
            CreateIndex("dbo.Color", "BrandId");
            CreateIndex("dbo.Color", "CategoryId");
            CreateIndex("dbo.Color", "ProductId");
            AddForeignKey("dbo.Color", "VarietyId", "dbo.Variety", "Id");
            AddForeignKey("dbo.Color", "TechnologyId", "dbo.Technology", "Id");
            AddForeignKey("dbo.Color", "TechHighLevelId", "dbo.TechHighLevel", "Id");
            AddForeignKey("dbo.Color", "SubrangeId", "dbo.Subrange", "Id");
            AddForeignKey("dbo.Color", "SegmentId", "dbo.Segment", "Id");
            AddForeignKey("dbo.Color", "ProgramId", "dbo.Program", "Id");
            AddForeignKey("dbo.Color", "ProductId", "dbo.Product", "Id");
            AddForeignKey("dbo.Color", "FormatId", "dbo.Format", "Id");
            AddForeignKey("dbo.Color", "CategoryId", "dbo.Category", "Id");
            AddForeignKey("dbo.Color", "BrandId", "dbo.Brand", "Id");
            AddForeignKey("dbo.Color", "AgeGroupId", "dbo.AgeGroup", "Id");
        }
    }
}
