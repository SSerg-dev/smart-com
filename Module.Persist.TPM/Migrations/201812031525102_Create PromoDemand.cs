namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePromoDemand : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PromoDemand",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        BrandTechId = c.Guid(nullable: false),
                        MechanicId = c.Guid(nullable: false),
                        Account = c.String(nullable: false, maxLength: 255),
                        Discount = c.Int(nullable: false),
                        Week = c.Int(nullable: false),
                        Baseline = c.Int(nullable: false),
                        Uplift = c.Int(nullable: false),
                        Incremental = c.Int(nullable: false),
                        Activity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.Mechanic", t => t.MechanicId)
                .Index(t => t.BrandTechId)
                .Index(t => t.MechanicId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PromoDemand", "MechanicId", "dbo.Mechanic");
            DropForeignKey("dbo.PromoDemand", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.PromoDemand", new[] { "MechanicId" });
            DropIndex("dbo.PromoDemand", new[] { "BrandTechId" });
            DropTable("dbo.PromoDemand");
        }
    }
}
