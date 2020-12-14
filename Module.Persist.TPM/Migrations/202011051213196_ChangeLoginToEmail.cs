namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeLoginToEmail : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
        }

        private string SqlString =
            @"
                UPDATE [DefaultSchemaSetting].[Recipient]
                 set [Value] = (SELECT Email FROM [DefaultSchemaSetting].[User] WHERE [Value] = [Name] AND [DefaultSchemaSetting].[User].DeletedDate IS NULL)
                   where (SELECT Email FROM [DefaultSchemaSetting].[User] WHERE [Value] = [Name] AND [DefaultSchemaSetting].[User].DeletedDate IS NULL) IS NOT NULL
                GO

                UPDATE [DefaultSchemaSetting].[User]
                   SET Name = Email
                 where Email is not null
                GO
            ";
    }
}
