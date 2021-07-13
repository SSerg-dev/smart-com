namespace Module.Persist.TPM.Migrations
{
	using Core.Settings;
	using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPluRole : DbMigration
    {
        public override void Up()
        {
			var defaultSchema = AppSettingsManager.GetSetting<string>("DefaultSchema", "dbo");
			Sql($@"

			DECLARE @ItemId UNIQUEIDENTIFIER
			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PromoProducts', 'DownloadTemplatePluXLSX')

			SELECT  @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'PromoProducts' AND Action = 'DownloadTemplatePluXLSX'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'

			INSERT INTO {defaultSchema}.AccessPoint(Disabled, Resource, Action) VALUES(0, 'PromoProducts', 'FullImportPluXLSX')

			SELECT  @ItemId = Id FROM [{defaultSchema}].[AccessPoint] where Resource = 'PromoProducts' AND Action = 'FullImportPluXLSX'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id from Jupiter.Role WHERE SystemName = 'Administrator'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'KeyAccountManager'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'DemandPlanning'
			INSERT INTO {defaultSchema}.AccessPointRole(AccessPointId, RoleId) SELECT @ItemId, Id FROM Jupiter.Role WHERE SystemName = 'SupportAdministrator'
			");
        }

        public override void Down()
        {
        }
    }
}
