namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System.Data.Entity.Migrations;

    public partial class Add_Data_For_ActualCOGSTn : DbMigration
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
            ('ActualCOGSTns', 'IsCOGSTnRecalculatePreviousYearButtonAvailable', 0, NULL)
            GO
            DECLARE @RoleIdASA uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdASA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='IsCOGSTnRecalculatePreviousYearButtonAvailable'))

            DECLARE @RoleIdAA uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdAA, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='IsCOGSTnRecalculatePreviousYearButtonAvailable'))

            DECLARE @RoleIdADF uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdADF, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='IsCOGSTnRecalculatePreviousYearButtonAvailable'))

            DECLARE @RoleIdAFE uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
            INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
            (RoleId, AccessPointId) values
            (@RoleIdAFE, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='ActualCOGSTns' and [Action]='IsCOGSTnRecalculatePreviousYearButtonAvailable'))
            GO
        ";
    }
}
