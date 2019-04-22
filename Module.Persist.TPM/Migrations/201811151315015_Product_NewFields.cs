namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_NewFields : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Product", "AgeGroupId", "dbo.AgeGroup");
            DropForeignKey("dbo.Product", "BrandId", "dbo.Brand");
            DropForeignKey("dbo.Product", "BrandTechId", "dbo.BrandTech");
            DropForeignKey("dbo.Product", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Product", "FormatId", "dbo.Format");
            DropForeignKey("dbo.Product", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.Product", "SegmentId", "dbo.Segment");
            DropForeignKey("dbo.Product", "SubrangeId", "dbo.Subrange");
            DropForeignKey("dbo.Product", "TechHighLevelId", "dbo.TechHighLevel");
            DropForeignKey("dbo.Product", "TechnologyId", "dbo.Technology");
            DropForeignKey("dbo.Product", "VarietyId", "dbo.Variety");
            DropIndex("dbo.Product", new[] { "CategoryId" });
            DropIndex("dbo.Product", new[] { "BrandId" });
            DropIndex("dbo.Product", new[] { "SegmentId" });
            DropIndex("dbo.Product", new[] { "TechnologyId" });
            DropIndex("dbo.Product", new[] { "TechHighLevelId" });
            DropIndex("dbo.Product", new[] { "ProgramId" });
            DropIndex("dbo.Product", new[] { "FormatId" });
            DropIndex("dbo.Product", new[] { "BrandTechId" });
            DropIndex("dbo.Product", new[] { "SubrangeId" });
            DropIndex("dbo.Product", new[] { "AgeGroupId" });
            DropIndex("dbo.Product", new[] { "VarietyId" });
            AddColumn("dbo.Product", "ProductRU", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "ProductEN", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "BrandFlag", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "IngredientVariety", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "ProductCategory", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "ProductType", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "MarketSegment", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "SupplySegment", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "FunctionalVariety", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "Size", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "BrandEssence", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "PackType", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "GroupSize", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "TradedUnitFormat", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "ConsumerPackFormat", c => c.String(maxLength: 255));
            DropColumn("dbo.Product", "SKU");
            DropColumn("dbo.Product", "EAN");
            DropColumn("dbo.Product", "CategoryId");
            DropColumn("dbo.Product", "BrandId");
            DropColumn("dbo.Product", "SegmentId");
            DropColumn("dbo.Product", "TechnologyId");
            DropColumn("dbo.Product", "TechHighLevelId");
            DropColumn("dbo.Product", "ProgramId");
            DropColumn("dbo.Product", "FormatId");
            DropColumn("dbo.Product", "BrandTechId");
            DropColumn("dbo.Product", "SubrangeId");
            DropColumn("dbo.Product", "AgeGroupId");
            DropColumn("dbo.Product", "VarietyId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Product", "VarietyId", c => c.Guid());
            AddColumn("dbo.Product", "AgeGroupId", c => c.Guid());
            AddColumn("dbo.Product", "SubrangeId", c => c.Guid());
            AddColumn("dbo.Product", "BrandTechId", c => c.Guid());
            AddColumn("dbo.Product", "FormatId", c => c.Guid());
            AddColumn("dbo.Product", "ProgramId", c => c.Guid());
            AddColumn("dbo.Product", "TechHighLevelId", c => c.Guid());
            AddColumn("dbo.Product", "TechnologyId", c => c.Guid());
            AddColumn("dbo.Product", "SegmentId", c => c.Guid());
            AddColumn("dbo.Product", "BrandId", c => c.Guid());
            AddColumn("dbo.Product", "CategoryId", c => c.Guid());
            AddColumn("dbo.Product", "EAN", c => c.String(maxLength: 255));
            AddColumn("dbo.Product", "SKU", c => c.String(nullable: false, maxLength: 255));
            DropColumn("dbo.Product", "ConsumerPackFormat");
            DropColumn("dbo.Product", "TradedUnitFormat");
            DropColumn("dbo.Product", "GroupSize");
            DropColumn("dbo.Product", "PackType");
            DropColumn("dbo.Product", "BrandEssence");
            DropColumn("dbo.Product", "Size");
            DropColumn("dbo.Product", "FunctionalVariety");
            DropColumn("dbo.Product", "SupplySegment");
            DropColumn("dbo.Product", "MarketSegment");
            DropColumn("dbo.Product", "ProductType");
            DropColumn("dbo.Product", "ProductCategory");
            DropColumn("dbo.Product", "IngredientVariety");
            DropColumn("dbo.Product", "BrandFlag");
            DropColumn("dbo.Product", "ProductEN");
            DropColumn("dbo.Product", "ProductRU");
            CreateIndex("dbo.Product", "VarietyId");
            CreateIndex("dbo.Product", "AgeGroupId");
            CreateIndex("dbo.Product", "SubrangeId");
            CreateIndex("dbo.Product", "BrandTechId");
            CreateIndex("dbo.Product", "FormatId");
            CreateIndex("dbo.Product", "ProgramId");
            CreateIndex("dbo.Product", "TechHighLevelId");
            CreateIndex("dbo.Product", "TechnologyId");
            CreateIndex("dbo.Product", "SegmentId");
            CreateIndex("dbo.Product", "BrandId");
            CreateIndex("dbo.Product", "CategoryId");
            AddForeignKey("dbo.Product", "VarietyId", "dbo.Variety", "Id");
            AddForeignKey("dbo.Product", "TechnologyId", "dbo.Technology", "Id");
            AddForeignKey("dbo.Product", "TechHighLevelId", "dbo.TechHighLevel", "Id");
            AddForeignKey("dbo.Product", "SubrangeId", "dbo.Subrange", "Id");
            AddForeignKey("dbo.Product", "SegmentId", "dbo.Segment", "Id");
            AddForeignKey("dbo.Product", "ProgramId", "dbo.Program", "Id");
            AddForeignKey("dbo.Product", "FormatId", "dbo.Format", "Id");
            AddForeignKey("dbo.Product", "CategoryId", "dbo.Category", "Id");
            AddForeignKey("dbo.Product", "BrandTechId", "dbo.BrandTech", "Id");
            AddForeignKey("dbo.Product", "BrandId", "dbo.Brand", "Id");
            AddForeignKey("dbo.Product", "AgeGroupId", "dbo.AgeGroup", "Id");
        }
    }
}
