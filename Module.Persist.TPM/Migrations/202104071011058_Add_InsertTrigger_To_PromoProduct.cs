namespace Module.Persist.TPM.Migrations
{
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Add_InsertTrigger_To_PromoProduct : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddTriggerScript = AddTriggerScript.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(AddTriggerScript);
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropTriggerScript = DropTriggerScript.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(DropTriggerScript);
        }

        string AddTriggerScript = @"
                CREATE TRIGGER [DefaultSchemaSetting].[PromoProduct_Isert] ON [DefaultSchemaSetting].[PromoProduct] FOR INSERT AS BEGIN
                    Declare @Id uniqueidentifier
                    set @Id = (select Id from inserted)

                    Update [DefaultSchemaSetting].[PromoProduct]
                    Set CreateDate = GetDate()
                    Where Id = @Id
                END
                GO

                ALTER TABLE [DefaultSchemaSetting].[PromoProduct] ENABLE TRIGGER [PromoProduct_Isert]
                GO";

        string DropTriggerScript = "DROP TRIGGER [DefaultSchemaSetting].[PromoProduct_Isert]";
    }
}
