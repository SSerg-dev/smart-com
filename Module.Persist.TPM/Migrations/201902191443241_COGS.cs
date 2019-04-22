namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class COGS : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.COGS",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        LVSpercent = c.Short(nullable: false),
                        ClientTreeId = c.Int(nullable: false),
                        BrandTechId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .Index(t => t.Id)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BrandTechId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.COGS", "ClientTreeId", "dbo.ClientTree");
            DropForeignKey("dbo.COGS", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.COGS", new[] { "BrandTechId" });
            DropIndex("dbo.COGS", new[] { "ClientTreeId" });
            DropIndex("dbo.COGS", new[] { "Id" });
            DropTable("dbo.COGS");
        }
    }
}
