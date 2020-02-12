namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ActualCOGSTI_Tables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActualCOGS",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        IsCOGSIncidentCreated = c.Boolean(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        BrandTechId = c.Guid(),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        LSVpercent = c.Single(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .Index(t => t.Id)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BrandTechId);
            
            CreateTable(
                "dbo.ActualTradeInvestment",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        IsTIIncidentCreated = c.Boolean(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        BrandTechId = c.Guid(),
                        TIType = c.String(maxLength: 255),
                        TISubType = c.String(maxLength: 255),
                        SizePercent = c.Single(nullable: false),
                        MarcCalcROI = c.Boolean(nullable: false),
                        MarcCalcBudgets = c.Boolean(nullable: false),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BrandTech", t => t.BrandTechId)
                .ForeignKey("dbo.ClientTree", t => t.ClientTreeId)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BrandTechId);
            
            AddColumn("dbo.COGS", "LSVpercent", c => c.Single(nullable: false));
            Sql("UPDATE COGS SET LSVpercent = LVSpercent");
            DropColumn("dbo.COGS", "LVSpercent");
        }
        
        public override void Down()
        {
            AddColumn("dbo.COGS", "LVSpercent", c => c.Single(nullable: false));
            Sql("UPDATE COGS SET LVSpercent = LSVpercent");
            DropForeignKey("dbo.ActualTradeInvestment", "ClientTreeId", "dbo.ClientTree");
            DropForeignKey("dbo.ActualTradeInvestment", "BrandTechId", "dbo.BrandTech");
            DropForeignKey("dbo.ActualCOGS", "ClientTreeId", "dbo.ClientTree");
            DropForeignKey("dbo.ActualCOGS", "BrandTechId", "dbo.BrandTech");
            DropIndex("dbo.ActualTradeInvestment", new[] { "BrandTechId" });
            DropIndex("dbo.ActualTradeInvestment", new[] { "ClientTreeId" });
            DropIndex("dbo.ActualCOGS", new[] { "BrandTechId" });
            DropIndex("dbo.ActualCOGS", new[] { "ClientTreeId" });
            DropIndex("dbo.ActualCOGS", new[] { "Id" });
            DropColumn("dbo.COGS", "LSVpercent");
            DropTable("dbo.ActualTradeInvestment");
            DropTable("dbo.ActualCOGS");
        }
    }
}
