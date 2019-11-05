using System;
using System.Data.Entity.Migrations;

namespace Module.Persist.TPM.Migrations
{
    public partial class ClientTreeBrandTech_CreateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientTreeBrandTech",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClientTreeId = c.Int(nullable: false),
                        BrandTechId = c.Guid(nullable: false),
                        Share = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BrandTechId);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientTreeBrandTech", "ClientTreeId", "dbo.ClientTree");
            DropForeignKey("dbo.ClientTreeBrandTech", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.ClientTreeBrandTech", new[] { "BrandTechId" });
            DropIndex("dbo.ClientTreeBrandTech", new[] { "ClientTreeId" });
            DropTable("dbo.ClientTreeBrandTech");
        }
    }
}
