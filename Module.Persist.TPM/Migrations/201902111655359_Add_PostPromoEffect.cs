namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PostPromoEffect : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostPromoEffect",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                        EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        ProductTreeId = c.Int(nullable: false),
                        EffectWeek1 = c.Double(nullable: false),
                        EffectWeek2 = c.Double(nullable: false),
                        TotalEffect = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .ForeignKey("dbo.ProductTree", t => t.ProductTreeId)
                .Index(t => t.ClientTreeId)
                .Index(t => t.ProductTreeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostPromoEffect", "ProductTreeId", "dbo.ProductTree");
            DropForeignKey("dbo.PostPromoEffect", "ClientTreeId", "dbo.ClientTree");
            DropIndex("dbo.PostPromoEffect", new[] { "ProductTreeId" });
            DropIndex("dbo.PostPromoEffect", new[] { "ClientTreeId" });
            DropTable("dbo.PostPromoEffect");
        }
    }
}
