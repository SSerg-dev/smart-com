namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TradeInvestments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TradeInvestment",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        BrandTechId = c.Guid(),
                        TIType = c.String(maxLength: 255),
                        TISubType = c.String(maxLength: 255),
                        SizePercent = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BrandTechId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TradeInvestment", "ClientTreeId", "dbo.ClientTree");
            DropForeignKey("dbo.TradeInvestment", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.TradeInvestment", new[] { "BrandTechId" });
            DropIndex("dbo.TradeInvestment", new[] { "ClientTreeId" });
            DropTable("dbo.TradeInvestment");
        }
    }
}
