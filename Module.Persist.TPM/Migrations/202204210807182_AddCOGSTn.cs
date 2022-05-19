namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCOGSTn : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.PlanCOGSTn",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Volume = c.Double(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        BrandTechId = c.Guid(),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.BrandTech", t => t.BrandTechId)
                .ForeignKey($"{defaultSchema}.ClientTree", t => t.ClientTreeId)
                .Index(t => t.Id)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BrandTechId);

            AddForeignKey($"{defaultSchema}.PlanCOGSTn", "BrandTechId", $"{defaultSchema}.BrandTech", "Id");
            AddForeignKey($"{defaultSchema}.PlanCOGSTn", "ClientTreeId", $"{defaultSchema}.ClientTree", "Id");

            CreateTable(
                $"{defaultSchema}.ActualCOGSTn",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Volume = c.Double(nullable: false),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        ClientTreeId = c.Int(nullable: false),
                        BrandTechId = c.Guid(),
                        StartDate = c.DateTimeOffset(precision: 7),
                        EndDate = c.DateTimeOffset(precision: 7),
                        Year = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.BrandTech", t => t.BrandTechId)
                .ForeignKey($"{defaultSchema}.ClientTree", t => t.ClientTreeId)
                .Index(t => t.Id)
                .Index(t => t.ClientTreeId)
                .Index(t => t.BrandTechId);

            AddForeignKey($"{defaultSchema}.ActualCOGSTn", "BrandTechId", $"{defaultSchema}.BrandTech", "Id");
            AddForeignKey($"{defaultSchema}.ActualCOGSTn", "ClientTreeId", $"{defaultSchema}.ClientTree", "Id");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.ActualCOGSTn", "ClientTreeId", $"{defaultSchema}.ClientTree");
            DropForeignKey($"{defaultSchema}.ActualCOGSTn", "BrandTechId", $"{defaultSchema}.BrandTech");
            DropForeignKey($"{defaultSchema}.PlanCOGSTn", "ClientTreeId", $"{defaultSchema}.ClientTree");
            DropForeignKey($"{defaultSchema}.PlanCOGSTn", "BrandTechId", $"{defaultSchema}.BrandTech");
            DropIndex($"{defaultSchema}.ActualCOGSTn", new[] { "BrandTechId" });
            DropIndex($"{defaultSchema}.ActualCOGSTn", new[] { "ClientTreeId" });
            DropIndex($"{defaultSchema}.ActualCOGSTn", new[] { "Id" });
            DropIndex($"{defaultSchema}.PlanCOGSTn", new[] { "BrandTechId" });
            DropIndex($"{defaultSchema}.PlanCOGSTn", new[] { "ClientTreeId" });
            DropIndex($"{defaultSchema}.PlanCOGSTn", new[] { "Id" });
            DropTable($"{defaultSchema}.ActualCOGSTn");
            DropTable($"{defaultSchema}.PlanCOGSTn");
        }
    }
}
