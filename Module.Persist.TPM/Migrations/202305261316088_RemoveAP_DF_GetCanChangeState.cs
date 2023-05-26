namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAP_DF_GetCanChangeState : DbMigration
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
            DECLARE @Role NVARCHAR(50);
            SET @Role = (Select Id FROM DefaultSchemaSetting.Role Where SystemName = 'DemandFinance');

            DECLARE @AP1 NVARCHAR(50);
            SET @AP1 = (Select Id FROM [DefaultSchemaSetting].[AccessPoint] Where Action = 'GetCanChangeStatePromoes');

            DECLARE @AP2 NVARCHAR(50);
            SET @AP2 = (Select Id FROM [DefaultSchemaSetting].[AccessPoint] Where Action = 'GetCanChangeStatePromoGridViews');

            DELETE FROM [DefaultSchemaSetting].[AccessPointRole]
                  WHERE (RoleId = @Role AND AccessPointId = @AP1) OR (RoleId = @Role AND AccessPointId = @AP2)
            GO
        ";
    }
}
