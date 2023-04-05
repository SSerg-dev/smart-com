using Core.Settings;

namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PlanPostPromoEffect : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            CreateTable(
                $"{defaultSchema}.PlanPostPromoEffect",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Disabled = c.Boolean(nullable: false),
                        DeletedDate = c.DateTimeOffset(precision: 7),
                        Size = c.String(maxLength: 255),
                        PlanPostPromoEffectW1 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PlanPostPromoEffectW2 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscountRangeId = c.Guid(nullable: false),
                        DurationRangeId = c.Guid(nullable: false),
                        BrandTechId = c.Guid(nullable: false),
                        ClientTreeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey($"{defaultSchema}.BrandTech", t => t.BrandTechId)
                .ForeignKey($"{defaultSchema}.ClientTree", t => t.ClientTreeId)
                .ForeignKey($"{defaultSchema}.DiscountRange", t => t.DiscountRangeId)
                .ForeignKey($"{defaultSchema}.DurationRange", t => t.DurationRangeId)
                .Index(t => t.Id)
                .Index(t => t.DiscountRangeId)
                .Index(t => t.DurationRangeId)
                .Index(t => t.BrandTechId)
                .Index(t => t.ClientTreeId);
            
            CreateTable(
                $"{defaultSchema}.DiscountRange",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        MinValue = c.Int(nullable: false),
                        MaxValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                $"{defaultSchema}.DurationRange",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(maxLength: 255),
                        MinValue = c.Int(nullable: false),
                        MaxValue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PlanPostPromoEffect", "DurationRangeId", $"{defaultSchema}.DurationRange");
            DropForeignKey($"{defaultSchema}.PlanPostPromoEffect", "DiscountRangeId", $"{defaultSchema}.DiscountRange");
            DropForeignKey($"{defaultSchema}.PlanPostPromoEffect", "ClientTreeId", $"{defaultSchema}.ClientTree");
            DropForeignKey($"{defaultSchema}.PlanPostPromoEffect", "BrandTechId", $"{defaultSchema}.BrandTech");
            DropIndex($"{defaultSchema}.DurationRange", new[] { "Id" });
            DropIndex($"{defaultSchema}.DiscountRange", new[] { "Id" });
            DropIndex($"{defaultSchema}.PlanPostPromoEffect", new[] { "ClientTreeId" });
            DropIndex($"{defaultSchema}.PlanPostPromoEffect", new[] { "BrandTechId" });
            DropIndex($"{defaultSchema}.PlanPostPromoEffect", new[] { "DurationRangeId" });
            DropIndex($"{defaultSchema}.PlanPostPromoEffect", new[] { "DiscountRangeId" });
            DropIndex($"{defaultSchema}.PlanPostPromoEffect", new[] { "Id" });
            DropTable($"{defaultSchema}.DurationRange");
            DropTable($"{defaultSchema}.DiscountRange");
            DropTable($"{defaultSchema}.PlanPostPromoEffect");
        }
    }
}
