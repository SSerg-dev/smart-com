namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Competitor_AP_For_SuperReader : DbMigration
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
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='Competitors' and [Action]='GetCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitors' and [Action]='GetDeletedCompetitor')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetHistoricalCompetitors')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitors' and [Action]='GetFilteredData'))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorBrandTechs' and [Action]='GetCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorBrandTechs' and [Action]='GetDeletedCompetitorBrandTech')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetHistoricalCompetitorBrandTechs')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorBrandTechs' and [Action]='GetFilteredData'))
            GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='CompetitorPromoes' and [Action]='GetCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedCompetitorPromoes' and [Action]='GetDeletedCompetitorPromo')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetHistoricalCompetitorPromoes')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='HistoricalCompetitorPromoes' and [Action]='GetFilteredData'))
            GO
        ";
    }
}
