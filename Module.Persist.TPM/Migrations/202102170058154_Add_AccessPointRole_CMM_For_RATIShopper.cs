namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AccessPointRole_CMM_For_RATIShopper : DbMigration
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
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='RATIShoppers' and[Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='RATIShoppers' and[Action]='GetRATIShoppers')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='RATIShoppers' and[Action]='GetRATIShopper')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='RATIShoppers' and[Action]='ExportXLSX')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='DeletedRATIShoppers' and[Action]='GetFilteredData')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='DeletedRATIShoppers' and[Action]='GetDeletedRATIShoppers')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='DeletedRATIShoppers' and[Action]='GetDeletedRATIShopper')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='HistoricalRATIShoppers' and[Action]='GetHistoricalRATIShoppers')),
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where[Resource]='HistoricalRATIShoppers' and[Action]='GetFilteredData'))";

    }
}
