namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Competitor_Tables : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.Competitor",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Name = c.String(maxLength: 124),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.Name, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_Competitor");

            CreateTable(
                $"{defaultSchema}.CompetitorPromo",
                c => new
                {
                    Id = c.Guid(nullable: false, identity: true),
                    Disabled = c.Boolean(nullable: false),
                    DeletedDate = c.DateTimeOffset(precision: 7),
                    CompetitorId = c.Guid(nullable: false),
                    ClientTreeObjectId = c.Int(nullable: false),
                    CompetitorBrandTechId = c.Guid(nullable: false),
                    Name = c.String(nullable: false, maxLength: 124),
                    Number = c.Int(nullable: false),
                    StartDate = c.DateTimeOffset(nullable: false, precision: 7),
                    EndDate = c.DateTimeOffset(nullable: false, precision: 7),
                    Discount = c.Double(),
                    Price = c.Double(),
                    Subrange = c.String(maxLength: 256),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.CompetitorBrandTech", t => t.CompetitorBrandTechId)
                .ForeignKey($"{defaultSchema}.ClientTree", t => t.ClientTreeObjectId)
                .ForeignKey($"{defaultSchema}.Competitor", t => t.CompetitorId)
                .Index(t => new { t.Number, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_CompetitorPromo")
                .Index(t => t.CompetitorId)
                .Index(t => t.ClientTreeObjectId)
                .Index(t => t.CompetitorBrandTechId);
            
            CreateTable(
                $"{defaultSchema}.CompetitorBrandTech",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        CompetitorId = c.Guid(),
                        BrandTech = c.String(nullable: false, maxLength: 124),
                        Color = c.String(nullable: false, maxLength: 24),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.Competitor", t => t.CompetitorId)
                .Index(t => new { t.CompetitorId, t.BrandTech, t.Disabled, t.DeletedDate }, unique: true, name: "Unique_CompetitorBrandTech");
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.CompetitorPromo", "PromoStatusId", $"{defaultSchema}.PromoStatus");
            DropForeignKey($"{defaultSchema}.CompetitorPromo", "CompetitorId", $"{defaultSchema}.Competitor");
            DropForeignKey($"{defaultSchema}.CompetitorPromo", "ClientTreeId", $"{defaultSchema}.ClientTree");
            DropForeignKey($"{defaultSchema}.CompetitorPromo", "CompetitorBrandTechId", $"{defaultSchema}.CompetitorBrandTech");
            DropForeignKey($"{defaultSchema}.CompetitorBrandTech", "CompetitorId", $"{defaultSchema}.Competitor");
            DropIndex($"{defaultSchema}.CompetitorBrandTech", "Unique_CompetitorBrandTech");
            DropIndex($"{defaultSchema}.CompetitorPromo", new[] { "PromoStatusId" });
            DropIndex($"{defaultSchema}.CompetitorPromo", new[] { "CompetitorBrandTechId" });
            DropIndex($"{defaultSchema}.CompetitorPromo", new[] { "ClientTreeId" });
            DropIndex($"{defaultSchema}.CompetitorPromo", new[] { "CompetitorId" });
            DropIndex($"{defaultSchema}.CompetitorPromo", "Unique_CompetitorPromo");
            DropIndex($"{defaultSchema}.Competitor", "Unique_Competitor");
            DropTable($"{defaultSchema}.CompetitorBrandTech");
            DropTable($"{defaultSchema}.CompetitorPromo");
            DropTable($"{defaultSchema}.Competitor");
        }
    }
}
