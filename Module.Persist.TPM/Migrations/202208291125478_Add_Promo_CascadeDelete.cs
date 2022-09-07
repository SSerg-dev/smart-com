namespace Module.Persist.TPM.Migrations
{
    using Core.Settings;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Promo_CascadeDelete : DbMigration
    {
        public override void Up()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropIndex($"{defaultSchema}.PreviousDayIncremental", "IX_PreviousDayIncremental_NONCLUSTERED");
            DropIndex($"{defaultSchema}.PreviousDayIncremental", "IX_PreviousDayIncremental_References_NONCLUSTERED");
            DropForeignKey($"{defaultSchema}.IncrementalPromo", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.BTLPromo", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PreviousDayIncremental", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoProduct", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoProductTree", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoStatusChange", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoSupportPromo", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoUpliftFailIncident", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoProductsCorrection", "PromoProductId", $"{defaultSchema}.PromoProduct");
            DropIndex($"{defaultSchema}.PreviousDayIncremental", new[] { "PromoId" });
            DropIndex($"{defaultSchema}.PreviousDayIncremental", new[] { "ProductId" });
            DropIndex($"{defaultSchema}.PromoStatusChange", new[] { "PromoId" });
            AlterColumn($"{defaultSchema}.PreviousDayIncremental", "PromoId", c => c.Guid(nullable: false));
            AlterColumn($"{defaultSchema}.PreviousDayIncremental", "ProductId", c => c.Guid(nullable: false));
            AlterColumn($"{defaultSchema}.PromoStatusChange", "PromoId", c => c.Guid(nullable: false));
            CreateIndex($"{defaultSchema}.PreviousDayIncremental", "PromoId");
            CreateIndex($"{defaultSchema}.PreviousDayIncremental", "ProductId");
            CreateIndex($"{defaultSchema}.PromoStatusChange", "PromoId");
            AddForeignKey($"{defaultSchema}.IncrementalPromo", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.BTLPromo", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PreviousDayIncremental", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoProduct", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoProductTree", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoStatusChange", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoSupportPromo", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoUpliftFailIncident", "PromoId", $"{defaultSchema}.Promo", "Id", cascadeDelete: true);
            AddForeignKey($"{defaultSchema}.PromoProductsCorrection", "PromoProductId", $"{defaultSchema}.PromoProduct", "Id", cascadeDelete: true);

            SqlString = SqlString.Replace("DefaultSchemaSetting", defaultSchema);
            Sql(SqlString);
        }
        
        public override void Down()
        {
            var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
            DropForeignKey($"{defaultSchema}.PromoProductsCorrection", "PromoProductId", $"{defaultSchema}.PromoProduct");
            DropForeignKey($"{defaultSchema}.PromoUpliftFailIncident", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoSupportPromo", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoStatusChange", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoProductTree", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PromoProduct", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.PreviousDayIncremental", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.BTLPromo", "PromoId", $"{defaultSchema}.Promo");
            DropForeignKey($"{defaultSchema}.IncrementalPromo", "PromoId", $"{defaultSchema}.Promo");
            DropIndex($"{defaultSchema}.PromoStatusChange", new[] { "PromoId" });
            DropIndex($"{defaultSchema}.PreviousDayIncremental", new[] { "ProductId" });
            DropIndex($"{defaultSchema}.PreviousDayIncremental", new[] { "PromoId" });
            AlterColumn($"{defaultSchema}.PromoStatusChange", "PromoId", c => c.Guid());
            AlterColumn($"{defaultSchema}.PreviousDayIncremental", "ProductId", c => c.Guid());
            AlterColumn($"{defaultSchema}.PreviousDayIncremental", "PromoId", c => c.Guid());
            CreateIndex($"{defaultSchema}.PromoStatusChange", "PromoId");
            CreateIndex($"{defaultSchema}.PreviousDayIncremental", "ProductId");
            CreateIndex($"{defaultSchema}.PreviousDayIncremental", "PromoId");
            AddForeignKey($"{defaultSchema}.PromoProductsCorrection", "PromoProductId", $"{defaultSchema}.PromoProduct", "Id");
            AddForeignKey($"{defaultSchema}.PromoUpliftFailIncident", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.PromoSupportPromo", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.PromoStatusChange", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.PromoProductTree", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.PromoProduct", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.PreviousDayIncremental", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.BTLPromo", "PromoId", $"{defaultSchema}.Promo", "Id");
            AddForeignKey($"{defaultSchema}.IncrementalPromo", "PromoId", $"{defaultSchema}.Promo", "Id");
        }
        private string SqlString = @"    
                INSERT INTO [DefaultSchemaSetting].[AccessPoint] ([Disabled],[Resource],[Action],[TPMmode]) VALUES
				(0, 'DeletedRollingScenarios',	'GetDeletedRollingScenarios', 1)
                GO
				DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'Administrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CustomerMarketing');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'CMManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SuperReader');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'KeyAccountManager');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandFinance');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'SupportAdministrator');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'FunctionalExpert');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
                DECLARE @RoleId uniqueidentifier = (SELECT[Id] FROM [DefaultSchemaSetting].[Role] where SystemName = 'DemandPlanning');
                INSERT INTO[DefaultSchemaSetting].[AccessPointRole]
                (RoleId, AccessPointId) values
                (@RoleId, (SELECT[Id] FROM [DefaultSchemaSetting].[AccessPoint] where [Resource]='DeletedRollingScenarios' and [Action]='GetDeletedRollingScenarios'))
                GO
        ";
    }
}
