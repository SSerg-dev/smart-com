namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ModifiedDate_Field : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.NonPromoSupportBrandTech", "ModifiedDate", c => c.DateTimeOffset(nullable: true, precision: 7));
            createTriggerScript = createTriggerScript.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(createTriggerScript);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.NonPromoSupportBrandTech", "ModifiedDate");
        }

        string createTriggerScript = @"
            CREATE TRIGGER [DefaultSchemaSetting].NonPromoSupportBrandTech_Update_Trigger
            ON [DefaultSchemaSetting].NonPromoSupportBrandTech
            AFTER INSERT, UPDATE AS
              UPDATE [DefaultSchemaSetting].NonPromoSupportBrandTech
              SET ModifiedDate = CURRENT_TIMESTAMP
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
        ";
    }
}
