namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_ActualCOGSTn_2 : DbMigration
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
            (Resource, Action, Disabled, DeletedDate) VALUES 
            ('ActualCOGSTns', 'PreviousYearPromoList', 0, NULL),
            ('ActualCOGSTns', 'CreateActualCOGSTnChangeIncidents', 0, NULL)
            GO
            DECLARE @RoleIdASA uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='PreviousYearPromoList')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='CreateActualCOGSTnChangeIncidents'))

            DECLARE @RoleIdAA uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='PreviousYearPromoList')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='CreateActualCOGSTnChangeIncidents'))

            DECLARE @RoleIdADF uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='PreviousYearPromoList')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='CreateActualCOGSTnChangeIncidents'))

            DECLARE @RoleIdAFE uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='PreviousYearPromoList')),
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='CreateActualCOGSTnChangeIncidents'))
            GO
        ";
    }
}
