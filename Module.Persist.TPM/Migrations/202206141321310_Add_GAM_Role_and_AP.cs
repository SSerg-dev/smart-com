namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_GAM_Role_and_AP : DbMigration
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
            IF NOT EXISTS(SELECT * FROM [DefaultSchemaSetting].[Role] WHERE [SystemName] = 'GAManager')
            BEGIN
            INSERT INTO [DefaultSchemaSetting].[Role]
                       ([Id]
                       ,[Disabled]
                       ,[DeletedDate]
                       ,[SystemName]
                       ,[DisplayName]
                       ,[IsAllow])
                 VALUES
                       (NEWID()
                       ,0
                       ,NULL
                       ,'GAManager'
                       ,'Growth Acceleration Manager'
                       ,1)
            END
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetClientTree' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetClientTrees' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetHierarchyDetail' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='GetClientTreeByObjectId' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientTrees' and [Action]='DownloadLogoFile' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoTypes' and [Action]='GetPromoType' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoTypes' and [Action]='GetPromoTypes' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoTypes' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='ExportXLSX' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='SchedulerClientTreeDTOs' and [Action]='GetSchedulerClientTreeDTOs' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='SchedulerClientTreeDTOs' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseClients' and [Action]='GetBaseClients' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='BaseClients' and [Action]='GetBaseClient' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoViews' and [Action]='GetPromoView' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoViews' and [Action]='GetPromoViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoViews' and [Action]='ExportSchedule' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoViews' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserLoopHandlers' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserLoopHandlers' and [Action]='GetUserLoopHandlers' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserLoopHandlers' and [Action]='GetUserLoopHandler' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='UserLoopHandlers' and [Action]='GetUser' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetUserDashboardsCount' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoROIReports' and [Action]='GetPromoROIReports' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoROIReports' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoROIReports' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoGridViews' and [Action]='GetPromoGridViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoGridViews' and [Action]='GetPromoGridView' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoGridViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoGridViews' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='GetClientDashboardViews' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='DownloadTemplateXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='ExportXLSX' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ClientDashboardViews' and [Action]='GetAllYEEF' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Security' and [Action]='Get' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Security' and [Action]='SaveGridSettings' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetPromoes' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetFilteredData' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='GetUserDashboardsCount' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Promoes' and [Action]='CheckIfLogHasErrors' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatuss' and [Action]='GetPromoStatuss' and [Disabled] = 0)),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatuss' and [Action]='GetFilteredData' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ProductTrees' and [Action]='DownloadLogoFile' and [Disabled] = 0))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'GAManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='PromoStatusChanges' and [Action]='PromoStatusChangesByPromo' and [Disabled] = 0))
            GO
            ";
    }
}
