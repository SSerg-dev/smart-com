namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AP_SA_RPA : DbMigration
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
        private string SqlString = @"
            DECLARE @RoleId uniqueidentifier = (SELECT [Id] FROM [DefaultSchemaSetting].[Role] where SystemName='SupportAdministrator');

            INSERT INTO [DefaultSchemaSetting].[AccessPointRole]
                ([Id],[RoleId],[AccessPointId])
            VALUES
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetRPASettings' AND Resource = 'RPASettings')),
				(NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'GetRPAs' AND Resource = 'RPASettings')),
				(NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'SaveRPA' AND Resource = 'RPAs')),
                (NEWID(), @RoleId, (SELECT TOP(1) Id FROM [DefaultSchemaSetting].[AccessPoint] WHERE Action = 'DownloadTemplateXLSX' AND Resource = 'RPAs'))
            GO
        ";
    }
}
