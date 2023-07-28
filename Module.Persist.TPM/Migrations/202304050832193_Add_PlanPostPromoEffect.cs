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
            
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
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
        
        private string SqlString = @"
            INSERT INTO [DefaultSchemaSetting].[DiscountRange] ([Id], [Name], [MinValue], [MaxValue]) VALUES
            (NEWID(), '0 – 5%', 0, 5),
            (NEWID(), '6 – 10%', 6, 10),
            (NEWID(), '11 – 15%', 11, 15),
            (NEWID(), '16 – 20%', 16, 20),
            (NEWID(), '21 – 25%', 21, 25),
            (NEWID(), '26 – 30%', 26, 30),
            (NEWID(), '31 – 35%', 31, 35),
            (NEWID(), '36 – 40%', 36, 40)
            GO
            INSERT INTO [DefaultSchemaSetting].[DurationRange] ([Id], [Name], [MinValue], [MaxValue]) VALUES
               (NEWID(), '0 – 7', 0, 7),
               (NEWID(), '8 – 14', 8, 14),
               (NEWID(), '15 – 21', 15, 21),
               (NEWID(), '22 and more', 22, 999)
            GO";
    }
}
