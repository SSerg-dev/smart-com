namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_NonPromoSupportBrandTech_Table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.NonPromoSupport", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.NonPromoSupport", new[] { "BrandTechId" });
            CreateTable(
                "dbo.NonPromoSupportBrandTech",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        NonPromoSupportId = c.Guid(nullable: false),
                        BrandTechId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.NonPromoSupport", t => t.NonPromoSupportId)
                .Index(t => t.NonPromoSupportId)
                .Index(t => t.BrandTechId);
            
            DropColumn("dbo.NonPromoSupport", "BrandTechId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.NonPromoSupport", "BrandTechId", c => c.Guid());
            DropForeignKey("dbo.NonPromoSupportBrandTech", "NonPromoSupportId", "dbo.NonPromoSupport");
            DropForeignKey("dbo.NonPromoSupportBrandTech", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.NonPromoSupportBrandTech", new[] { "BrandTechId" });
            DropIndex("dbo.NonPromoSupportBrandTech", new[] { "NonPromoSupportId" });
            DropTable("dbo.NonPromoSupportBrandTech");
            CreateIndex("dbo.NonPromoSupport", "BrandTechId");
            AddForeignKey("dbo.NonPromoSupport", "BrandTechId", "dbo.BrandTech", "Id");
        }
    }
}
