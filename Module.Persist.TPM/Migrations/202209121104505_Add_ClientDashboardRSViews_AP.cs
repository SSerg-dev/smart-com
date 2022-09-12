namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_ClientDashboardRSViews_AP : DbMigration
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
                INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                           ([Id], [Disabled], [DeletedDate], [Resource], [Action], [Description], [TPMmode])
                     VALUES
                           (NEWID(), 0, NULL, 'ClientDashboardRSViews', 'GetClientDashboardRSViews', '', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                           ([Id], [Disabled], [DeletedDate], [Resource], [Action], [Description], [TPMmode])
                     VALUES
                           (NEWID(), 0, NULL, 'ClientDashboardRSViews', 'ExportXLSX', '', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                           ([Id], [Disabled], [DeletedDate], [Resource], [Action], [Description], [TPMmode])
                     VALUES
                           (NEWID(), 0, NULL, 'ClientDashboardRSViews', 'GetFilteredData', '', 1)
                GO
                INSERT INTO [DefaultSchemaSetting].[AccessPoint]
                           ([Id], [Disabled], [DeletedDate], [Resource], [Action], [Description], [TPMmode])
                     VALUES
                           (NEWID(), 0, NULL, 'ClientDashboardRSViews', 'GetAllYEEF', '', 1)
                GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetClientDashboardRSViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetAllYEEF' and [Disabled] = 0))
            GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetClientDashboardRSViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetAllYEEF' and [Disabled] = 0))
            GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetClientDashboardRSViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetAllYEEF' and [Disabled] = 0))
            GO

                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetClientDashboardRSViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardRSViews' and [Action]='GetAllYEEF' and [Disabled] = 0))
            GO
            ";
    }
}
