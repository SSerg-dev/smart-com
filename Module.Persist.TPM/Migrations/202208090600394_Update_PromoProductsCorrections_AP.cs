namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_PromoProductsCorrections_AP : DbMigration
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
                DECLARE @AccessPointOld uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] WHERE [Resource] = 'PromoProductsCorrection' AND [Action] = 'PromoProductCorrectionDelete');
                UPDATE [DefaultSchemaSetting].[AccessPoint] SET [Resource] = 'PromoProductsCorrections'
				WHERE [Id] = @AccessPointOld

				DECLARE @AccessPointIdNew uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] WHERE [Resource] = 'PromoProductsCorrections' AND [Action] = 'PromoProductCorrectionDelete');
                UPDATE [DefaultSchemaSetting].[AccessPointRole] SET [AccessPointId] = @AccessPointIdNew
                WHERE [AccessPointId]=@AccessPointOld
                GO
        ";
    }
}
