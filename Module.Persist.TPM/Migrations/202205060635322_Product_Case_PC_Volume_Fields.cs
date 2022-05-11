namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Product_Case_PC_Volume_Fields : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Product", "CaseVolume", c => c.Double());
            AddColumn($"{defaultSchema}.Product", "PCVolume", c => c.Double());
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Product", "CaseVolume");
            DropColumn($"{defaultSchema}.Product", "PCVolume");
        }
    }
}
