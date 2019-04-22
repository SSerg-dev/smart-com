namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TPM_models : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgeGroup",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Brand",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.BrandTech",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Category",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Client",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        RegionId = c.Guid(nullable: false),
                        CommercialNetId = c.Guid(nullable: false),
                        CommercialSubnetId = c.Guid(nullable: false),
                        DistributorId = c.Guid(nullable: false),
                        StoreTypeId = c.Guid(nullable: false),
                        Store = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommercialSubnet", t => t.CommercialSubnetId)
                .ForeignKey("dbo.Distributor", t => t.DistributorId)
                .ForeignKey("dbo.Region", t => t.RegionId)
                .ForeignKey("dbo.StoreType", t => t.StoreTypeId)
                .Index(t => t.RegionId)
                .Index(t => t.CommercialSubnetId)
                .Index(t => t.DistributorId)
                .Index(t => t.StoreTypeId);
            
            CreateTable(
                "dbo.CommercialSubnet",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                        CommercialNetId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CommercialNet", t => t.CommercialNetId)
                .Index(t => t.Name, unique: true)
                .Index(t => t.CommercialNetId);
            
            CreateTable(
                "dbo.CommercialNet",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Distributor",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Region",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.StoreType",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Format",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        GRD = c.String(maxLength: 255),
                        SKU = c.String(nullable: false, maxLength: 255),
                        CategoryId = c.Guid(),
                        BrandId = c.Guid(),
                        SegmentId = c.Guid(),
                        TechnologyId = c.Guid(),
                        TechHighLevelId = c.Guid(),
                        ProgramId = c.Guid(),
                        FormatId = c.Guid(),
                        BrandTechId = c.Guid(),
                        SubrangeId = c.Guid(),
                        AgeGroupId = c.Guid(),
                        VarietyId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AgeGroup", t => t.AgeGroupId)
                .ForeignKey("dbo.Brand", t => t.BrandId)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.Category", t => t.CategoryId)
                .ForeignKey("dbo.Format", t => t.FormatId)
                .ForeignKey("dbo.Program", t => t.ProgramId)
                .ForeignKey("dbo.Segment", t => t.SegmentId)
                .ForeignKey("dbo.Subrange", t => t.SubrangeId)
                .ForeignKey("dbo.TechHighLevel", t => t.TechHighLevelId)
                .ForeignKey("dbo.Technology", t => t.TechnologyId)
                .ForeignKey("dbo.Variety", t => t.VarietyId)
                .Index(t => t.CategoryId)
                .Index(t => t.BrandId)
                .Index(t => t.SegmentId)
                .Index(t => t.TechnologyId)
                .Index(t => t.TechHighLevelId)
                .Index(t => t.ProgramId)
                .Index(t => t.FormatId)
                .Index(t => t.BrandTechId)
                .Index(t => t.SubrangeId)
                .Index(t => t.AgeGroupId)
                .Index(t => t.VarietyId);
            
            CreateTable(
                "dbo.Program",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Segment",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Subrange",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.TechHighLevel",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Technology",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Variety",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Product", "VarietyId", "dbo.Variety");
            DropForeignKey("dbo.Product", "TechnologyId", "dbo.Technology");
            DropForeignKey("dbo.Product", "TechHighLevelId", "dbo.TechHighLevel");
            DropForeignKey("dbo.Product", "SubrangeId", "dbo.Subrange");
            DropForeignKey("dbo.Product", "SegmentId", "dbo.Segment");
            DropForeignKey("dbo.Product", "ProgramId", "dbo.Program");
            DropForeignKey("dbo.Product", "FormatId", "dbo.Format");
            DropForeignKey("dbo.Product", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.Product", "BrandTechId", "dbo.BrandTech");
            DropForeignKey("dbo.Product", "BrandId", "dbo.Brand");
            DropForeignKey("dbo.Product", "AgeGroupId", "dbo.AgeGroup");
            DropForeignKey("dbo.Client", "StoreTypeId", "dbo.StoreType");
            DropForeignKey("dbo.Client", "RegionId", "dbo.Region");
            DropForeignKey("dbo.Client", "DistributorId", "dbo.Distributor");
            DropForeignKey("dbo.Client", "CommercialSubnetId", "dbo.CommercialSubnet");
            DropForeignKey("dbo.CommercialSubnet", "CommercialNetId", "dbo.CommercialNet");
            DropIndex("dbo.Variety", new[] { "Name" });
            DropIndex("dbo.Technology", new[] { "Name" });
            DropIndex("dbo.TechHighLevel", new[] { "Name" });
            DropIndex("dbo.Subrange", new[] { "Name" });
            DropIndex("dbo.Segment", new[] { "Name" });
            DropIndex("dbo.Program", new[] { "Name" });
            DropIndex("dbo.Product", new[] { "VarietyId" });
            DropIndex("dbo.Product", new[] { "AgeGroupId" });
            DropIndex("dbo.Product", new[] { "SubrangeId" });
            DropIndex("dbo.Product", new[] { "BrandTechId" });
            DropIndex("dbo.Product", new[] { "FormatId" });
            DropIndex("dbo.Product", new[] { "ProgramId" });
            DropIndex("dbo.Product", new[] { "TechHighLevelId" });
            DropIndex("dbo.Product", new[] { "TechnologyId" });
            DropIndex("dbo.Product", new[] { "SegmentId" });
            DropIndex("dbo.Product", new[] { "BrandId" });
            DropIndex("dbo.Product", new[] { "CategoryId" });
            DropIndex("dbo.Format", new[] { "Name" });
            DropIndex("dbo.StoreType", new[] { "Name" });
            DropIndex("dbo.Region", new[] { "Name" });
            DropIndex("dbo.Distributor", new[] { "Name" });
            DropIndex("dbo.CommercialNet", new[] { "Name" });
            DropIndex("dbo.CommercialSubnet", new[] { "CommercialNetId" });
            DropIndex("dbo.CommercialSubnet", new[] { "Name" });
            DropIndex("dbo.Client", new[] { "StoreTypeId" });
            DropIndex("dbo.Client", new[] { "DistributorId" });
            DropIndex("dbo.Client", new[] { "CommercialSubnetId" });
            DropIndex("dbo.Client", new[] { "RegionId" });
            DropIndex("dbo.Category", new[] { "Name" });
            DropIndex("dbo.BrandTech", new[] { "Name" });
            DropIndex("dbo.Brand", new[] { "Name" });
            DropIndex("dbo.AgeGroup", new[] { "Name" });
            DropTable("dbo.Variety");
            DropTable("dbo.Technology");
            DropTable("dbo.TechHighLevel");
            DropTable("dbo.Subrange");
            DropTable("dbo.Segment");
            DropTable("dbo.Program");
            DropTable("dbo.Product");
            DropTable("dbo.Format");
            DropTable("dbo.StoreType");
            DropTable("dbo.Region");
            DropTable("dbo.Distributor");
            DropTable("dbo.CommercialNet");
            DropTable("dbo.CommercialSubnet");
            DropTable("dbo.Client");
            DropTable("dbo.Category");
            DropTable("dbo.BrandTech");
            DropTable("dbo.Brand");
            DropTable("dbo.AgeGroup");
        }
    }
}
