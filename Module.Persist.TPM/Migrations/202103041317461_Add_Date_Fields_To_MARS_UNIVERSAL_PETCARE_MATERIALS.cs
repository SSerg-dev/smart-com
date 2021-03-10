namespace Module.Persist.TPM.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using Core.Settings;

    public partial class Add_Date_Fields_To_MARS_UNIVERSAL_PETCARE_MATERIALS : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            AddDates = AddDates.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(AddDates);
        }

        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropDates = DropDates.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(DropDates);
        }

        private string AddDates = 
        @"
            IF NOT EXISTS (
                SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MARS_UNIVERSAL_PETCARE_MATERIALS' AND COLUMN_NAME = 'START_DATE'
            )
            ALTER TABLE DefaultSchemaSetting.MARS_UNIVERSAL_PETCARE_MATERIALS ADD [START_DATE] DATETIME NULL;

            IF NOT EXISTS (
                SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MARS_UNIVERSAL_PETCARE_MATERIALS' AND COLUMN_NAME = 'END_DATE'
            )
            ALTER TABLE DefaultSchemaSetting.MARS_UNIVERSAL_PETCARE_MATERIALS ADD [END_DATE] DATETIME NULL;
        ";

        private string DropDates =
        @"
            IF EXISTS (
                SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MARS_UNIVERSAL_PETCARE_MATERIALS' AND COLUMN_NAME = 'START_DATE'
            )
            ALTER TABLE DefaultSchemaSetting.MARS_UNIVERSAL_PETCARE_MATERIALS DROP COLUMN [START_DATE];

            IF EXISTS (
                SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'MARS_UNIVERSAL_PETCARE_MATERIALS' AND COLUMN_NAME = 'END_DATE'
            )
            ALTER TABLE DefaultSchemaSetting.MARS_UNIVERSAL_PETCARE_MATERIALS DROP COLUMN [END_DATE];
        ";
    }
}
