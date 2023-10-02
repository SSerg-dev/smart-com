namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexClientTreeBrandTech__IDX : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.ClientTreeBrandTech", new[] { "ClientTreeId" });
            DropIndex($"{defaultSchema}.ClientTreeBrandTech", new[] { "BrandTechId" });
            AlterColumn($"{defaultSchema}.ClientTreeBrandTech", "ParentClientTreeDemandCode", c => c.String(maxLength: 60, unicode: false));
            CreateIndex($"{defaultSchema}.ClientTreeBrandTech", new[] { "ClientTreeId", "BrandTechId", "ParentClientTreeDemandCode", "DeletedDate" }, unique: true, name: "ClientTreeBrandTech__IDX");
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.ClientTreeBrandTech", "ClientTreeBrandTech__IDX");
            AlterColumn($"{defaultSchema}.ClientTreeBrandTech", "ParentClientTreeDemandCode", c => c.String());
            CreateIndex($"{defaultSchema}.ClientTreeBrandTech", "BrandTechId");
            CreateIndex($"{defaultSchema}.ClientTreeBrandTech", "ClientTreeId");
        }
    }
}
