namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsGAManagerApproved : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "IsGAManagerApproved", c => c.Boolean());
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.Promo", "IsGAManagerApproved");
           
        }
        
    }
}
