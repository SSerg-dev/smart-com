namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ModifiedDate_Field1 : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddColumn($"{defaultSchema}.Promo", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn($"{defaultSchema}.PromoProduct", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn($"{defaultSchema}.PromoProductsCorrection", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn($"{defaultSchema}.PromoSupport", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            AddColumn($"{defaultSchema}.NonPromoSupport", "ModifiedDate", c => c.DateTimeOffset(precision: 7));
            createTriggerScript = createTriggerScript.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(createTriggerScript);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropColumn($"{defaultSchema}.NonPromoSupport", "ModifiedDate");
            DropColumn($"{defaultSchema}.PromoSupport", "ModifiedDate");
            DropColumn($"{defaultSchema}.PromoProductsCorrection", "ModifiedDate");
            DropColumn($"{defaultSchema}.PromoProduct", "ModifiedDate");
            DropColumn($"{defaultSchema}.Promo", "ModifiedDate");
        }

        string createTriggerScript = @"
            CREATE TRIGGER [DefaultSchemaSetting].NonPromoSupport_Update_Trigger
            ON [DefaultSchemaSetting].NonPromoSupport
            AFTER INSERT, UPDATE AS
              UPDATE [DefaultSchemaSetting].NonPromoSupport
              SET ModifiedDate = CURRENT_TIMESTAMP
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
        GO
            CREATE TRIGGER [DefaultSchemaSetting].PromoSupport_Update_Trigger
            ON [DefaultSchemaSetting].PromoSupport
            AFTER INSERT, UPDATE AS
              UPDATE [DefaultSchemaSetting].PromoSupport
              SET ModifiedDate = CURRENT_TIMESTAMP
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
        GO
            CREATE TRIGGER [DefaultSchemaSetting].Promo_Update_Trigger
            ON [DefaultSchemaSetting].Promo
            AFTER INSERT, UPDATE AS
              UPDATE [DefaultSchemaSetting].Promo
              SET ModifiedDate = CURRENT_TIMESTAMP
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
        GO
            CREATE TRIGGER [DefaultSchemaSetting].PromoProduct_Update_Trigger
            ON [DefaultSchemaSetting].PromoProduct
            AFTER INSERT, UPDATE AS
              UPDATE [DefaultSchemaSetting].PromoProduct
              SET ModifiedDate = CURRENT_TIMESTAMP
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
        GO
            CREATE TRIGGER [DefaultSchemaSetting].PromoProductsCorrection_Update_Trigger
            ON [DefaultSchemaSetting].PromoProductsCorrection
            AFTER INSERT, UPDATE AS
              UPDATE [DefaultSchemaSetting].PromoProductsCorrection
              SET ModifiedDate = CURRENT_TIMESTAMP
              WHERE Id IN (SELECT DISTINCT Id FROM Inserted)
        ";
    }
}
