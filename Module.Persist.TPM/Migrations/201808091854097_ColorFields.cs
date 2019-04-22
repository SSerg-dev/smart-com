namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColorFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Color", "Disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Color", "DeletedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn("dbo.Color", "ProductId", c => c.Guid());
            AddColumn("dbo.Color", "CategoryId", c => c.Guid());
            AddColumn("dbo.Color", "BrandId", c => c.Guid());
            AddColumn("dbo.Color", "SegmentId", c => c.Guid());
            AddColumn("dbo.Color", "TechnologyId", c => c.Guid());
            AddColumn("dbo.Color", "TechHighLevelId", c => c.Guid());
            AddColumn("dbo.Color", "ProgramId", c => c.Guid());
            AddColumn("dbo.Color", "FormatId", c => c.Guid());
            AddColumn("dbo.Color", "BrandTechId", c => c.Guid());
            AddColumn("dbo.Color", "SubrangeId", c => c.Guid());
            AddColumn("dbo.Color", "AgeGroupId", c => c.Guid());
            AddColumn("dbo.Color", "VarietyId", c => c.Guid());
            AddColumn("dbo.Color", "SystemName", c => c.String(maxLength: 10));
            CreateIndex("dbo.Color", "ProductId");
            CreateIndex("dbo.Color", "CategoryId");
            CreateIndex("dbo.Color", "BrandId");
            CreateIndex("dbo.Color", "SegmentId");
            CreateIndex("dbo.Color", "TechnologyId");
            CreateIndex("dbo.Color", "TechHighLevelId");
            CreateIndex("dbo.Color", "ProgramId");
            CreateIndex("dbo.Color", "FormatId");
            CreateIndex("dbo.Color", "BrandTechId");
            CreateIndex("dbo.Color", "SubrangeId");
            CreateIndex("dbo.Color", "AgeGroupId");
            CreateIndex("dbo.Color", "VarietyId");
            AddForeignKey("dbo.Color", "AgeGroupId", "dbo.AgeGroup", "Id");
            AddForeignKey("dbo.Color", "BrandId", "dbo.Brand", "Id");
            AddForeignKey("dbo.Color", "BrandTechId", "dbo.BrandTech", "Id");
            AddForeignKey("dbo.Color", "CategoryId", "dbo.Category", "Id");
            AddForeignKey("dbo.Color", "FormatId", "dbo.Format", "Id");
            AddForeignKey("dbo.Color", "ProductId", "dbo.Product", "Id");
            AddForeignKey("dbo.Color", "ProgramId", "dbo.Program", "Id");
            AddForeignKey("dbo.Color", "SegmentId", "dbo.Segment", "Id");
            AddForeignKey("dbo.Color", "SubrangeId", "dbo.Subrange", "Id");
            AddForeignKey("dbo.Color", "TechHighLevelId", "dbo.TechHighLevel", "Id");
            AddForeignKey("dbo.Color", "TechnologyId", "dbo.Technology", "Id");
            AddForeignKey("dbo.Color", "VarietyId", "dbo.Variety", "Id");
            DropColumn("dbo.Color", "PropertyName");
            DropColumn("dbo.Color", "PropertyValue");
            DropColumn("dbo.Color", "ColorValue");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Color", "ColorValue", c => c.String(nullable: false));
            AddColumn("dbo.Color", "PropertyValue", c => c.String(nullable: false, maxLength: 255));
            AddColumn("dbo.Color", "PropertyName", c => c.String(nullable: false, maxLength: 255));
            DropForeignKey("dbo.Color", "VarietyId", "dbo.Variety");
            DropForeignKey("dbo.Color", "TechnologyId", "dbo.Technology");
            DropForeignKey("dbo.Color", "TechHighLevelId", "dbo.TechHighLevel");
            DropForeignKey("dbo.Color", "SubrangeId", "dbo.Subrange");
            DropForeignKey("dbo.Color", "SegmentId", "dbo.Segment");
            DropForeignKey("dbo.Color", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.Color", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Color", "FormatId", "dbo.Format");
            DropForeignKey("dbo.Color", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Color", "BrandTechId", "dbo.BrandTech");
            DropForeignKey("dbo.Color", "BrandId", "dbo.Brand");
            DropForeignKey("dbo.Color", "AgeGroupId", "dbo.AgeGroup");
            DropIndex("dbo.Color", new[] { "VarietyId" });
            DropIndex("dbo.Color", new[] { "AgeGroupId" });
            DropIndex("dbo.Color", new[] { "SubrangeId" });
            DropIndex("dbo.Color", new[] { "BrandTechId" });
            DropIndex("dbo.Color", new[] { "FormatId" });
            DropIndex("dbo.Color", new[] { "ProgramId" });
            DropIndex("dbo.Color", new[] { "TechHighLevelId" });
            DropIndex("dbo.Color", new[] { "TechnologyId" });
            DropIndex("dbo.Color", new[] { "SegmentId" });
            DropIndex("dbo.Color", new[] { "BrandId" });
            DropIndex("dbo.Color", new[] { "CategoryId" });
            DropIndex("dbo.Color", new[] { "ProductId" });
            DropColumn("dbo.Color", "SystemName");
            DropColumn("dbo.Color", "VarietyId");
            DropColumn("dbo.Color", "AgeGroupId");
            DropColumn("dbo.Color", "SubrangeId");
            DropColumn("dbo.Color", "BrandTechId");
            DropColumn("dbo.Color", "FormatId");
            DropColumn("dbo.Color", "ProgramId");
            DropColumn("dbo.Color", "TechHighLevelId");
            DropColumn("dbo.Color", "TechnologyId");
            DropColumn("dbo.Color", "SegmentId");
            DropColumn("dbo.Color", "BrandId");
            DropColumn("dbo.Color", "CategoryId");
            DropColumn("dbo.Color", "ProductId");
            DropColumn("dbo.Color", "DeletedDate");
            DropColumn("dbo.Color", "Disabled");
        }
    }
}
